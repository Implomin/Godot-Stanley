using Godot;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Narrator
{
	public partial class NarratorUtil{

	//private readonly string BACKEND = "http://localhost:5051/voice";



 	public static string allActionsToJsonString(ArrayList inputActions, ArrayList inputLocations){
		string res = "{\"actions\":[{\"action\": \""+inputActions[0]+"\", \"value\":\""+inputLocations[0]+"\"}";
		for(int i = 1; i < inputActions.Count; i++){
			//res += input[i] + ", ";
			res += ",{\"action\": \""+inputActions[i]+"\", \"value\":\""+inputLocations[i]+"\"}";
		};
		return res + "]}";
	}

	public static string allActionsToString(ArrayList inputActions, ArrayList inputLocations){
		string res = "";
		for(int i = 1; i < inputActions.Count; i++){
			res += inputActions[i] + " at " + inputLocations[i]+", ";
		};
		return res + "]}";
	}

	public static void sendPromptRequest(HttpRequest httpRequest, string request) {
		string BACKEND = "http://localhost:5051/prompt";
		Error err = httpRequest.Request(
		BACKEND,
		new string[]{"Content-Type: application/json"},
		HttpClient.Method.Post,
		request
		);
		if (err == Error.Busy) {
			GD.Print("Waiting for other request to complete");
		}
		if (err != Error.Ok) {
			GD.PushError("An error occured: " + err);
		}
	}

	public static void sendTextRequest(HttpRequest httpRequest) {
		string BACKEND = "http://localhost:5051/text";
		Error err = httpRequest.Request(
		BACKEND,
		new string[]{},
		HttpClient.Method.Get,
		""
		);
		if (err != Error.Ok) {
			GD.PushError("An error occured: " + err);
		}
	}


	private static string getTextFromArray(string[] textArray) {
		string result = "";
		for (int i = 0; i < textArray.Length;i++) {
			result += " " + textArray[i];
		}
		Array.Clear(textArray);
		return result;
	}

	public async static void iterateText(Label label, String text, float wavLength, Node timer) {
		// calculate number of words AND letters to make a better assumption
		int modifier = 4;
		string[] words = text.Split(" ");
		int wordsLength = words.Length;
		float result = (wordsLength / wavLength) * modifier;
		int wPS = (int) Math.Ceiling(result);

		string[] copiedWords = new string[wPS];
		int last = wordsLength % wPS;

		for (int i = 0; i < wavLength; i++) {
			if( wordsLength - (i * wPS) == last ) {
				Array.Copy(words, wPS * i, copiedWords, 0, last);
				label.Text = getTextFromArray(copiedWords);
				await timer.ToSignal(timer.GetTree().CreateTimer(modifier), SceneTreeTimer.SignalName.Timeout);
				label.Text = "";
				return;
			}else {
				Array.Copy(words, wPS * i, copiedWords, 0, wPS);
			}

			label.Text = getTextFromArray(copiedWords);
			await timer.ToSignal(timer.GetTree().CreateTimer(modifier), SceneTreeTimer.SignalName.Timeout);
		}
	}
	}
}
