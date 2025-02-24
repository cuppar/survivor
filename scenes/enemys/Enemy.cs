using Godot;
using Survivor.Globals.Extensions;

public partial class Enemy : CharacterBody2D
{
    [Export] public Player Player { get; set; } = null!;
    [Export] public Vector2 Direction { get; set; } = Vector2.Zero;
    [Export] public float Speed { get; set; } = 100f;

    #region EnemyType

    private EnemyType _enemyType = null!;

    [Export]
    public EnemyType EnemyType
    {
        get => _enemyType;
        set => SetEnemyType(value);
    }

    private async void SetEnemyType(EnemyType value)
    {
        await this.EnsureReadyAsync();
        _enemyType = value;
        Sprite.Texture = EnemyType.Texture;
    }

    #endregion

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Velocity = (Player.Position - Position).Normalized() * Speed;
        MoveAndCollide(Velocity * (float)delta);
    }

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Sprite2D Sprite { get; set; } = null!;

    #endregion
}