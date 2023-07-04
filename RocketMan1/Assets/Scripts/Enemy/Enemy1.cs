using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{

    private LevelGenerator lvlGen;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;        
    }

    private void Start()
    {
        lvlGen = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        Initiate();
    }

    void Update()
    {        
        CheckHealth();
    }
    private void FixedUpdate()
    {
        Checkers();
        if (targetInRange)
        {
            LineOfSight();
            if (targetInView)
            {
                Move();
            }
            else
            {
                StopMoving();
            }
        }
        else
        {
            StopMoving();
        }
    }

    private void LineOfSight()
    {
        difference = target.position - transform.position;
        rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        sight.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

        hit = Physics2D.Raycast(sight.transform.position, sight.transform.right, searchRadius, ~layerToIgnore);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                targetInView = true;
                Debug.DrawLine(sight.transform.position, hit.point, Color.green);
            }
            else
            {
                targetInView = false;
                Debug.DrawLine(sight.transform.position, hit.point, Color.red);
            }

        }
    }

    private void Checkers()
    {
        targetInRange = Physics2D.OverlapCircle(transform.position, searchRadius, whatIsTarget);
    }
}
