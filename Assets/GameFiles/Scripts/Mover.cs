using System;
using DesignPatterns.CreationalPatterns;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

public class Mover : Singleton<Mover>
{
    public bool HasMovableUnitSelected(out MovableUnit _selectedUnit) => (_selectedUnit = selectedUnit) != null;

    public float indicatorYLock = 0.1f;
    public float moveRayRange = 100f;
    public LayerMask floorMask;
    [Required] public Camera cam;
    
    [Required] public GameObject moveIndicator;
    [ReadOnly] public MovableUnit selectedUnit;
    [ReadOnly] public Vector3 mousePos;
    [ReadOnly] public Vector3 worldPos;
    

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
        Ray rayMouse = cam.ScreenPointToRay(mousePos);
        if (!Physics.Raycast(rayMouse.origin, rayMouse.direction, out RaycastHit hit, moveRayRange, floorMask)) return;
        Debug.DrawLine(rayMouse.origin, hit.point, Color.aquamarine);
        worldPos = new Vector3(hit.point.x, indicatorYLock, hit.point.z);
        moveIndicator.transform.position = worldPos;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(worldPos, 1f);
    }
}