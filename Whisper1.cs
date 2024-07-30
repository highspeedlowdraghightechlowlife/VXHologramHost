using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using System.Collections;
using System.Threading.Tasks;
using System.IO;

public class Whisper1 : MonoBehaviour
{
    public Text outputText;
    public Button startRecoButton;
    public Image progressBar;

    private object threadLocker = new object();
    private bool waitingForReco;
    private string message;
    private float time;
    private readonly int duration = 15;

    private bool micPermissionGranted = false;
    private bool commandReadyToSend;
    private string curCommand;

    private SpeechRecognizer speechRecognizer;
    private SpeechConfig speechConfig;
    private bool isListening = false;
    private bool keywordDetected = false;
    private readonly string keyword = "Hi Vivi"; // The keyword to listen for
    private string modelFilePath;

    private void Start()
    {
        if (outputText == null)
        {
            UnityEngine.Debug.LogError("outputText property is null! Assign a UI Text element to it.");
        }
        else if (startRecoButton == null)
        {
            message = "startRecoButton property is null! Assign a UI Button to it.";
            UnityEngine.Debug.LogError(message);
        }
        else
        {
            micPermissionGranted = true;
            message = "Speak into your microphone.";
            startRecoButton.onClick.AddListener(StartListening);
        }

        // Handle the model file path
        string modelFile = "669dc1cf-9942-41f9-baa5-273e674e71ce.table";
        string persistentFilePath = Path.Combine(Application.persistentDataPath, modelFile);

        if (!File.Exists(persistentFilePath))
        {
            Debug.Log("Model file not in Persistent path");
            string streamingAssetPath = Path.Combine(Application.streamingAssetsPath, modelFile);

            // Load from StreamingAssets and copy to persistent data path
            File.Copy(streamingAssetPath, persistentFilePath);
            Debug.Log("Model file copied to Persistent path");
        }

        modelFilePath = persistentFilePath;

        // Initialize speech configuration
        speechConfig = SpeechConfig.FromSubscription(Key.subscriptionKey, Key.region);
    }

    private void Update()
    {
        lock (threadLocker)
        {
            if (startRecoButton != null)
            {
                startRecoButton.interactable = !waitingForReco && micPermissionGranted;
            }

            if (outputText != null)
            {
                outputText.text = message;
            }

            if (commandReadyToSend)
            {
                commandReadyToSend = false;
                CommandCompleted();
            }
        }
    }

    private async void StartListening()
    {
        if (isListening) return;

        isListening = true;
        Debug.Log("Speak into your microphone.");

        await ContinuousRecognitionWithKeywordSpottingAsync().ConfigureAwait(false);
    }

    public async Task ContinuousRecognitionWithKeywordSpottingAsync()
    {

        var model = KeywordRecognitionModel.FromFile((string)modelFilePath);
        // Creates a speech recognizer using microphone as audio input.
        using (var recognizer = new SpeechRecognizer(speechConfig))
        {
            // Subscribes to events.
            recognizer.Recognizing += (s, e) =>
            {
                if (e.Result.Reason == ResultReason.RecognizingKeyword)
                {
                    Debug.Log($"RECOGNIZING KEYWORD: Text={e.Result.Text}");
                }
                else if (e.Result.Reason == ResultReason.RecognizingSpeech)
                {
                    Debug.Log($"RECOGNIZING: Text={e.Result.Text}");
                }
            };

            recognizer.Recognized += (s, e) =>
            {
                var result = e.Result;
                var transcription = result.Text.Trim();
                Debug.Log(transcription);

                if (result.Reason == ResultReason.RecognizedKeyword)
                {
                    Debug.Log($"RECOGNIZED KEYWORD: Text={e.Result.Text}");
                    Debug.Log("Keyword recognized, waiting for command...");
                    message = "Keyword recognized...";
                    keywordDetected = true; // Set the flag when keyword is recognized
                }
                else if (result.Reason == ResultReason.RecognizedSpeech)
                {
                    Debug.Log("Recognized: " + transcription);
                    if (keywordDetected && isListening)
                    {
                        // Remove the keyword from the transcription
                        string command = transcription.Replace(keyword, "").Trim();
                        curCommand = command;
                        commandReadyToSend = true;
                        keywordDetected = false; // Reset the flag after capturing the command
                    }
                }
                else if (result.Reason == ResultReason.NoMatch)
                {
                    message = "No speech could be recognized.";
                    Debug.Log("No speech could be recognized.");
                }
                else if (result.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(result);
                    string newMessage = $"CANCELED: Reason={cancellation.Reason} ErrorDetails={cancellation.ErrorDetails}";
                    Debug.Log(newMessage);
                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Debug.LogError("Error details: " + cancellation.ErrorDetails);
                        Debug.LogError("Did you set the speech resource key and region values?");
                        isListening = false;
                        message = "Recognition stopped due to an error.";
                    }
                }
            };

            recognizer.Canceled += (s, e) =>
            {
                Debug.Log($"CANCELED: Reason={e.Reason}");

                if (e.Reason == CancellationReason.Error)
                {
                    Debug.Log($"CANCELED: ErrorCode={e.ErrorCode}");
                    Debug.Log($"CANCELED: ErrorDetails={e.ErrorDetails}");
                    Debug.Log($"CANCELED: Did you update the subscription info?");
                }
            };

            recognizer.SessionStarted += (s, e) =>
            {
                Debug.Log("Session started event.");
            };

            recognizer.SessionStopped += (s, e) =>
            {
                Debug.Log("Session stopped event.");
                Debug.Log("Stop recognition.");
                message = "Received: " + curCommand; // Update the message with the command
                //message = "Listening...";
            };

            // Starts continuous recognition using the keyword model.
            await recognizer.StartKeywordRecognitionAsync(model).ConfigureAwait(false);

            // Continue listening indefinitely
            while (isListening)
            {
                await Task.Delay(100); // Small delay to prevent tight loop
            }

            // Stops recognition.
            await recognizer.StopKeywordRecognitionAsync().ConfigureAwait(false);
        }
    }

    private void CommandCompleted()
    {
        ManagerTestScript.instance.onSendCommand(curCommand);
    }

    private void OnDestroy()
    {
        speechRecognizer?.Dispose();
        isListening = false; // Ensure listening stops if the object is destroyed
    }
}
