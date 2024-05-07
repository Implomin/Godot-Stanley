using Godot;
using System;

public partial class PickupLocationHandler : Node3D
{
	/* [Export] Node3D playerCameraHandler; */
	[Export] Node3D playerLookAt;
	/* Node3D playerPositionNode; */
	public override void _Ready() {
		/* playerPositionNode = GetParent<Node3D>(); */
	}

	public override void _PhysicsProcess(double delta)
	{
/* 		if(!isPickedUp){	
			if(playerCameraHandler.Rotation.X > 0.9f)
			{
				Vector2 directionalVector = new Vector2(Mathf.Sin(playerCameraHandler.Rotation.Y), Mathf.Cos(playerCameraHandler.Rotation.Y));
				this.GlobalPosition = new Vector3(directionalVector.X*1.2f + playerCameraHandler.GlobalPosition.X, playerLookAt.GlobalPosition.Y, directionalVector.Y*1.2f + playerCameraHandler.GlobalPosition.Z);
			}else{
				this.GlobalPosition = new Vector3(playerLookAt.GlobalPosition.X, playerLookAt.GlobalPosition.Y, playerLookAt.GlobalPosition.Z);
			}
		} */
		this.GlobalPosition = playerLookAt.GlobalPosition;
	}
}
