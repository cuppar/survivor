using Godot;

[GlobalClass]
public partial class GemRes : PickupRes
{
    [Export] public int XP { get; set; } = 1;

    public override void Activate()
    {
        base.Activate();
        GD.Print($"+ {XP} XP");
        player.GainXP(XP);
    }
}