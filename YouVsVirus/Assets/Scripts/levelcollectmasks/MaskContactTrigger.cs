using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class MaskContactTrigger : MonoBehaviour
    {      
        void OnTriggerEnter(Collider other)
        {
            Debug.Log ("Something collided with a mask");
        }
    }
}