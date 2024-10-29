using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Collections;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController charControll;
    private Transform cameraPos;
    private Animator anim;

    [Header("Movement")]
    private float utilizedSpeed;
    private bool isMoving;
    private float normalHeight = 1.79f;
    private float normalCenterY = 0.85f;

    public float walkSpeed;
    public float sprintSpeed;

    private Vector2 moveInput;
    private bool sprintInput;
    
    [Header("Crouch")]
    private bool crouchInput;
    private bool isCrouched;
    public float crouchSpeed;
    private float crouchHeight = 1.02f;
    private float crouchCenterY = 0.59f;

    [Header("Jump")]
    private bool isGrounded;
    [SerializeField] private Transform playerFeet;
    private float yForce;
    void Start()
    {
        charControll = GetComponent<CharacterController>();
        cameraPos = Camera.main.transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        MovementControll();
        CrouchControll();
        JumpControll();

        SpeedController();
    
        if (yForce > -9.81f)
        {
            yForce += -9.81f * Time.deltaTime;
        }
    }

    private void SpeedController()
    {
        float targetSpeed;

        if (moveInput == Vector2.zero)
        {
            targetSpeed = 0;
        }
        else if (crouchInput)
        {
            targetSpeed = crouchSpeed;
        }
        else if (sprintInput)
        {
            targetSpeed = sprintSpeed;
        }
        else
        {
            targetSpeed = walkSpeed;
        }

        utilizedSpeed = Mathf.Lerp(utilizedSpeed, targetSpeed, 0.1f);

        if (Mathf.Abs(utilizedSpeed - targetSpeed) < 0.01f)
        {
            utilizedSpeed = targetSpeed;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        sprintInput = value > 0.5f;
    }

    private void MovementControll()
    {
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        movement = cameraPos.TransformDirection(movement);
        movement.y = 0;

        charControll.Move(movement * Time.deltaTime * utilizedSpeed);
        charControll.Move(new Vector3(0, yForce, 0) * Time.deltaTime);

        if(movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 10);
        }

        isMoving = movement != Vector3.zero;
        anim.SetFloat("speed", utilizedSpeed);
    }

    //Crouch Controll
    public void OnCrouch(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        crouchInput = value > 0.5f;
    }

    private void CrouchControll()
    {
        anim.SetBool("isCrouch", isCrouched);

        if (crouchInput)
        {
            isCrouched = true;
        }

        if (isCrouched)
        {
            charControll.center = new Vector3(charControll.center.x, crouchCenterY, charControll.center.z);
            charControll.height = crouchHeight;

            float raycastDistance = 1.5f;
            RaycastHit hit;
            if (!crouchInput && !Physics.Raycast(transform.position, Vector3.up, out hit, raycastDistance, ~LayerMask.GetMask("Player")))
            {
                isCrouched = false;
            }
            //Debug.DrawRay(transform.position, Vector3.up * raycastDistance, Color.red);
        }
        else
        {
            charControll.center = new Vector3(charControll.center.x, normalCenterY, charControll.center.z);
            charControll.height = normalHeight;
        }
    }

    //Jump Controll
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            anim.SetTrigger("isJump");
            yForce = 5f;
        }
    }

    private void JumpControll()
    {
        isGrounded = Physics.CheckSphere(playerFeet.position, 0.3f, ~LayerMask.GetMask("Player"));

        anim.SetBool("isGrounded", isGrounded);
    }
   

}
