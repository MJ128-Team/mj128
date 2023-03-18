using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidLive : MonoBehaviour
{
    private Camera mainCam;
    private float camWidth;
    private float camHeight;

    [SerializeField] private Vector3 spawnOffsetY = Vector3.zero;
    private float spawnPosX;
    private float spawnPosY;
    private float spawnPosXMax;
    private float spawnPosXMin;

    [SerializeField] private bool isStatic = true;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private int directionX;

    [SerializeField] private bool isOutOfView = true;
    [SerializeField] private bool isPassedBy = false;

    void Start()
    {
        mainCam = Camera.main;
        camHeight = mainCam.orthographicSize;
        camWidth = camHeight * mainCam.aspect;

        Spawn();
    }

    void Update()
    {
        HandleMovement();
        HandleRespawn();
    }

    // Needd to add a collider (isTrigger) to the camera
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Camera")
        {
            isOutOfView = false;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Camera")
        {
            isOutOfView = true;
            isPassedBy = true;
        }
    }

    void Spawn() // Spawn the asteroid at a random position before camera
    {   
        if (isStatic)
        {
            spawnPosX = Random.Range(-camWidth, camWidth);
            spawnPosY = Random.Range(-camHeight, camHeight);
        }
        else
        {
            // calculate x offset depending on direction and speed
            // so that the asteroid will be able reach the camera

            directionX = Random.Range(0, 2) == 0 ? -1 : 1;

            float xOffset = (camWidth + transform.localScale.x) / speed;

            if (directionX > 0)
            {
              spawnPosXMax = -camWidth - xOffset;
              spawnPosXMin = -camWidth;
            }
            else
            {
              spawnPosXMax = camWidth;
              spawnPosXMin = camWidth + xOffset;
            }

            spawnPosX = Random.Range(spawnPosXMin, spawnPosXMax);
            spawnPosY = Random.Range(spawnPosXMin, spawnPosXMax);
        }

        transform.position = new Vector3(spawnPosX, spawnPosY, 0) + spawnOffsetY;

        isOutOfView = true;
        isPassedBy = false;
    }

    void HandleMovement()
    {
        if(!isStatic)
        {
            transform.Translate(Vector2.left * speed * directionX * Time.deltaTime);
        }
    }

    void HandleRespawn()
    {
        if(isOutOfView && isPassedBy)
        {
            Spawn();
        }
    }
}
