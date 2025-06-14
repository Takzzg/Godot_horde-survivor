using Godot;
using System;

public partial class Utils : Node
{
    public enum RarityEnum { TEST, COMMON, UNCOMMON, RARE, EPIC, LEGENDARY, UNIQUE }

    public partial class EffectLabel : Label
    {
        public enum TypeEnum { NEGATIVE, POSITIVE }
        public EffectLabel(TypeEnum type, string text) : base()
        {
            Text = $"{(type == TypeEnum.POSITIVE ? "+" : "-")} {text}";
            AddThemeColorOverride("font_color", type == TypeEnum.POSITIVE ? Colors.LightGreen : Colors.IndianRed);
            HorizontalAlignment = HorizontalAlignment.Center;
        }
    }

    public static Vector2 GetPointArounOrigin(Vector2 center, float distance, float rotation)
    {
        return new Vector2(
            center.X + distance * (float)Math.Cos(rotation),
            center.Y + distance * (float)Math.Sin(rotation)
        );
    }

    public static Vector2 GetRandomPointOnCircle(Vector2 center, float radius)
    {
        float angle = GameManager.Instance.RNG.RandiRange(0, 359);
        return GetPointArounOrigin(center, radius, angle);
    }
}
