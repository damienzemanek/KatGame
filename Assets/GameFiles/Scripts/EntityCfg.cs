using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class EntityCfg : ScriptableObject
{
    [BoxGroup("Entity")] public string entityName;
    [BoxGroup("Entity")] public int maxHealth = 5;
}