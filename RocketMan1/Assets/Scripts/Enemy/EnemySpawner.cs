using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    private LevelGenerator lvlGen;

    [SerializeField] private float timeBtwnSpawn;
    private float _timeBtwnSpawn;

    [SerializeField] private GameObject enemy;
    [SerializeField] private bool canSpawn;

    [SerializeField] private int maxEnemiesToSpawn;
    [SerializeField] List<GameObject> spawnedEnemies;

    public bool CanSpawn { get => canSpawn; set => canSpawn = value; }

    private void Awake()
    {
        lvlGen = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        canSpawn = false;
    }

    void Update()
    {
        if (canSpawn)
        {
            if (spawnedEnemies.Count < maxEnemiesToSpawn)
            {
                if (_timeBtwnSpawn <= 0)
                {
                    GameObject e = Instantiate(enemy, transform.position, Quaternion.identity);
                    spawnedEnemies.Add(e);
                    lvlGen.Map.Add(e);

                    _timeBtwnSpawn = timeBtwnSpawn;
                }
                else
                {
                    _timeBtwnSpawn -= Time.deltaTime;
                }
            }
        }
    }
}
