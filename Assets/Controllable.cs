using System;
using UnityEngine;

public abstract class Controllable : MonoBehaviour {
	public float forward;
	public float side;
	public float mouseX;
	public float mouseY;
	public bool jump;
	public bool crouch;
	public bool fire1;

	public void ResetControls() {
		forward = 0.0f;
		side = 0.0f;
		mouseX = 0.0f;
		mouseY = 0.0f;
		jump = false;
		crouch = false;
		fire1 = false;
	}

	public abstract void SetCamera(Camera followCamera);
}
