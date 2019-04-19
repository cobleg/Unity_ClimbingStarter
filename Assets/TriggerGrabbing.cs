using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGrabbing : MonoBehaviour
{
    public GameObject rootPos;  //anchor position

    void OnTriggerEnter(Collider obj)
    {
        obj.GetComponentInParent<ClimbUp>().GrabEdge(rootPos.transform); //get position of the anchor
    }
}
