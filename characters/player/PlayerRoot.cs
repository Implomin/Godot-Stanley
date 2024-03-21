using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class PlayerRoot : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public Vector2 inputVector; 
	[Signal] public delegate void SetMovementStateEventHandler(MovementState movementState);
	[Signal] public delegate void SetMovementDirectionEventHandler(Vector2 directionVector);

	[Export] Dictionary movementStates;

	float sprintingVal = 1;
	[Export] float sprintMultiplier = 1.3f;


    public override void _Ready()
    {
        EmitSignal(SignalName.SetMovementState, movementStates["stand"]);
    }
	
	public override void _PhysicsProcess (double delta)
	{
		inputVector = Input.GetVector("movement_right", "movement_left", "movement_down", "movement_up");
		EmitSignal(SignalName.SetMovementDirection, inputVector);

		if(!IsOnFloor()){
			EmitSignal(SignalName.SetMovementState, movementStates["airborne"]);

		}else{
			HandleGroundMovement();
		}
	}

	private bool DirectionalMovementKeyPressed(){
		return inputVector != Vector2.Zero;
	}

	private void HandleGroundMovement(){
		if (Input.IsActionPressed("movement")){
			if (DirectionalMovementKeyPressed()){
				if (Input.IsActionPressed("movement_sprint")){
					EmitSignal(SignalName.SetMovementState, movementStates["sprint"]);
					GD.Print("sprinting");
				}
				else{
					EmitSignal(SignalName.SetMovementState, movementStates["walk"]);
				}
			}
		}else{
			EmitSignal(SignalName.SetMovementState, movementStates["stand"]);
		}
	}
}
