using Godot;
using Survivor.Globals.Extensions;
using Survivor.Interfaces;

namespace Survivor;

[GlobalClass]
public partial class Player : CharacterBody2D, IAttackable
{
    #region 最近的敌人

    public Enemy? nearestEnemy;
    public float distanceOfNearestEnemy = float.MaxValue;

    #endregion

    #region 移动

    [Export] public float Speed { get; set; } = 150f;

    private void Move(double delta)
    {
        Velocity = Input.GetVector("left", "right", "up", "down") * Speed;
        MoveAndCollide(Velocity * (float)delta);
    }

    #endregion

    #region 生命

    #region Health

    private float _health = 100;

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
        UI_Health.Value = Health;
        if (Health <= 0)
            GD.Print($"Die!");
    }

    #endregion

    #region 受伤

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }


    private void OnSelfDamageArea_BodyEntered(Node body)
    {
        if (body is Enemy enemy)
            TakeDamage(enemy.Damage);
    }

    private void OnHurtTimer_Timeout()
    {
        SelfDamageCollisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
        SelfDamageCollisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
    }

    #endregion

    #endregion

    #region 生命周期

    public override void _Ready()
    {
        base._Ready();

        SetLevel(1);
        SetXP(0);
        SetMaxXP_CurrentLevel(5);
        TotalXP = 0;

        SelfDamageArea.BodyEntered += OnSelfDamageArea_BodyEntered;
        HurtTimer.Timeout += OnHurtTimer_Timeout;
        MagnetArea.AreaEntered += OnMagnetArea_AreaEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Move(delta);

        if (IsInstanceValid(nearestEnemy))
        {
            distanceOfNearestEnemy = nearestEnemy!.distanceFromPlayer;
        }
        else
        {
            distanceOfNearestEnemy = float.MaxValue;
        }
    }

    #endregion

    #region 经验

    #region XP

    private int _xp;

    [Export]
    public int XP
    {
        get => _xp;
        set => SetXP(value);
    }

    private async void SetXP(int value)
    {
        await this.EnsureReadyAsync();
        _xp = value;
        while (_xp > MaxXP_CurrentLevel)
        {
            Level++;
            _xp -= MaxXP_CurrentLevel;

            if (Level <= 20)
            {
                MaxXP_CurrentLevel += 10;
            }
            else if (Level <= 40)
            {
                MaxXP_CurrentLevel += 13;
            }
            else
            {
                MaxXP_CurrentLevel += 16;
            }
        }

        XPBar.Value = _xp;

        #region test

        LevelLabel.Text = "Lv " + _level + $"({XP}/{MaxXP_CurrentLevel} {TotalXP})";

        #endregion
    }

    #endregion

    public int TotalXP { get; set; }

    #region Level

    private int _level = 1;

    [Export]
    public int Level
    {
        get => _level;
        set => SetLevel(value);
    }

    private async void SetLevel(int value)
    {
        await this.EnsureReadyAsync();
        _level = value;
        LevelLabel.Text = "Lv " + _level + $"({XP}/{MaxXP_CurrentLevel} {TotalXP})";
    }

    #endregion

    #region MaxXP_CurrentLevel

    private int _maxXP_CurrentLevel = 5;

    [Export]
    public int MaxXP_CurrentLevel
    {
        get => _maxXP_CurrentLevel;
        set => SetMaxXP_CurrentLevel(value);
    }

    private async void SetMaxXP_CurrentLevel(int value)
    {
        await this.EnsureReadyAsync();
        _maxXP_CurrentLevel = value;
        XPBar.MaxValue = _maxXP_CurrentLevel;
    }

    #endregion

    public void GainXP(int xp)
    {
        TotalXP += xp;
        XP += xp;
    }

    #endregion

    #region 拾取

    private void OnMagnetArea_AreaEntered(Area2D area)
    {
        if (area is IFollower follower)
            follower.Follow(this);
    }

    #endregion

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Area2D SelfDamageArea { get; set; } = null!;

    [Export] public ProgressBar UI_Health { get; set; } = null!;
    [Export] public Timer HurtTimer { get; set; } = null!;
    [Export] public CollisionShape2D SelfDamageCollisionShape { get; set; } = null!;
    [Export] public TextureProgressBar XPBar { get; set; } = null!;
    [Export] public Label LevelLabel { get; set; } = null!;
    [Export] public Area2D MagnetArea { get; set; } = null!;

    #endregion
}