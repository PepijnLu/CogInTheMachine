using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace NavKeypad
{
    public class Keypad : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent onAccessGranted;
        [SerializeField] private UnityEvent onAccessDenied;
        [Header("Combination Code (9 Numbers Max)")]
        [SerializeField] private List<int> keypadCombo = new List<int>();

        public UnityEvent OnAccessGranted => onAccessGranted;
        public UnityEvent OnAccessDenied => onAccessDenied;

        [Header("Settings")]
        [SerializeField] private string accessGrantedText = "Granted";
        [SerializeField] private string accessDeniedText = "Denied";

        [Header("Visuals")]
        [SerializeField] private float displayResultTime = 1f;
        [Range(0, 5)]
        [SerializeField] private float screenIntensity = 2.5f;
        [Header("Colors")]
        [SerializeField] private Color screenNormalColor = new Color(0.98f, 0.50f, 0.032f, 1f); //orangy
        [SerializeField] private Color screenDeniedColor = new Color(1f, 0f, 0f, 1f); //red
        [SerializeField] private Color screenGrantedColor = new Color(0f, 0.62f, 0.07f); //greenish
        [Header("SoundFx")]
        [SerializeField] private AudioClip buttonClickedSfx;
        [SerializeField] private AudioClip accessDeniedSfx;
        [SerializeField] private AudioClip accessGrantedSfx;
        [Header("Component References")]
        [SerializeField] private Renderer panelMesh;
        [SerializeField] private TMP_Text keypadDisplayText;
        [SerializeField] private AudioSource audioSource;


        private List<int> currentInput = new List<int>();
        private bool displayingResult = false;
        private bool accessWasGranted = false;

        private void Awake()
        {
            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);
        }

        void Start()
        {

        }


        //Gets value from pressedbutton
        public void AddInput(int input)
        {
            audioSource.PlayOneShot(buttonClickedSfx);
            if (displayingResult || accessWasGranted) return;
            switch (input)
            {
                case 10:
                    CheckCombo();
                    break;
                default:
                    if (currentInput != null && currentInput.Count == 4) // 9 max passcode size 
                    {
                        return;
                    }
                    currentInput.Add(input);
                    string newInput = "";
                    foreach(int textInput in currentInput)
                    {
                        newInput = newInput + textInput.ToString();
                    }
                    keypadDisplayText.text = newInput;
                    break;
            }

        }
        public void CheckCombo()
        {
            // if (int.TryParse(currentInput, out var currentKombo))
            // {
            //     bool granted = currentKombo == keypadCombo;
            //     if (!displayingResult)
            //     {
            //         StartCoroutine(DisplayResultRoutine(granted));
            //     }
            // }
            // else
            // {
            //     Debug.LogWarning("Couldn't process input for some reason..");
            // }
            int numbersCorrect = 0;
            bool granted = false;
            List<int> codeToCheck = new List<int>();
            foreach(int textInput in keypadCombo)
            {
                codeToCheck.Add(textInput);
            }
            foreach(int textInput in currentInput)
            {
                if (codeToCheck.Contains(textInput))
                {
                    codeToCheck.Remove(textInput);
                    numbersCorrect++;
                }
            }
            
            if (numbersCorrect == keypadCombo.Count)
            {
                granted = true;
            }   

            if (!displayingResult)
            {
                StartCoroutine(DisplayResultRoutine(granted));
            }
        }

        //mainly for animations 
        private IEnumerator DisplayResultRoutine(bool granted)
        {
            displayingResult = true;

            if (granted) AccessGranted();
            else AccessDenied();

            yield return new WaitForSeconds(displayResultTime);
            displayingResult = false;
            if (granted) yield break;
            ClearInput();
            panelMesh.material.SetVector("_EmissionColor", screenNormalColor * screenIntensity);

        }

        private void AccessDenied()
        {
            keypadDisplayText.text = accessDeniedText;
            onAccessDenied?.Invoke();
            panelMesh.material.SetVector("_EmissionColor", screenDeniedColor * screenIntensity);
            audioSource.PlayOneShot(accessDeniedSfx);
        }

        private void ClearInput()
        {
            currentInput.Clear();
            keypadDisplayText.text = "";
        }

        private void AccessGranted()
        {
            accessWasGranted = true;
            keypadDisplayText.text = accessGrantedText;
            onAccessGranted?.Invoke();
            panelMesh.material.SetVector("_EmissionColor", screenGrantedColor * screenIntensity);
            audioSource.PlayOneShot(accessGrantedSfx);
        }

    }
}