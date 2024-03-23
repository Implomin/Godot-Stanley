using Godot;
using System;
using System.Linq;

public partial class PromptParser : Node
{
	String currentPrompt;
	private void OnSendDataToParser(String[] data)
	{
		GD.Print(ParseElements(data));
	}

	public string ParseElements(String[] data){
		for(int i = 0; i < data.Length; i++){
			if(data[i] == "right"){
				currentPrompt += "player turned right, ";
			}
			else if(data[i] == "left"){
				currentPrompt += "player turned left, ";
			}
			else{
				currentPrompt += "player did something unexpected, ";
			}
		}
		return currentPrompt;
	}
}
