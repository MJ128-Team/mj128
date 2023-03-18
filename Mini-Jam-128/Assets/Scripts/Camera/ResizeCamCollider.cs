using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCamCollider : MonoBehaviour
{
    private BoxCollider2D camCollider;
    [Range(0.0f, 10.0f)]
    [SerializeField] private float sizeOffset = 0;

    void Start()
    {
        Debug.Log("Camera size: " + Camera.main.orthographicSize);
        
        camCollider = GetComponent<BoxCollider2D>();
        camCollider.size = new Vector2(Camera.main.orthographicSize * Camera.main.aspect * 2, Camera.main.orthographicSize * 2);
        camCollider.size += new Vector2(sizeOffset, sizeOffset);
    }

}
