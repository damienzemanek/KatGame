using System;
using System.Collections.Generic;
using System.Linq;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Region : MonoBehaviour
{
    const string k_regionOwnedNameFormat = "Owner: {0}";
    
    public string ownerName;
    public string regionName;
    public float captureThreshold = 100f;
    
    [Required] public TextMeshPro txt_regionName;
    [Required] public TextMeshPro txt_ownerName;   
    [Required] public MeshRenderer colorRenderer;
    [ReadOnly] public float captureProgress;
    [ReadOnly] public string capturingOwner;
    [ReadOnly] public bool isOwned = false;
    
    [SerializeReference] public List<Army> armies;

    [Serializable]
    public class Army
    {
        public string ownerName;
        [ShowInInspector] public int totalStrength => ownables.Sum(o => o.strengthValue);
        public List<Ownable> ownables = new();
    }

    void Start()
    {
        StopHovered();
        if(!isOwned) colorRenderer.gameObject.SetActive(false);
    }

    public void Hovered()
    {
        txt_regionName.gameObject.SetActive(true);
        txt_regionName.text = regionName;
        txt_ownerName.gameObject.SetActive(true);
        txt_ownerName.text = (isOwned) ? string.Format(k_regionOwnedNameFormat, ownerName) : "Not Owned";
    }

    public void StopHovered()
    {
        txt_regionName.gameObject.SetActive(false);
        txt_ownerName.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (armies == null || armies.Count == 0)
        {
            capturingOwner = null;
            captureProgress = 0f;
            return;
        }

        var rankedArmies = armies
            .OrderByDescending(a => a.totalStrength)
            .ToList();

        Army strongest = rankedArmies[0];
        if (isOwned && strongest.ownerName == ownerName)
        {
            captureProgress = 0;
            return;
        }

        int secondStrength = rankedArmies.Count > 1
            ? rankedArmies[1].totalStrength
            : 0;

        float pressure = strongest.totalStrength - secondStrength;
        if (pressure <= 0) return;

        // A different faction has taken the lead.
        if (capturingOwner != strongest.ownerName)
        {
            capturingOwner = strongest.ownerName;
            captureProgress = 0f;
        }

        captureProgress += pressure * Time.fixedDeltaTime;

        // Capture completed.
        if (captureProgress >= captureThreshold) Capture();
    }

    public void Capture()
    {
        ownerName = capturingOwner;
        isOwned = true;
        colorRenderer.gameObject.SetActive(true);
        colorRenderer.material = PersistentConfiguration.Instance.players[ownerName].playerMaterial;

        capturingOwner = null;
        captureProgress = 0f;
        txt_ownerName.text = (isOwned) ? string.Format(k_regionOwnedNameFormat, ownerName) : "Not Owned";
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.Has(out Ownable ownable)) return;
        var ownerArmy = armies.FirstOrDefault(a => a.ownerName == ownable.ownerName);
        if (ownerArmy == null)
        {
            armies ??= new List<Army>();
            var army = new Army();
            army.ownerName = ownable.ownerName;
            armies.Add(army);
            army.ownables.Add(ownable);
            return;
        }
        ownerArmy.ownables.Add(ownable);
    }
    
    void OnTriggerExit(Collider other)
    {
        if (!other.Has(out Ownable ownable)) return;
        var ownerArmy = armies.FirstOrDefault(a => a.ownerName == ownable.ownerName);
        if (ownerArmy == null) return;
        ownerArmy.ownables.Remove(ownable);
        if (ownerArmy.ownables.Count == 0)
            armies.Remove(ownerArmy);
    }

}
