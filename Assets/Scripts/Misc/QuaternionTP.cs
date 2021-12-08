using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionTP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(this.transform.rotation.x + " " + this.transform.rotation.y + " " + this.transform.rotation.z + " " + this.transform.rotation.w);
    }

    
}
