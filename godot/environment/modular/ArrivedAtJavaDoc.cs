using Godot;
using System;
using System.Reflection;
using Narrator;

public partial class ArrivedAtJavaDoc : Area3D
{
	[Export] public string action = "free-prompt";
	[Export] public string location = "picked up JavaDoc";
	[Export] private bool good = true;
	[Signal] public delegate void SendArrivedAtImportantEventHandler(String action, String location);

	public override void _Ready()
	{
		NarratorLogicComponent narrator = GetParent<NarratorLogicComponent>();
		Connect(SignalName.SendArrivedAtImportant, Callable.From(() => narrator.OnImportantSignalRecieved(action, location)));
	}
	public void _OnBodyEntered(PlayerRoot body)
	{
		EmitSignal(SignalName.SendArrivedAtImportant, null);
		if(good)
		{
			body.hasGoodJavaDoc = true;
		}
		else
		{
			body.hasBadJavaDoc = true;
		}
		QueueFree();
	}
}
