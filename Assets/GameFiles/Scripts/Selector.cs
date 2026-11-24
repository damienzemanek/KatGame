using System;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public enum SelectionState
    {
        None,
        Selected,
        QueuedAction,
    }
    
    public SelectionState selectionState;
    public Selectable currentlySelected;

    void Update()
    {
        throw new NotImplementedException();
    }

    public void PollForSelection()
    {
        Ray 
        
    }

    public void SelectSomething()
    {
        selectionState = SelectionState.Selected;
        
    }
}




public abstract class Selectable : MonoBehaviour
{
    public class SelectContext
    {
        public GameObject selected;
    }

    public SelectContext ctx = new();

    public void Select(Selector selector, out SelectContext ctx)
    {
        ctx = this.ctx;
    }

    void OnValidate()
    {
        ctx.selected ??= gameObject;
    }
}