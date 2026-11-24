using DesignPatterns.CreationalPatterns;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class InspectorUI : Singleton<InspectorUI>
{
    const string k_currInspTxtFormat = "Inspecting: {0}";
    
    [ReadOnly] public Inspectable currentlyInspected;
    [Required] public TextMeshProUGUI txt_currentlyInspectedText;

    public void Inspect(Inspectable _inspected)
    {
        Debug.Log("Inspect");
        currentlyInspected = _inspected;
        txt_currentlyInspectedText.text = string.Format(k_currInspTxtFormat, _inspected.name);
    }

    public void UnInspect()
    {
        Debug.Log("UnInspect");
        currentlyInspected = null;
        txt_currentlyInspectedText.text = "";
    }
}