using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwayingCam : MonoBehaviour
{
    [SerializeField] float rotationSpeed, rotationTime, timeBetweenCodes, codeShowedForTime;
    [SerializeField] Transform originalTransform, codeTransform;
    [SerializeField] TextMeshProUGUI codeText;
    [SerializeField] List<int> codeAsList = new List<int>();
    [SerializeField] List<Transform> cameraLocations = new List<Transform>();
    float time, codeTime, codeShowTime;
    int numbersShowed = 0;
    bool shouldBeRotating, prePlayed, setupDone;

    void Awake()
    {

    }
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (DevShit.instance.gameStarted && !setupDone)
        {
            transform.position = cameraLocations[3].position;
            shouldBeRotating = true;
            setupDone = true;
            codeTime = -15;
        }
        if (shouldBeRotating)
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
            time += Time.deltaTime;
            codeTime += Time.deltaTime;
        }

        if (time > rotationTime)
        {
            rotationSpeed *= -1;
            time = 0;
        }

        if ((codeTime > (timeBetweenCodes - 0.2f)) && !prePlayed)
        {
            AudioManager.instance.PlaySound(AudioManager.instance.audioSources["tvStaticSfx"]);
            prePlayed = true;
        }

        if (codeTime > timeBetweenCodes)
        {
            ShowACode(numbersShowed);
        }
    }

    void ShowACode(int i)
    {
        if (codeText.text != codeAsList[i].ToString())
        {
            shouldBeRotating = false;
            codeText.text = codeAsList[i].ToString();
            transform.position = codeTransform.position;
            transform.rotation = codeTransform.rotation;
            AudioManager.instance.audioSources["birdsSfx"].Pause();
        }
        codeShowTime += Time.deltaTime;
        if (codeShowTime > codeShowedForTime)
        {
            codeTime = 0;
            codeShowTime = 0;
            numbersShowed++;
            shouldBeRotating = true;
            prePlayed = false;
            if (numbersShowed == 4)
            {
                numbersShowed = 0;
            }
            AudioManager.instance.audioSources["birdsSfx"].UnPause();
            AudioManager.instance.audioSources["tvStaticSfx"].Stop();
            transform.position = cameraLocations[i].position;
            transform.rotation = cameraLocations[i].rotation;
        }
    }
    
}
