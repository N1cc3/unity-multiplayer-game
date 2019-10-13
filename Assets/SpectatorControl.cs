using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.ForceMode;
using static UnityEngine.Space;
using static UnityEngine.Vector3;

public class SpectatorControl : MonoBehaviour, IControllable
{
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 1.0f;

    public void Forward(float amount)
    {
        transform.Translate(-1 * moveSpeed * amount * forward);
    }

    public void Side(float amount)
    {
        transform.Translate(-1 * moveSpeed * amount * right);
    }

    public void MouseX(float amount)
    {
        transform.Rotate(up, rotateSpeed * amount, World);
    }

    public void MouseY(float amount)
    {
        transform.Rotate(right, rotateSpeed * amount);
    }

    public void Jump()
    {
        transform.Translate(moveSpeed * up);
    }

    public void Crouch()
    {
        transform.Translate(moveSpeed * down);
    }
}
