using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the movement of the player with given input from the input manager
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float lookSpeed = 60f;
    public float jumpPower = 8f;
    public float gravity = 9.91f;
    public float jumpTimeLeniency = 0.1f;
    float timeToStopBeingLenient = 0;
    public Shooter playerShooter;
    public Health playerHealth;
    public GameManager gameManager;
    public List<GameObject> disableWhileDead;
    bool doubleJumpAvaible= false;

    private CharacterController controller;
    private InputManager inputManager;
    

    /// <summary>
    /// Description:
    /// Standard Unity function called once before the first Update call
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Start()
    {
        SetUpCharacterController();
        SetUpInputManager();
    }

    private void SetUpCharacterController() {
        controller = GetComponent<CharacterController>();
        if (controller==null) {
            Debug.LogError("player control not ");
        }
    }

    private void SetUpInputManager()
    {
        inputManager = InputManager.instance;
    }

    /// <summary>
    /// Description:
    /// Standard Unity function called once every frame
    /// Input:
    /// none
    /// Return:
    /// void (no return)
    /// </summary>
    void Update()
    {
        if (playerHealth.currentHealth<=0)
        {
            foreach (GameObject inGameObject in disableWhileDead)
            {
                inGameObject.SetActive(false);
            }
            return;
        }
        else
        {
            foreach (GameObject inGameObject in disableWhileDead)
            {
                inGameObject.SetActive(true);
            }
        }

        ProcessMovement();
        ProcessRotation();
        MorbCheck();
    }
    Vector3 moveDirection;
    void ProcessMovement()
    {
        float leftRightInput = inputManager.horizontalMoveAxis;
        float forwardBackwardInput = inputManager.verticalMoveAxis;
        bool jumpPressed = inputManager.jumpPressed;

        if (controller.isGrounded)
        {
            doubleJumpAvaible = true;
            timeToStopBeingLenient = Time.time + jumpTimeLeniency;
            moveDirection = new Vector3(leftRightInput, 0, forwardBackwardInput);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection =moveDirection* moveSpeed;

            if (jumpPressed)
            {
                moveDirection.y = jumpPower;
            }
        }
        else
        {

            moveDirection = new Vector3(leftRightInput * moveSpeed, moveDirection.y, forwardBackwardInput * moveSpeed);
            moveDirection=transform.TransformDirection(moveDirection);
            if(jumpPressed && Time.time < timeToStopBeingLenient)
            {
                moveDirection.y = jumpPower;
            }
            else if(jumpPressed && doubleJumpAvaible)
            {
                moveDirection.y = jumpPower;
                doubleJumpAvaible = false;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        if (controller.isGrounded&&moveDirection.y<0)
        {
            moveDirection.y = -0.3f;

        }
        controller.Move(moveDirection * Time.deltaTime);
    }

    void ProcessRotation()
    {
        float horizontalLookInput = inputManager.horizontalLookAxis;
        Vector3 playerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(playerRotation.x, playerRotation.y + horizontalLookInput * lookSpeed * Time.deltaTime, playerRotation.z));
    }

    void MorbCheck()
    {
        bool morbPressed = inputManager.morbPressed;
        if (morbPressed && GameManager.morbs==100)
        {
            gameManager.StartMorb();
        }
    }
}
