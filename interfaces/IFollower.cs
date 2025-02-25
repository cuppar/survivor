using Godot;

namespace Survivor.Interfaces;

public interface IFollower
{
    [Export] public float FollowSpeed { get; set; }
    public Vector2 FollowDirection { get; set; }
    public bool CanFollow { get; set; }

    public Node2D? FollowTarget { get; set; }


    public void Follow(Node2D target)
    {
        FollowTarget = target;
        CanFollow = true;
    }

    public void Unfollow()
    {
        CanFollow = false;
        FollowTarget = null;
    }

    public void FollowUpdate(double delta)
    {
        if (CanFollow && FollowTarget != null)
        {
            var self = ((Node2D)this);
            FollowDirection = (FollowTarget.GlobalPosition - self.GlobalPosition).Normalized();
            self.GlobalPosition += FollowDirection * FollowSpeed * (float)delta;
        }
    }
}