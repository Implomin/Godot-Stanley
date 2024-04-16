using Godot;
using System;
using System.Collections;
public partial class NarratorLogicComponent : Node
{
	private ArrayList playerActions = new ArrayList();
	//private readonly string BACKEND = "http://localhost:5051/voice";
	private readonly string BACKEND = "http://localhost:5051/prompt";


	public void OnSignalArrayRevcieved(string[] signal)
	{
		GD.Print("Narrator recieved signals: " + signal);
		playerActions.Add(signal);
	}
	// Change String to Object with action (enum) and value string
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
			HttpRequest httpRequest = GetNode<HttpRequest>("HTTPRequest");
			httpRequest.RequestCompleted += _on_http_request_request_completed;
			GD.Print("The player has " + allActionsToString(playerActions));
				Error err = httpRequest.Request(
				BACKEND,
				new string[]{"Content-Type: application/json"},
				HttpClient.Method.Post,
				"{\"actions\":[{\"action\": \"TURN\", \"value\":\"left\"}]}"
				);
				if (err != Error.Ok) {
					GD.PushError("An error occured: " + err);
				}
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

	private void _on_http_request_request_completed(long result, long response_code, string[] headers, byte[] body)
	{
		AudioStreamPlayer audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		AudioStreamWav wav = new Godot.AudioStreamWav();
		wav.Data = body;
		wav.Format = AudioStreamWav.FormatEnum.Format16Bits;
		wav.MixRate = 16000;
		audioStreamPlayer.Stream = wav;
		audioStreamPlayer.Play();
		GD.Print("RESPONSE CODE: " + response_code);
	}
}
