using Godot;
using System;

public partial class BasicWeapon : Node2D
{
    [Export]
    public Timer Timer;
    [Export]
    public Node2D BulletOrigin;
    [Export]
    public int BulletOriginDistance;
    [Export]
    private PackedScene _basicBulletScene;

    public override void _Ready()
    {
        Timer.Timeout += Shoot;
        BulletOrigin.Position = BulletOrigin.Position with { X = BulletOriginDistance };
    }

    public void Shoot()
    {
        // rotate gun
        Rotate(GameManager.Instance.RNG.RandfRange(0, 360));
        // create bullet
        BasicBullet bullet = _basicBulletScene.Instantiate<BasicBullet>();

        // move bullet to origin
        bullet.Rotation = Rotation;
        bullet.Position = BulletOrigin.GlobalPosition;
        bullet.TopLevel = true;

        // add bullet to scene
        AddChild(bullet);
    }
}
