using Godot;
using System;
using System.Reflection;
using Narrator;

public partial class ArrivedAtEnd : Area3D
{
	[Export] public string action = "ARRIVE";
	[Export] public string location = "his own desk.";
	[Export] private bool good = true;
	[Signal] public delegate void SendArrivedAtImportantEventHandler(String action, String location);
	[Export] PlayerRoot player;
	[Export] OmniLight3D light;

	public override void _Process(double delta) {
		base._Process(delta);
		if(player.hasBadJavaDoc || player.hasGoodJavaDoc){
			light.Set("light_energy", 4f);
		}
	}

	public void _OnBodyEntered(PlayerRoot body)
	{
		if(player.hasBadJavaDoc || player.hasGoodJavaDoc){
			if(player.hasBadJavaDoc)
				{
					location += "He has disobeyed you, since he picked up the bad JavaDoc. ";
				}
				if(player.hasGoodJavaDoc)
				{
					location += "He has picked up the right JavaDoc. ";
				}
				location += "Based on this information, you can decide whether to fire him or not.";
			NarratorLogicComponent narrator = GetParent<NarratorLogicComponent>();
			Connect(SignalName.SendArrivedAtImportant, Callable.From(() => narrator.OnImportantSignalRecieved(action, location)));
			EmitSignal(SignalName.SendArrivedAtImportant, null);
		}
	}
}
