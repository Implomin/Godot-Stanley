using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class player_root : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public Vector2 inputVector; 
	[Signal] public delegate void SetMovementStateEventHandler(movementState movementState);
	[Signal] public delegate void SetMovementDirectionEventHandler(movementState movementState);

	[Export] Dictionary movementStates;

	float sprintingVal = 1;
	[Export] float sprintMultiplier = 1.3f;


    public override void _Ready()
    {
        EmitSignal(SignalName.SetMovementState, movementStates["stand"]);
    }

    public override void _Input(InputEvent @event)
    {
		if (Input.IsActionPressed("movement")){
			inputVector = Input.GetVector("movement_right", "movement_left", "movement_backward", "movement_forward");
			if (DirectionalMovementKeyPressed()){
				if (Input.IsActionPressed("movement_sprint")){
					EmitSignal(SignalName.SetMovementState, movementStates["sprint"]);
				}
				else{
					EmitSignal(SignalName.SetMovementState, movementStates["walk"]);
				}
			}
		}
		else{
			EmitSignal(SignalName.SetMovementState, movementStates["stand"]);
		}
	}
	
	public override void _PhysicsProcess (double delta)
	{
		if(!IsOnFloor()){
			EmitSignal(SignalName.SetMovementState, movementStates["airborne"]);
		}

		if(DirectionalMovementKeyPressed()){ 
			EmitSignal(SignalName.SetMovementDirection, new Vector3(inputVector.X, 0, inputVector.Y));
		}
	}

	private bool DirectionalMovementKeyPressed()
	{
		return inputVector != Vector2.Zero;
	}
}
