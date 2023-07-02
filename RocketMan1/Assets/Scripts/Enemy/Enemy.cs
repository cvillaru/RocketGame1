using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Enemy
    [SerializeField] private float health;

    #endregion

    #region Search Algorithm;
    [Header("Search Algorithm")]
    [SerializeField] protected float searchRadius;
    [SerializeField] protected LayerMask whatIsTarget;
    [SerializeField] protected Transform target;


    [SerializeField] protected GameObject sight;
    protected float rotationZ;
    protected Vector3 difference;
    protected RaycastHit2D hit;

    [SerializeField] protected bool targetInView;
    [SerializeField] protected bool targetInRange;

    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField] protected float speed;
    [SerializeField] protected Vector3 direction;
    [SerializeField] protected Vector3 movement;

    #endregion

    #region states
    protected enum states
    {
        patrol,
        aggro,
        aware,
        hurt
    }
    protected states currentState;
    #endregion

    protected Rigidbody2D rb;

    protected void Initiate()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.queriesStartInColliders = false;
    }

    protected void Move()
    {
        // move
        direction = (target.position - transform.position).normalized;
        movement = direction * speed;
        rb.velocity = movement;
    }

    protected void StopMoving()
    {
        rb.velocity = Vector3.zero;
    }

    protected void CheckHealth()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    protected void TakeDamage(int damageTaken)
    {
        health -= damageTaken;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);

    }


}
