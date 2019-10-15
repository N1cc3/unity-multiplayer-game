using System;
using UnityEngine;

/**
 * Use protected field values as controls.
 * Call ResetInputs after fields have been used.
 */
public abstract class Controllable : MonoBehaviour
{
    protected float Forward { get; private set; }
    protected float Side { get; private set; }
    protected float MouseX { get; private set; }
    protected float MouseY { get; private set; }
    protected bool Jump { get; private set; }
    protected bool Crouch { get; private set; }

    public void SetForward(float amount)
    {
        Forward = amount;
    }

    public void SetSide(float amount)
    {
        Side = amount;
    }

    public void SetMouseX(float amount)
    {
        MouseX = amount;
    }

    public void SetMouseY(float amount)
    {
        MouseY = amount;
    }

    public void SetJump()
    {
        Jump = true;
    }

    public void SetCrouch()
    {
        Crouch = true;
    }

    protected void ResetInputs()
    {
        Forward = 0.0f;
        Side = 0.0f;
        MouseX = 0.0f;
        MouseY = 0.0f;
        Jump = false;
        Crouch = false;
    }

    public abstract void SetCamera(Camera followCamera);
}
