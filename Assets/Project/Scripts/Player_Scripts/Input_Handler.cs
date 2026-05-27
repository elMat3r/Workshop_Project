using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Handler : MonoBehaviour
{
    public void OnJump(InputValue value)
    {
        if (value.isPressed && Horda_Manager.Instance != null)
        {
            Horda_Manager.Instance.CommandJump();
        }
    }
    public void OnCrouch(InputValue value)
    {
        if (value.isPressed && Horda_Manager.Instance != null)
        {
            Horda_Manager.Instance.CommandDash();
        }
    }
}
