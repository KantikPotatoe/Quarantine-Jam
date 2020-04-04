using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AIController : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField] private float idleSpeed = 2.0f;
    [SerializeField] private float suspectSpeed = 3.0f;
    [SerializeField] private float alertSpeed = 5.0f;
    [SerializeField] private float stepLength = 1.5f;
    [SerializeField] private List<Transform> pathPoints;
    [SerializeField] private bool RoundRobin = false;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] footprints = new Sprite[2];
    private int footID = 0;

    [Header("Hunt properties")]
    [SerializeField] private float CatchRange = 0.5f;

    private Vector3 target;
    private int targetIterator = 1;
    private int targetID = 0;
    private Vector3 monsterPosition;
    private float speed;
    private float currentStepLength = 0;

    private event Action StateBehavior;


    void Start()
    {
        spriteRenderer.sprite = footprints[footID];
        SetStateIdle();
        target = pathPoints[0].position;
        monsterPosition = transform.position;
    }

    void Update()
    {
        GoToTarget();
        StateBehavior?.Invoke();
        Senses();
    }

    #region StateSetters
    void SetStateIdle()
    {
        StateBehavior = StateIdle;
        speed = idleSpeed;
    }

    void SetStateSuspect()
    {
        StateBehavior = StateSuspect;
        speed = suspectSpeed;
    }

    void SetStateAlert()
    {
        StateBehavior = StateAlert;
        speed = alertSpeed;
    }
    #endregion

    #region States
    void StateIdle()
    {
        if (Vector3.Distance(target, transform.position) < CatchRange) 
            SetNextTarget();
    }

    void StateSuspect()
    {

    }

    void StateAlert()
    {

    }
    #endregion

    void GoToTarget()
    {
        monsterPosition = Vector3.MoveTowards(monsterPosition, target, speed * Time.deltaTime);
        AnimateFootprints();
    }

    void SetNextTarget()
    {
        targetID = (targetID + targetIterator) % pathPoints.Count;
        target = pathPoints[targetID].position;
        
        if (!RoundRobin)
            if (targetID == pathPoints.Count - 1) targetIterator = -1;
            else if (targetID == 0) targetIterator = 1;
    }

    void Senses()
    {
        //FindObjectOfType<PlayerController>().transform.position;
    }

    void AnimateFootprints()
    {
        currentStepLength += speed * Time.deltaTime;

        if (currentStepLength > stepLength) 
        { 
            footID = (footID + 1) %2;
            spriteRenderer.sprite = footprints[footID];
            currentStepLength = 0;

            transform.position = monsterPosition;
            transform.LookAt(target, Vector3.right);

            Vector2 direction = target - transform.position;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);

        }
    }
}
