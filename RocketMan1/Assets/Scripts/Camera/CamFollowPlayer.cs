using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    /*    [SerializeField] private Transform target;
        public float smoothSpeed = 0.125f;
        public float maxZoom = 10f;
        private float baseZoom = 5f;
        private Rigidbody2D targetRigidbody;
        private Camera mainCamera;

        private bool targetPresent;

        public Transform Target { get => target; set => target = value; }

        void Start()
        {
            if (target != null)
            {
                targetRigidbody = target.GetComponent<Rigidbody2D>();
            }
            mainCamera = Camera.main;
            mainCamera.orthographicSize = baseZoom;

        }

        void FixedUpdate()
        {
            if (target != null && target.gameObject.activeSelf)
            {
                targetRigidbody = target.GetComponent<Rigidbody2D>();
                float velocityFactor = Mathf.Clamp01(targetRigidbody.velocity.magnitude / 10f);
                float dynamicZoom = baseZoom + maxZoom * velocityFactor;
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, dynamicZoom, smoothSpeed);

                if (targetRigidbody.velocity.magnitude < 0.1f)
                {
                    mainCamera.orthographicSize = baseZoom;
                }

                Vector3 desiredPosition = target.position + new Vector3(0, 0, -10f);
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                transform.position = smoothedPosition;
            }
        }*/

    [SerializeField] private Transform target;
    public float smoothSpeed = 0.125f;
    public float maxZoom = 10f;
    private float baseZoom = 5f;
    private Rigidbody2D targetRigidbody;
    private Camera mainCamera;

    public Transform Target { get => target; set => target = value; }

    void Start()
    {
        if (target != null)
        {
            targetRigidbody = target.GetComponent<Rigidbody2D>();
        }
        mainCamera = Camera.main;
        mainCamera.orthographicSize = baseZoom;
    }

    void FixedUpdate()
    {
        if (target != null && target.gameObject.activeSelf)
        {
            targetRigidbody = target.GetComponent<Rigidbody2D>();
            float velocityFactor = Mathf.Clamp01(targetRigidbody.velocity.magnitude / 10f);
            float dynamicZoom = baseZoom + maxZoom * velocityFactor;
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, dynamicZoom, smoothSpeed);

            if (targetRigidbody.velocity.magnitude < 0.1f)
            {
                mainCamera.orthographicSize = baseZoom;
            }

            Vector3 desiredPosition = target.position + new Vector3(0, 0, -10f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }


}
