using System.Linq;
using Godot;
using Godot.Collections;

public partial class PlayerHealth : BasePlayerComponent
{
    public bool Alive = true;
    public int Health = 50;
    public int MaxHealth = 50;
    public double InvulnerableLength = 0.25;
    public Timer InvulnerableTimer;

    [Signal]
    public delegate void PlayerReceiveDamageEventHandler(int amount);
    [Signal]
    public delegate void PlayerDeathEventHandler();

    public PlayerHealth(PlayerScene player) : base(player)
    {
        // create invulnerability timer
        InvulnerableTimer = new Timer() { Autostart = false, OneShot = true, WaitTime = InvulnerableLength };
        AddChild(InvulnerableTimer);
    }

    private void ReceiveDamage(int amount)
    {
        if (!Alive) return;

        // GD.Print($"player received {amount} damage");
        Health -= amount;
        _player.PlayerStats.IncreaseDamageReceived(amount);

        if (Health <= 0)
        {
            PlayerDie();
            return;
        }

        InvulnerableTimer.Start();
        EmitSignal(SignalName.PlayerReceiveDamage, amount);
    }

    private void PlayerDie()
    {
        GD.Print($"PlayerScene OnPlayerDeath()");

        Alive = false;
        _player.PlayerMovement.SetPhysicsProcess(false);

        Array<Node> weapons = _player.WeaponsContainer.GetChildren();
        foreach (ProjectileWeapon weapon in weapons.Cast<ProjectileWeapon>()) { weapon.WeaponShooting.TimedAttackSetRunning(false); }

        EmitSignal(SignalName.PlayerDeath);
    }

    public void OnCollision(Dictionary collision)
    {
        if (!InvulnerableTimer.IsStopped()) return;

        BasicEnemy enemy = GameManager.Instance.EnemiesManager.FindEnemyByBodyRid((Rid)collision["rid"]);
        ReceiveDamage(enemy.Damage);
    }
}