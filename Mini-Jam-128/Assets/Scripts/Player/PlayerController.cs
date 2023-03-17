using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Forces
    [SerializeField] private float upSpeed = 5f;
    [SerializeField] private float sideSpeed = 5f;

    // Animation
    [SerializeField] private bool isTilted = false;
    [SerializeField] private float tiltAngle = 10.0f;
    [SerializeField] private float tiltSpeed = 0.1f;

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

    void Update() {

        if(InGameManager.instance.GetShields() <= 0)
        {
          Debug.Log("No shields");
            OnDie();
        }

        AutoMoveUp();
        HandleMovement();
        SmoothTilt();  
    }

    void HandleMovement()
    {
        // Movement
        // if (Input.GetKey(KeyCode.W))
        // {
        //     transform.position += Vector3.up * upSpeed * Time.deltaTime;
        // }
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
}
