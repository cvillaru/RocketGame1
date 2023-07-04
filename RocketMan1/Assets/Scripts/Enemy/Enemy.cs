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
    [SerializeField] public float searchRadius;
    [SerializeField] protected LayerMask whatIsTarget;
    [SerializeField] protected LayerMask layerToIgnore;
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

    protected Rigidbody2D rb;

    protected void Initiate()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.queriesStartInColliders = false;
    }

    private void Update()
    {
        CheckHealth();
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

    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);

    }


}
