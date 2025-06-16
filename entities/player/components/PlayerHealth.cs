using Godot;
using Godot.Collections;

public partial class PlayerHealth : BaseComponent<PlayerScene>
{
    public bool Alive = false;
    private static readonly int _startingHealth = 10;
    public int Health = _startingHealth;
    public int MaxHealth = _startingHealth;

    public double InvulnerableLength = 0.25;
    public Timer InvulnerableTimer;

    public PlayerHealth(PlayerScene player) : base(player)
    {
        // create invulnerability timer
        InvulnerableTimer = new Timer() { Autostart = false, OneShot = true, WaitTime = InvulnerableLength };
        AddChild(InvulnerableTimer);

        Ready += () => Alive = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!Alive) return;
        Parent.PlayerStats.IncreaseTimeAlive(delta);
    }

    private void ReceiveDamage(int amount)
    {
        if (!Alive) return;

        // GD.Print($"player received {amount} damage");
        Health -= amount;

        Parent.PlayerStats.IncreaseDamageReceived(amount);
        Parent.PlayerUI.GameplayUI.UpdateHealthBar(Health, MaxHealth);

        if (Health <= 0)
        {
            PlayerDie();
            return;
        }

        InvulnerableTimer.Start();

        Parent.DebugTryUpdateField("current_health", $"{Health} / {MaxHealth}");
    }

    private void PlayerDie()
    {
        GD.Print($"PlayerScene OnPlayerDeath()");

        Alive = false;
        Parent.PlayerMovement.SetPhysicsProcess(false);
        Parent.PlayerUI.ShowDeathUI();
        Parent.PlayerWeapons.OnPlayerDeath();

        GameManager.Instance.EnemiesManager.OnPlayerDeath();

        Parent.DebugTryUpdateField("player_alive", Alive ? "Alive" : "Dead");
    }

    public void OnCollision(Dictionary collision)
    {
        if (!InvulnerableTimer.IsStopped()) return;

        EnemyEntity enemy = GameManager.Instance.EnemiesManager.FindEnemyByBodyRid((Rid)collision["rid"]);
        ReceiveDamage(enemy.Damage);
    }

    // -------------------------------------------- Debug --------------------------------------------
    public override void DebugCreateSubCategory(DebugCategory category)
    {
        category.CreateDivider("Health");
        category.CreateLabelField("current_health", "HP", $"{Health} / {MaxHealth}");
        category.CreateLabelField("player_alive", "State", Alive ? "Alive" : "Dead");
    }
}