using UnityEngine;

public class Ownable : MonoBehaviour
{
    public string ownerName;

    public int strengthValue;

    public enum OwnableType
    {
        None,
        Unit,
        Building
    }

}
