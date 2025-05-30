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

    public PlayerHealth(PlayerScene player) : base(player)
    {
        // create invulnerability timer
        InvulnerableTimer = new Timer() { Autostart = false, OneShot = true, WaitTime = InvulnerableLength };
        AddChild(InvulnerableTimer);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!Alive) return;
        _player.PlayerStats.IncreaseTimeAlive(delta);
    }

    private void ReceiveDamage(int amount)
    {
        if (!Alive) return;

        // GD.Print($"player received {amount} damage");
        Health -= amount;

        _player.PlayerStats.IncreaseDamageReceived(amount);
        _player.PlayerUI.GameplayUI.UpdateHealthBar(Health, MaxHealth);

        if (Health <= 0)
        {
            PlayerDie();
            return;
        }

        InvulnerableTimer.Start();

        DebugTryUpdateField("current_health", $"{Health} / {MaxHealth}");
    }

    private void PlayerDie()
    {
        GD.Print($"PlayerScene OnPlayerDeath()");

        Alive = false;
        _player.PlayerMovement.SetPhysicsProcess(false);
        _player.PlayerUI.ShowDeathUI();

        GameManager.Instance.EnemiesManager.OnPlayerDeath();

        Array<Node> weapons = _player.WeaponsContainer.GetChildren();
        foreach (ProjectileWeapon weapon in weapons.Cast<ProjectileWeapon>()) { weapon.OnPlayerDeath(); }

        DebugTryUpdateField("player_alive", Alive ? "Alive" : "Dead");
    }

    public void OnCollision(Dictionary collision)
    {
        if (!InvulnerableTimer.IsStopped()) return;

        BasicEnemy enemy = GameManager.Instance.EnemiesManager.FindEnemyByBodyRid((Rid)collision["rid"]);
        ReceiveDamage(enemy.Damage);
    }

    public override DebugCategory DebugCreateCategory()
    {
        DebugCategory category = new("Player Health");
        category.CreateLabelField("current_health", "HP", $"{Health} / {MaxHealth}");
        category.CreateLabelField("player_alive", "State", Alive ? "Alive" : "Dead");
        return category;
    }
}