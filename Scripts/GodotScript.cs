using Godot;

public partial class GodotScript : Sprite2D
{
	private int _speed = 400;
	private float _angularSpeed = Mathf.Pi;
	private double _delta;
	private bool _isJumping = false;

	public override void _Ready()
	{
		Engine.PhysicsTicksPerSecond = 75;
	}

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if (@event is not InputEventKey key) return;

		if (key.Keycode == Key.Up && key.IsPressed())
			Jump();
		if (key.Keycode == Key.Up && key.IsReleased())
			Unjump();
    }

    public override void _Process(double delta)
	{
		var deltaFloat = (float) delta;
		Rotation += GetRotationFromInput(delta);
		_delta = delta;
	}

	private float GetRotationFromInput(double delta)
	{
		var direction = 0;
		
		if (Input.IsActionPressed("ui_left"))
			direction = -1;
		if (Input.IsActionPressed("ui_right"))
			direction = 1;

		return _angularSpeed * direction * (float)delta;
	}

	private void Jump()
	{
		if (_isJumping) return;
		_isJumping = true;
		var velocity = Vector2.Up.Rotated(Rotation) * _speed;

		Position += velocity * (float) _delta * 10;
	}

	private void Unjump()
	{
		var velocity = Vector2.Up.Rotated(Rotation) * _speed;

		Position -= velocity * (float) _delta * 10;
		_isJumping = false;
	}
}
