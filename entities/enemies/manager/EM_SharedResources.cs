using System;
using System.Collections.Generic;
using Godot;

public partial class EM_SharedResources(EnemiesManager manager) : EM_BaseComponent(manager)
{
    private readonly Dictionary<float, CircleShape2D> _registeredShapes = [];

    public CircleShape2D RegisterCircleShape(float radius)
    {
        if (!_registeredShapes.ContainsKey(radius))
        {
            _registeredShapes.Add(radius, new CircleShape2D() { Radius = radius });
            DebugTryUpdateField("registered_count", _registeredShapes.Keys.Count.ToString());
        }
        return _registeredShapes[radius];
    }

    public void UnregisterShape(float radius)
    {
        if (!_registeredShapes.ContainsKey(radius)) { throw new NotImplementedException(); }
        _registeredShapes.Remove(radius);
        DebugTryUpdateField("registered_count", _registeredShapes.Keys.Count.ToString());
    }

    public CircleShape2D GetShape(float radius)
    {
        if (!_registeredShapes.ContainsKey(radius)) { throw new NotImplementedException(); }
        return _registeredShapes[radius];
    }

    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("EM shared resources");
        category.CreateLabelField("registered_count", "Count", _registeredShapes.Keys.Count.ToString());
        return category;
    }
}