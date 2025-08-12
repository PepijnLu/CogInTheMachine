using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class RadioInteract : MonoBehaviour
{
    Player player;
    public TextMeshProUGUI turnOffRadioText;
    bool isOn, displayingText;
    [SerializeField] AudioSource radioSwitchSound;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        isOn = true;
    }
    private void Update()
    {
        if (player.inRadioRange)
        {
            // var ray = cam.ScreenPointToRay(Input.mousePosition);
            // Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

            if (Physics.Raycast(Player.instance.ray, out var hit))
            {
                if (hit.collider.TryGetComponent(out RadioInteract radio))
                {
                    if (isOn)
                    {
                        if (!displayingText)
                        {
                            turnOffRadioText.text = "Turn Off";
                            displayingText = true;
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            AudioManager.instance.audioSources["radioSfx"].Pause();
                            isOn = false;
                            turnOffRadioText.text = "Turn On";
                            radioSwitchSound.Play();
                            Debug.Log("turn off radio");
                            displayingText = true;
                        }
                    }
                    else
                    {
                        turnOffRadioText.text = "Turn On";
                        displayingText = true;
                        turnOffRadioText.gameObject.SetActive(true);
                        if (Input.GetMouseButtonDown(0))
                        {
                            radioSwitchSound.Play();
                            AudioManager.instance.audioSources["radioSfx"].UnPause();
                            isOn = true;
                            turnOffRadioText.text = "Turn Off";
                            displayingText = true;
                        }
                    }
                }
                else
                {
                    if (displayingText)
                    {
                        turnOffRadioText.text = "";
                        displayingText = false;
                    }
                }
            }

        }
        else
        {
            if (displayingText)
            {
                turnOffRadioText.text = "";
                displayingText = false;
            }
        }
    }
}
