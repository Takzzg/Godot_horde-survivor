using System.Linq;
using Godot;

public partial class PlayerHealth : Node
{
    PlayerScene _player;

    public bool Alive = true;
    public int Health = 50;

    [Signal]
    public delegate void PlayerReceiveDamageEventHandler(int amount);
    [Signal]
    public delegate void PlayerDeathEventHandler();

    public PlayerHealth(PlayerScene player)
    {
        _player = player;

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

        Godot.Collections.Array<Node> weapons = _player.WeaponsContainer.GetChildren();
        foreach (ProjectileWeapon weapon in weapons.Cast<ProjectileWeapon>()) { weapon.WeaponShooting.TimedAttackSetRunning(false); }
    }
}