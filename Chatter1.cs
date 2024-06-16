using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.Http;
using SimpleJSON;
using System.Text;
using System.Threading.Tasks;
using UniJSON;

public class Chatter1 : MonoBehaviour
{
    //this is the ec2 version of chatter for the LLM
    public static string Server { get; set; } = "ec2-54-153-218-198.ap-southeast-2.compute.amazonaws.com";
    public static string Port { get; set; } = "7860";
    public static string URL { get; set; } = $"http://{Server}:{Port}/api/v1/chat";

    private static List<Message> History = new List<Message>(); // Variable to store conversation history
    private static HttpClient client = new HttpClient();

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

    public async static void PromptAI(string user_input, string character)
    {
        print("tester " + user_input);

        // Construct the message for the user input
        var userMessage = new Message { role = "You", content = user_input };
        History.Add(userMessage);
        var oobaParams = new OobaParameters
        {
            user_input = user_input,
            character = character,
            mode = "chat",
            messages = History
        };

        // Serialize the parameters to JSON
        var oobaJson = JsonUtility.ToJson(oobaParams);
        Debug.Log($"Sending JSON payload: {oobaJson}");  // Log the JSON payload
        var content = new StringContent(oobaJson, Encoding.UTF8, "application/json");

        var message = new HttpRequestMessage(HttpMethod.Post, URL)
        {
            Content = content
        };

        await Task.Run(async () => { await ConnectToServerAndReadResponse(client, message); });
    }

    private static async Task ConnectToServerAndReadResponse(HttpClient httpClient, HttpRequestMessage message)
    {
        using (message)
        {
            try
            {
                var response = await httpClient.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);
                if (!response.IsSuccessStatusCode)
                {
                    Debug.LogError($"Failed to connect to server. Status code: {response.StatusCode}");
                    return;
                }

                using (var streamReader = new System.IO.StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    Debug.Log("Reading response stream...");
                    var rawResponse = await streamReader.ReadToEndAsync();  // Read the entire response as a string
                    Debug.Log($"Raw server response: {rawResponse}");  // Log the raw response
                    JSONNode root1 = JSON.Parse(rawResponse);
                    JSONNode visible = root1["results"][0]["history"]["visible"];
                    string visibleValue = visible[0][1].Value;

                    //this is the parsed json response
                    JSONNode internalMessages = root1["results"][0]["history"]["internal"];
                    string internalMessage = internalMessages[0][1].Value;

                    Debug.Log("ROOT" + root1);
                    Debug.Log("VIS " + visibleValue);
                    Debug.Log("INTERNL " + internalMessage);

                    // Process the response
                    var responseLines = rawResponse.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    foreach (var line in responseLines)
                    {
                        if (line.StartsWith("data:"))
                        {
                            string eventData = line.Substring(5);
                            Debug.Log($"Parsed event data: {eventData}");

                            // Assuming the response is a JSON object with the required structure
                            // Parse the response JSON
                            try
                            {
                                var jsonObject = JsonUtility.FromJson<RootObject>(eventData);
                                if (jsonObject != null && jsonObject.choices != null && jsonObject.choices.Count > 0)
                                {
                                    string appendText = jsonObject.choices[0].message.content;
                                    Debug.Log($"Parsed text: {appendText}");

                                    // Append AI response to history
                                    var aiMessage = new Message { role = "Vivi", content = appendText };
                                    History.Add(aiMessage);

                                    // For simplicity, let's just log the entire history
                                    foreach (var msg in History)
                                    {
                                        Debug.Log($"{msg.role}: {msg.content}");
                                    }
                                }
                                else
                                {
                                    Debug.LogWarning("Received JSON is null or does not contain choices.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.LogError($"Error parsing JSON response: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Exception occurred while connecting to server: {ex.Message}");
            }
        }
    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    [Serializable]
    private class RootObject
    {
        public List<Choice> choices;
    }

    [Serializable]
    private class Choice
    {
        public Message message;
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            PromptAI("Hello how are you", "Vivi");
        }
    }

}
