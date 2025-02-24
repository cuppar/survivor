using Godot;
using Survivor.Globals.Extensions;

namespace Survivor;
[GlobalClass]
public partial class Player : CharacterBody2D
{
    #region 移动

    [Export] public float Speed { get; set; } = 150f;

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
        if (Health <= 0)
        {
            GD.Print($"Die!");
            QueueFree();
        }
    }

    #endregion

    #region 受伤

    private void TakeDamage(float damage)
    {
        Health -= damage;
    }


    private void OnSelfDamageArea_BodyEntered(Node body)
    {
        if (body is Enemy enemy)
            TakeDamage(enemy.Damage);
    }

    #endregion

    #endregion

    #region 生命周期

    public override void _Ready()
    {
        base._Ready();
        SelfDamageArea.BodyEntered += OnSelfDamageArea_BodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Velocity = Input.GetVector("left", "right", "up", "down") * Speed;
        MoveAndCollide(Velocity * (float)delta);
    }

    #endregion

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Area2D SelfDamageArea { get; set; } = null!;

    #endregion
}