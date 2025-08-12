using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class DoorknobInteract : MonoBehaviour
{
    Player player;
    public TextMeshProUGUI turnOffRadioText;
    bool displayingText;
    [SerializeField] AudioSource radioSwitchSound;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    private void Update()
    {
        if (player.inDoorknobRange)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Player.instance.ray, out var hit))
                {
                    if (hit.collider.TryGetComponent(out DoorknobInteract doorknob))
                    {
                        if (!displayingText)
                        {
                            StartCoroutine(DisplayHintText());
                        }
                    }
                }
            }

        }
    }

    IEnumerator DisplayHintText()
    {
        displayingText = true;
        player.hintText.text = "The door is locked. Try using the keypad instead.";
        yield return new WaitForSeconds(4);
        player.hintText.text = "";
        displayingText = false;
    }
}
