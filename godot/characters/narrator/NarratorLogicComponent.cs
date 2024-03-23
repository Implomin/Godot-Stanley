using Godot;
using System;
using System.Collections;

public partial class NarratorLogicComponent : Node
{
	private ArrayList playerActions = new ArrayList();
	public void OnSignalRecieved(String signal)
	{
		GD.Print("Narrator recieved signal: " + signal);
		playerActions.Add(signal);
	}
		public void OnImportantSignalRecieved(String signal)
	{
		GD.Print("Narrator recieved important signal: " + signal);
		GD.Print(playerActions);
	}
}
