using Godot;
using System;
using System.Collections;
public partial class NarratorLogicComponent : Node
{
	private ArrayList playerActions = new ArrayList();
	private ArrayList playerLocations = new ArrayList();

	//private readonly string BACKEND = "http://localhost:5051/voice";
	private readonly string BACKEND = "http://localhost:5051/prompt";

	private bool importantAction = false;
	private string importantSignalAction = "";
	private string importantSignalLocation = "";


	public void OnSignalArrayRevcieved(string[] signal)
	{
		GD.Print("Narrator recieved signals: " + signal);
		playerActions.Add(signal);
		playerLocations.Add(signal);
	}
	// Change String to Object with action (enum) and value string
	public void OnSignalRecieved(String signalAction, string signalLocation)
	{
		GD.Print("Signal: " + signalAction + "/" + signalLocation);
		playerActions.Add(signalAction);
		playerLocations.Add(signalLocation);

	}

	public void OnImportantSignalRecieved(String signalAction, string signalLocation)
	{
		GD.Print("Important signal: " + signalAction + "/" + signalLocation);
		importantSignalAction = signalAction;
		importantSignalLocation = signalLocation;
		importantAction = true;
/* 		printAllActions(); */
		// this may be cool in some cases idk
	}


	public override void _Process(double delta)
	{
		if(importantAction){
			HttpRequest httpRequest = GetNode<HttpRequest>("HTTPRequest");
			httpRequest.RequestCompleted += _on_http_request_request_completed;
			GD.Print("The player has " + importantSignalAction + " at " + importantSignalLocation);
				Error err = httpRequest.Request(
				BACKEND,
				new string[]{"Content-Type: application/json"},
				HttpClient.Method.Post,
				//"{\"actions\":[{\"action\": \""+importantAction+"\", \"value\":\""+importantLocation+"\"}]}"
				"{\"actions\":[{\"action\": \""+importantSignalAction+"\", \"value\":\""+importantSignalLocation+"\"},]}"
				);
				if (err != Error.Ok) {
					GD.PushError("An error occured: " + err);
				}
			importantAction = false;
			
		}else{
		if(playerActions.Count > 6){
			HttpRequest httpRequest = GetNode<HttpRequest>("HTTPRequest");
			httpRequest.RequestCompleted += _on_http_request_request_completed;
			GD.Print("The player has " + allActionsToString(playerActions, playerLocations));
				Error err = httpRequest.Request(
				BACKEND,
				new string[]{"Content-Type: application/json"},
				HttpClient.Method.Post,
				allActionsToJsonString(playerActions, playerLocations)
				);
				if (err != Error.Ok) {
					GD.PushError("An error occured: " + err);
				}
			playerActions.Clear();
			playerLocations.Clear();
		}

		}
	}

 	private static String allActionsToJsonString(ArrayList inputActions, ArrayList inputLocations){
		string res = "{\"actions\":[{\"action\": \""+inputActions[0]+"\", \"value\":\""+inputLocations[0]+"\"}";
	for(int i = 1; i < inputActions.Count; i++){
			//res += input[i] + ", ";
			res += ",{\"action\": \""+inputActions[i]+"\", \"value\":\""+inputLocations[i]+"\"}";
		};
		return res + "]}";
	}

	 	private static String allActionsToString(ArrayList inputActions, ArrayList inputLocations){
		string res = "";
	for(int i = 1; i < inputActions.Count; i++){
			res += inputActions[i] + " at " + inputLocations[i]+", ";
		};
		return res + "]}";
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
