using Godot;
using Survivor;
using Survivor.Interfaces;

public partial class Projectile : Area2D
{
    [Export] public Vector2 Direction { get; set; } = Vector2.Right;
    [Export] public float Speed { get; set; } = 200;
    [Export] public float Damage { get; set; } = 1;

    public override void _Ready()
    {
        base._Ready();
        BodyEntered += OnBodyEntered;
        VisibleOnScreenNotifier.ScreenExited += OnScreenExited;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Position += Direction * Speed * (float)delta;
    }

    private void OnScreenExited()
    {
        QueueFree();
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is IAttackable attackable and not Player)
        {
            attackable.TakeDamage(Damage);
            if (attackable is Enemy enemy)
            {
                enemy.KnockBack = Direction * 100;
            }
        }
    }

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public VisibleOnScreenNotifier2D VisibleOnScreenNotifier { get; set; } = null!;

    #endregion
}