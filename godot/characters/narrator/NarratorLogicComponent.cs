using Godot;
using System;
using System.Collections;
using Narrator;
public partial class NarratorLogicComponent : Node
{
	private ArrayList playerActions = new ArrayList();
	private ArrayList playerLocations = new ArrayList();
	private bool importantAction = false;
	private String importantSignalAction = "";
	private String importantSignalLocation = "";


	public void OnSignalArrayRevcieved(string[] signal)
	{
		GD.Print("Narrator recieved signals: " + signal);
		playerActions.Add(signal);
		playerLocations.Add(signal);
	}
	// Change String to Object with action (enum) and value string
	public void OnSignalRecieved(String signalAction, String signalLocation)
	{
		GD.Print("Signal: " + signalAction + "/" + signalLocation);
		playerActions.Add(signalAction);
		playerLocations.Add(signalLocation);

	}

	public void OnImportantSignalRecieved(String signalAction, String signalLocation)
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
			sendRequest("{\"actions\":[{\"action\": \""+importantSignalAction+"\", \"value\":\""+importantSignalLocation+"\"},]}");
			GD.Print("The player has " + importantSignalAction + " at " + importantSignalLocation);
			importantAction = false;
			
		}
		if(playerActions.Count > 6){
			GD.Print("The player has " + NarratorUtil.allActionsToString(playerActions, playerLocations));
			sendRequest(NarratorUtil.allActionsToJsonString(playerActions, playerLocations));
			playerActions.Clear();
			playerLocations.Clear();
		}
	}


	public void sendRequest(string request) {
		string BACKEND = "http://localhost:5051/prompt";
		HttpRequest httpRequest = GetNode<HttpRequest>("HTTPRequest");
		Error err = httpRequest.Request(
		BACKEND,
		new string[]{"Content-Type: application/json"},
		HttpClient.Method.Post,
		request
		);
		if (err != Error.Ok) {
			GD.PushError("An error occured: " + err);
		}
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
