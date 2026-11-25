using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [BoxGroup("Entity")] [ReadOnly] public int currentHealth;
    [BoxGroup("Entity")] [ReadOnly] public LayerMask entityMask;
    public Action OnDie;

    public virtual void TakeDmg(int am, Entity attackedBy)
    {
        currentHealth -= am;
        if (am > 0) return;
        OnDie?.Invoke();
    }
}