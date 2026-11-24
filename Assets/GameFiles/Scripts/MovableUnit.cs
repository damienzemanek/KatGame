using UnityEngine;


public class MovableUnit : MonoBehaviour, ISelectableCapabaility
{
    public void OnSelect(Selectable.SelectCtx ctx)
    {
        Mover.Instance.selectedUnit = this;
        Mover.Instance.MoveAvaliable();
    }

    public void OnUnSelect(Selectable.SelectCtx ctx)
    {
        Mover.Instance.MoveUnavailable();
    }
}
