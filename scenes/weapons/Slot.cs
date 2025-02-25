using Godot;
using System;
using Survivor;
using Survivor.Globals.Extensions;

public partial class Slot : PanelContainer
{
    #region Weapon

    private Weapon? _weapon;

    [Export]
    public Weapon? Weapon
    {
        get => _weapon;
        set => SetWeapon(value);
    }

    private async void SetWeapon(Weapon? value)
    {
        await this.EnsureReadyAsync();
        _weapon = value;
        if (Weapon != null)
        {
            TextureRect.Texture = Weapon.Texture;
            CooldownTimer.WaitTime = Weapon.Cooldown;
        }
    }

    #endregion
    
    
    public override void _Ready()
    {
        base._Ready();
        CooldownTimer.Timeout += OnCooldownTimer_Timeout;
    }

    private void OnCooldownTimer_Timeout()
    {
        if (Weapon != null)
        {
            var player = (Player)Owner;
            Weapon.Activate(player, player.nearestEnemy, GetTree());
            CooldownTimer.WaitTime = Weapon.Cooldown;
        }
    }


    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Timer CooldownTimer { get; set; } = null!;

    [Export] public TextureRect TextureRect { get; set; } = null!;

    #endregion
}