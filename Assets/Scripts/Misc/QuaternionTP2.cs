using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionTP2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 v = new Vector3(5, 2, 3);

        Quaternion q = new Quaternion(0, 1, 0, 1);
        q.Normalize();
        Quaternion q2 = new Quaternion(1, 0, 1, 0);
        q2.Normalize();

        Debug.Log(q * v);
        Debug.Log(q2 * v);

        Matrix4x4 mat = Matrix4x4.TRS(new Vector3(5, 2, 3), q2, Vector3.one);
        Debug.Log(mat);
    }

    
}
