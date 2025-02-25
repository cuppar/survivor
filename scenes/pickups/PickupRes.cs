using Godot;
using Survivor;

[GlobalClass]
public partial class PickupRes : Resource
{
    [Export] public string Title { get; set; } = "";
    [Export] public Texture2D Icon { get; set; } = null!;
    [Export(PropertyHint.MultilineText)] public string Description { get; set; } = "";

    public Player player = null!;

    public virtual void Activate()
    {
        GD.Print($"Pick up {Title}");
    }
}