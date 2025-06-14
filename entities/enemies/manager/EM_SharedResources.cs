using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class EM_SharedResources(EnemiesManager manager) : EM_BaseComponent(manager)
{
    private readonly Dictionary<float, CircleShape2D> _registeredShapes = [];

    public CircleShape2D RegisterCircleShape(float radius)
    {
        if (!_registeredShapes.ContainsKey(radius))
        {
            _registeredShapes.Add(radius, new CircleShape2D() { Radius = radius });

            _manager.DebugTryUpdateField("registered_count", _registeredShapes.Keys.Count.ToString());
            _manager.DebugTryUpdateField("registered_radii", _registeredShapes.Keys.ToArray().Join(", "));
        }
        return _registeredShapes[radius];
    }

    public void UnregisterShape(float radius)
    {
        if (!_registeredShapes.ContainsKey(radius)) { throw new NotImplementedException(); }
        _registeredShapes.Remove(radius);

        _manager.DebugTryUpdateField("registered_count", _registeredShapes.Keys.Count.ToString());
        _manager.DebugTryUpdateField("registered_radii", _registeredShapes.Keys.ToArray().Join(", "));
    }

    public CircleShape2D GetShape(float radius)
    {
        if (!_registeredShapes.ContainsKey(radius)) { throw new NotImplementedException(); }
        return _registeredShapes[radius];
    }
}