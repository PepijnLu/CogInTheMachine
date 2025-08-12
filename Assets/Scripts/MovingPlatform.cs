using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Player player;
    Vector3 currentPos, prevPos, addedMovement, addedMovementInAir;
    bool playerOnIt;
    // Start is called before the first frame update
    void Start()
    {
        currentPos = transform.position;
        prevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        currentPos = transform.position;
        addedMovement = currentPos - prevPos;
        if (player != null)
        {
            if (player.onLadder)
            {
                player = null;
            }
        }
        if (player != null)
        {
            if (playerOnIt)
            {
                player.gameObject.transform.position += addedMovement;
            }
            else
            {
                player.gameObject.transform.position += addedMovementInAir;
            }
            
            if (player.lastGroundTouched != gameObject)
            {
                player = null;
            }
        }
        prevPos = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<Player>();
            playerOnIt = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player = collision.gameObject.GetComponent<Player>();
            playerOnIt = false;
            addedMovementInAir = addedMovement;
        }
    }
}
