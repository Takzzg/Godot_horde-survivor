using Godot;

public partial class TestScenario : Node2D
{
    public override void _Ready()
    {
        TEST_SpawnEnemies();
    }

    public void TEST_SpawnEnemies()
    {
        Vector2[] positions = [
            new Vector2(100, 0),
            new Vector2(100, 0),
            new Vector2(100, 0),
            new Vector2(100, 0),
            new Vector2(100, 0),
            new Vector2(100, 50),
            new Vector2(110, 50),
            new Vector2(120, 50),
            new Vector2(100, 100),
            new Vector2(110, 100),
            new Vector2(120, 100),
            new Vector2(130, 100),
            new Vector2(140, 100),
        ];

        foreach (Vector2 pos in positions)
        {
            BasicEnemy enemy = new(pos, 50, 0, 0);
            GameManager.Instance.EnemyManager.SpawnEnemy(enemy);
        }
    }
}