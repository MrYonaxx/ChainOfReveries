using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[System.Serializable]
public class TargetsCamera
{
    public Transform transform;
    public int priority;

    public TargetsCamera(Transform t, int p)
    {
        transform = t;
        priority = p;
    }
}


public class CameraBattleController : MonoBehaviour
{
    [SerializeField]
    List<TargetsCamera> targets = new List<TargetsCamera>();

    [Title("Object Reference")]
    [SerializeField]
    BoxCollider focusLevel;


    [Title("Parameter")]
    [SerializeField]
    Vector3 offset = Vector3.zero;
    [SerializeField]
    float smoothTime = 0.5f;

    // Utiliser par l'animator (c'est un peu bizarre)
    [SerializeField]
    [ReadOnly]
    float bonusSmoothTime = 0f;



    private Vector3 velocity;
    private Vector3 newPos;

    private bool canFocus = true;


    private Bounds cameraView;
    public Bounds CameraView
    {
        get { return cameraView; }
    }


    private Camera cam;
    public Camera Camera
    {
        get { return cam; }
    }


    private void Start()
    {
        cam = GetComponent<Camera>();
    }


    private void LateUpdate()
    {
        MoveCamera();
    }


    //Move camera position smoothly by calculate position of all targets
    void MoveCamera()
    {
        //Calculate centerpoint between all targets to have a center for camera
        Bounds targetsBounds = CalculateNewBoundsEncapsulate();
        Vector3 centerPoint = targetsBounds.center;

        //Calculate camera view and resize zoom to fit the bound of camera view
        Bounds bluePlane = CalculateBoundsCameraView(centerPoint);

        // Clamp the camera view
        Bounds d = focusLevel.bounds;
        float x = 0;
        if (bluePlane.min.x < d.min.x)
            x = d.min.x - bluePlane.min.x;
        else if (bluePlane.max.x > d.max.x)
            x = d.max.x - bluePlane.max.x;

        float y = 0;
        if (bluePlane.min.y < d.min.y)
            y = d.min.y - bluePlane.min.y;
        else if (bluePlane.max.y > d.max.y)
            y = d.max.y - bluePlane.max.y;

        //Calculate new Position for the camera by calculating centerpoint with an offset
        newPos = centerPoint + offset;
        newPos.x += x;
        newPos.y += y;
        newPos.z = cam.transform.position.z;

        cameraView = bluePlane;
        cameraView.center += new Vector3(x, y);

        //Change transform position smoothly without jitter from new Pos vector we got.
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, Mathf.Max(0, smoothTime + bonusSmoothTime));
    }


    Bounds CalculateNewBoundsEncapsulate()
    {
        // int c'est plus léger
        List<int> finalTargets = new List<int>();

        // Check highest camera priority
        int bestPriority = -9999;
        for (int i = 0; i < targets.Count; i++)
        {
            if(targets[i].priority > bestPriority)
            {
                bestPriority = targets[i].priority;
                finalTargets.Clear();
                finalTargets.Add(i);
            }
            else if (targets[i].priority == bestPriority)
            {
                finalTargets.Add(i);
            }
        }

        if (finalTargets.Count == 0)
            return new Bounds();

        Bounds bounds = new Bounds(targets[finalTargets[0]].transform.position, Vector3.zero);

        //Encapsule all targets in the bounds
        for (int i = 1; i < finalTargets.Count; i++)
        {
            bounds.Encapsulate(targets[finalTargets[i]].transform.position);
        }
        return bounds;

    }

    private Bounds CalculateBoundsCameraView(Vector3 center)
    {
        float frustumHeight = 2f * cam.orthographicSize;
        if (frustumHeight >= focusLevel.size.y)
        {
            frustumHeight = focusLevel.size.y;
        }
        float frustumWidth = frustumHeight * cam.aspect;
        return new Bounds(center, new Vector3(frustumWidth, frustumHeight));
    }







    public void AddTarget(Transform t, int priority)
    {
        targets.Add(new TargetsCamera(t, priority));
    }


    public void ModifyTargetPriority(Transform t, int newPriority)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if(targets[i].transform == t)
            {
                targets[i].priority = newPriority;
                return;
            }
        }
    }

    public void RemoveTarget(Transform t)
    {
        for (int i = targets.Count-1; i >= 0; i--)
        {
            if (targets[i].transform == t)
            {
                targets.RemoveAt(i);
                return;
            }
        }
    }






#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
        if (focusLevel == null)
            return;

        Bounds CameraWire = CalculateNewBoundsEncapsulate();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(CameraWire.center, CameraWire.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(newPos, CalculateBoundsCameraView(CameraWire.center).size);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(CameraWire.center, 0.3f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(focusLevel.transform.position.x - focusLevel.center.x, focusLevel.transform.position.y + focusLevel.center.y), focusLevel.size);
        Gizmos.color = Color.magenta;
        /*if (DebugArea)
        {
            if (cam == null)
                cam = GetComponent<Camera>();

            Bounds CameraWire = CalculateNewBoundsEncapsulate();
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(CameraWire.center, CameraWire.size);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(newPos, CalculateBoundsCameraView(CameraWire.center).size);
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(CameraWire.center, 0.3f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector3(focusLevel.transform.position.x - focusLevel.center.x, focusLevel.transform.position.y + focusLevel.center.y), focusLevel.size);
            Gizmos.color = Color.magenta;

        }*/
    }
#endif

   /* public void ChangeFocusState()
    {
        canFocus = !canFocus;
    }*/











}