using Godot;
using System;

public partial class PhysicsBox : RigidBody3D
{
	[Export] public int forceVar = 50;
	[Export] public int torqueForceVar = 1;

	public override void _PhysicsProcess(double delta)
	{
		if(Input.IsActionPressed("movement_up"))
		{
			ApplyForce(Vector3.Forward.Rotated(Vector3.Up, RotationDegrees.Y) * forceVar);
		}
		if(Input.IsActionPressed("movement_left"))
		{
			ApplyTorque(Vector3.Down * torqueForceVar);	
		}
		if(Input.IsActionPressed("movement_right"))
		{
			ApplyTorque(Vector3.Up * torqueForceVar);	
		}
	}
}
