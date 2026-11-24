using UnityEngine;

public class Inspectable : MonoBehaviour, ISelectableCapabaility
{
    public void OnSelect(Selectable.SelectCtx ctx)
    {
        InspectorUI.Instance.Inspect(this);
    }

    public void OnKeepSelect(Selectable.SelectCtx ctx)
    {
        
    }

    public void OnUnSelect(Selectable.SelectCtx ctx)
    {
        InspectorUI.Instance.UnInspect();
    }
}
