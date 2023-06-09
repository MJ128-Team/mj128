using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Forces
    private Rigidbody2D rb;
    [SerializeField] private float upSpeed = 0.0f;
    [SerializeField] private float sideSpeed = 0.0f;
    [SerializeField] private float minPosX;
    [SerializeField] private float maxPosX;
    
    // Animation
    [SerializeField] private bool isTilted = false;
    [SerializeField] private float tiltAngle = 10.0f;
    [SerializeField] private float tiltSpeed = 0.1f;
    
    [SerializeField] private bool isPlayingIntro = false;
    private float introTime = 0.0f;

    // Speed Change
    private bool isChangingSpeed = false;
    private float changeSpeedTimer = 0.0f;
    private float changeSpeedDuration = 1.0f;
    private float targetUpSpeed = 0.0f;
    private float lastUpSpeed;

    // Particles
    public ParticleSystem thrusterParticles;

    // Audio
    public AudioSource thrustersSource;
    private bool isPlayingThrusters = false;
    public AudioClip startExplosionClip;
    private bool isExplosionPlayed = false;
    public AudioClip thrustersClip;
    public AudioClip fuelClip;
    public List<AudioClip> crashClips;

    void Start() {
        upSpeed = 0.0f;
        rb = GetComponent<Rigidbody2D>();
    }


    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            other.gameObject.GetComponent<Obstacle>().OnCrash();
            // Trigger Crash Sound
            int randomCrashClip = Random.Range(0, crashClips.Count);
            AudioManager.instance.TriggerSfx(crashClips[randomCrashClip]);
        }
        else if (other.gameObject.CompareTag("Collectable"))
        {
            // Trigger Collect Sound
            AudioManager.instance.TriggerSfx(fuelClip);
            InGameManager.instance.IncreasePower(3.0f);
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Intro"))
        {
            EndIntroTubeAnimation();
        }
    }

    void Update() {

        if (isPlayingIntro)
        {
            UpdateIntroTubeAnimation();
        } 
        else
        {
            if(InGameManager.instance.GetShields() <= 0)
            {
                OnDie();
            }
        }

        HandleSpeedChange();
        AutoMoveUp();

        if (InGameManager.instance.IsGameStarted()){
            if(upSpeed > 0f)
            {
                HandleMovement();
                SmoothTilt(); 
            }
        }
         
    }

    void HandleMovement()
    {       
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * sideSpeed * Time.deltaTime;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPosX, maxPosX), transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * sideSpeed * Time.deltaTime;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPosX, maxPosX), transform.position.y, transform.position.z);
        }
    }

    void HandleSpeedChange()
    { 
        if ( upSpeed != targetUpSpeed)
        {
            changeSpeedTimer += Time.deltaTime;

            float speedChangeTransition = changeSpeedTimer / changeSpeedDuration;
            upSpeed = Mathf.Lerp(lastUpSpeed, targetUpSpeed, speedChangeTransition);
            //thrusterParticles.emissionRate = upSpeed * 10f;

            if(changeSpeedTimer >= changeSpeedDuration)
            {
                upSpeed = targetUpSpeed; // Make sure we reach the target value
                isChangingSpeed = false;
                changeSpeedTimer = 0.0f;
            }

        }
    }

    void AutoMoveUp()
    {
        transform.position += Vector3.up * upSpeed * Time.deltaTime;
        // with rigidbody apply force
        //rb.AddForce(Vector2.up * upSpeed);

    }

    void SmoothTilt()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, tiltAngle), tiltSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), tiltSpeed);
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -tiltAngle), tiltSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), tiltSpeed);
        }
    }

    void OnDie()
    {
        // Trigger Animation/Particle Effect


        // Trigger Sound Effect
        thrustersSource.Stop();
        // thrusterParticles.Stop();
        // thrusterParticles.gameObject.SetActive(false);

        Destroy(gameObject);
    }

    void UpdateIntroTubeAnimation()
    {
        introTime += Time.deltaTime;
        if (introTime < 2.0f)
        {
          // Anything?
        }
        if (introTime > 2.0f && introTime < 4.0f)
        {   
            if (!isExplosionPlayed)
            {
                AudioManager.instance.TriggerSfx(startExplosionClip);
                isExplosionPlayed = true;
            }
            if( !thrustersSource.isPlaying)
            {
                thrustersSource.Play();
            }

            SetUpSpeed(40.0f, 2.0f);
        }
    }

    public void SetUpSpeed(float newValue, float duration)
    {
        lastUpSpeed = upSpeed;
        targetUpSpeed = newValue;
        changeSpeedDuration = duration;
        changeSpeedTimer = 0.0f;
        isChangingSpeed = true;
    }

    public void LaunchTubeAnimation()
    {
        isPlayingIntro = true;
        introTime = 0.0f;
        upSpeed = 0.0f;
    }

    public void EndIntroTubeAnimation()
    {
        SetUpSpeed(10.0f, 2.0f);
        isPlayingIntro = false;
        InGameManager.instance.StartGame();
    }

    public void SetXBoundaries(float posXmin, float posXmax)
    {
        minPosX = posXmin;
        maxPosX = posXmax;
    }

}
