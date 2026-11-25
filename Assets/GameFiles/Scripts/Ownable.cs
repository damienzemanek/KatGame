using System;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class Ownable : MonoBehaviour
{
    public enum OwnableType
    {
        None,
        Unit,
        Building
    }
    
    public string ownerName;
    [Required] public UnitCfg cfg;
    [Required] public Entity associatedEntity;
    [ReadOnly] public OwnableType ownableType;

    private void Awake() => associatedEntity ??= this.Get<Entity>();
}
