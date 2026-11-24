using System;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using static Selectable;

public class Selector : MonoBehaviour
{
    public bool HasHovered(out Selectable _currentlyHovered)
        => (_currentlyHovered = currentlyHovered) != null;
    
    public enum SelectionState
    {
        None,
        Selected,
        QueuedAction,
    }
    
    public float selectorRayDist = 100f;
    
    [ReadOnly] public SelectionState selectionState;
    [ReadOnly] public Selectable currentlySelected;
    [ReadOnly] public Selectable currentlyHovered;
    [ReadOnly] public Vector2 mousePos;
    [BoxGroup("Refs")] [Required] public Camera cam;
    [BoxGroup("Refs")] [Required] public InputReader_RTS inputReader;
    
    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        PollForSelection();
    }

    public void PollForSelection()
    {
        currentlyHovered = null;
        Ray rayMouse = cam.ScreenPointToRay(mousePos);
        Debug.DrawRay(rayMouse.origin, rayMouse.direction * selectorRayDist, Color.red);
        if (!Physics.Raycast(rayMouse.origin, rayMouse.direction, out RaycastHit hit, selectorRayDist)) return;
        if (!hit.collider.gameObject.Has(out Selectable hover)) return;
        currentlyHovered = hover;
    }

    public void TrySelectSomething()
    {
        if (!HasHovered(out var hovered))
        {
            if(currentlySelected != null) currentlySelected.UnSelect(this, out _);
            return;
        }
        selectionState = SelectionState.Selected;
        if(currentlySelected != null) currentlySelected.UnSelect(this, out _);
        currentlySelected = hovered;
        currentlySelected.Select(this, out _);
    }
    
    
}