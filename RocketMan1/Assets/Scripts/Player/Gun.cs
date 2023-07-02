using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    ObjectPool objPool;

    GameObject bullet;

    [SerializeField] private float fireRate;
    [SerializeField] private Transform firePoint;

    private float timeBtwnShots;

    public static Gun Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        objPool = ObjectPool.Instance;
    }

    public void Shoot() 
    {
        if (timeBtwnShots <= 0)
        {
            bullet = objPool.SpawnFromPool("Gun1", transform.position, firePoint.rotation);

            timeBtwnShots = fireRate;
        }
        else
        {
            timeBtwnShots -= Time.deltaTime;
        }
    }
}

