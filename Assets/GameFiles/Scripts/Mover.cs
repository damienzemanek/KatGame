using System;
using DesignPatterns.CreationalPatterns;
using Sirenix.OdinInspector;
using UnityEngine;

public class Mover : Singleton<Mover>
{
    public bool HasMovableUnitSelected(out MovableUnit _selectedUnit) => (_selectedUnit = selectedUnit) != null;
    
    public Vector3 moveIndicatorPosLoc;

    public float moveRayRange = 100f;
    public LayerMask floorMask;
    [Required] public Camera cam;
    
    [Required] public GameObject moveIndicator;
    [ReadOnly] public MovableUnit selectedUnit;
    [ReadOnly] public Vector3 mousePos;
    

    public void MoveAvaliable()
    {
        moveIndicator.SetActive(true);
    }

    public void MoveUnavailable()
    {
        moveIndicator.SetActive(false);
        selectedUnit = null;
    }

    void Update()
    {
        if (!HasMovableUnitSelected(out _)) return;
        PollForMoving();
    }

    public void PollForMoving()
    {
        Ray rayMouse = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(rayMouse.origin, rayMouse.direction, out RaycastHit hit, moveRayRange, floorMask)) return;
        Debug.DrawLine(rayMouse.origin, hit.point, Color.aquamarine);
    }
}