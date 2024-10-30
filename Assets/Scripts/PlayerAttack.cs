using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject swordHand;
    private Animator anim;

    private bool attackInput;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        AttackControll();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Canceled)
        {
            attackInput = true;
        }
    }

    private void AttackControll()
    {
        if (attackInput && !IsAttacking())
        {
            attackInput = false;
            anim.SetTrigger("isAttack");
        }

        if (IsAttacking())
        {
            attackInput = false;
        }
    }

    private bool IsAttacking()
    {
        // Acessa o estado atual do layer base do Animator
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // Verifica se a animação atual é "attackone" e se ainda não terminou
        return stateInfo.IsName("Attack_one") && stateInfo.normalizedTime < 1f;
    }
}
