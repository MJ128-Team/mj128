using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Forces
    [SerializeField] private float upSpeed = 0.0f;
    [SerializeField] private float sideSpeed = 0.0f;

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

    void Start() {
        upSpeed = 0.0f;
    }


    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("colision with obstacle");
            other.gameObject.GetComponent<Obstacle>().OnCrash();
        }
        else if (other.gameObject.CompareTag("Collectable"))
        {
            // Get Obstacle Controller and call its DoEffect() method
              InGameManager.instance.IncreasePower(3.0f);
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Intro"))
        {
            Debug.Log("Escaped Intro");
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
              Debug.Log("No shields");
                OnDie();
            }
        }

        HandleSpeedChange();
        AutoMoveUp();

        if (InGameManager.instance.IsGameStarted()){
            HandleMovement();
            SmoothTilt(); 
        }
         
    }

    void HandleMovement()
    {       
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down * upSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * sideSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * sideSpeed * Time.deltaTime;
        }
    }

    void HandleSpeedChange()
    { 
        if ( upSpeed != targetUpSpeed)
        {
            changeSpeedTimer += Time.deltaTime;

            float speedChangeTransition = changeSpeedTimer / changeSpeedDuration;
            upSpeed = Mathf.Lerp(lastUpSpeed, targetUpSpeed, speedChangeTransition);

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
    }

    void SmoothTilt()
    {
        // Tilt
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, tiltAngle), tiltSpeed);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
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
            SetUpSpeed(40.0f, 2.0f);
            // upSpeed = Mathf.Lerp(0.0f, 30.0f, introTime - 2.0f);
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
        Debug.Log("LaunchTubeAnimation");
        isPlayingIntro = true;
        upSpeed = 0.0f;
    }

    public void EndIntroTubeAnimation()
    {

        // upSpeed = 30.0f;
        SetUpSpeed(25.0f, 2.0f);
        isPlayingIntro = false;
        InGameManager.instance.StartGame();
    }
}
