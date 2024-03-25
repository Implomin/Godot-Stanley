using Godot;
using System;
using System.Collections;

public partial class NarratorLogicComponent : Node
{
	private ArrayList playerActions = new ArrayList();

	public void OnSignalArrayRevcieved(string[] signal)
	{
		GD.Print("Narrator recieved signals: " + signal);
		playerActions.Add(signal);
	}
	public void OnSignalRecieved(String signal)
	{
		GD.Print("Narrator recieved signal: " + signal);
		playerActions.Add(signal);
	}
		public void OnImportantSignalRecieved(String signal)
	{
		GD.Print("Narrator recieved important signal: " + signal); // send this to the actual parser
/* 		printAllActions(); */
		// this may be cool in some cases idk
	}

	public override void _Process(double delta)
	{
		if(playerActions.Count > 6){
			GD.Print("The player has " + allActionsToString(playerActions));
			playerActions.Clear();
		}
	}

	private static String allActionsToString(ArrayList input){
		string res = "";
	for(int i = 0; i < input.Count; i++){
			res += input[i] + ", ";
		};
		return res;
	}
}
