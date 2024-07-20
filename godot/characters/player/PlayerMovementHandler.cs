using Godot;
using System;

public partial class PlayerMovementHandler : Node3D
{
	AudioStreamPlayer audioStreamPlayer;
	[Export] ShapeCast3D pickedUpObjectShapecast;
	[Export] CharacterBody3D playerCharacter;
	[Export] public float baseMovementSpeed = 5.0f;
	[Export] AudioStreamPlayer SFXJump;
	private float acceleration;
	private float speed;
	Vector3 destDirection;
	Vector3 medianVelocity;
	float gravDir;
	[Export] float gravityStrength = 0.5f;
	[Export] float jumpStrength = 4f;
	[Export] float jumpAcceleration = 16f;
	float cameraRotationInternal;
	private void OnSetMovementState (MovementState movementState)
	{
		speed = movementState.movementSpeedModifier * baseMovementSpeed;
		acceleration = movementState.acceleration;
		//GD.Print(movementState.id);
	}

	private void OnSetMovementDirection (Vector2 direction)
	{
		destDirection = new Vector3(direction.X, 0, direction.Y).Rotated(Vector3.Up, cameraRotationInternal);
	}

	private void OnSetCameraRotation (float cameraRotation)
	{
		cameraRotationInternal = cameraRotation;
	}

	public override void _PhysicsProcess(double delta){
		medianVelocity.X = speed * destDirection.Normalized().X;
		medianVelocity.Z = speed * destDirection.Normalized().Z;

 		bool isGrounded = playerCharacter.IsOnFloor();
		bool isOnPickedUpObject = pickedUpObjectShapecast.IsColliding();
		ApplyGravity(isGrounded, (float)delta);

 		if(Input.IsActionJustPressed("movement_jump") && isGrounded && !isOnPickedUpObject){
			//GD.Print("jump");
			SFXJump.Play(0);
			medianVelocity.Y = jumpStrength * 8;
			
		}

		playerCharacter.Velocity = ReusableMethods.LerpVector3WithDifferentWeightForY(playerCharacter.Velocity, medianVelocity, (float)delta*acceleration, (float)delta*jumpAcceleration/*1 - (float)Mathf.Pow(0.5f, delta * acceleration), 1 - (float)Mathf.Pow(0.5f, delta * jumpAcceleration)*/);
	
		playerCharacter.MoveAndSlide();
	}

		private void ApplyGravity(bool isGrounded, float delta){
		if(isGrounded){
			gravDir = 0;
		}
		else{
			gravDir -= gravityStrength * 40 * delta;
		}
		medianVelocity.Y = gravDir;
	}
}
