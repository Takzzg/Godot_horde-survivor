using System;
using System.Collections.Generic;
using Godot;

public partial class EnemyManager : Node2D
{
    public List<BasicEnemy> EnemiesList = [];

    public int SharedHitBoxSize;
    public int SharedHurtBoxSize;
    public CircleShape2D SharedHitBox;
    public CircleShape2D SharedHurtBox;
    public Texture2D SharedTexture;
    public Timer Timer;

    public EnemyManager(int size, Texture2D texture)
    {
        TextureFilter = TextureFilterEnum.Nearest;

        SharedTexture = texture;
        SharedHitBoxSize = size;
        SharedHurtBoxSize = size + 4;

        SharedHitBox = new CircleShape2D() { Radius = SharedHitBoxSize };
        SharedHurtBox = new CircleShape2D() { Radius = SharedHurtBoxSize };

        Timer = new Timer() { Autostart = false, OneShot = false, WaitTime = 0.25 };
        Timer.Timeout += SpawnEnemy;
        AddChild(Timer);
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveEnemies();
    }

    public void SpawnEnemy()
    {
        // get position
        Vector2 pos = Utils.GetRandomPointOnCircle(GameManager.Instance.MainGame.Player.Position, GameManager.RENDER_DISTANCE / 2);
        Transform2D posTransform = new(0, pos);

        BasicEnemy enemy = new()
        {
            Position = pos,
            Health = 50,
            Speed = 25,
            Damage = 5,
            SpriteRid = RenderingServer.CanvasItemCreate(),
            BodyRid = PhysicsServer2D.BodyCreate(),
            HurtboxRid = PhysicsServer2D.AreaCreate(),
        };

        // create body
        PhysicsServer2D.BodyAddShape(enemy.BodyRid, SharedHitBox.GetRid(), Transform2D.Identity);
        PhysicsServer2D.BodySetSpace(enemy.BodyRid, GetWorld2D().Space);
        PhysicsServer2D.BodySetState(enemy.BodyRid, PhysicsServer2D.BodyState.Transform, posTransform);
        PhysicsServer2D.BodySetMode(enemy.BodyRid, PhysicsServer2D.BodyMode.RigidLinear);
        PhysicsServer2D.BodySetParam(enemy.BodyRid, PhysicsServer2D.BodyParameter.GravityScale, 0);
        PhysicsServer2D.BodySetCollisionLayer(enemy.BodyRid, 2);
        PhysicsServer2D.BodySetCollisionMask(enemy.BodyRid, 3);

        // create hurtbox
        PhysicsServer2D.AreaAddShape(enemy.HurtboxRid, SharedHurtBox.GetRid(), Transform2D.Identity);
        PhysicsServer2D.AreaSetTransform(enemy.HurtboxRid, posTransform);
        PhysicsServer2D.AreaSetSpace(enemy.HurtboxRid, GetWorld2D().Space);
        PhysicsServer2D.AreaSetCollisionLayer(enemy.HurtboxRid, 4);
        PhysicsServer2D.AreaSetCollisionMask(enemy.HurtboxRid, 8);
        PhysicsServer2D.AreaSetMonitorable(enemy.HurtboxRid, true);

        // create sprite
        Vector2 textureSize = SharedTexture.GetSize();
        RenderingServer.CanvasItemSetParent(enemy.SpriteRid, GetCanvasItem());
        RenderingServer.CanvasItemAddTextureRect(enemy.SpriteRid, new Rect2(-textureSize / 2, textureSize), SharedTexture.GetRid());
        RenderingServer.CanvasItemSetDefaultTextureFilter(enemy.SpriteRid, RenderingServer.CanvasItemTextureFilter.Nearest);
        RenderingServer.CanvasItemSetTransform(enemy.SpriteRid, posTransform);

        // // create debug shapes
        // RenderingServer.CanvasItemAddCircle(enemy.SpriteRid, Vector2.Zero, SharedHitBoxSize, new Color(Colors.Blue, 0.125f));
        // RenderingServer.CanvasItemAddCircle(enemy.SpriteRid, Vector2.Zero, SharedHurtBoxSize, new Color(Colors.Pink, 0.125f));

        GD.Print($"spawning enemy at {pos}");
        EnemiesList.Add(enemy);
    }

    public void MoveEnemies()
    {
        Vector2 playerPos = GameManager.Instance.MainGame.Player.Position;

        for (int i = 0; i < EnemiesList.Count; i++)
        {
            BasicEnemy enemy = EnemiesList[i];
            Vector2 dir = enemy.Position.DirectionTo(playerPos).Normalized() * enemy.Speed;

            PhysicsServer2D.BodySetState(enemy.BodyRid, PhysicsServer2D.BodyState.LinearVelocity, dir);

            Transform2D posTransform = (Transform2D)PhysicsServer2D.BodyGetState(enemy.BodyRid, PhysicsServer2D.BodyState.Transform);
            PhysicsServer2D.AreaSetTransform(enemy.HurtboxRid, posTransform);
            RenderingServer.CanvasItemSetTransform(enemy.SpriteRid, posTransform);

            enemy.Position = posTransform.Origin;
        }
    }

    public BasicEnemy FindEnemyByAreaRid(Rid areaRid)
    {
        foreach (BasicEnemy enemy in EnemiesList) { if (enemy.HurtboxRid.Equals(areaRid)) return enemy; }
        throw new Exception($"no enemy found with HurtBoxRid = {areaRid}");
    }

    public void EnemyReceiveDamage(BasicEnemy enemy, int damage)
    {
        enemy.Health -= damage;
        if (enemy.Health > 0) return;
        DestroyEnemy(enemy);
    }

    public void DestroyEnemy(BasicEnemy enemy)
    {
        PhysicsServer2D.FreeRid(enemy.BodyRid);
        PhysicsServer2D.FreeRid(enemy.HurtboxRid);

        RenderingServer.FreeRid(enemy.SpriteRid);

        EnemiesList.Remove(enemy);
    }
}