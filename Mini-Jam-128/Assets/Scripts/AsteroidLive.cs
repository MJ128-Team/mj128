using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidLive : MonoBehaviour
{
    private Camera mainCam;
    private float camWidth;
    private float camHeight;

    private GameObject player;

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

    public Sprite[] asteroidSprites;
    private SpriteRenderer asteroidRenderer;
    private PolygonCollider2D asteroidCollider;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        mainCam = Camera.main;
        camHeight = mainCam.orthographicSize;
        camWidth = camHeight * mainCam.aspect;

        asteroidRenderer = GetComponent<SpriteRenderer>();
        asteroidCollider = GetComponent<PolygonCollider2D>();

        RandomSprite();

        if(!isStatic)
        {
            directionX = Random.Range(0, 2) == 0 ? -1 : 1;
            speed = Random.Range(0.1f, speed);
        }

        if(mainCam.transform.position.y > transform.position.y)
        {
            Spawn();
        }
    }

    void Update()
    {
        HandleMovement();
        //HandleRespawn();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "MainCamera")
        {
            // Debug.Log("Asteroid entered camera view");

            isOutOfView = false;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "MainCamera")
        {
            // Debug.Log("Asteroid exited camera view");

            isOutOfView = true;
            isPassedBy = true;

            Spawn();
        }
    }

    void OnDestroy()
    {
        // if(player)
        // {
        //     GameObject.Instantiate(gameObject, transform.GetComponentInParent<Transform>()); // Re-instantiate the asteroid
        // }
        
    }


    void Spawn() // Spawn the asteroid at a random position before camera
    {   
        RandomSprite();

        if (isStatic)
        {
            spawnPosX = Random.Range(-camWidth, camWidth);
            spawnPosY = Random.Range(-camHeight, camHeight*2);
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
            spawnPosY = Random.Range(-camHeight, camHeight*2);
        }

        transform.position = new Vector3(spawnPosX, spawnPosY + mainCam.transform.position.y, 0) + spawnOffsetY;

        isOutOfView = true;
        isPassedBy = false;
    }

    void RandomSprite()
    {
        if(asteroidSprites.Length > 0)
        {
            int randomIndex = Random.Range(0, asteroidSprites.Length);
            asteroidRenderer.sprite = asteroidSprites[randomIndex];

            asteroidCollider.pathCount = asteroidRenderer.sprite.GetPhysicsShapeCount();
            List<Vector2> path = new List<Vector2>();
            for (int i = 0; i < asteroidCollider.pathCount; i++) {
                path.Clear();
                asteroidRenderer.sprite.GetPhysicsShape(i, path);
                asteroidCollider.SetPath(i, path.ToArray());
            }
        }
    }

    //void RandomPosition()

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
            // Debug.Log("Ready to respawn");
            Spawn();
        }
    }
}
