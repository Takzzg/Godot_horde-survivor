public partial class BasicEnemy : EnemyEntity
{
    public BasicEnemy()
    {
        Speed = 25;
        Damage = 1;
        ExperienceDropped = 1;

        HitboxRadius = 6;
        HurtboxRadius = 10;
    }
}