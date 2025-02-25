using Godot;

public partial class DamagePopup : Label
{
    private static PackedScene SelfPrefab { get; } =
        ResourceLoader.Load<PackedScene>("res://scenes/enemys/damage_popup.tscn");

    public override void _Ready()
    {
        Popup();
    }

    private async void Popup()
    {
        using var tween = GetTree().CreateTween();
        tween.TweenProperty(this, "scale", new Vector2(2, 2), .1);
        tween.Chain().TweenProperty(this, "scale", new Vector2(1, 1), .1);
        await ToSignal(tween, Tween.SignalName.Finished);
        QueueFree();
    }

    public static DamagePopup Create() => SelfPrefab.Instantiate<DamagePopup>();
}