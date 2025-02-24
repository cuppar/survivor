using System;
using Godot;
using Godot.Collections;
using Survivor.Globals.Extensions;

namespace Survivor;

public partial class Spawner : Node2D
{
    [Export] public Player Player { get; set; } = null!;
    [Export] public PackedScene EnemyScene { get; set; } = null!;

    #region 生命周期

    public override void _Ready()
    {
        base._Ready();
        Timer.Timeout += OnTimerTimeout;
    }

    #region 事件

    private void OnTimerTimeout()
    {
        Second++;
        Amount(Second % 10);
    }

    #endregion

    #endregion

    #region 派生逻辑

    [Export] public Array<EnemyType> EnemyTypes { get; set; } = null!;

    public void Spawn(Vector2 position)
    {
        var enemy = EnemyScene.Instantiate<Enemy>();

        enemy.EnemyType = EnemyTypes[Math.Min(Minute, EnemyTypes.Count - 1)];
        enemy.Position = position;
        enemy.Player = Player;

        GetTree().CurrentScene.AddChild(enemy);
    }

    public void Amount(int number = 1)
    {
        for (int i = 0; i < number; i++)
            Spawn(GetRandomPosition());
    }

    #region 派生位置

    [Export] public float Distance { set; get; } = 400;

    private Vector2 GetRandomPosition()
    {
        return Player.Position + Distance * Vector2.Right.Rotated((float)GD.RandRange(0, 2 * Math.PI));
    }

    #endregion

    #endregion


    #region 存活时间

    #region Minute

    private int _minute;

    [Export]
    public int Minute
    {
        get => _minute;
        set => SetMinute(value);
    }

    private async void SetMinute(int value)
    {
        await this.EnsureReadyAsync();
        _minute = value;
        MinuteLabel.Text = _minute.ToString();
    }

    #endregion

    #region Second

    private int _second;

    [Export]
    public int Second
    {
        get => _second;
        set => SetSecond(value);
    }

    private async void SetSecond(int value)
    {
        await this.EnsureReadyAsync();
        _second = value;
        if (Second >= 10)
        {
            Second = 0;
            Minute++;
        }

        SecondLabel.Text = $"{Second:00}";
    }

    #endregion

    #endregion

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Label MinuteLabel { get; set; } = null!;

    [Export] public Label SecondLabel { get; set; } = null!;

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Timer Timer { get; set; } = null!;

    #endregion

    #endregion
}