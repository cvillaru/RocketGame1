using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Mouse")]
    [SerializeField] private float offset = -90f;
    [SerializeField] private float stopDistance;
    [SerializeField] private float maxDistance;

    [Header("Thruster")]
    [SerializeField] private float maxForce;
    [SerializeField] private float minForce;
    [SerializeField] private float deceleration;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float dangerousSpeedThreshold;

    [Header("Gun")]
    [SerializeField] private GameObject gun;
    private Gun g;

    [Header("Mouse - Debuger")]
    [SerializeField] private float distance;
    [SerializeField] private Vector3 mousePos;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float angle;

    [Header("Thruster - Debuger")]
    [SerializeField] private float force;
    [SerializeField] private float magnitude;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        g = Gun.Instance;
    }

    private void RotateTowards()
    {
        // Get mouse position in world space
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // Calculate the angle between the game object's position and the mouse cursor position
        angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;

        // Add the offset angle to the calculated angle
        angle += offset;

        // Set the game object's rotation to the adjusted angle
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void MoveTowards()
    {
        //particles.Play();

        // Calculate the distance between the game object's position and the mouse cursor position
        distance = Vector3.Distance(transform.position, mousePos);

        if (distance <= stopDistance)
        {
            // Slow down the game object gradually to a halt if within the stop distance
            rb.velocity = Vector2.MoveTowards(rb.velocity, Vector2.zero, deceleration * Time.deltaTime);
            return;
        }

        // Calculate the force to apply to the game object based on the distance
        force = Mathf.Lerp(minForce, maxForce, distance / maxDistance);

        // Calculate the direction to apply the force in
        direction = (mousePos - transform.position).normalized;

        // Apply the force to the game object
        GetComponent<Rigidbody2D>().AddForce(direction * force);
    }


    private void FixedUpdate()
    {
        RotateTowards();

        magnitude = rb.velocity.magnitude;

        if (Input.GetMouseButton(1))
        {
            MoveTowards();
        }
        else
        {
            /*if (rb.velocity.magnitude.Equals(0))
            {
                particles.Stop();
            }
            particles.Stop();*/
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            g.Shoot();
        }
    }
}
