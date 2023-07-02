using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    //LevelGenerator lg;

    [SerializeField] private Vector2 position;
    [SerializeField] private bool hasSpawned;

    public bool HasSpawned { get => hasSpawned; set => hasSpawned = value; }

    private void Awake()
    {
        //lg = GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>();


        //Debug.Log(position);



    }
}
