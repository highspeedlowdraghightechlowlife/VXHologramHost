using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using UnityEngine.Rendering;

public class NPC : MonoBehaviour
{
    private SpeechConfig speechConfig;
    private SpeechSynthesizer synthesizer;
    public Animator animator;


    private void Start()
    {
        // Set up the Speech Config
        //replace this with your API key and endpoint
        //this will NOT work right now if you try to run it, it was set up with a private API key, replace with RMIT azure key 
        string subscriptionKey = Key.subscriptionKey;
        string region = Key.region;
        speechConfig = SpeechConfig.FromSubscription(subscriptionKey, region);
        speechConfig.SpeechSynthesisVoiceName = "en-US-AriaNeural";
        speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw24Khz16BitMonoPcm);

        // Create the Speech Synthesizer
        synthesizer = new SpeechSynthesizer(speechConfig, null);
        animator = GetComponent<Animator>();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void ReadResult(string airesponse, ConversationResult res)
    {
        Debug.Log("read result has been called");
        string response = "Please say again.";
        animator.SetBool("IsTalking", false);

        if (res.result.prediction.topIntent == "TellMe")
        {
            animator.SetTrigger("IsTalking0");
            HelloWorld.Instance.SynthesizeSpeech(airesponse);

        }
        else if (res.result.prediction.topIntent == "Location")
        {
            animator.SetTrigger("IsTalking1");
            HelloWorld.Instance.SynthesizeSpeech(airesponse);

        }
        else
        {
            Debug.Log(response);
        }
    }
        #region comment
            /*        // Check if there is a result and if the top scoring intent is "TellMe"
                    if (res != null && res.result.prediction.topIntent == "TellMe")
                    {
                        // Loop through all the entities
                        foreach (var entity in res.result.prediction.entities)
                        {
                            if (entity.category == "Laboratory")
                            {
                                response = "This is the VXLab, where we have cutting edge equipment and facilities.";
                                // Call the method to synthesize speech only if the entity category is "Laboratory"
                                HelloWorld.Instance.SynthesizeSpeech(response);
                                animator.SetBool("IsTalking", true);
                            }
                            else if (entity.category == "Greeting")
                            {
                                response = "Hi, welcome!";
                                HelloWorld.Instance.SynthesizeSpeech(response);
                                animator.SetTrigger("WaveTrigger");
                            }
                            else if (entity.category == "Point")
                            {
                                response = (entity.text);
                                getDirection(entity.text);
                            }
                            else
                            {
                                response = "Sorry, I didn't catch that.";
                            }
                        }
                        Debug.Log(response);
                    }*/
            #endregion
    void getDirection(string directionText)
    {
        switch (directionText)
        {
            case "forward":
                animator.SetTrigger("Forward");
                break;
            case "behind":
                animator.SetTrigger("Behind");
                break;
            case "left":
                animator.SetTrigger("LeftTrigger");
                break;
            case "right":
                animator.SetTrigger("RightTrigger");
                break;
            case "somewhere":
                animator.SetTrigger("Somewhere");
                break;
        }

    }
}
