using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
    private Controllable _controllable;
    private Transform _spawnPoint;

    private GameObject _spectatorPrefab;
    private GameObject _mediumTransportPrefab;
    private GameObject _spectator;
    private bool _inSpectatorMode;

    private void Awake()
    {
        _spectatorPrefab = Resources.Load("spectator") as GameObject;
        _mediumTransportPrefab = Resources.Load("medium_transport") as GameObject;
    }

    public override void OnStartLocalPlayer()
    {
        _spectator = Instantiate(_spectatorPrefab);
        SetControlledObject(_spectator);
        _spawnPoint = GameObject.FindWithTag("Respawn").transform;
    }

    private void SetControlledObject(GameObject obj)
    {
        _controllable = obj.GetComponent<Controllable>();
        _inSpectatorMode = obj == _spectator;
        _controllable.SetCamera(Camera.main);
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetButtonDown("Cancel")) SpectatorMode();
        if (Input.GetButtonDown("Spawn1")) Spawn1();
        if (Input.GetButtonDown("Spawn2")) Spawn2();

        if (_controllable == null) return;
        _controllable.SetForward(Input.GetAxis("Vertical"));
        _controllable.SetSide(Input.GetAxis("Horizontal"));
        _controllable.SetMouseX(Input.GetAxis("Mouse X"));
        _controllable.SetMouseY(Input.GetAxis("Mouse Y"));
        _controllable.SetJump(Input.GetButton("Jump"));
        _controllable.SetCrouch(Input.GetButton("Crouch"));
    }

    private void SpectatorMode()
    {
        var cameraT = Camera.main.transform;
        var cameraPosition = cameraT.position;
        _spectator.transform.position = cameraPosition;
        _spectator.transform.LookAt(cameraT.rotation * -Vector3.forward + cameraPosition);
        SetControlledObject(_spectator);
    }

    private void Spawn1()
    {
        var mediumTransport = Instantiate(_mediumTransportPrefab, _spawnPoint.position, _spawnPoint.rotation);
        SetControlledObject(mediumTransport);
    }

    private void Spawn2()
    {
        if (!_inSpectatorMode) return;
        _spectator.GetComponent<SpectatorControl>().BuildTurret();
    }
}
