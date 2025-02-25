using Godot;
using Survivor;
using Survivor.Globals.Extensions;
using Survivor.Interfaces;

public partial class Pickup : Area2D, IFollower
{
    #region Player

    private Player _player = null!;

    [Export]
    public Player Player
    {
        get => _player;
        set => SetPlayer(value);
    }

    private async void SetPlayer(Player value)
    {
        await this.EnsureReadyAsync();
        _player = value;
        PickupType.player = _player;
    }

    #endregion

    #region PickupType

    private PickupRes _pickupType = null!;

    [Export]
    public PickupRes PickupType
    {
        get => _pickupType;
        set => SetPickupType(value);
    }

    private async void SetPickupType(PickupRes value)
    {
        await this.EnsureReadyAsync();
        _pickupType = value;
        Sprite.Texture = _pickupType.Icon;
    }

    #endregion

    #region 跟随

    public float FollowSpeed { get; set; } = 175;
    public Vector2 FollowDirection { get; set; }
    public bool CanFollow { get; set; }
    public Node2D? FollowTarget { get; set; }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player)
        {
            PickupType.Activate();
            QueueFree();
        }
    }

    #endregion

    #region 生命周期

    public override void _Ready()
    {
        base._Ready();
        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        ((IFollower)this).FollowUpdate(delta);
    }

    #endregion

    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Sprite2D Sprite { get; set; } = null!;

    #endregion
}