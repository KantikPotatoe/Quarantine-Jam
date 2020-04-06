using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D), typeof(CircleCollider2D))]
public class AIController : MonoBehaviour
{
    #region movements
    [Header("Movements")]
    [SerializeField] private float idleSpeed = 2.0f;
    [SerializeField] private float suspectSpeed = 3.0f;
    [SerializeField] private float alertSpeed = 5.0f;
    [SerializeField] private float stepLength = 1.5f;
    [SerializeField] private List<Transform> pathPoints;
    [SerializeField] private bool RoundRobin = false;
    #endregion

    #region sprites
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] footprints = new Sprite[2];
    private int footID = 0;
    #endregion

    #region hunt
    [Header("Hunt properties")]
    [SerializeField] private float CatchRange = 0.5f;
    [SerializeField] private PolygonCollider2D View;
    [SerializeField] private Collider2D RunEar;
    [SerializeField] private Collider2D WalkEar;
    [SerializeField] private GameObject player;
    [SerializeField] private AIDestinationSetter destination;
    [SerializeField] private AIPath path;
    [SerializeField] private float suspectThreshold = 0.75f;
    [SerializeField] private float alertThreshold = 4.0f;
    [SerializeField] private float waitTime = 3.0f;
    #endregion

    #region private variables
    private PlayerController pc;
    private BoxCollider2D playerCollider;
    private int targetIterator = 1;
    private int targetID = 0;
    private float currentStepLength = 0;
    private float spotted = 0.0f;

    private event Action StateBehavior;
    #endregion

    void Start()
    {
        if (!player) player = FindObjectOfType<PlayerController>().gameObject;
        if (!View) View = GetComponent<PolygonCollider2D>();
        if (!RunEar) RunEar = GetComponents<CircleCollider2D>()[0];
        if (!WalkEar) WalkEar = GetComponents<CircleCollider2D>()[1];
        playerCollider = player.GetComponent<BoxCollider2D>();
        pc = player.GetComponent<PlayerController>();

        spriteRenderer.sprite = footprints[footID];
        SetStateIdle();
        destination.target.position = pathPoints[0].position;
    }

    void Update()
    {
        AnimateFootprints();
        StateBehavior?.Invoke();
        Senses();
    }

    #region StateSetters
    void SetStateIdle() {
        if (StateBehavior != StateIdle) { 
            StateBehavior = StateIdle;
            path.maxSpeed = idleSpeed;
            SetNextTarget();
        }
    }

    bool reached;
    void SetStateSuspect() {
        if (StateBehavior != StateSuspect && StateBehavior != StateAlert) { 
            StateBehavior = StateSuspect;
            path.maxSpeed = suspectSpeed;
            reached = false;
        }
    }

    int spotsChecked;
    void SetStateAlert() {
        if (StateBehavior != StateAlert) {
            StateBehavior = StateAlert;
            path.maxSpeed = alertSpeed;
            spotsChecked = 0;
            reached = false;
        }
    }
    #endregion

    #region States
    void StateIdle()
    {
        if (distanceToTarget() < CatchRange) 
            SetNextTarget();
    }

    float clock;
    void StateSuspect()
    {
        if (distanceToTarget() < CatchRange && !reached)
        {
            clock = Time.time;
            reached = true;
        }
        else if (reached && Time.time - clock > waitTime)
            SetStateIdle();
    }

    void StateAlert()
    {
        if (distanceToTarget() < CatchRange &&  reached)
        {
            if (spotsChecked++ < 3)
            {
                overlaps.Clear();
                RunEar.OverlapCollider(filter.NoFilter(), overlaps);
                if (overlaps.Count > 0)
                {
                    foreach (Collider2D c in overlaps)
                    {
                        if (c.CompareTag("SearchSpot"))
                        {
                            destination.target.position = c.transform.position;
                        }
                    }
                } 
            }
            else
            {
                SetStateIdle();
            }
        }
        else if (distanceToTarget() < CatchRange && !reached)
        {
            reached = true;
        }
    }
    #endregion

    void SetNextTarget()
    {
        targetID = (targetID + targetIterator) % pathPoints.Count;
        destination.target.position = pathPoints[targetID].position;
        
        if (!RoundRobin)
            if (targetID == pathPoints.Count - 1) targetIterator = -1;
            else if (targetID == 0) targetIterator = 1;
    }

    ContactFilter2D filter = new ContactFilter2D();
    List<Collider2D> overlaps = new List<Collider2D>();
    void Senses()
    {
        bool isInSpot = false;
        
        overlaps.Clear();
        // View sense
        View.OverlapCollider(filter.NoFilter(), overlaps);
        if (overlaps.Contains(playerCollider))
        {
            LayerMask mask = LayerMask.GetMask("Walls");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, Vector2.Distance(player.transform.position, transform.position), mask);
            if (!hit.collider)
            {
                isInSpot = true;
                if (spotted < alertThreshold) spotted += Time.deltaTime;
            }
        }

        // Hearing sense
        if (pc._moveVelocity.magnitude != 0)
        {
            hear(ref RunEar, pc.MovementSpeedSprint, pc._movementSpeed, ref filter, ref overlaps, ref isInSpot);
            hear(ref WalkEar, pc.MovementSpeedDefault, pc._movementSpeed, ref filter, ref overlaps, ref isInSpot);
        }
        
        if (isInSpot) 
        {
            if (spotted > alertThreshold)
            {
                destination.target.position = player.transform.position;
                SetStateAlert();
            } 
            else if (spotted > suspectThreshold)
            {
                destination.target.position = player.transform.position;
                SetStateSuspect();
            }

        }

        else if (spotted > 0.0f)
        {
            spotted -= Time.deltaTime;
        }
    }

    void hear(ref Collider2D range, float speedTheshold, float speed, ref ContactFilter2D filter, ref List<Collider2D> overlaps, ref bool isInSpot)
    {
        range.OverlapCollider(filter.NoFilter(), overlaps);
        if (overlaps.Contains(playerCollider))
        {
            if (speed >= speedTheshold)
            {
                isInSpot = true;
                if (spotted < alertThreshold) spotted += Time.deltaTime;
            }
        }
    }

    void AnimateFootprints()
    {
        currentStepLength += path.maxSpeed * Time.deltaTime;

        if (currentStepLength > stepLength) 
        { 
            footID = (footID + 1) %2;
            spriteRenderer.sprite = footprints[footID];
            currentStepLength = 0;

            transform.position = destination.transform.position;
            destination.transform.position = transform.position;

            if (Vector2.Distance(destination.target.position, transform.position) > CatchRange)
                transform.LookAt(destination.target, Vector3.right);

            Vector2 direction = destination.target.position - transform.position;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);

        }
    }
    
    float distanceToTarget()
    {
        return Vector2.Distance(destination.target.position, transform.position);
    }
}
