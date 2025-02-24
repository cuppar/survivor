using Godot;
using Survivor.Globals.Extensions;

namespace Survivor;
public partial class Enemy : CharacterBody2D
{
    #region 基本信息

    [ExportGroup("基本信息")] [Export] public Player Player { get; set; } = null!;

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
        Damage = EnemyType.Damage;
    }

    #endregion

    #endregion

    #region 移动

    [ExportGroup("移动")] [Export] public Vector2 Direction { get; set; } = Vector2.Zero;
    [Export] public float Speed { get; set; } = 100f;

    #endregion

    #region 攻击

    [ExportGroup("攻击")] [Export] public float Damage { get; set; }

    #endregion

    #region 生命周期

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Velocity = (Player.Position - Position).Normalized() * Speed;
        MoveAndCollide(Velocity * (float)delta);
    }

    #endregion

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Sprite2D Sprite { get; set; } = null!;

    #endregion
}