using Godot;
using System;
using System.Reflection;

public partial class ArrivedAt : Area3D
{
	[Export] public string arrivedAtContent = "unexpected";
	[Export] public bool important = false;
	[Signal] public delegate void SendArrivedAtEventHandler(String arrivedAtContent);
	[Signal] public delegate void SendArrivedAtImportantEventHandler(String arrivedAtContent);

	public override void _Ready()
	{
		NarratorLogicComponent narrator = GetParent<NarratorLogicComponent>();
		Connect(SignalName.SendArrivedAtImportant, Callable.From(() => narrator.OnImportantSignalRecieved(arrivedAtContent)));
		Connect(SignalName.SendArrivedAt, Callable.From(() => narrator.OnSignalRecieved(arrivedAtContent)));

	}
	public void _OnBodyEntered(Godot.CharacterBody3D body)
	{
		//GD.Print("Player - body entered arrivedAt; Sending signal to corresponding node");
		if(important){
			
			EmitSignal(SignalName.SendArrivedAtImportant, null);
		}else{
			EmitSignal(SignalName.SendArrivedAt, null);
		}
	}
}
