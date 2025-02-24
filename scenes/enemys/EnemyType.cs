using Godot;

[GlobalClass]
public partial class EnemyType : Resource
{
    [Export] public string Title { get; set; } = "";
    [Export] public Texture2D Texture { get; set; } = null!;
    [Export] public float Health { get; set; }
    [Export] public float Damage { get; set; }
}