public partial class EM_Difficulty : BaseComponent<EnemiesManager>
{
    public const int ENEMY_COUNT_HARD_CAP = 1500;

    public int MaxEnemies = 100;
    public int HealthMultiplierBloated = 10;
    public int DamageMultiplierBloated = 10;

    private readonly int _secondsBetweenIncrement = 60;
    private double _timeUntilIncrement;

    public EM_Difficulty(EnemiesManager parent) : base(parent)
    {
        _timeUntilIncrement = _secondsBetweenIncrement;
        UpdateDifficultyValues();
    }

    public override void _PhysicsProcess(double delta)
    {
        _timeUntilIncrement -= delta;
        UpdateDifficultyValues();
    }

    public void UpdateDifficultyValues()
    {
        if (_timeUntilIncrement <= 0)
        {
            if (MaxEnemies < ENEMY_COUNT_HARD_CAP) MaxEnemies += 100;
            HealthMultiplierBloated += 1;
            DamageMultiplierBloated += 1;

            _timeUntilIncrement = _secondsBetweenIncrement;
        }

        DebugUpdateValues();
    }

    public override void DebugCreateSubCategory(DebugCategory category)
    {
        category.CreateDivider("Difficulty");
        category.CreateLabelField("time_between_inc", "Seconds between increments", _secondsBetweenIncrement.ToString());
        category.CreateLabelField("time_remaining", "Time remaining", Utils.DeltaToTimeString(_timeUntilIncrement));
        category.CreateLabelField("max_enemies", "Max enemies.", $"{MaxEnemies}/{ENEMY_COUNT_HARD_CAP}");
        category.CreateLabelField("health_multiplier", "Health mult.", $"x{(float)HealthMultiplierBloated / 10}");
        category.CreateLabelField("damage_multiplier", "Damage mult.", $"x{(float)DamageMultiplierBloated / 10}");
    }

    private void DebugUpdateValues()
    {
        Parent.DebugTryUpdateField("time_remaining", Utils.DeltaToTimeString(_timeUntilIncrement));
        Parent.DebugTryUpdateField("max_enemies", $"{MaxEnemies}/{ENEMY_COUNT_HARD_CAP}");
        Parent.DebugTryUpdateField("health_multiplier", $"x{(float)HealthMultiplierBloated / 10}");
        Parent.DebugTryUpdateField("damage_multiplier", $"x{(float)DamageMultiplierBloated / 10}");
    }
}