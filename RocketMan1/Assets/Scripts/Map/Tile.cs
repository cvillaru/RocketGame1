using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Range(0.0f, 1.0f)] public float spawnChance;

    [SerializeField] private GameObject[] openings;
    [SerializeField] private GameObject[] enemySpanwers;

    [SerializeField] private bool startTile;
    [SerializeField] private bool finishTile;

    public GameObject[] Openings { get => openings; }
    public bool StartTile { get => startTile; set => startTile = value; }
    public bool FinishTile { get => finishTile; set => finishTile = value; }
    public GameObject[] EnemySpanwers { get => enemySpanwers; set => enemySpanwers = value; }

    private void Awake()
    {
        foreach (GameObject enemySpawner in enemySpanwers)
        {
            enemySpawner.SetActive(false);
        }
    }
    
}
