using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingHolo : MonoBehaviour
{
    private static readonly Color Green = new Color(0, 0.2f, 0);
    private static readonly Color Red = new Color(0.2f, 0, 0);
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    public MeshRenderer holo;
    public BoxCollider area;

    private Game _game;

    private IEnumerable<Unbuildable> _blockers;

    private void Start()
    {
        _game = FindObjectOfType<Game>();
    }

    private void Update()
    {
        _blockers = _game.Unbuildables.Where(unbuildable =>
            unbuildable.area.bounds.Intersects(area.bounds)
        );
        holo.material.SetColor(EmissionColor, _blockers.Any() ? Red : Green);
    }

    public bool CanBeBuilt()
    {
        return !_blockers.Any();
    }
}
