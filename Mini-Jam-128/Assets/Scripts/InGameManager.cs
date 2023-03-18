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

    // Game State
    [SerializeField] private int level = 0;
    [SerializeField] private int asteroidsAmmount = 8;
    private Transform asteroidField;
    public GameObject asteroidPrefab;

    // Powerups
    public GameObject powerUpFuelPrefab;
    [SerializeField] private float fuelPowerUpFreq = 4;
    private float fuelPowerUpTimer = 0;

    // Other
    [SerializeField] private bool isPlayingIntro = true;
    [SerializeField] private bool isGameStarted = false;

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

        // Lauch tube animation
        pc.LaunchTubeAnimation();

        // Spawn Asteroid Field
        asteroidField = GameObject.FindGameObjectWithTag("AsteroidField").transform;
        SpawnAsteroids();
    }

    void Update()
    {
        if(shields <= 0)
        {
            isGameStarted = false;
            // Game Over
            Debug.Log("GAME OVER");
        }

        HandleAsteroids();
        HandlePowerUpFuel();
    }

    void FixedUpdate()
    {
        if(isGameStarted)
        {
            gameTime += Time.fixedDeltaTime;
            DrainPower();
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
        }
        
    }

    void SpawnAsteroids(int currentAmmount = 0)
    {
        for (int i = currentAmmount; i < asteroidsAmmount; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-10, 10), Random.Range(0, 200), 0) + asteroidField.position;
            GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity, asteroidField);
        }
    }

    void SpawnFuel()
    {
        GameObject refill = Instantiate(powerUpFuelPrefab, asteroidField);
    }

    void HandlePowerUpFuel()
    {
        if(isGameStarted){
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

    public bool IsGameStarted()
    {
        return isGameStarted;
    }

    public void StartGame()
    {
        isPlayingIntro = false;
        isGameStarted = true;
    }
}
