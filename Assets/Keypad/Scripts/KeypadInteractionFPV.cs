using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NavKeypad { 
    public class KeypadInteractionFPV : MonoBehaviour
    {
        Player player;
        bool displayingText;
        [SerializeField] TextMeshProUGUI textToDisplay;
        void Start()
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
        private void Update()
        {
            if (player.inKeypadRange)
            {
                // var ray = cam.ScreenPointToRay(Input.mousePosition);
                // Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
                if (Physics.Raycast(Player.instance.ray, out var hit))
                {
                    if (hit.collider.TryGetComponent(out KeypadButton keypadButton))
                    {
                        if (!displayingText)
                        {
                            textToDisplay.text = "[LMB] Use Keypad";
                            displayingText = true;
                        }
                        if (Input.GetMouseButtonDown(0))
                        {
                            keypadButton.PressButton();
                        }
                    }
                    else
                    {
                        if (displayingText)
                        {
                            textToDisplay.text = "";
                            displayingText = false;
                        }
                    }
                }
            }
        }
    }
}

