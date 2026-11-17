using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader_RTS : MonoBehaviour, IA_RTS.IPlayerActions
{
    // public void OnScroll(InputAction.CallbackContext context)
    // {
    //     var scrollVal = context.ReadValue<Vector2>();
    //     if(scrollVal.y > 0) receiver.ScrollUpMoveRight(true);
    //     else if(scrollVal.y < 0) receiver.ScrollUpMoveLeft(true);
    // }
    

    public void OnMove(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        var scrollVal = context.ReadValue<Vector2>();
       // if(scrollVal.y > 0)
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}