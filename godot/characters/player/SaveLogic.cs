using Godot;
using System;

public partial class SaveLogic : Node
{

	private bool lockedFps = false;
	public override void _Process(double delta) {
		if(Input.IsActionJustPressed("ui_esc"))
		{
			GetTree().Quit();
		}

		if(Input.IsActionJustPressed("ui_x"))
		{
			lockedFps = !lockedFps;
			if(lockedFps)
			{
				Engine.MaxFps = 60;
			}
			else{
				Engine.MaxFps = 0;
			}
		}
	}
	/* [Export] CharacterBody3D player;
	[Export] Node3D cameraNode;
	[Export] int randomnessFactor = 10;
	Random random = new Random();

	float startRotation = 0;
	float rotated;
	//min 2*length of string as length in seconds, length not added when plater is static
	String[] savedData;
	[Export] int singleArrayDataLimit = 10;
	int increment = 0; */

	/* [Signal] public delegate void SendDataToLogicHandlerEventHandler(String[] data); */

	/* public override void _Ready()
	{
		savedData = new String[singleArrayDataLimit];
	} */

	/* public override void _PhysicsProcess(double delta)
	{
		if (Engine.GetPhysicsFrames() % 120 == 0){
			rotated = startRotation - cameraNode.Rotation.Y;
			//GD.Print("rotated: " + rotated);
			startRotation = cameraNode.Rotation.Y;
			if(singleArrayDataLimit > increment){
				//TODO handle external signals when player reaches a specific point / some action takes place
				 if(externalEvent){
					increment++
				}
				else if(rotated > 1.2)
				{
					//GD.Print("rotated right");
					savedData[increment] = "right";
					increment++;
				}
				else if(rotated < -1.2)
				{
					//GD.Print("rotated left");
					savedData[increment] = "left";
					increment++;
				}
			}else{
				//GD.Print(savedData);
				if(random.Next(1, randomnessFactor) <= 1){
					EmitSignal(SignalName.SendDataToLogicHandler, savedData);
				}
				increment = 0;
			}
		}
	} */
}
