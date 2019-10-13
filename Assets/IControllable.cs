using UnityEngine;

public interface IControllable
{
    void Forward(float amount);
    void Side(float amount);
    void MouseX(float amount);
    void MouseY(float amount);
    void Jump();
    void Crouch();

    void SetCamera(Camera followCamera);
}
