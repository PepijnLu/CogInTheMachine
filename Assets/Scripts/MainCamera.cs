using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    float elapsedTime = 0;
    [SerializeField] float moveSpeed, rotateSpeed;
    [SerializeField] GameObject otherRooms, cursor, cube, uselessCanvas;
    [SerializeField] Player player;
    bool cutsceneStarted;
    void FixedUpdate()
    {   
        elapsedTime += Time.deltaTime;

        if (elapsedTime > 270 && player.ableToEnd)
        {
            if (!cutsceneStarted)
            {
                StartCutscene();
            }
            MoveInCutscene();
        }
    }

    void StartCutscene()
    {
        player.rb.useGravity = false;
        player.rb.isKinematic = true;
        cutsceneStarted = true;
        otherRooms.SetActive(true);
        cursor.SetActive(false);
        uselessCanvas.SetActive(false);
        transform.SetParent(null);
        transform.Translate(0.1f, 0f, 0f);
        transform.LookAt(Player.instance.gameObject.transform);
        Player.instance.inBigRoom = false;
        DevShit.instance.gameStarted = false;
        StartCoroutine(AudioManager.instance.AudioFadeOut(AudioManager.instance.audioSources["stairRoomMusic"], 2f));
        AudioManager.instance.PlaySound(AudioManager.instance.audioSources["orbitalMusic"]);
        StartCoroutine(AudioManager.instance.AudioFadeIn(AudioManager.instance.audioSources["orbitalMusic"], 2f));
    }

    void MoveInCutscene()
    {
        transform.Translate(0, 0f, -moveSpeed);
        transform.Rotate(0f, 0f, rotateSpeed);
        moveSpeed += Time.deltaTime;
    }   

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "FogToggle")
        {
            RenderSettings.fog = false;
        }
    }
}
