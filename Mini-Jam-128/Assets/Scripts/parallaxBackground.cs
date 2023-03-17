using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxBackground : MonoBehaviour
{
    [SerializeField] private float parallaxEffect = 0.5f;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureSizeX;
    private float textureSizeY;
    private float parentScale;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        textureSizeX = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;
        textureSizeY = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.y;
        parentScale = transform.localScale.y;
    }

    void LateUpdate()
    {
        float deltaMovement = cameraTransform.position.y - lastCameraPosition.y;
        transform.position += new Vector3(0, deltaMovement * parallaxEffect, 0);

        if (cameraTransform.position.y > transform.position.y + textureSizeY/2)
        {
            // Debug.Log("Move UP!");
            transform.position += new Vector3(0, textureSizeY, 0);
        }
        else if (cameraTransform.position.y < transform.position.y - textureSizeY/2)
        {
            transform.position -= new Vector3(0, textureSizeY, 0);
        }

        lastCameraPosition = cameraTransform.position;
    }


}
