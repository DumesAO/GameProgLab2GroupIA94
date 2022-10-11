using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the movement of the player with given input from the input manager
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float lookSpeed = 60f;
    public float moveSpeed = 2f;
    public float jumpPower = 8f;
    public float gravity = 9.91f;
    public float jumpTimeLeniency = 0.1f;
    float timeToStopBeingLenient = 0;
    public List<GameObject> disableWhileDead;

    private CharacterController controller;
    

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
        controller = GetComponent<CharacterController>();
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
        ProcessMovement();
        ProcessRotation();
    }
    Vector3 moveDirection;
    void ProcessMovement()
    {
        float leftRightInput = Input.GetAxis("Horizontal");
        float forwardBackwardInput = Input.GetAxis("Vertical");
        bool jumpPressed = Input.GetButtonDown("Jump");

        if (controller.isGrounded)
        {
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
        float horizontalLookInput = Input.GetAxis("Mouse X");
        Vector3 playerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(playerRotation.x, playerRotation.y + horizontalLookInput * lookSpeed * Time.deltaTime, playerRotation.z));
    }

}
