public partial class PlayerDebug : BasePlayerComponent
{
    private DebugManager.DebugTitle _title;
    private DebugCategoryComponent _movement;
    private DebugCategoryComponent _health;
    private DebugCategoryComponent _experience;

    public PlayerDebug(PlayerScene player) : base(player)
    {
        _title = new DebugManager.DebugTitle("Player");
        DebugManager.Instance.RenderNode(_title);
        TreeExiting += _title.QueueFree;

        // create movement cat
        _movement = new DebugCategoryComponent((instance) =>
        {
            instance.TryCreateCategory(new DebugManager.DebugCategory("player_movement", "Movement"));
            instance.TryCreateField("player_speed", "Speed", _player.PlayerMovement.Speed.ToString());
            instance.TryCreateField("player_pos", "Pos", _player.Position.ToString("0.0"));

            _player.PlayerMovement.PlayerMove += () => { instance.TryUpdateField("player_pos", _player.Position.ToString("0.0")); };
        });
        AddChild(_movement);

        // create health cat
        _health = new DebugCategoryComponent((instance) =>
        {
            instance.TryCreateCategory(new DebugManager.DebugCategory("player_health", "Health"));
            instance.TryCreateField("current_health", "HP", $"{_player.PlayerHealth.Health} / {_player.PlayerHealth.MaxHealth}");
            instance.TryCreateField("player_alive", "State", _player.PlayerHealth.Alive ? "Alive" : "Dead");

            _player.PlayerHealth.PlayerReceiveDamage += (_) => instance.TryUpdateField("current_health", $"{_player.PlayerHealth.Health} / {_player.PlayerHealth.MaxHealth}");
        });
        AddChild(_health);

        // create experience cat
        _experience = new DebugCategoryComponent((instance) =>
        {
            instance.TryCreateCategory(new DebugManager.DebugCategory("player_experience", "Experience"));
            instance.TryCreateField("player_level", "Level", _player.PlayerExperience.PlayerLevel.ToString());
            instance.TryCreateField("current_xp", "XP", $"{_player.PlayerExperience.CurrentExperience} / {_player.PlayerExperience.ExperienceToNextLevel}");

            _player.PlayerExperience.PlayerExperienceGain += () =>
            {
                instance.TryUpdateField("current_xp", $"{_player.PlayerExperience.CurrentExperience} / {_player.PlayerExperience.ExperienceToNextLevel}");
            };
            _player.PlayerExperience.PlayerLevelUp += () =>
            {
                instance.TryUpdateField("player_level", _player.PlayerExperience.PlayerLevel.ToString());
                instance.TryUpdateField("current_xp", $"{_player.PlayerExperience.CurrentExperience} / {_player.PlayerExperience.ExperienceToNextLevel}");
            };
        });
        AddChild(_experience);
    }
}