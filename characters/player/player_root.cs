using Godot;
using System;
using System.Collections.Generic;

public partial class player_root : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	public Vector2 inputVector; 
	[Signal] public delegate void SetMovementStateEventHandler(movementState movementState);
	[Export] Dictionary movementStates;


	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

    public override void _Input(InputEvent @event)
    {
		if (Input.IsActionPressed("movement")){
			inputVector = Input.GetVector("movement_right", "movement_left", "movement_backward", "movement_forward");
			if (inputVector != Vector2.Zero){
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
}
