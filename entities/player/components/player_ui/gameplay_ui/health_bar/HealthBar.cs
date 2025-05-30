using Godot;

public partial class HealthBar : Control
{
    [Export]
    public ProgressBar ProgressBar;

    [Export]
    public Label CurrentLabel;
    [Export]
    public Label MaxLabel;

    public void UpdateValue(int current, int max)
    {
        float percentage = current * 100 / max;

        ProgressBar.Value = percentage;
        CurrentLabel.Text = current.ToString();
        MaxLabel.Text = max.ToString();
    }
}
