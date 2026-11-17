using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class CamMovement : MonoBehaviour
{
    [Required] public CharacterController cont;
    [FormerlySerializedAs("camTarg")] [Required] public Transform camZoomTarg;
    
    [BoxGroup("Zoom")] [FormerlySerializedAs("localZMinMax")] public Vector2 zoomLocalZMinMax;
    [BoxGroup("Zoom")] public float smoothing;
    [BoxGroup("Zoom")] [ReadOnly] public Vector3 smoothingVel;
    [BoxGroup("Zoom")] Vector3 orig;
    [BoxGroup("Zoom")] [ShowInInspector, ReadOnly] Vector3 maxBack;
    [BoxGroup("Zoom")] [ShowInInspector, ReadOnly] Vector3 maxFwd;

    [BoxGroup("Move")] Vector3 origPos;
    [BoxGroup("Move")] public float moveSpeedScalar = 5f;
    [BoxGroup("Move")] public Vector2 zoomMoveRemap = new Vector2(0.5f, 3f);
    
    #if UNITY_EDITOR
    private void OnValidate()
    {
        maxBack = new Vector3(orig.x, orig.y, zoomLocalZMinMax.x);
        maxFwd = new Vector3(orig.x,  orig.y, zoomLocalZMinMax.y);
    }
    #endif
    
    private void Awake()
    {
        orig = camZoomTarg.localPosition;
        maxBack = new Vector3(orig.x, orig.y, zoomLocalZMinMax.x);
        maxFwd = new Vector3(orig.x,  orig.y, zoomLocalZMinMax.y);
    }

    public void Move(Vector2 input)
    {
        Vector3 moveDirection = new Vector3(input.x, 0f, input.y);
        float zoomMultiplier = Mathf.Lerp(zoomMoveRemap.y, zoomMoveRemap.x, zoomDiff / 50f);
        cont.Move(moveDirection * ((moveSpeedScalar * zoomMultiplier) * Time.deltaTime));
    }

    public float zoomDiff
    {
        get
        {
            var ret = maxBack.z - camZoomTarg.transform.localPosition.z;
            ret = Mathf.Abs(ret);
            Debug.Log(ret);
            return ret;
        }
    }

    public void ZoomBack()
    {
        camZoomTarg.transform.localPosition = Vector3.SmoothDamp(
            camZoomTarg.transform.localPosition,
            maxBack,
            ref smoothingVel,
            smoothing);
        Debug.Log(zoomDiff);
    }

    public void ZoomFwd()
    {
        camZoomTarg.transform.localPosition = Vector3.SmoothDamp(
            camZoomTarg.transform.localPosition,
            maxFwd,
            ref smoothingVel,
            smoothing);
        Debug.Log(zoomDiff);
    }
}