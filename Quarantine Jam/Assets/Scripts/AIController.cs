using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D), typeof(CircleCollider2D))]
public class AiController : MonoBehaviour
{
    #region movements

    [Header("Movements")] [SerializeField] private float idleSpeed = 2.0f;
    [SerializeField] private float suspectSpeed = 3.0f;
    [SerializeField] private float alertSpeed = 5.0f;
    [SerializeField] private float stepLength = 1.5f;
    [SerializeField] private List<Transform> pathPoints;

    [FormerlySerializedAs("RoundRobin")] [SerializeField]
    private bool roundRobin;

    #endregion

    #region sprites

    [Header("Sprites")] [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] footprints = new Sprite[2];
    private int _footId;

    #endregion

    #region hunt

    [FormerlySerializedAs("CatchRange")] [Header("Hunt properties")] [SerializeField]
    private float catchRange = 0.5f;

    [FormerlySerializedAs("View")] [SerializeField]
    private PolygonCollider2D view;

    [FormerlySerializedAs("RunEar")] [SerializeField]
    private Collider2D runEar;

    [FormerlySerializedAs("WalkEar")] [SerializeField]
    private Collider2D walkEar;

    [SerializeField] private GameObject player;
    [SerializeField] private AIDestinationSetter destination;
    [SerializeField] private AIPath path;
    [SerializeField] private float suspectThreshold = 0.75f;
    [SerializeField] private float alertThreshold = 4.0f;
    [SerializeField] private float waitTime = 3.0f;

    #endregion

    #region private variables

    private PlayerController _pc;
    private BoxCollider2D _playerCollider;
    private int _targetIterator = 1;
    private int _targetId;
    private float _currentStepLength;
    private float _spotted;

    private event Action StateBehavior;

    #endregion

    private void Start()
    {
        if (!player) player = FindObjectOfType<PlayerController>().gameObject;
        if (!view) view = GetComponent<PolygonCollider2D>();
        if (!runEar) runEar = GetComponents<CircleCollider2D>()[0];
        if (!walkEar) walkEar = GetComponents<CircleCollider2D>()[1];
        _playerCollider = player.GetComponent<BoxCollider2D>();
        _pc = player.GetComponent<PlayerController>();

        spriteRenderer.sprite = footprints[_footId];
        SetStateIdle();
        destination.target.position = pathPoints[0].position;
    }

    private void Update()
    {
        AnimateFootprints();
        StateBehavior?.Invoke();
        Senses();
    }

    #region StateSetters

    private void SetStateIdle()
    {
        if (StateBehavior == StateIdle) return;
        StateBehavior = StateIdle;
        path.maxSpeed = idleSpeed;
        SetNextTarget();
    }

    private bool _reached;

    private void SetStateSuspect()
    {
        if (StateBehavior == StateSuspect || StateBehavior == StateAlert) return;
        StateBehavior = StateSuspect;
        path.maxSpeed = suspectSpeed;
        _reached = false;
    }

    private int _spotsChecked;

    private void SetStateAlert()
    {
        if (StateBehavior == StateAlert) return;
        StateBehavior = StateAlert;
        path.maxSpeed = alertSpeed;
        _spotsChecked = 0;
        _reached = false;
    }

    #endregion

    #region States

    private void StateIdle()
    {
        if (DistanceToTarget() < catchRange)
            SetNextTarget();
    }

    private float _clock;

    private void StateSuspect()
    {
        if (DistanceToTarget() < catchRange && !_reached)
        {
            _clock = Time.time;
            _reached = true;
        }
        else if (_reached && Time.time - _clock > waitTime)
            SetStateIdle();
    }

    private void StateAlert()
    {
        if (DistanceToTarget() < catchRange && _reached)
        {
            if (_spotsChecked++ < 3)
            {
                _overlaps.Clear();
                runEar.OverlapCollider(_filter.NoFilter(), _overlaps);
                if (_overlaps.Count <= 0) return;
                foreach (var c in _overlaps.Where(c => c.CompareTag("SearchSpot")))
                {
                    destination.target.position = c.transform.position;
                }
            }
            else
            {
                SetStateIdle();
            }
        }
        else if (DistanceToTarget() < catchRange && !_reached)
        {
            _reached = true;
        }
    }

    #endregion

    private void SetNextTarget()
    {
        _targetId = (_targetId + _targetIterator) % pathPoints.Count;
        destination.target.position = pathPoints[_targetId].position;

        if (!roundRobin)
            if (_targetId == pathPoints.Count - 1) _targetIterator = -1;
            else if (_targetId == 0) _targetIterator = 1;
    }

    private ContactFilter2D _filter;
    private List<Collider2D> _overlaps = new List<Collider2D>();

    private void Senses()
    {
        var isInSpot = false;

        _overlaps.Clear();
        // View sense
        view.OverlapCollider(_filter.NoFilter(), _overlaps);
        if (_overlaps.Contains(_playerCollider))
        {
            LayerMask mask = LayerMask.GetMask("Walls");
            var position = player.transform.position;
            var transform1 = transform;
            var position1 = transform1.position;
            var hit = Physics2D.Raycast(position1, position - position1,
                Vector2.Distance(position, position1), mask);
            if (!hit.collider)
            {
                isInSpot = true;
                if (_spotted < alertThreshold) _spotted += Time.deltaTime;
            }
        }

        // Hearing sense
        if (Math.Abs(_pc.MoveVelocity.magnitude - float.Epsilon) > 0)
        {
            Hear(ref runEar, _pc.MovementSpeedSprint, _pc.movementSpeed, ref _filter, ref _overlaps, ref isInSpot);
            Hear(ref walkEar, _pc.MovementSpeedDefault, _pc.movementSpeed, ref _filter, ref _overlaps, ref isInSpot);
        }

        if (isInSpot)
        {
            if (_spotted > alertThreshold)
            {
                destination.target.position = player.transform.position;
                SetStateAlert();
            }
            else if (_spotted > suspectThreshold)
            {
                destination.target.position = player.transform.position;
                SetStateSuspect();
            }
        }

        else if (_spotted > 0.0f)
        {
            _spotted -= Time.deltaTime;
        }
    }

    private void Hear(ref Collider2D range, float speedThreshold, float speed, ref ContactFilter2D filter,
        ref List<Collider2D> overlaps, ref bool isInSpot)
    {
        range.OverlapCollider(filter.NoFilter(), overlaps);
        if (!overlaps.Contains(_playerCollider)) return;
        if (!(speed >= speedThreshold)) return;
        isInSpot = true;
        if (_spotted < alertThreshold) _spotted += Time.deltaTime;
    }

    private void AnimateFootprints()
    {
        _currentStepLength += path.maxSpeed * Time.deltaTime;

        if (!(_currentStepLength > stepLength)) return;
        _footId = (_footId + 1) % 2;
        spriteRenderer.sprite = footprints[_footId];
        _currentStepLength = 0;

        var destTransform = destination.transform;
        var localTransform = transform;
        localTransform.position = destTransform.position;
        destTransform.position = localTransform.position;

        if (Vector2.Distance(destination.target.position, transform.position) > catchRange)
            transform.LookAt(destination.target, Vector3.right);

        Vector2 direction = destination.target.position - transform.position;
        transform.rotation =
            Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
    }

    private float DistanceToTarget()
    {
        return Vector2.Distance(destination.target.position, transform.position);
    }
}