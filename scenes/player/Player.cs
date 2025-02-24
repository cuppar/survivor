using Godot;

[GlobalClass]
public partial class Player : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 150f;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Velocity = Input.GetVector("left", "right", "up", "down") * Speed;
        MoveAndCollide(Velocity * (float)delta);
    }
}