using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Presses : MonoBehaviour
{
    [SerializeField] float extendSpeed, contractSpeed, extendDuration, contractDuration, pauseTimeExtended, pauseTimeContracted;
    [SerializeField] bool pressToClunk;
    public AudioSource clunk, retract;
    bool clunked, retractSoundPlayed;
    float elapsedTime;
    void FixedUpdate()
    {
        if (Player.instance.inBigRoom)
        {
            Move();
        }
    }

    void Move()
    {
        elapsedTime += Time.deltaTime;
        
        if (elapsedTime < extendDuration)
        {
            transform.Translate(0f, 0f, -extendSpeed * Time.deltaTime);
        }
        if ((elapsedTime > (extendDuration - 1f)) && pressToClunk && !clunked && Player.instance.inBigRoom)
        {
            Debug.Log("CLUNK");
            clunk.Play();
            clunked = true;
        }
        if (elapsedTime > (extendDuration + pauseTimeExtended))
        {
            if (elapsedTime < (extendDuration + pauseTimeExtended + contractDuration))
            {
                if (!retractSoundPlayed && Player.instance.inBigRoom)
                {
                    retract.Play();
                    retractSoundPlayed = true;
                }
                transform.Translate(0f, 0f, contractSpeed * Time.deltaTime);
            }
            else if (elapsedTime > (extendDuration + pauseTimeExtended + contractDuration + pauseTimeContracted))
            {
                elapsedTime = 0;
                clunked = false;
                retractSoundPlayed = false;
            }
        }
    }
}
