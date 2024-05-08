using Godot;
using System;

public partial class PlayerCameraHandler : Node3D
{
    private float yaw = 0f;
	private float pitch = 0f;
	[Export] private float yawSensitivity = 0.002f;
	[Export] private float pitchSensitivity = 0.002f;
	[Export] private float yawAcceleration = 55f;
	[Export] private float pitchAcceleration = 55f;
	[Signal] public delegate void SetCameraRotationEventHandler(float cameraRotation);


	public override void _Ready(){
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
    public override void _Input(InputEvent @event)
    {
		if(@event is InputEventMouseMotion){
			InputEventMouseMotion mouseEvent = (InputEventMouseMotion)@event;
			yaw += mouseEvent.Relative.X * yawSensitivity;
			yaw = Mathf.Wrap(yaw, -180, 180);

			pitch += mouseEvent.Relative.Y * pitchSensitivity;
			pitch = Mathf.Clamp(pitch, -1.5f, 1.5f);
		}
    }

	public override void _Process(double delta){
		Rotation = new Vector3(pitch, -yaw, 0);
		EmitSignal(SignalName.SetCameraRotation, Rotation.Y);
	}
}
