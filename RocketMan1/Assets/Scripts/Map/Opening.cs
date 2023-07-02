using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour
{
    [SerializeField] private bool blockerAdded;
    [SerializeField] private Vector2 position;

    public bool BlockerAdded { get => blockerAdded; set => blockerAdded = value; }
    public Vector2 Position { get => position; set => position = value; }


    private void Awake()
    {
        position = transform.position;
    }
}
