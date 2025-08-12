using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public GameObject lastGroundTouched;
    [SerializeField] float moveSpeed, mouseSensitivity, jumpForce, airSlowDown, runTimerCooldown = 3, timeBeforeBreathe = 1f, timeToBreathe = 0.5f, crouchSpeed, crouchHeight, crouchCenterYAdjustment = -0.5f;
    float runCooldownTime, mouseX, mouseY, moveX,  moveZ, airMoveSlowDown, originalHeight, runTime,footstepRunTimer;
    public Transform playerCamera;
    public GameObject currentInteractable;
    public GameObject breatheIndicator;
    public GameObject forestScene;
    bool isJumpCooldown;
    private float verticalLookRotation;
    public Rigidbody rb;
    public bool isGrounded, onLadder, moveInputting, inKeypadRange, inRadioRange, inDoorknobRange, inBigRoom, ableToEnd;
    public static Player instance;
    public TextMeshProUGUI eToInteractText, hintText;
    Vector3 move, airMovement;
    bool jumped, crouched, stoodUp, isCrouching, runOnCooldown, running;
    [SerializeField] AudioSource footstepSound;
    [SerializeField] Transform lastCheckpoint;
    public Ray ray;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        eToInteractText.gameObject.SetActive(false);
        breatheIndicator.SetActive(false);

        airMoveSlowDown = 1 -  airSlowDown;

        originalHeight = gameObject.transform.localScale.z;
        //originalCenter = characterController.center;
    }

    void Update()
    {
        if (DevShit.instance.gameStarted)
        {
            Inputs();
        }

        if (inKeypadRange || inRadioRange || inDoorknobRange)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        }
    }

    void FixedUpdate()
    {
        if (DevShit.instance.gameStarted)
        {
            Jump();
            Crouch();
            StandUp();
            Run();
        }

        if (!onLadder)
        {
            move = (transform.right * moveX + transform.forward * moveZ).normalized * moveSpeed * Time.deltaTime;
        }
        else
        {
            move = (transform.right * moveX + transform.up * moveZ).normalized * moveSpeed * 3 * Time.deltaTime;
        }
        if (running && !runOnCooldown)
        {
            move *= 2.5f;
        }

        if (isCrouching)
        {
            move *= crouchSpeed;
        }


        if (!isGrounded)
        {
            transform.position += move * airMoveSlowDown + airMovement * airSlowDown;
        }
        else
        {
            transform.position += move;
            if ((move.x != 0) && (move.z != 0))
            {
                PlayFootsteps();
            }
        }
    }

    void Run()
    {
        if (!runOnCooldown)
        {
            if (running)
            {
                runTime += Time.deltaTime;
                if (runTime >= timeBeforeBreathe)
                {
                    if (runTime <= (timeBeforeBreathe + timeToBreathe))
                    {
                        if (!breatheIndicator.activeSelf)
                        {
                            breatheIndicator.SetActive(true);
                        }
                    }
                    else 
                    {
                        breatheIndicator.SetActive(false);
                        runOnCooldown = true;
                        runTime = 0;
                    }
                }
            }
            else
            {
                if (breatheIndicator.activeSelf)
                {
                    breatheIndicator.SetActive(false);
                }
            }
        }
        else
        {
            runCooldownTime += Time.deltaTime;
            if (runCooldownTime >= runTimerCooldown)
            {
                runOnCooldown = false;
                runCooldownTime = 0;
            }
            if (breatheIndicator.activeSelf)
            {
                breatheIndicator.SetActive(false);
            }

        }

        if (!running)
        {
            if (runTime > 0)
            {
                runTime -= Time.deltaTime;
            }
            else if (runTime < 0)
            {
                runTime = 0;
            }
        }
    }

    void Inputs()
    {
        // Handle mouse look
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        playerCamera.localEulerAngles = Vector3.right * verticalLookRotation;


        // Handle movement
        moveX = 0;
        moveZ = 0;

        if (Input.GetKey(KeyCode.W))    {   moveZ += 1;  }
        if (Input.GetKey(KeyCode.S))    {   moveZ += -1;  }
        if (Input.GetKey(KeyCode.A))    {   moveX += -1;  }
        if (Input.GetKey(KeyCode.D))    {   moveX += 1;  }

        if (Mathf.Abs(moveX) + Mathf.Abs(moveZ) == 0)
        {
            moveInputting = false;
        }
        else
        {
            moveInputting = true;
        }

        //Running
        if (isGrounded && Input.GetKey(KeyCode.LeftShift) && moveInputting && !isCrouching)
        {
            running = true;
        }
        else
        {
            running = false;
        }

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded && !isJumpCooldown)
        {
            jumped = true;
        }
        //Handle crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouched = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            stoodUp = true;
        }

        if (runTime >= 1)
        {
            if (runTime <= 1.5)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    breatheIndicator.SetActive(false);
                    runTime = 0;
                }
            }
        }
        else if(!breatheIndicator.activeSelf && runTime > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                runOnCooldown = true;
            }
        }
    }

    void PlayFootsteps()
    {
        footstepRunTimer += Time.deltaTime;

        if (running && !runOnCooldown)
        {
            if (footstepRunTimer >= 0.4f)
            {
                footstepRunTimer = 0;
                footstepSound.Play();

            }
        }
        else
        {
            if (footstepRunTimer >= 1f)
            {
                footstepRunTimer = 0;
                footstepSound.Play();
            }
        }
    }
    void Crouch()
    {
        if (crouched)
        {
            if (isCrouching) return;
            //gameObject.transform.localScale.z = crouchHeight;
            gameObject.transform.localScale -= new Vector3(0f, crouchHeight, 0);
            gameObject.transform.position += new Vector3(0, -crouchCenterYAdjustment, 0);
            //characterController.center = new Vector3(originalCenter.x, originalCenter.y + crouchCenterYAdjustment, originalCenter.z);
            isCrouching = true;
            crouched = false;
        }
    }

    void StandUp()
    {
        if (stoodUp)
        {
            if (!isCrouching) return;
            gameObject.transform.localScale += new Vector3(0f, crouchHeight, 0);
            //characterController.center = originalCenter;
            isCrouching = false;
            stoodUp = false;
        }
    }

    void Jump()
    {
        if (jumped)
        {
            float newJumpForce = jumpForce;
            if (isCrouching)
            {
                newJumpForce *= crouchHeight / originalHeight;
            }
            Debug.Log(newJumpForce);
            rb.AddForce(Vector3.up * newJumpForce, ForceMode.Impulse);
            isJumpCooldown = true;
            StartCoroutine(JumpCooldown());
            jumped = false;
        }
    }

    void DisplayHint(string text, float duration)
    {
        hintText.text = text;
        StartCoroutine(HintTime(duration));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = true;
            lastGroundTouched = collision.gameObject;
            airMovement = new Vector3(0, 0, 0);
            
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = true;
            airMovement = new Vector3(0, 0, 0);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = false;
            airMovement = move;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Ladder")
        {
            onLadder = true;
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.isKinematic = false;
            airMovement = new Vector3(0, 0, 0);
        }

        if (collider.gameObject.tag == "Keypad")
        {
            inKeypadRange = true;
        }
        if (collider.gameObject.tag == "Radio")
        {
            inRadioRange = true;
        }

        if (collider.gameObject.tag == "TV")
        {
            forestScene.SetActive(true);
        }
        if (collider.gameObject.tag == "Doorknob")
        {
            inDoorknobRange = true;
        }

        if (collider.gameObject.tag == "Death")
        {
            transform.position = lastCheckpoint.position;
            transform.rotation = lastCheckpoint.rotation; 
            playerCamera.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (collider.gameObject.tag == "Checkpoint")
        {
            lastCheckpoint.position = collider.transform.position;
            lastCheckpoint.rotation = collider.transform.rotation;
            Debug.Log("checkpoint:" + lastCheckpoint);
        }

        if (collider.gameObject.tag == "BigRoomCheck")
        {
            ableToEnd = true;
        }

        if (collider.gameObject.tag == "Hint")
        {
            Hints hints = collider.gameObject.GetComponent<Hints>();
            if (hintText.text != hints.hint)
            {
                DisplayHint(hints.hint, hints.duration);
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {

        if (!onLadder)
        {
            if (collider.gameObject.tag == "Ladder")
            {
                onLadder = true;
                rb.useGravity = false;
                rb.isKinematic = true;
                rb.isKinematic = false;
                airMovement = new Vector3(0, 0, 0);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Ladder")
        {
            onLadder = false;
            rb.useGravity = true;
            airMovement = move;
        }

        if (collider.gameObject.tag == "Keypad")
        {
            inKeypadRange = false;
        }
        if (collider.gameObject.tag == "Radio")
        {
            inRadioRange = false;
        }

        if (collider.gameObject.tag == "TV")
        {
            forestScene.SetActive(false);
        }
        if (collider.gameObject.tag == "Doorknob")
        {
            inDoorknobRange = false;
        }
    }

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        isJumpCooldown = false;
    }   

    IEnumerator HintTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        hintText.text = "";
    }
}
