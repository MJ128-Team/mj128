using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float smoothFactor = 0.5f;
    [SerializeField] private Vector3 offset = Vector3.zero;

    void Start()
    {
        player = GameObject.Find("Player").transform; //Should use Tag instead of name
    }

    void LateUpdate() {
        Vector3 finalPosition = player.position + offset;
        finalPosition.x = transform.position.x;
        finalPosition.z = transform.position.z;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, finalPosition , smoothFactor);
        transform.position = smoothedPosition;
    }


}
