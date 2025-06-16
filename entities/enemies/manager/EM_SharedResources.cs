using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class EM_SharedResources(EnemiesManager manager) : BaseComponent<EnemiesManager>(manager)
{
    private readonly Dictionary<float, CircleShape2D> _registeredShapes = [];

    public CircleShape2D RegisterCircleShape(float radius)
    {
        if (!_registeredShapes.ContainsKey(radius))
        {
            _registeredShapes.Add(radius, new CircleShape2D() { Radius = radius });

            Parent.DebugTryUpdateField("registered_count", _registeredShapes.Keys.Count.ToString());
            Parent.DebugTryUpdateField("registered_radii", _registeredShapes.Keys.ToArray().Join(", "));
        }
        return _registeredShapes[radius];
    }

    public void UnregisterShape(float radius)
    {
        if (!_registeredShapes.ContainsKey(radius)) { throw new NotImplementedException(); }
        _registeredShapes.Remove(radius);

        Parent.DebugTryUpdateField("registered_count", _registeredShapes.Keys.Count.ToString());
        Parent.DebugTryUpdateField("registered_radii", _registeredShapes.Keys.ToArray().Join(", "));
    }

    public CircleShape2D GetShape(float radius)
    {
        if (!_registeredShapes.ContainsKey(radius)) { throw new NotImplementedException(); }
        return _registeredShapes[radius];
    }

    public override void DebugCreateSubCategory(DebugCategory category)
    {
        category.CreateDivider("Shared resources");
        category.CreateLabelField("registered_count", "Count", _registeredShapes.Count.ToString());
        category.CreateLabelField("registered_radii", "Radii", _registeredShapes.Keys.ToArray().Join(", "));
    }
}