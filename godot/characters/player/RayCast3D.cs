using Godot;
using System;

public partial class RayCast3D : Godot.RayCast3D
{

	[Export] Node3D pickUpObjLocation;
	Pickupable lookingAtObject = new Pickupable(); 


	public override void _Process(double delta) {
		if (IsColliding()) {
			lookingAtObject = (Pickupable)GetCollider();

			if(Input.IsActionJustPressed("interaction_pickup"))
			{
				lookingAtObject.isPickedUp = !lookingAtObject.isPickedUp;
				GD.Print("Picked up: " + lookingAtObject.isPickedUp);
				//lookingAtObject.LinearVelocity = Vector3.Zero;
			}
		}else{
			lookingAtObject.isPickedUp = false;
		}

		if(lookingAtObject.isPickedUp){
			lookingAtObject.GravityScale = 0f;
/* 			lookingAtObject.GlobalPosition = lookingAtObject.GlobalPosition.Lerp(pickUpObjLocation.GlobalPosition, 0.5f);
			lookingAtObject.GlobalRotation = lookingAtObject.GlobalRotation.Lerp(pickUpObjLocation.GlobalRotation, 0.5f); */
			lookingAtObject.LinearVelocity = (pickUpObjLocation.GlobalPosition - lookingAtObject.GlobalPosition)*15;
			lookingAtObject.AngularVelocity = pickUpObjLocation.GlobalRotation - lookingAtObject.GlobalRotation;

			//lookingAtObject.GlobalRotation = lookingAtObject.GlobalRotation.Lerp(pickUpObjLocation.GlobalRotation, 0.5f);

			//lookingAtObject.GlobalTransform = pickUpObjLocation.GlobalTransform;
		}
		else
		{
			lookingAtObject.GravityScale = 1f;
		}

	}
}
