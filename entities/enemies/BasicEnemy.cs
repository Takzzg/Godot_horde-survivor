public partial class BasicEnemy : EnemyEntity
{
    public BasicEnemy()
    {
        Health = 10;
        Speed = 25;
        Damage = 1;
        ExperienceDropped = 1;

        HitboxRadius = 6;
        HurtboxRadius = 10;
    }
}