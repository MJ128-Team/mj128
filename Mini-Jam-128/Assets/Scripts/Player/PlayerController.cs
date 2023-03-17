using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Forces
    [SerializeField] private float upSpeed = 5f;
    [SerializeField] private float sideSpeed = 5f;

    void Update() {
        HandleMovement();
        AutoMoveUp();
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

    void AutoMoveUp() {
        transform.position += Vector3.up * upSpeed * Time.deltaTime;
    }
}
