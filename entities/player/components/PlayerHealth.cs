using System.Linq;
using Godot;
using Godot.Collections;

public partial class PlayerHealth : Node
{
    PlayerScene _player;

    public bool Alive = true;
    public int Health = 50;
    public double InvulnerableLength = 0.25;
    public Timer InvulnerableTimer;

    [Signal]
    public delegate void PlayerReceiveDamageEventHandler(int amount);
    [Signal]
    public delegate void PlayerDeathEventHandler();

    public PlayerHealth(PlayerScene player)
    {
        _player = player;

        // create invulnerability timer
        InvulnerableTimer = new Timer() { Autostart = false, OneShot = true, WaitTime = InvulnerableLength };
        AddChild(InvulnerableTimer);

        // bind signals
        PlayerReceiveDamage += OnReceiveDamage;
        PlayerDeath += OnPlayerDeath;
    }

    private void OnReceiveDamage(int amount)
    {
        if (!Alive) return;

        // GD.Print($"player received {amount} damage");
        Health -= amount;
        if (Health < 0) Health = 0;

        if (Health > 0) return;
        EmitSignal(SignalName.PlayerDeath);
    }

    private void OnPlayerDeath()
    {
        GD.Print($"PlayerScene OnPlayerDeath()");

        Alive = false;
        _player.PlayerMovement.SetPhysicsProcess(false);

        Array<Node> weapons = _player.WeaponsContainer.GetChildren();
        foreach (ProjectileWeapon weapon in weapons.Cast<ProjectileWeapon>()) { weapon.WeaponShooting.TimedAttackSetRunning(false); }
    }

    public void OnCollision(Dictionary collision)
    {
        GD.Print($"InvulnerableTimer.IsStopped(): {InvulnerableTimer.IsStopped()}");
        if (!InvulnerableTimer.IsStopped()) return;

        BasicEnemy enemy = GameManager.Instance.EnemiesManager.FindEnemyByBodyRid((Rid)collision["rid"]);
        EmitSignal(SignalName.PlayerReceiveDamage, enemy.Damage);
        InvulnerableTimer.Start();
    }
}