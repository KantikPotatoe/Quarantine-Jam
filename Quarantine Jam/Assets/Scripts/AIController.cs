using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AIController : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField]
    private float idleSpeed;
    [SerializeField]
    private float suspectSpeed;
    [SerializeField]
    private float alertSpeed;
    [SerializeField]
    private float stepLength;

    [Header("Sprites")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite[] footprints = new Sprite[2];
    private int footID = 0;

    private Transform target;
    private Vector3 monsterPosition;
    private float speed;
    private float currentStepLength = 0;

    private event Action StateBehavior;


    void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;
        speed = idleSpeed;
        spriteRenderer.sprite = footprints[footID];
        monsterPosition = transform.position;
    }

    void Update()
    {
        StateBehavior?.Invoke();
        GoToTarget();
    }

    void StateIdle()
    {

    }

    void StateSuspect()
    {

    }

    void StateAlert()
    {

    }

    void GoToTarget()
    {
        monsterPosition = Vector3.MoveTowards(monsterPosition, target.transform.position, speed * Time.deltaTime);
        AnimateFootprints();
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

            Vector2 direction = target.position - transform.position;
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);

        }
    }
}
