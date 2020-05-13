using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaperRotate : MonoBehaviour
{
    // Update is called once per frame, makes toilet paper rotate
    void Update()
    {
        Vector3 rotateVec = new Vector3(0, 45, 0);
        transform.Rotate(rotateVec * Time.deltaTime);
    }
}

