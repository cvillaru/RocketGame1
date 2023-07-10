using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AI : MonoBehaviour
{
    [Header("External Scripts")]
    [SerializeReference] protected Pathfinding.AIPath aiPath;
    [SerializeReference] protected Pathfinding.AIDestinationSetter aiDestingationSetter;

    [SerializeField] protected float health;

    [Header("AI")]
    [SerializeField] protected float searchRadius;
    [SerializeField] protected LayerMask layerToIgnore;
    [SerializeField] protected LayerMask whatIsTarget;
    [SerializeField] protected Transform target;
    [SerializeField] protected GameObject sight;

    [SerializeField] protected float overlapCheckRadius;
    [SerializeField] protected LayerMask overlapCheck;

    protected float rotationZ;
    protected Vector2 difference;
    protected RaycastHit2D hit;

    protected bool targetInRange;
    protected bool targetInView;
    protected bool enemyOverlapped;

    protected void Initiate()
    {
        aiPath = GetComponent<Pathfinding.AIPath>();
        Physics2D.queriesStartInColliders = false;
        aiDestingationSetter = GetComponent<Pathfinding.AIDestinationSetter>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
        aiDestingationSetter.target = target;
    }

    private void Update()
    {
        CheckHealth();
    }

    protected void CheckHealth()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;

    }

    protected void Move() 
    {
        aiPath.canMove = true;
    }

    protected void StopMoving()
    {
        aiPath.canMove = false;
    }

    // moves around map - default state
    protected void Patrol() 
    {
        Debug.Log("Patrolling");
        return;
    }

    // attempts to chase and attack player
    protected void Aggro()
    {
        Debug.Log("Aggrovated");
        return;
    }

    // when enemy loses sight of player after aggro state.
    protected void Aware()
    {
        Debug.Log("Aware");
        return;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, overlapCheckRadius);

    }
}
