using System;
using UnityEngine;
using static System.Single;
using static UnityEngine.Space;
using static UnityEngine.Vector3;

public class SpectatorControl : MonoBehaviour, IControllable
{
    public float moveSpeed = 1.0f;
    public float rotateSpeed = 1.0f;

    private TerrainCollider _terrainCollider;

    private GameObject _headquartersHoloPrefab;
    private GameObject _headquartersPrefab;

    private GameObject _hqHolo;
    private GameObject _hq;

    private void Awake()
    {
        _headquartersHoloPrefab = Resources.Load("headquarters_holo") as GameObject;
        _headquartersPrefab = Resources.Load("headquarters") as GameObject;
    }

    private void Start()
    {
        _terrainCollider = Terrain.activeTerrain.GetComponent<TerrainCollider>();
        _hqHolo = Instantiate(_headquartersHoloPrefab);
    }

    private void Update()
    {
        if (_hqHolo == null) return;
        var t = transform;
        _terrainCollider.Raycast(new Ray(t.position, -t.forward), out var hit, PositiveInfinity);
        _hqHolo.transform.position = hit.point;

        if (!Input.GetButtonDown("Fire1")) return;
        _hq = Instantiate(_headquartersPrefab, _hqHolo.transform.position, _hqHolo.transform.rotation);
        Destroy(_hqHolo);
        _hqHolo = null;
    }

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

    public void SetCamera(Camera followCamera)
    {
        var cameraTransform = followCamera.transform;
        cameraTransform.SetParent(transform, false);
        cameraTransform.localPosition = forward;
        cameraTransform.LookAt(transform);
    }
}
