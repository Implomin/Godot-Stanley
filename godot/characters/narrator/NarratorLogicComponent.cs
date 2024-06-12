using Godot;
using System;
using System.Collections;
using Narrator;
using System.Linq;
using System.Threading;
public partial class NarratorLogicComponent : Node
{
	private ArrayList playerActions = new ArrayList();
	private ArrayList playerLocations = new ArrayList();
	private bool importantAction = false;
	private String importantSignalAction = "";
	private String importantSignalLocation = "";
	private HttpRequest httpRequest;
	private Label label;
	private AudioStreamPlayer audioStreamPlayer;
	private AudioStreamWav wav;
	[Export] AudioStreamPlayer MUSOffice;

	public override void _Ready()
	{
		base._Ready();
		httpRequest = GetNode<HttpRequest>("HTTPRequest");
		audioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		wav = new Godot.AudioStreamWav();
		wav.Format = AudioStreamWav.FormatEnum.Format16Bits;
		wav.MixRate = 16000;
		label = GetNode<Label>("Label");
		MUSOffice.Play();
	}

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
			NarratorUtil.sendPromptRequest(httpRequest, "{\"actions\":[{\"action\": \""+importantSignalAction+"\", \"value\":\""+importantSignalLocation+"\"},]}");
			GD.Print("The player has " + importantSignalAction + " at " + importantSignalLocation);
			importantAction = false;
			
		}
		if(playerActions.Count > 6){
			GD.Print("The player has " + NarratorUtil.allActionsToString(playerActions, playerLocations));
			NarratorUtil.sendPromptRequest(httpRequest, NarratorUtil.allActionsToJsonString(playerActions, playerLocations));
			playerActions.Clear();
			playerLocations.Clear();
		}
	}

	private void playSound(byte[] body)
	{

	}

	
	private async void _on_http_request_request_completed(long result, long response_code, string[] headers, byte[] body)
	{
		if (headers.Contains("Content-Type: text/plain")){
			// add text to label
			GD.Print("RESPONSE CODE FROM TEXT: " + response_code);

			String text = System.Text.Encoding.UTF8.GetString(body).Replace("\n", "");
			NarratorUtil.iterateText(label, text, (float) wav.GetLength(), this);
			return;
		}

		if (headers.Contains("Content-Type: audio/wav")) {
			GD.Print("RESPONSE CODE FROM PROMPT: " + response_code);
			
			wav.Data = body;
			GD.Print("Audio length: " + wav.GetLength());
			NarratorUtil.sendTextRequest(httpRequest);
			audioStreamPlayer.Stream = wav;
			audioStreamPlayer.Play();
		}

		
	}
}
