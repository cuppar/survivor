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
        Timer.Timeout += OnTimer_Timeout;
        PatternTimer.Timeout += OnPatternTimer_Timeout;
        EliteTimer.Timeout += OnEliteTimer_Timeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        _canSpawn = GetTree().GetNodesInGroup("Enemy").Count < 700;
    }

    #endregion

    #region 派生逻辑

    [Export] public Array<EnemyType> EnemyTypes { get; set; } = null!;
    private bool _canSpawn = true;

    public void Spawn(Vector2 position, bool isElite = false)
    {
        if (!_canSpawn && !isElite)
            return;

        var enemy = EnemyScene.Instantiate<Enemy>();

        enemy.EnemyType = EnemyTypes[Math.Min(Minute, EnemyTypes.Count - 1)];
        enemy.Position = position;
        enemy.Player = Player;
        enemy.IsElite = isElite;

        GetTree().CurrentScene.AddChild(enemy);
    }

    public void Amount(int number = 1)
    {
        for (int i = 0; i < number; i++)
            Spawn(GetRandomPosition());
    }

    #region 计时和普通怪物派生

    private void OnTimer_Timeout()
    {
        Second++;
        Amount(Second % 10);
    }

    #endregion

    #region 按模式派生

    private void OnPatternTimer_Timeout()
    {
        Amount(75);
    }

    #endregion

    #region 精英怪派生

    private void OnEliteTimer_Timeout()
    {
        Spawn(GetRandomPosition(), true);
    }

    #endregion

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
            _second = 0;
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
    [Export] public Timer Timer { get; set; } = null!;
    [Export] public Timer PatternTimer { get; set; } = null!;
    [Export] public Timer EliteTimer { get; set; } = null!;

    #endregion
}