using Godot;
using System;
using System.Reflection;
using Narrator;

public partial class ArrivedAt : Area3D
{
	[Export] public string action = "unexpected";
	[Export] public string location = "unexpected";
	[Export] public bool important = false;
	[Export] public bool oneTime = false;
	private bool notDone = true;
	[Signal] public delegate void SendArrivedAtEventHandler(String action, String location);
	[Signal] public delegate void SendArrivedAtImportantEventHandler(String action, String location);

	public override void _Ready()
	{
		NarratorLogicComponent narrator = GetParent<NarratorLogicComponent>();
		Connect(SignalName.SendArrivedAtImportant, Callable.From(() => narrator.OnImportantSignalRecieved(action, location)));
		Connect(SignalName.SendArrivedAt, Callable.From(() => narrator.OnSignalRecieved(action, location)));

	}
	public void _OnBodyEntered(Godot.CharacterBody3D body)
	{
		//GD.Print("Player - body entered arrivedAt; Sending signal to corresponding node");
		
		if(notDone){
			if(oneTime){
				notDone = false;
		}
		if(important){
			EmitSignal(SignalName.SendArrivedAtImportant, null);
		}else{
			EmitSignal(SignalName.SendArrivedAt, null);
		}
		}
	}
}
