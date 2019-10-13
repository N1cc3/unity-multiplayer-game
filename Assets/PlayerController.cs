using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float horizontalSpeed = 1.0f;
    public float verticalSpeed = 1.0f;

    public GameObject horizontalTransform;
    public GameObject verticalTransform;

    private void Update()
    {
        var h = horizontalSpeed * Input.GetAxis("Mouse X");
        var v = verticalSpeed * Input.GetAxis("Mouse Y");
        horizontalTransform.transform.Rotate(Vector3.up, h);
        verticalTransform.transform.Rotate(Vector3.right, v);
    }
}
