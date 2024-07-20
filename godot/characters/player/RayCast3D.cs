using Godot;
using System;

public partial class RayCast3D : Godot.RayCast3D
{

	[Export] Node3D pickUpObjLocation;
	[Export] AudioStreamPlayer SFXGrab;
	[Export] AudioStreamPlayer SFXDrop;
	Pickupable lookingAtObject = null; 
	bool currentlyPickingUp = false;
	bool isColliding = false;

	public override void _PhysicsProcess(double delta)
	{
		if(IsColliding() && lookingAtObject == null){
			lookingAtObject = (Pickupable)GetCollider();
		}
	}

	public override void _Process(double delta) {

		if(lookingAtObject != null){
			if(currentlyPickingUp){
				lookingAtObject.GravityScale = 0f;
				lookingAtObject.LinearVelocity = (pickUpObjLocation.GlobalPosition - lookingAtObject.GlobalPosition)*20;
				//lookingAtObject.AngularVelocity = pickUpObjLocation.GlobalRotation - lookingAtObject.GlobalRotation;

				if(Input.IsActionJustPressed("interaction_pickup")){
					lookingAtObject.isPickedUp = false;
					currentlyPickingUp = false;
					GD.Print("Dropped object");
					SFXDrop.Play();
				}
			}else{
				lookingAtObject.GravityScale = 1f;

				if(IsColliding()){
					lookingAtObject = (Pickupable)GetCollider();
					GD.Print("obj detected: " + lookingAtObject.Name);

					if(Input.IsActionJustPressed("interaction_pickup")){
						lookingAtObject.isPickedUp = true;
						currentlyPickingUp = true;
						GD.Print("Picked up object");
						SFXGrab.Play();
					}
				}
			}
		}

/* 			lookingAtObject.GlobalPosition = lookingAtObject.GlobalPosition.Lerp(pickUpObjLocation.GlobalPosition, 0.5f);
			lookingAtObject.GlobalRotation = lookingAtObject.GlobalRotation.Lerp(pickUpObjLocation.GlobalRotation, 0.5f); */
			//lookingAtObject.GlobalRotation = lookingAtObject.GlobalRotation.Lerp(pickUpObjLocation.GlobalRotation, 0.5f);
			//lookingAtObject.GlobalTransform = pickUpObjLocation.GlobalTransform;

	}
}
