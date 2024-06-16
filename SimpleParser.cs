using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleJSON;

[Serializable]
public class SimpleParser : MonoBehaviour
{
    public static string jsonResponse = @"
{
    ""results"": [{
        ""history"": {
            ""internal"": [
                [""Hello how are you"", ""I'm doing well, thank you. How can I help you?\n""]
            ],
            ""visible"": [
                [""Hello how are you"", ""I&#x27;m doing well, thank you. How can I help you?\n""]
            ]
        }
    }]
}";

 
    void Start()
    {
        Debug.Log(jsonResponse);
        ParseJson(jsonResponse);
    }

    void ParseJson(string jsonResponse)
    {
        //var root = JsonUtility.FromJson<Root>(jsonResponse);
        JSONNode root1 = JSON.Parse(jsonResponse);
        JSONNode visible = root1["results"][0]["history"]["visible"];
        string visibleValue = visible[0][1].Value;

        JSONNode internalMessages = root1["results"][0]["history"]["internal"];
        string internalMessage = internalMessages[0][1].Value;

        Debug.Log("ROOT" + root1);
        Debug.Log("VIS " + visibleValue);
        Debug.Log("INTERNL " + internalMessage);

    }
}
