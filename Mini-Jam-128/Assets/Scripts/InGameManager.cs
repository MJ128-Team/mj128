using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;

    private GameObject player;
    private PlayerController pc;

    // Stats
    [SerializeField] private int shields = 3;
    [SerializeField] private float power = 3.0f; // in seconds
    [SerializeField] private float powerMax = 3.0f;
    [SerializeField] private float gameTime = 0;
    [SerializeField] private float gameTimeBest;

    // Game State
    [SerializeField] private int level = 0;
    [SerializeField] private float secToNexLevel = 3f; // Check also RestetLevel()
    private float currentFuelPowerUpFreq;
    private bool isLevelUp = false;

    public GameObject playerPrefab;
    public Transform initPosition;
    
    // Asteroids
    [SerializeField] private int asteroidsAmmount = 8;
    private int currentAsteroidsAmmount = 8;
    private Transform asteroidField;
    public GameObject asteroidPrefab;

    // Powerups
    public GameObject powerUpFuelPrefab;
    [SerializeField] private float fuelPowerUpFreq = 4;
    private float fuelPowerUpTimer = 0;

    // Other
    [SerializeField] private bool isPlayingIntro = true;
    [SerializeField] private bool isGameStarted = false;
    [SerializeField] private bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();

        UpdateMovementXBoundaries();
        pc.LaunchTubeAnimation();
        

        // Spawn Asteroid Field
        asteroidField = GameObject.FindGameObjectWithTag("AsteroidField").transform;
        SpawnAsteroids();
    }

   

    void Update()
    {
        if(isGameStarted)
        {
          if(shields <= 0 && !isGameOver)
          {
              isGameStarted = false;

              HandleGameOver("crash");
          }

          HandleAsteroids();
          HandlePowerUpFuel();
        }
    }

    void FixedUpdate()
    {
        if(isGameStarted && !isGameOver)
        {
            gameTime += Time.fixedDeltaTime;
            DrainPower();
            HandleLevel();
        }
    }

    void DrainPower()
    {
        if( power > 0)
        {
            power -= Time.fixedDeltaTime;
            InGameUIManager.instance.OnFuelChanged(power/powerMax);
        } 
        else if( power < 0)
        {
            power = 0;

            InGameUIManager.instance.OnFuelChanged(power/powerMax);
            
            HandleGameOver("fuel");
        }
        
    }

    void UpdateMovementXBoundaries()
    {
        float playerWidth = player.GetComponent<Collider2D>().bounds.size.x;
        // Debug.Log("Player Width: " + playerWidth);
        float xOffset = Camera.main.orthographicSize * Camera.main.aspect - playerWidth;
        pc.SetXBoundaries(initPosition.position.x - xOffset, initPosition.position.x + xOffset);
    }

    void SpawnAsteroids(int currentAmmount = 0)
    {
        for (int i = currentAmmount; i < asteroidsAmmount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-10, 10), Random.Range(0, 200), 0) + asteroidField.position;
            GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity, asteroidField);
        }
    }

    void ClearAsteroids()
    {
        foreach (Transform child in asteroidField)
        {
            Destroy(child.gameObject);
        }
    }

    void SpawnFuel()
    {
        GameObject refill = Instantiate(powerUpFuelPrefab, asteroidField);
    }

    void HandlePowerUpFuel()
    {
        if(isGameStarted && !isGameOver){
            fuelPowerUpTimer -= Time.deltaTime;

            if (fuelPowerUpTimer <= 0)
            {
                SpawnFuel();
                fuelPowerUpTimer = fuelPowerUpFreq;
            }
        }
    }

    void HandleAsteroids()
    {
        if (asteroidField.childCount < asteroidsAmmount)
        {
            SpawnAsteroids(asteroidField.childCount);
        }
    }

    void HandleLevel()
    {
        // Debug.Log("GAME TIME: "+gameTime);
        // Debug.Log((gameTime % secToNexLevel+1) + " -> " + secToNexLevel);
        // Debug.Log("LEVEL: "+level);

        if ((gameTime % secToNexLevel+1) > secToNexLevel)
        { 
            if(!isLevelUp)
            {
                isLevelUp = true;
                IncreaseLevel();
                // secToNexLevel += 3f;
            }
        }
        else
        {
            if(isLevelUp)
            {
                isLevelUp = false;
            }
        }
    }

    void IncreaseLevel()
    {
        level++;
        currentAsteroidsAmmount++;
        //Increase asteroids size
        currentFuelPowerUpFreq -= 0.1f;

        Debug.Log("LEVEL UP: " + level);
        Debug.Log("ASTEROIDS AMMOUNT: " + currentAsteroidsAmmount);
        Debug.Log("ASTEROIDS SIZE: " + "TODO")
        Debug.Log("FUEL POWER UP FREQ: " + currentFuelPowerUpFreq);
    }

    void ResetLevel()
    {
        level = 0;
        secToNexLevel = 3f;
        currentAsteroidsAmmount = asteroidsAmmount;
        currentFuelPowerUpFreq = fuelPowerUpFreq;
    }

    void HandleGameOver(string cause)
    {
        if(cause == "crash")
        {
            Debug.Log("GAME OVER: CRASHED");
        }
        else if(cause == "fuel")
        {
            Debug.Log("GAME OVER: NO FUEL");
            // TODO: Game Over other way 
        }

        HandleBestTime(gameTime);
        InGameUIManager.instance.OnGameOver(gameTime, gameTimeBest);
        isGameOver = true; 
    }

    void HandleBestTime(float gameTime)
    {
        if(gameTime > gameTimeBest)
        {
            gameTimeBest = gameTime;
        }
    }

    public float GetPower()
    {
        return power;
    }

    public float GetPowerNorm()
    {
        return power / powerMax;
    }

    public void IncreasePower(float amount)
    {
        power += amount;
        if (power > powerMax)
        {
            power = powerMax;
        }
    }

    public float GetShields()
    {
        return shields;
    }

    public void IncreaseShields()
    {
        shields++;
        InGameUIManager.instance.OnShieldsChanged(shields);
    }

    public void DecreaseShields()
    {
        shields--;
        InGameUIManager.instance.OnShieldsChanged(shields);
    }

    public void InstaDeath()
    {
        shields = 0;
        InGameUIManager.instance.OnShieldsChanged(shields);
    }

    public bool IsPlayingIntro()
    {
        return isPlayingIntro;
    }

    public bool IsGameStarted()
    {
        return isGameStarted;
    }

    public void StartGame()
    {
        isPlayingIntro = false;
        isGameStarted = true;
    }

    public void RestartGame()
    {
        isGameOver = false;
        isGameStarted = false;
        isPlayingIntro = true;

        // Reset Stats
        shields = 3;
        power = powerMax;
        gameTime = 0;
        fuelPowerUpTimer = 0;

        ResetLevel();

        InGameUIManager.instance.OnShieldsChanged(shields);
        InGameUIManager.instance.OnFuelChanged(power/powerMax);

        // Reset Player
        if(!player)
        {
            player = Instantiate(playerPrefab, initPosition.position, Quaternion.identity);
            pc = player.GetComponent<PlayerController>();
            UpdateMovementXBoundaries();
        }
        else
        {
            player.transform.position = initPosition.position;
            player.transform.rotation = Quaternion.identity;
        }
        Camera.main.transform.GetComponent<CameraFollow>().MoveToPlayer();
        pc.LaunchTubeAnimation();

        // Reset Asteroids
        foreach (Transform child in asteroidField)
        {
            Destroy(child.gameObject);
        }

        ClearAsteroids();
        SpawnAsteroids();
    }
}
