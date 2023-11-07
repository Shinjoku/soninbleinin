using Godot;

public partial class SwordHitbox : Area2D
{
	public Vector2 KnockbackVector = Vector2.Zero;
	public int KnockbackForce = 80;
	public int Damage = 1;
}
