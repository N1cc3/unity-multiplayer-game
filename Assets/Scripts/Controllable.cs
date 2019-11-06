using UnityEngine;
using Mirror;

public abstract class Controllable : NetworkBehaviour {
	protected Controls controls;
	protected NetworkIdentity player;

	public struct Controls {
		public readonly float forward;
		public readonly float side;
		public readonly float mouseX;
		public readonly float mouseY;
		public readonly bool jump;
		public readonly bool crouch;
		public readonly bool fire1;

		public Controls(float forward, float side, float mouseX, float mouseY, bool jump, bool crouch, bool fire1) {
			this.forward = forward;
			this.side = side;
			this.mouseX = mouseX;
			this.mouseY = mouseY;
			this.jump = jump;
			this.crouch = crouch;
			this.fire1 = fire1;
		}
	}

	public override void OnStartClient() {
		base.OnStartClient();
		if (isServer) return;
		var rb = GetComponent<Rigidbody>();
		if (!rb) return;
		rb.isKinematic = true;
	}

	public void SetControls(Controls newControls) {
		controls = newControls;
	}

	public void ResetControls() {
		controls = new Controls();
	}

	public void SetPlayer(GameObject playerObj) {
		player = playerObj.GetComponent<NetworkIdentity>();
	}

	public abstract void SetCamera(Camera followCamera);
}
