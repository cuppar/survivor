using Godot;

public partial class Enemy : CharacterBody2D
{
    [Export] public Player Player { get; set; } = null!;
    [Export] public Vector2 Direction { get; set; } = Vector2.Zero;
    [Export] public float Speed { get; set; } = 100f;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Velocity = (Player.Position - Position).Normalized() * Speed;
        MoveAndCollide(Velocity * (float)delta);
    }
}