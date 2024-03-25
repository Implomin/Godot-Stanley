using Godot;
using System;

public partial class ArrivedAt : Area3D
{
	[Export] public string arrivedAtContent = "unexpected";
	[Export] public bool important = false;
	[Signal] public delegate void SendArrivedAtEventHandler(String arrivedAtContent);
	[Signal] public delegate void SendArrivedAtImportantEventHandler(String arrivedAtContent);

	public void _OnBodyEntered(Godot.CharacterBody3D body)
	{
		//GD.Print("Player - body entered arrivedAt; Sending signal to corresponding node");
		if(important){
			EmitSignal(SignalName.SendArrivedAtImportant, arrivedAtContent);
		}else{
			EmitSignal(SignalName.SendArrivedAt, arrivedAtContent);
		}
	}
}
