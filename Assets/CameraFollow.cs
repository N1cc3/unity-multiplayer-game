using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public void SetTarget(Transform target)
    {
        gameObject.transform.SetParent(target, false);
        transform.localPosition = target.position + new Vector3(0, 0f, 1f);
        transform.LookAt(target);
    }
}
