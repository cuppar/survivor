using Godot;

[GodotClassName("Abc")]
[GlobalClass]
public abstract partial class Weapon : Resource
{
    [Export] public string Title { get; set; } = "";
    [Export] public Texture2D Texture { get; set; } = null!;
    
    [Export] public float Damage { get; set; }
    [Export] public float Speed { get; set; }
    [Export] public float Cooldown { get; set; }

    [Export]
    public PackedScene ProjectilePrefab { get; set; } =
        ResourceLoader.Load<PackedScene>("res://scenes/projectiles/projectile.tscn");

    public abstract void Activate(Node2D source, Node2D? target, SceneTree sceneTree);
}