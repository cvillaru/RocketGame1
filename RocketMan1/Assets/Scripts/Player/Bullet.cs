using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float life;
    [SerializeField] private Rigidbody2D rb;

    private float elapsedTime;
    private Coroutine timeCoroutine;

    private void OnEnable()
    {
        elapsedTime = 0f;
        timeCoroutine = StartCoroutine(EndLife());
    }

    private void FixedUpdate()
    {
        rb.velocity = gameObject.transform.up * speed;
    }

    private IEnumerator EndLife()
    {
        while (elapsedTime < life)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }

        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
