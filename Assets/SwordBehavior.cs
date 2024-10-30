using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private MeshCollider meshColl;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }

    private void Start()
    {
        meshColl = GetComponent<MeshCollider>();
    }
    private void Update()
    {
        if (IsAttacking())
        {
            meshColl.enabled = true;
        }
        else
        {
            meshColl.enabled = false;
        }
    }

    private bool IsAttacking()
    {    
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        // Verifica se a animação atual é "attackone" e se ainda não terminou
        return stateInfo.IsName("Attack_one") && stateInfo.normalizedTime > 0.5f;
    }
}
