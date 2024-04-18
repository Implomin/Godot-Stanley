using Godot;
using System;
using System.Collections;

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

    public static void sendRequest(HttpRequest httpRequest, string request) {
		string BACKEND = "http://localhost:5051/prompt";
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
    }
}