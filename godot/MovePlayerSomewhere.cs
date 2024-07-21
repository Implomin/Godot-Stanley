using Godot;
using System;

public partial class MovePlayerSomewhere : Area3D
{
	[Export] Node3D goalLocation;

	public void _OnBodyEntered(PlayerRoot body)
	{
		body.GlobalPosition = goalLocation.GlobalPosition;
	}
}
