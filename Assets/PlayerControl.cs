using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.ForceMode;

public class PlayerControl : NetworkBehaviour
{
    private IControllable _controllable;
    private Transform _spawnPoint;

    private GameObject _spectatorPrefab;
    private GameObject _mediumTransportPrefab;

    private void Awake()
    {
        _spectatorPrefab = Resources.Load("spectator") as GameObject;
        _mediumTransportPrefab = Resources.Load("medium_transport") as GameObject;
    }

    public override void OnStartLocalPlayer()
    {
        var spectator = Instantiate(_spectatorPrefab);
        SetControlledObject(spectator);
        _spawnPoint = GameObject.FindWithTag("Respawn").transform;
    }

    public void SetControlledObject(GameObject obj)
    {
        _controllable = obj.GetComponent<IControllable>();
        _controllable.SetCamera(Camera.main);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Spawn1")) Spawn1();
        if (!isLocalPlayer || _controllable == null) return;
        _controllable.Forward(Input.GetAxis("Vertical"));
        _controllable.Side(Input.GetAxis("Horizontal"));
        _controllable.MouseX(Input.GetAxis("Mouse X"));
        _controllable.MouseY(Input.GetAxis("Mouse Y"));
        if (Input.GetButton("Jump")) _controllable.Jump();
        if (Input.GetButton("Crouch")) _controllable.Crouch();
    }

    private void Spawn1()
    {
        var mediumTransport = Instantiate(_mediumTransportPrefab, _spawnPoint.position, _spawnPoint.rotation);
        SetControlledObject(mediumTransport);
    }
}
