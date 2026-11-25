using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "New Unit Config",  menuName = "SO/Unit Config")]
public class UnitCfg : EntityCfg
{
    [BoxGroup("Unit")] public int strengthValue = 1;
    [BoxGroup("Unit")] public int dmg = 1;
    [BoxGroup("Unit")] public float attackIntervalDuration = 1f;
    [BoxGroup("Unit")] public float maxAttackRange = 1f;
}