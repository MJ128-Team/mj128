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
    }

    void Update()
    {
        if(shields <= 0)
        {
            isGameStarted = false;
            // Game Over
            Debug.Log("GAME OVER");
        }
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
        } 
        else if( power < 0)
        {
            power = 0;
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
    }

    public float GetShields()
    {
        return shields;
    }

    public void IncreaseShields()
    {
        shields++;
    }

    public void DecreaseShields()
    {
        shields--;
    }

    public void InstaDeath()
    {
        shields = 0;
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
