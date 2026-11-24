using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;


[DefaultExecutionOrder(-999)]
public class InputReader_RTS : MonoBehaviour, IA_RTS.IPlayerActions
{
    [Required, SerializeField] CamMovement cam;
    IA_RTS ia;
    private Vector2 moveInput;

    // public void OnScroll(InputAction.CallbackContext context)
    // {
    //     var scrollVal = context.ReadValue<Vector2>();
    //     if(scrollVal.y > 0) receiver.ScrollUpMoveRight(true);
    //     else if(scrollVal.y < 0) receiver.ScrollUpMoveLeft(true);
    // }

    private void Awake()
    {
        ia = new IA_RTS();
        ia.Player.SetCallbacks(this);
        ia.Enable();
    }

    private void Update()
    {
        if (moveInput.sqrMagnitude > 0f)
            cam.Move(moveInput);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    

    public void OnLook(InputAction.CallbackContext context)
    {
        
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        var scrollVal = context.ReadValue<Vector2>(); 
        if(scrollVal.y > 0) cam.ZoomFwd();
        else if(scrollVal.y < 0) cam.ZoomBack();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        
    }
}