using System;
using System.Collections.Generic;
using System.Linq;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;


public class Unit : Entity
{
    private const int MAX_ENTITES_IN_RANGE = 24;
    
    public bool HasTarget(out Ownable targ) => (targ = target) != null;
    
    public Targeting targeting = Targeting.Closest;
    [BoxGroup("Unit")] [Required] public NavMeshAgent agent;
    [BoxGroup("Unit")] [Required] public Ownable ownable;
    [BoxGroup("Unit")] [ReadOnly] public List<Ownable> ownablesInRange = new();
    [BoxGroup("Unit")] [ReadOnly] public List<Ownable> enemyOwnablesInRange = new();
    [BoxGroup("Unit")] [ReadOnly] public Ownable target;

    float attackTimer = 0f;
    Collider[] inRange = new Collider[MAX_ENTITES_IN_RANGE];
    List<Ownable> visionBuffer = new();

    public void CheckVision()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            ownable.cfg.maxAttackRange,
            inRange,
            entityMask
        );

        visionBuffer.Clear();

        for (int i = 0; i < count; i++)
        {
            if (!inRange[i].Has(out Ownable o)) continue; // not ownable gc
            if (visionBuffer.Contains(o)) continue; // dbl add gc

            visionBuffer.Add(o); // add to buffer

            if (!ownablesInRange.Contains(o)) // is new? -> enter
                UnitEntered(o);
        }

        for (int i = ownablesInRange.Count - 1; i >= 0; i--)
        {
            var previous = ownablesInRange[i];

            if (!visionBuffer.Contains(previous))
                UnitExited(previous);
        }

        ownablesInRange.Clear();
        ownablesInRange.AddRange(visionBuffer);
    }

    public enum Targeting
    {
        Closest,
        Defend,
        Strongest,
        Weakest
    }

    public void ReTarget()
    {
        switch (targeting)
        {
            case Targeting.Defend: target = null;      return;
            case Targeting.Closest:
                enemyOwnablesInRange.Sort((a, b) =>
                {
                    var distA = (transform.position - a.transform.position).sqrMagnitude;
                    var distB = (transform.position - b.transform.position).sqrMagnitude;
                    return distA.CompareTo(distB);
                });
                break;
            case Targeting.Strongest:
                enemyOwnablesInRange.Sort((a, b) => b.cfg.strengthValue.CompareTo(a.cfg.strengthValue));
                break;

            case Targeting.Weakest:
                enemyOwnablesInRange.Sort((a, b) => a.cfg.strengthValue.CompareTo(b.cfg.strengthValue));
                break;
        }

        if(enemyOwnablesInRange.Count > 0) target = enemyOwnablesInRange[0];
    }

    public void UnitEntered(Ownable ownable)
    {
        if (ownable.ownerName == this.ownable.ownerName) return;
        ownablesInRange.Add(ownable);
        if (ownable.ownerName != this.ownable.ownerName) return;
        enemyOwnablesInRange.Add(ownable);
        ReTarget();
    }

    public void UnitExited(Ownable ownable)
    {
        if (ownable.ownerName == this.ownable.ownerName) return;
        ownablesInRange.Remove(ownable);
        enemyOwnablesInRange.Remove(ownable);
        ReTarget();
    }

    private void FixedUpdate()
    {
        CheckVision();
        if (ownablesInRange.Count <= 0) return;
        if (!HasTarget(out var targ)) return;
        
        // attack timer 
        attackTimer += Time.deltaTime;
        attackTimer = Math.Clamp(attackTimer, attackTimer, ownable.cfg.attackIntervalDuration);
        
        // range check
        var distToTarg = (transform.position - targ.transform.position).sqrMagnitude;
        if (distToTarg < ownable.cfg.maxAttackRange) return; // not in range
        agent.SetDestination(targ.transform.position.With(y: 0)); // set dest to target
        
        if (attackTimer >= ownable.cfg.attackIntervalDuration)
        {
            attackTimer = 0;
            AttackTarget(targ);
        }
    }


    public void AttackTarget(Ownable targ)
    {
        targ.associatedEntity.TakeDmg(ownable.cfg.dmg, this); // actually attack the target
        // sound
        // animation
    }

    public override void TakeDmg(int am, Entity attackedBy)
    {
        base.TakeDmg(am, attackedBy);
        if (targeting != Targeting.Defend) return;          // not defending
        if (attackedBy is not Unit attackedByUnit) return;  
        
        var potentialNewTarget = attackedByUnit.ownable;
        
        if (target != null)
        {
            float distToCurrentTarg = (transform.position - target.transform.position).sqrMagnitude;
            float distToPotentialTarg = (transform.position - potentialNewTarget.transform.position).sqrMagnitude;
            if (distToCurrentTarg >= distToPotentialTarg) return; // can only defend against targets in own range
        }

        target = potentialNewTarget;
    }
}
