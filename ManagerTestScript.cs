using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Text;
using System.Xml.Linq;

public class ManagerTestScript : MonoBehaviour
{
    // url to send requests
    public string url;

    // LUIS subscription key
    public string subscriptionKey;

    // target to send requests to
    public NPC resultTarget;

    // event called when a command is ready to be sent
    public delegate void SendCommand(string command);
    public SendCommand onSendCommand;

    // called when the player starts to record their voice
    public System.Action onStartRecordVoice;

    // called when the player stops recording their voice
    public System.Action onEndRecordVoice;

    // instance
    public static ManagerTestScript instance;

    public SwitchCamera switcher;

    void Awake()
    {
        // set the instance to this script
        instance = this;
    }

    public void setNPC(NPC my_NPC)
    {
        resultTarget = my_NPC;
    }

    void OnEnable()
    {
        onSendCommand += OnSendCommand;
    }

    void OnDisable()
    {
        onSendCommand -= OnSendCommand;
    }

    // called when the command is ready to be sent
    void OnSendCommand(string command)
    {
        if (switcher.selectedVersion == 0)
        {
            StartCoroutine(SendRequest(command));
        }
        else
        {
            StartCoroutine(SendRequest1(command));
            //StartCoroutine(SendRequest1(command));
        }

    }

    IEnumerator SendRequest(string command)
    {
        Debug.LogError("started manager");
        if (string.IsNullOrEmpty(command))
            yield return null;

        //string originalText = "Tell me about the laboratory";
        string originalText = command;
        //replace with your keys
        string url = Key.url;
        string apiKey = Key.apiKey;
        string escapedText = Escape(originalText);
        string requestData = "{\"kind\":\"Conversation\",\"analysisInput\":{\"conversationItem\":{\"id\":\"PARTICIPANT_ID_HERE\",\"text\":\"" + command + "\",\"modality\":\"text\",\"language\":\"en\",\"participantId\":\"PARTICIPANT_ID_HERE\"}},\"parameters\":{\"projectName\":\"vlada-lang\",\"verbose\":true,\"deploymentName\":\"deployment1\",\"stringIndexType\":\"TextElement_V8\"}}";

        // string requestData1 = "{\"kind\":\"Conversation\",\"analysisInput\":{\"conversationItem\":{\"id\":\"PARTICIPANT_ID_HERE\",\"text\":" + escapedText + ",\"modality\":\"text\",\"language\":\"EN\",\"participantId\":\"PARTICIPANT_ID_HERE\"}},\"parameters\":{\"projectName\":\"TestApp\",\"verbose\":true,\"deploymentName\":\"mydeployment1\",\"stringIndexType\":\"TextElement_V8\"}}";
        string Escape(string text)
        {
            return "\"" + text.Replace("\"", "\\\"") + "\"";
        }
        //string requestData = "{\"kind\":\"Conversation\",\"analysisInput\":{\"conversationItem\":{\"id\":\"PARTICIPANT_ID_HERE\",\"text\":\"Tell me about the laboratory\",\"modality\":\"text\",\"language\":\"EN\",\"participantId\":\"PARTICIPANT_ID_HERE\"}},\"parameters\":{\"projectName\":\"TestApp\",\"verbose\":true,\"deploymentName\":\"mydeployment1\",\"stringIndexType\":\"TextElement_V8\"}}";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Ocp-Apim-Subscription-Key", apiKey);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                //CLUResult result = JsonUtility.FromJson<CLUResult>(webRequest.downloadHandler.text);
                Debug.Log("JSON Data: " + webRequest.downloadHandler.text);
                //ConversationResult conversationResult = JsonUtility.FromJson<ConversationResult>(jsonString);
                ConversationResult conversationResult = JsonUtility.FromJson<ConversationResult>(Encoding.Default.GetString(webRequest.downloadHandler.data));
                // Print the deserialized object
                Debug.Log("Kind: " + conversationResult.kind + " Query: " + conversationResult.result.query + " Top Intent: " + conversationResult.result.prediction.topIntent);
                if (conversationResult.result.prediction.entities != null)
                {
                    foreach (var entity in conversationResult.result.prediction.entities)
                    {
                        Debug.Log("Entity Category: " + entity.category + " text " + entity.text);

                    }
                }
                resultTarget.ReadResult(conversationResult);
            }
        }
    }

    IEnumerator SendRequest1(string command)
    {
        if (string.IsNullOrEmpty(command))
            yield return null;

        // string originalText = "Tell me about the laboratory";
        Chatter.CallPromptAI(command, "Vivi", this, OnAssistantMessageReceived);

        void OnAssistantMessageReceived(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Debug.Log("Received message from assistant: " + message);
                StartCoroutine(ProcessAssistantResponse(message));
            }
            else
            {
                Debug.LogError("Error receiving message from assistant.");
            }
        }
    }

    IEnumerator ProcessAssistantResponse(string message)
    {
        //replace this with your API key and endpoint
        //this will NOT work right now if you try to run it, it was set up with a private API key, replace with RMIT azure key 
        string url = Key.url;
        string apiKey = Key.apiKey;
        string escapedText = Escape(message);
        string originalText = message;
        //replace with your keys
        string requestData = "{\"kind\":\"Conversation\",\"analysisInput\":{\"conversationItem\":{\"id\":\"PARTICIPANT_ID_HERE\",\"text\":\"" + message + "\",\"modality\":\"text\",\"language\":\"en\",\"participantId\":\"PARTICIPANT_ID_HERE\"}},\"parameters\":{\"projectName\":\"vlada-lang\",\"verbose\":true,\"deploymentName\":\"deployment1\",\"stringIndexType\":\"TextElement_V8\"}}";

        // string requestData1 = "{\"kind\":\"Conversation\",\"analysisInput\":{\"conversationItem\":{\"id\":\"PARTICIPANT_ID_HERE\",\"text\":" + escapedText + ",\"modality\":\"text\",\"language\":\"EN\",\"participantId\":\"PARTICIPANT_ID_HERE\"}},\"parameters\":{\"projectName\":\"TestApp\",\"verbose\":true,\"deploymentName\":\"mydeployment1\",\"stringIndexType\":\"TextElement_V8\"}}";
        string Escape(string text)
        {
            return "\"" + text.Replace("\"", "\\\"") + "\"";
        }
        //string requestData = "{\"kind\":\"Conversation\",\"analysisInput\":{\"conversationItem\":{\"id\":\"PARTICIPANT_ID_HERE\",\"text\":\"Tell me about the laboratory\",\"modality\":\"text\",\"language\":\"EN\",\"participantId\":\"PARTICIPANT_ID_HERE\"}},\"parameters\":{\"projectName\":\"TestApp\",\"verbose\":true,\"deploymentName\":\"mydeployment1\",\"stringIndexType\":\"TextElement_V8\"}}";

        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestData);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Ocp-Apim-Subscription-Key", apiKey);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                //CLUResult result = JsonUtility.FromJson<CLUResult>(webRequest.downloadHandler.text);
                Debug.Log("JSON Data: " + webRequest.downloadHandler.text);
                //ConversationResult conversationResult = JsonUtility.FromJson<ConversationResult>(jsonString);
                ConversationResult conversationResult = JsonUtility.FromJson<ConversationResult>(Encoding.Default.GetString(webRequest.downloadHandler.data));
                // Print the deserialized object
                Debug.Log("Kind: " + conversationResult.kind + " Query: " + conversationResult.result.query + " Top Intent: " + conversationResult.result.prediction.topIntent);
                if (conversationResult.result.prediction.entities != null)
                {
                    foreach (var entity in conversationResult.result.prediction.entities)
                    {
                        Debug.Log("Entity Category: " + entity.category + " text " + entity.text);

                    }
                }


                resultTarget.ReadAIResult(message, conversationResult);
            }
        }
    }
}
