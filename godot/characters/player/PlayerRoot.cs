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

	private Dictionary movementStates = new Dictionary();

	float sprintingVal = 1;
	[Export] float sprintMultiplier = 1.3f;
	[Export] AudioStreamPlayer SFXFootsteps;


	public override void _Ready()
	{
		movementStates.Add("stand", Godot.ResourceLoader.Load("characters/player/states/stand.tres"));
		movementStates.Add("walk", Godot.ResourceLoader.Load("characters/player/states/walk.tres"));
		movementStates.Add("sprint", Godot.ResourceLoader.Load("characters/player/states/sprint.tres"));
		movementStates.Add("airborne", Godot.ResourceLoader.Load("characters/player/states/airborne.tres"));

		EmitSignal(SignalName.SetMovementState, movementStates["stand"]);
	}
	
	public override void _Process (double delta)
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
					if (!SFXFootsteps.Playing){
						SFXFootsteps.Play();
					}
				}
				else{
					EmitSignal(SignalName.SetMovementState, movementStates["walk"]);
					if (!SFXFootsteps.Playing){
						SFXFootsteps.Play();
					}
				}
			}else{
			EmitSignal(SignalName.SetMovementState, movementStates["stand"]);
		}
		}else{
			EmitSignal(SignalName.SetMovementState, movementStates["stand"]);
			SFXFootsteps.Stop();
		}
	}
}
