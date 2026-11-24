using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;


[DefaultExecutionOrder(-999)]
public class InputReader_RTS : MonoBehaviour, IA_RTS.IPlayerActions
{
    [Required, SerializeField] CamMovement cam;
    [Required, SerializeField] Selector selector;
    [Required, SerializeField] Mover mover;
    IA_RTS ia;
    Vector2 moveInput;


    // public void OnScroll(InputAction.CallbackContext context)
    // {
    //     var scrollVal = context.ReadValue<Vector2>();
    //     if(scrollVal.y > 0) receiver.ScrollUpMoveRight(true);
    //     else if(scrollVal.y < 0) receiver.ScrollUpMoveLeft(true);
    // }

    void Awake()
    {
        ia = new IA_RTS();
        ia.Player.SetCallbacks(this);
        ia.Enable();
    }

    void Update()
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

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if(context.performed) selector.TrySelectSomething();
    }

    public void OnMouse(InputAction.CallbackContext context)
    {
        Vector2 pos = context.ReadValue<Vector2>();
        selector.mousePos = pos;
        mover.mousePos = pos;
    }
}