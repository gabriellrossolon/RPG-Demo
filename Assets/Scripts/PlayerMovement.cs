using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController charControll;
    private Transform cameraPos;
    private Animator anim;

    private float utilizedSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    private Vector2 moveInput;
    private bool sprintInput;
    private bool isMoving;
    void Start()
    {
        charControll = GetComponent<CharacterController>();
        cameraPos = Camera.main.transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        MovementControll();
    }

    //Essa função é chamada pelo Player Input (InputSystem)
    public void OnMove(InputAction.CallbackContext context)
    {
        //Quando o Player Input chama essa função, ele devolve um Vector2,com base nos botões clicados (W A S D), que fica atribuido a moveInput
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();

        sprintInput = value > 0.5f;
    }

    private void MovementControll()
    {
        if(moveInput == Vector2.zero)
        {
            utilizedSpeed = 0;
        }

        //Atribuimos o valor de X e Y a movement, que veio la no Player Input.
        Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);

        //O movimento do personagem fica relativo a posição e rotação da camera.
        movement = cameraPos.TransformDirection(movement);
        movement.y = 0;

        //Acessamos o CharacterController e dizemos que o seu .Move é o valor dado pelo Input System (W A S D) * nossa moveSpeed
        charControll.Move(movement * Time.deltaTime * utilizedSpeed);
        //Executamos o .Move novamente, mas desta vez, tratando apenas da gravidade. (-9.81f é um valor de gravidade padrão)
        charControll.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime);

        //Model olha pra direção que esta andando
        if(movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * 10);
        }

        isMoving = movement != Vector3.zero;
        anim.SetFloat("speed", utilizedSpeed);

        //SPRINT CONTROLL
        if (sprintInput)
        {
            utilizedSpeed = sprintSpeed;
        }
        else
        {
            utilizedSpeed = walkSpeed;
        }
    }
}
