using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using System.Net.Http;
using SimpleJSON;
using System.IO;

public class Chatter : MonoBehaviour
{
    public static string Server { get; set; } = "ec2-54-153-218-198.ap-southeast-2.compute.amazonaws.com";
    public static string Port { get; set; } = "7860";
    public static string URL { get; set; } = $"http://{Server}:{Port}/api/v1/chat";

    private static List<Message> History = new List<Message>(); // Variable to store conversation history
    private static HttpClient client = new HttpClient();
    private static string assistantMessage = null;// string.Empty; // Static variable to store the latest assistant message

    [Serializable]
    public class Message
    {
        public string role;
        public string content;
    }

    [Serializable]
    public class OobaParameters
    {
        public string user_input;
        public string mode;
        public string character;
        public List<Message> messages;
    }

    // Public static method to be called from another class
    public static void CallPromptAI(string userInput, string character, MonoBehaviour caller, Action<string> callback)
    {
        caller.StartCoroutine(PromptAI(userInput, character, callback));
    }

    private static IEnumerator PromptAI(string userInput, string character, Action<string> callback)
    {
        // Construct the message for the user input
        var userMessage = new Message { role = "user", content = userInput };
        History.Add(userMessage);

        // Construct the parameters with history and user input
        var oobaParams = new OobaParameters
        {
            user_input = userInput,
            mode = "chat",
            character = character,
            messages = History
        };

        // Serialize the parameters to JSON
        var oobaJson = JsonUtility.ToJson(oobaParams);
        Debug.Log($"Sending JSON payload: {oobaJson}");

        // Create UnityWebRequest
        using (UnityWebRequest request = new UnityWebRequest(URL, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(oobaJson);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send request and wait for response
            yield return request.SendWebRequest();

            // Check for errors
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                callback?.Invoke(null);
                yield break;
            }

            // Parse and log the response
            assistantMessage = ParseResponse(request.downloadHandler.text);
            Debug.Log("Assistant's message: " + assistantMessage);
            callback?.Invoke(assistantMessage);
        }
    }

    private static string ParseResponse(string jsonResponse)
    {
       // LLMResponse responseData = JsonUtility.FromJson<LLMResponse>(jsonResponse);
        Debug.Log("Reading response stream...");
       // var rawResponse = await streamReader.ReadToEndAsync();  // Read the entire response as a string
        Debug.Log($"Raw server response: {jsonResponse}");  // Log the raw response
        JSONNode root1 = JSON.Parse(jsonResponse);
        JSONNode visible = root1["results"][0]["history"]["visible"];
        string visibleValue = visible[0][1].Value;

        //this is the parsed json response
        JSONNode internalMessages = root1["results"][0]["history"]["internal"];
        string internalMessage = internalMessages[0][1].Value;

        Debug.Log("ROOT" + root1);
        Debug.Log("VIS " + visibleValue);
        Debug.Log("INTERNL " + internalMessage);
        // Extract the assistant's message content
        /*        if (responseData.choices != null && responseData.choices.Length > 0)
                {
                    return responseData.choices[0].message.content;
                }
                else
                {
                    Debug.LogError("No choices in the response");
                    return string.Empty;
                }*/
        return internalMessage;
    }


    // Harcoded for testing 

    /*  private static IEnumerator PromptAI(string userInput, MonoBehaviour caller, Action<string> callback)
      {
          assistantMessage = "They are to the right";
          yield return null;
          callback?.Invoke(assistantMessage);
      }
  */

    // Public static method to retrieve the latest assistant message
    //public static string GetAssistantMessage()
    // {
    //return assistantMessage;
    //}
}
