using System;
using UnityEngine;

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

    public void SetJump(bool jump)
    {
        Jump = jump;
    }

    public void SetCrouch(bool crouch)
    {
        Crouch = crouch;
    }

    public abstract void SetCamera(Camera followCamera);
}
