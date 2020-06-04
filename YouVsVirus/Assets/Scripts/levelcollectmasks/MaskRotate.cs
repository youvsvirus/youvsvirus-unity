using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskRotate : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 rotateVec = new Vector3(0, 0, 45);
        transform.Rotate(rotateVec * Time.deltaTime);
    }
}
