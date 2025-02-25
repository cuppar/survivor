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

    #region IsElite

    private bool _isElite;

    [Export]
    public bool IsElite
    {
        get => _isElite;
        set => SetIsElite(value);
    }

    private async void SetIsElite(bool value)
    {
        await this.EnsureReadyAsync();
        _isElite = value;
        if (IsElite)
        {
            Sprite.Material = ResourceLoader.Load<Material>("uid://j8upyv4v6hv1");
            Scale = new Vector2(1.5f, 1.5f);
        }
    }

    #endregion

    #endregion

    #region 移动

    [ExportGroup("移动")] [Export] public float Speed { get; set; } = 100f;
    private Vector2 _knockBack;

    private void Move(double delta)
    {
        Velocity = (Player.Position - Position).Normalized() * Speed;
        _knockBack = _knockBack.MoveToward(Vector2.Zero, 1);
        Velocity += _knockBack;

        var collision = MoveAndCollide(Velocity * (float)delta);

        var collider = collision?.GetCollider();
        if (collider is Enemy enemy)
        {
            enemy._knockBack = (enemy.GlobalPosition - GlobalPosition).Normalized() * 50;
        }
    }

    #endregion

    #region 攻击

    [ExportGroup("攻击")] [Export] public float Damage { get; set; }

    #endregion

    #region 生命周期

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        CheckSeparation();
        Move(delta);
    }

    #endregion

    #region 优化

    private void CheckSeparation()
    {
        var separation = (Player.Position - Position).Length();
        if (separation >= 500 && !IsElite)
            QueueFree();
    }

    #endregion

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Sprite2D Sprite { get; set; } = null!;

    #endregion
}