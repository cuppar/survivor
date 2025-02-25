using Godot;

[GlobalClass]
public partial class SingleShot : Weapon
{
    private void Shoot(Node2D source, Node2D? target, SceneTree sceneTree)
    {
        if (!IsInstanceValid(target)) return;

        var projectile = ProjectilePrefab.Instantiate<Projectile>();
        projectile.Position = source.Position;
        projectile.Damage = Damage;
        projectile.Speed = Speed;
        projectile.Direction = (target!.Position - source.Position).Normalized();
        sceneTree.CurrentScene.AddChild(projectile);
    }

    public override void Activate(Node2D source, Node2D? target, SceneTree sceneTree)
    {
        Shoot(source, target, sceneTree);
    }
}