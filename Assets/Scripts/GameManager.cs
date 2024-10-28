using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Trava o cursor no centro da tela
        Cursor.visible = false; // Torna o cursor invisível
    }
}
