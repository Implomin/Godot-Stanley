using Godot;
using System;

public partial class SaveLogic : Node
{
	[Export] CharacterBody3D player;
	[Export] Node3D cameraNode;

	float startRotation = 0;
	float rotated;
	//min 2*length of string as length in seconds, length not added when plater is static
	String[] savedData;
	[Export] int singleArrayDataLimit = 10;
	int increment = 0;

	[Signal] public delegate void SendDataToParserEventHandler(String[] data);

	public override void _Ready()
	{
		savedData = new String[singleArrayDataLimit];
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Engine.GetPhysicsFrames() % 120 == 0){
			rotated = startRotation - cameraNode.Rotation.Y;
			GD.Print("rotated: " + rotated);
			startRotation = cameraNode.Rotation.Y;
			if(singleArrayDataLimit > increment){
				//TODO handle external signals when player reaches a specific point / some action takes place
				/* if(externalEvent){
					increment++
				}
				else */ if(rotated > 1.2)
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
				increment = 0;
				EmitSignal(SignalName.SendDataToParser, savedData);
			}
		}
	}
}
