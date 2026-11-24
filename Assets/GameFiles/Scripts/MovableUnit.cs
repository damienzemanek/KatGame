using UnityEngine;
using UnityEngine.AI;


public class MovableUnit : MonoBehaviour, ISelectableCapabaility
{
    public NavMeshAgent agent;
    
    public void OnSelect(Selectable.SelectCtx ctx)
    {
        Mover.Instance.selectedUnit = this;
        Mover.Instance.MoveAvaliable();
    }

    public void OnKeepSelect(Selectable.SelectCtx ctx)
    {
        agent.SetDestination(Mover.Instance.worldPos);
    }

    public void OnUnSelect(Selectable.SelectCtx ctx)
    {
        agent.SetDestination(Mover.Instance.worldPos);
        Mover.Instance.MoveUnavailable();
    }
}
