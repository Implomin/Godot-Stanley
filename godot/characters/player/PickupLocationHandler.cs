using Godot;
using System;

public partial class PickupLocationHandler : Node3D
{
	[Export] Node3D playerCameraHandler;
	[Export] Node3D playerLookAt;
	Node3D playerPositionNode;
	bool isPickedUp = false;
	public override void _Ready() {
		playerPositionNode = GetParent<Node3D>();
	}

	public override void _PhysicsProcess(double delta)
	{
		if(!isPickedUp){	
			if(playerCameraHandler.Rotation.X > 0.3f)
			{
				Vector2 directionalVector = new Vector2(playerLookAt.GlobalPosition.X, playerLookAt.GlobalPosition.Z).Normalized();
				GD.Print (directionalVector);
				this.GlobalPosition = new Vector3(playerPositionNode.GlobalPosition.X + directionalVector.X * 2, playerLookAt.GlobalPosition.Y, playerPositionNode.GlobalPosition.Z + directionalVector.Y * 2);
			}else{
				this.GlobalPosition = new Vector3(playerLookAt.GlobalPosition.X, playerLookAt.GlobalPosition.Y, playerLookAt.GlobalPosition.Z);
			}
		}
		
	}
}
