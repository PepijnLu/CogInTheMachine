using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevShit : MonoBehaviour
{
    public GameObject hallwayDoor, roomDoor, wakeUpText;
    [SerializeField] Light pointLight;
    public static DevShit instance;
    public bool gameStarted;
    [SerializeField] RectTransform blinkUp, blinkDown;


    public float moveDistance = 32;
    public float moveDuration = 15; // Duration in seconds
    public float rotationDuration = 2f; // Duration of the rotation in seconds
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float startTime, blinkingTime;
    int timesBlinked;
    bool moving, moved, isRotating, rotated, doneBlinking, doorSoundPlayed;

    public Transform roomDoorTarget; // The target object to rotate around
    public float rotationSpeed = 20f; // Rotation speed in degrees per second

    float elapsedTime = 0f;
    float initialRotation = 0f;

    float lightDecreaseStep, doorMoveStep;
    [SerializeField] AudioSource doorOpenSfx, firstRoomDoorOpenSfx;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        float totalFrameDuration = moveDuration / Time.deltaTime;

        Debug.Log("total frame duration: " + totalFrameDuration);
        Debug.Log("Time.deltaTime: " + Time.deltaTime);

        lightDecreaseStep = pointLight.intensity / totalFrameDuration;
        doorMoveStep = moveDistance / totalFrameDuration;

        Debug.Log("light descrease step: " + lightDecreaseStep);
        Debug.Log("door move step: " + doorMoveStep);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !gameStarted)
        {
            gameStarted = true;
            StartCoroutine(Blink());
            wakeUpText.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (!rotated)
        {
            RotateDoor();
        }
        if (!moved)
        {
            MoveDoor();
        }
        // if (!doneBlinking && gameStarted)
        // {
        //     Blinking();
        // }
    }

    // void Blinking()
    // {
    //     if (timesBlinked == 0)
    //     {
    //         blinkingTime += Time.deltaTime;
    //         float duration =  0.0625f;

    //         //Open eyes first time
    //         if (elapsedTime <= duration)
    //         {
    //             blinkDown.anchoredPosition += new Vector2(0f, -48);
    //             blinkUp.anchoredPosition += new Vector2(0f, 48);
    //             blinkingTime += 0.02f;
    //         }
    //         else
    //         {
    //             timesBlinked++;
    //             blinkingTime = 0;
    //         }
    //     }

    //     //Close eyes first time
    //      if (timesBlinked == 1)
    //     {
    //         blinkingTime += Time.deltaTime;
    //         float duration =  0.0125f;

    //         //Open eyes first time
    //         if (elapsedTime <= duration)
    //         {
    //             blinkDown.anchoredPosition += new Vector2(0f, 24);
    //             blinkUp.anchoredPosition += new Vector2(0f, -24);
    //             blinkingTime += 0.02f;
    //         }
    //         else
    //         {
    //             timesBlinked++;
    //             blinkingTime = 0;
    //         }
    //     }
    // }
    IEnumerator Blink()
    {
        float elapsedTime = 0;
        float duration =  0.125f;

        //Open eyes first time
        while (elapsedTime <= duration)
        {
            blinkDown.anchoredPosition += new Vector2(0f, -20.25f);
            blinkUp.anchoredPosition += new Vector2(0f, 20.25f);
            elapsedTime += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        //Close eyes first time
        elapsedTime = 0;
        duration = 0.125f;
        while (elapsedTime <= duration)
        {
            blinkDown.anchoredPosition += new Vector2(0f, 20.25f);
            blinkUp.anchoredPosition += new Vector2(0f, -20.25f);
            elapsedTime += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.15f);
        elapsedTime = 0;
        duration = 0.125f;
        //Open eyes second time
        while (elapsedTime <= duration)
        {
            blinkDown.anchoredPosition += new Vector2(0f, -20.25f);
            blinkUp.anchoredPosition += new Vector2(0f, 20.25f);
            elapsedTime += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        //Close eyes second time
        elapsedTime = 0;
        duration = 0.125f;
        while (elapsedTime <= duration)
        {
            blinkDown.anchoredPosition += new Vector2(0f, 20.25f);
            blinkUp.anchoredPosition += new Vector2(0f, -20.25f);
            elapsedTime += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.3f);
        elapsedTime = 0;
        duration = 1f;
        //Open eyes third time
        while (elapsedTime <= duration)
        {
            blinkDown.anchoredPosition += new Vector2(0f, -2.25f);
            blinkUp.anchoredPosition += new Vector2(0f, 2.25f);
            elapsedTime += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
    }

    void RotateDoor()
    {
        float angle = 2f;
        if (isRotating)
        {
            if (!doorSoundPlayed)
            {
                firstRoomDoorOpenSfx.Play();
                doorSoundPlayed = true;
            }

            if (elapsedTime < rotationDuration)
            {
                float currentAngle = Mathf.Lerp(initialRotation, angle, elapsedTime / rotationDuration);
                roomDoor.transform.RotateAround(roomDoorTarget.position, Vector3.down, currentAngle - transform.eulerAngles.y);
                elapsedTime += Time.deltaTime;
            }
            else if (isRotating)
            {
                roomDoor.transform.RotateAround(roomDoorTarget.position, Vector3.down, angle - transform.eulerAngles.y);
                isRotating = false;
                rotated = true;
            }
        }
    }

    void MoveDoor()
    {
        float timePassed = 0;
        if (moving)
        {
            timePassed += 0.01f;
            hallwayDoor.transform.position += new Vector3(0f, 0f, doorMoveStep);
            pointLight.intensity -= lightDecreaseStep;

            if (timePassed >= (moveDuration / 2))
            {
                moving = false;
                moved = true;
                pointLight.intensity = 0f;
                doorOpenSfx.Stop();
            }
        }
    }

    public void StartRotating()
    {
        isRotating = true;
    }

    public void StartMoving()
    {
        startPosition = hallwayDoor.transform.position;
        targetPosition = startPosition + new Vector3(0, 0, moveDistance);
        startTime = Time.time;
        moving = true;
        doorOpenSfx.Play();
        StartCoroutine(AudioManager.instance.AudioFadeOut(AudioManager.instance.audioSources["hallwayMusic"], 2f));
        StartCoroutine(AudioManager.instance.AudioFadeIn(AudioManager.instance.audioSources["stairRoomMusic"], 2f));
        StartCoroutine(SetInBigRoom());
        AudioManager.instance.PlaySound(AudioManager.instance.audioSources["stairRoomMusic"]);
    }

    IEnumerator SetInBigRoom()
    {
        yield return new WaitForSeconds(2f);
        Player.instance.inBigRoom = true;
    }
}
