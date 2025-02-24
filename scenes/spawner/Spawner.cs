using Godot;

public partial class Spawner : Node2D
{
    [Export] public Player Player { get; set; } = null!;
    [Export] public PackedScene EnemyScene { get; set; } = null!;

    #region 派生逻辑

    [Export] public float Distance { get; set; } = 400;

    #endregion


    #region Child

    [ExportGroup("ChildDontChange")]
    [Export]
    public Label MinuteLabel { get; set; } = null!;

    [Export] public Label SecondLabel { get; set; } = null!;

    #endregion
}