using Godot;

public partial class GrassEffect : Node2D
{
	private AnimatedSprite2D _animatedSprite;
	
	public override void _Ready()
	{
		_animatedSprite = GetNode<AnimatedSprite2D>(nameof(AnimatedSprite2D));
		_animatedSprite.Play("Animate");
	}

	public void OnAnimateSpriteAnimationFinished() => QueueFree();
}
