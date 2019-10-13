using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.ForceMode;

public class PlayerControl : NetworkBehaviour
{
    private IControllable _controllable;

    public override void OnStartLocalPlayer()
    {
        var spectatorPrefab = Resources.Load("Spectator");
        var spectator = Instantiate(spectatorPrefab) as GameObject;
        SetControlledObject(spectator);
    }

    public void SetControlledObject(GameObject obj)
    {
        _controllable = obj.GetComponent<IControllable>();
        Camera.main.GetComponent<CameraFollow>().SetTarget(obj.transform);
    }

    private void Update()
    {
        if (!isLocalPlayer || _controllable == null) return;
        _controllable.Forward(Input.GetAxis("Vertical"));
        _controllable.Side(Input.GetAxis("Horizontal"));
        _controllable.MouseX(Input.GetAxis("Mouse X"));
        _controllable.MouseY(Input.GetAxis("Mouse Y"));
        if (Input.GetButton("Jump")) _controllable.Jump();
        if (Input.GetButton("Crouch")) _controllable.Crouch();
    }
}
