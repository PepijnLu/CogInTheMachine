using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStair : MonoBehaviour
{
    public MovingStair nextStair, firstStair;
    public bool isResetLoopStair, isFirstStair;
    float extendValue;
    float maxExtendRange = 5.5f;
    float extendSpeed = 1;
    [SerializeField] bool isExtending, isActive;
    bool nextStepStarted;
    Vector3 startPos;
    AudioSource doorOpenSfx;
    bool audioPlayed;
    // Start is called before the first frame update
    void Start()
    {
        isExtending = true;
        startPos = transform.position;

        if (isFirstStair)
        {
            isActive = true;
        }

        doorOpenSfx = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isActive)
        {
            //StartCoroutine(ExtendAndRetract());
            ExtendAndRetract();
        }
    }

    void ExtendAndRetract()
    {
        Debug.Log("isExtending: " + isExtending);
        float randomNextExtend = Random.Range(1f, maxExtendRange - 1f);
        if (isExtending)
        {
            if (!audioPlayed)
            {
                doorOpenSfx.Play();
                audioPlayed = true;
            }
            transform.Translate(-1 * extendSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x >= startPos.x + randomNextExtend)
            {
                if (nextStair != null && !nextStepStarted)
                {
                    Debug.Log("extend next stair: " + nextStair.gameObject.name);
                    nextStair.isActive = true;
                    nextStepStarted = true;
                }
                if (isResetLoopStair)
                {
                    firstStair.isActive = true;
                }
            }
            if (transform.position.x >= startPos.x + maxExtendRange)
            {
                if (doorOpenSfx.isPlaying)
                {
                    doorOpenSfx.Stop();
                    audioPlayed = false;
                }
                float fullyExtendedTime = Random.Range(1f, 3.5f);
                StartCoroutine(FullyExtendedTimer(fullyExtendedTime));
                //float duration = 0f;
                // while (duration < fullyExtendedTime)
                // {
                //     duration += Time.deltaTime;
                //     yield return null;
                // }
                isActive = false;
                isExtending = false;
            }
        }
        else
        {
            if (!audioPlayed)
            {
                doorOpenSfx.Play();
                audioPlayed = true;
            }
            transform.Translate(1  * extendSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x <= startPos.x)
            {
                if (doorOpenSfx.isPlaying)
                {
                    doorOpenSfx.Stop();
                    audioPlayed = false;
                }
                isActive = false;
                isExtending = true;
                nextStepStarted = false;
            }
        }

    }

    IEnumerator FullyExtendedTimer(float timerLength)
    {
        yield return new WaitForSeconds(timerLength);
        isActive = true;
    }
}
