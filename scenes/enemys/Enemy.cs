using System.Globalization;
using Godot;
using Survivor.Globals.Extensions;
using Survivor.Interfaces;

namespace Survivor;

public partial class Enemy : CharacterBody2D, IAttackable
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
        Health = EnemyType.Health;
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

    [ExportGroup("移动")] [Export] public float Speed { get; set; } = 30f;
    public Vector2 KnockBack { get; set; }

    private void Move(double delta)
    {
        Velocity = (Player.Position - Position).Normalized() * Speed;
        KnockBack = KnockBack.MoveToward(Vector2.Zero, 1);
        Velocity += KnockBack;

        var collision = MoveAndCollide(Velocity * (float)delta);

        var collider = collision?.GetCollider();
        if (collider is Enemy enemy)
        {
            enemy.KnockBack = (enemy.GlobalPosition - GlobalPosition).Normalized() * 50;
        }
    }

    #endregion

    #region Health

    private float _health;

    [Export]
    public float Health
    {
        get => _health;
        set => SetHealth(value);
    }

    private async void SetHealth(float value)
    {
        await this.EnsureReadyAsync();
        _health = value;
        if (Health <= 0) QueueFree();
    }

    #endregion

    #region 伤害

    #region 攻击

    [ExportGroup("攻击")] [Export] public float Damage { get; set; }

    #endregion

    #region 受伤

    public void TakeDamage(float damage)
    {
        var tween = CreateTween();
        tween.TweenProperty(this, "modulate", Colors.Red, .2);
        tween.Chain().TweenProperty(this, "modulate", Colors.White, .2);
        tween.BindNode(this);

        DamagePopup(damage);
        Health -= damage;
    }

    private void DamagePopup(float amount)
    {
        var damagePopup = global::DamagePopup.Create();
        damagePopup.Position = Position + new Vector2(-50, -25);
        damagePopup.Text = amount.ToString(CultureInfo.InvariantCulture);
        GetTree().CurrentScene.AddChild(damagePopup);
    }

    #endregion

    #endregion

    #region 生命周期

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        CheckSeparation();
        Move(delta);
    }

    #endregion

    #region 距离玩家距离

    public float distanceFromPlayer;

    private void CheckSeparation()
    {
        distanceFromPlayer = (Player.Position - Position).Length();
        if (distanceFromPlayer >= 500 && !IsElite)
            QueueFree();

        if (distanceFromPlayer < Player.distanceOfNearestEnemy)
            Player.nearestEnemy = this;
    }

    #endregion

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Sprite2D Sprite { get; set; } = null!;

    #endregion
}