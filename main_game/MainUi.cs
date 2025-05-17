using Godot;
using System;

public partial class MainUi : Control
{
    [Export]
    private Label _healthLabel;

    public void UpdateHealthLabel(int amount)
    {
        _healthLabel.Text = amount.ToString();
    }
}
