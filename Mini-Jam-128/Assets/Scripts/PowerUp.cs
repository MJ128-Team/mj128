using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private Camera mainCam;
    private float camWidth;
    private float camHeight;

    [SerializeField] private string kind; // TODO: Enum
    [SerializeField] private float value = 10.0f; // in seconds 

    //Spawn  
    [SerializeField] private float spawnOffsetY = 0.0f;  

    void Start()
    {
        mainCam = Camera.main;
        camHeight = mainCam.orthographicSize;
        camWidth = camHeight * mainCam.aspect;

        RandomPosition();
    }

    public void OnCollect()
    {
        if(kind == "fuel")
        {
          InGameManager.instance.IncreasePower(value);
          // Efecto de sonido
          // Efecto de particulas
          Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "MainCamera")
        {
            Destroy(gameObject);
        }
    }

    void RandomPosition()
    {
        float spawnPosX = Random.Range(-camWidth * 0.8f, camWidth * 0.8f);
        float spawnPosY = Random.Range(-camHeight, camHeight);
        transform.position = new Vector3(spawnPosX, spawnPosY + mainCam.transform.position.y + spawnOffsetY, 0) ;
    }
}
