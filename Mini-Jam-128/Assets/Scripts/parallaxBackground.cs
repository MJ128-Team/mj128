using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxBackground : MonoBehaviour
{
    [SerializeField] private float parallaxEffect = 0.5f;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        float deltaMovement = cameraTransform.position.y - lastCameraPosition.y;

        transform.position += new Vector3(0, deltaMovement * parallaxEffect, 0);
        //transform.position += deltaMovement * parallaxEffect;
        
        lastCameraPosition = cameraTransform.position;
    }


}
