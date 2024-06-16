//Vladislava Simakov
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.CognitiveServices.Speech;
using UnityEngine.Rendering;
using WebSocketSharp;
using System.Collections;
using System.Reflection;
using UniVRM10;
using Unity.VisualScripting;

public class NPC : MonoBehaviour
{
    private SpeechConfig speechConfig;
    private SpeechSynthesizer synthesizer;
    public Animator animator;
    public Text output_text;
    public Vector3 avator_posi;

    public enum Direction
    {
        FRONT = 0,
        RIGHT = 90,
        BACK = 180,
        LEFT = 270
    }

    private void Start()
    {
        // replace this with your key 
        speechConfig = SpeechConfig.FromSubscription(Key.subscriptionKey, Key.region);
        speechConfig.SpeechSynthesisVoiceName = "en-US-AriaNeural";
        speechConfig.SetSpeechSynthesisOutputFormat(SpeechSynthesisOutputFormat.Raw24Khz16BitMonoPcm);

        // Create the Speech Synthesizer
        synthesizer = new SpeechSynthesizer(speechConfig, null);
        animator = GetComponent<Animator>();

        avator_posi = new Vector3(0, 0, 0);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void ReadResult(ConversationResult res)
    {
        Debug.Log("read result has been called");
        string topIntent = res.result.prediction.topIntent;
        string response = "Please say again.";


        // Check if there is a result and if the top scoring intent is "TellMe"
        if (res != null && res.result.prediction.topIntent == "TellMe")
        {
            // Loop through all the entities
            foreach (var entity in res.result.prediction.entities)
            {

                    if (entity.category == "Greeting")
                    {
                        response = "Hi, welcome!";
                        animator.SetTrigger("WaveTrigger");
                        StartCoroutine(UpdateOutputText(response));
                        HelloWorld.Instance.SynthesizeSpeech(response);                  

                    break;
                    }
                    else if (entity.category == "Management")
                    {
                        response = "Please contact Dr Ian Peake, lab manager, for all booking and access enquiries.";
                        StartCoroutine(UpdateOutputText(response));
                        HelloWorld.Instance.SynthesizeSpeech(response);
                        animator.SetTrigger("WaveTrigger");
                    break;
                    }
                    else if (entity.category == "Opening Hours")
                    {
                        response = "The official hours are Monday to Friday, nine to five. After-hours and weekend access can be arranged.";
                        StartCoroutine(UpdateOutputText(response));
                        HelloWorld.Instance.SynthesizeSpeech(response);
                        animator.SetTrigger("Offer");
                    break;
                    }
                    else if (entity.category == "Staff")
                    {
                        response = "Dr Ian Peake is the digital solutions architect, and Dr James Harland is the director of operations.";
                            HelloWorld.Instance.SynthesizeSpeech(response);
                            animator.SetTrigger("WaveTrigger");
                        break;
                    }
                    else if (entity.category == "Identity")
                    {
                        response = "My name is Vivienne, your virtual concierge. I am here to assist users and visitors of the VXLab at RMIT. Please speak clearly when you address me.";
                    StartCoroutine(UpdateOutputText(response));
                        HelloWorld.Instance.SynthesizeSpeech(response);
                    string trigger = "IsTalking" + getRandomTrigger(1);
                    string trigger1 = trigger;
                    animator.SetTrigger(trigger1);
                    break;
                    }
                    else if (entity.category == "Robots")
                    {
                        response = "Robotics is one of the main research interests of this lab, and we have several robots here. Rosie, the two-armed swordfighting robot, and Tiago which has an extendable torso and a manipulator arm . We also have an industrial robot arm.";
                    StartCoroutine(UpdateOutputText(response));
                        HelloWorld.Instance.SynthesizeSpeech(response);
                        string trigger = "IsTalking" + getRandomTrigger(1);
                        string trigger1 = trigger;
                        animator.SetTrigger(trigger1);
                        break;
                    }
                    else if (entity.category == "NOVA ball")
                    {
                        response = "The NOVA motion simulator is a virtual vehicle, combining virtual reality with unlimited motion. It has a three hundred and sixty degree range of motion and is used for flight and racing simulation.";
                    StartCoroutine(UpdateOutputText(response));
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    string trigger = "IsTalking" + getRandomTrigger(1);
                    string trigger1 = trigger;
                    animator.SetTrigger(trigger1);
                    break;
                    }
                    else if (entity.category == "RACE Hub")
                    {
                        response = "The R-M-I-T AWS Supercomputing Hub is a commercial cloud supercomputing facility. It is used for research, digital innovation, education and industry applications, as well as robot soccer and deep racing.";
                    StartCoroutine(UpdateOutputText(response));
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    string trigger = "IsTalking" + getRandomTrigger(1);
                    string trigger1 = trigger;
                    animator.SetTrigger(trigger1);
                    break;
                    }
                    else if (entity.category == "Virtual Reality")
                    {
                        response = "Virtual, augmented and annotated reality is one of the research areas of the VX Lab. This includes use of the MetaQuest Pro, surveying, instruction, science, gaming and industry.";
                   
                    StartCoroutine(UpdateOutputText(response));
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    string trigger = "IsTalking" + getRandomTrigger(1);
                    string trigger1 = trigger;
                    animator.SetTrigger(trigger1);
                    break;
                    }
                    else if (entity.category == "Purpose")
                    {
                    response = "The VXLab, or Virtual Experiences Laboratory, is a multi-disciplinary virtual laboratory connecting visualisation and automation facilities in RMIT and industry. We are always working on something new.";
                    StartCoroutine(UpdateOutputText(response));
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    string trigger = "IsTalking" + getRandomTrigger(1);
                    string trigger1 = trigger;
                    animator.SetTrigger(trigger1);
                    break;
                    }
                    else if (entity.category == "Rokoko Smartsuits")
                    {
                        response = "We have facilities that support custom motion capture, using the Rokoko Smartsuit Pro. Please find them in the Gov Lab";
                    StartCoroutine(UpdateOutputText(response));
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    string trigger = "IsTalking" + getRandomTrigger(1);
                    string trigger1 = trigger;
                    animator.SetTrigger(trigger1);
                    break;
                    }
                    else if (entity.category == "Laboratory")
                    {
                        response = "This is the Virtual Experiences  Lab, where we have cutting edge equipment and facilities.";
                    StartCoroutine(UpdateOutputText(response));
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    string trigger = "IsTalking" + getRandomTrigger(1);
                    string trigger1 = trigger;
                    animator.SetTrigger(trigger1);
                    break;
                    }
                    else if (entity.category == "GOV Lab")
                    {
                        response = "The Gov Lab, or Global Operations Visualization Laboratory, is used for data visualisation and space exploration. It features a tiled display driven by rendering middleware like SAGE and Google Liquid Galaxy.";
                    StartCoroutine(UpdateOutputText(response));
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    string trigger = "IsTalking" + getRandomTrigger(1);
                    string trigger1 = trigger;
                    animator.SetTrigger(trigger1);

                    break;
                    }
                    else if (entity.category == "Tour")
                    {
                        //this is hard coded because I didn't feel like looking up five objects individually
                        response = "Welcome to the Virtual Experiences lab! I am speaking to you from a 32-inch lenticular display. In the race hub, the room parallel to this one on the left, you" +
                            " will find the 60-inch screen with a lifesize version of me, as well as the Amazon Web Services supercomputing hub. If you'll look behind me, you'll find the GOV lab" +
                            " which uses state of the art displays for data visualisation. To your right, you'll see Rosie and Tiago, our other residents. Next to them is the planar display screen."
                            + " Finally, behind you in the corner is our unlimited motion flight simulator. Ask me for more details!";
                        StartCoroutine(UpdateOutputText("Welcome to the VXLab!"));
                        HelloWorld.Instance.SynthesizeSpeech(response);
                        StartCoroutine(TriggerAnimations());
                     break;
                    }
                    //legacy
                    else if (entity.category == "Point")
                    {
                        animator.SetTrigger("Reset");
                        response = (entity.text);
                        getDirection(entity.text);
                    }
                    else
                    {
                        response = "Sorry, I didn't catch that.";
                        StartCoroutine(UpdateOutputText("..."));
                        HelloWorld.Instance.SynthesizeSpeech(response);
                    }



                #region legacy
                /*
                if (entity.category == "Greeting")
                {
                    response = "Hi, welcome!";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetTrigger("WaveTrigger");
                    break;
                }
                else if (entity.category == "Management")
                {
                    response = "Please contact Dr Ian Peake, lab manager, for all booking and access enquiries.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetTrigger("WaveTrigger");
                    break;
                }
                else if (entity.category == "Opening Hours")
                {
                    response = "The official hours are Monday to Friday, nine to five. After-hours and weekend access can be arranged.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                    break;
                }
                else if (entity.category == "Staff")
                {
                    response = "Dr Ian Peake is the digital solutions architect, and Dr James Harland is the director of operations.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetTrigger("WaveTrigger");
                    break;
                }
                else if (entity.category == "Identity")
                {
                    response = "My name is Vivienne, your virtual concierge. I am here to assist users and visitors of the VXLab at RMIT. Please speak clearly when you address me.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                    break;
                }
                else if (entity.category == "NOVA ball")
                {
                    response = "The NOVA motion simulator is a virtual vehicle, combining virtual reality with unlimited motion. It has a three hundred and sixty degree range of motion and is used for flight and racing simulation.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                    break;
                }
                else if (entity.category == "RACE Hub")
                {
                    response = "The R-M-I-T AWS Supercomputing Hub is a commercial cloud supercomputing facility. It is used for research, digital innovation, education and industry applications, as well as robot soccer and deep racing.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                    break;
                }
                else if (entity.category == "GOV Lab")
                {
                    response = "The Gov Lab, or Global Operations Visualization Laboratory, is used for data visualisation and space exploration. It features a tiled display driven by rendering middleware like SAGE and Google Liquid Galaxy.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                    break;
                }
                else if (entity.category == "Robots")
                {
                    response = "Robotics is one of the main research interests of this lab, and we have several robots here. Rosie, the two-armed swordfighting robot, and Tiago which has an extendable torso and a manipulator arm . We also have an industrial robot arm.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                    break;
                }
                else if (entity.category == "Virtual Reality")
                {
                    response = "Virtual, augmented and annotated reality is one of the research areas of the VX Lab. This includes use of the MetaQuest Pro, surveying, instruction, science, gaming and industry.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                    break;
                }
                else if (entity.category == "Purpose")
                {
                    response = "The VXLab, or Virtual Experiences Laboratory, is a multi-disciplinary virtual laboratory connecting visualisation and automation facilities in RMIT and industry. We are always working on something new.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                    break;
                }
                else if (entity.category == "Rokoko Smartsuits")
                {
                    response = "We have facilities that support custom motion capture, using the Rokoko Smartsuit Pro. Please find them in the Gov Lab";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                    break;
                }
                else if (entity.category == "Laboratory")
                {
                    response = "This is the Virtual Experiences  Lab, where we have cutting edge equipment and facilities.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                    break;
                }
                else if (entity.category == "Tour")
                {
                    //response = "Tour this, bitch.";
                    response = "Welcome to the Virtual Experiences lab! I am speaking to you from a 32-inch lenticular display. In the race hub, the room parallel to this one on the left, you" +
                        " will find the 60-inch screen with a lifesize version of me, as well as the Amazon Web Services supercomputing hub. If you'll look behind me, you'll find the GOV lab" +
                        " which uses state of the art displays for data visualisation. To your right, you'll see Rosie and Tiago, our other residents. Next to them is the planar display screen."
                        + " Finally, behind you in the corner is our unlimited motion flight simulator. Ask me for more details!";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                    break;
                }
                //legacy
                else if (entity.category == "Point")
                {
                    response = (entity.text);
                    getDirection(entity.text);
                }
                else if (entity.category == "Facilities")
                {
                    response = "LALALALALA FACILITIES";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                }
                else
                {
                    response = "Sorry, I didn't catch that.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                }
                */
                #endregion
            }
            Debug.Log(response);
        }
        //location section- get direction here 
        //updated to work with database
        else if (res != null && res.result.prediction.topIntent == "Location")
        {
            foreach (var entity in res.result.prediction.entities)
            {

                LabAsset lab_asset = ObjectManagement.GetObject(entity.category);
                if (lab_asset != null)
                {
                    response = lab_asset.location_info;
                    Debug.Log("response: " + response);
                    if (response == null)
                    {
                        response = "Haha, this is a bug!";
                    }
                    StartCoroutine(UpdateOutputText(response));
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    Direction dir_to_point = getBestDirection(lab_asset.getPosition());
                    getEnumDirection(dir_to_point);
                }
                else
                {
                    StartCoroutine(UpdateOutputText(response));
                    HelloWorld.Instance.SynthesizeSpeech(response);
                }
                #region legacy
                /*
                if (entity.text == "Rosie")
                {
                    response = "Rosie is the red robot to your right.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    getDirection(entity.text);
                    //animator.SetBool("IsTalking", true);
                }
                else if (entity.text == "Tiago")
                {
                    response = "Tiago is to the right, the white robot.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                }
                else if (entity.category == "NOVA ball")
                {
                    response = "The NOVA flight simulator is behind you, behind the screen.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                }
                else if (entity.category == "GOV Lab")
                {
                    response = "The Gov Lab is the room behind me, slightly to the right.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                }
                else if (entity.category == "RACE Hub")
                {
                    response = "The RACE hub is to the left, in the room parallel to this one.";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                }
                else if (entity.category == "Rokoko Smartsuits")
                {
                    response = "Please find the Rokoko smartsuits in the Gov Lab, behind me, slightly to the right.";
                    HelloWorld.Instance.SynthesizeSpeech(response);     
                    animator.SetBool("IsTalking", true);
                }
                else if (entity.category == "Facilities")
                {
                    response = "UNGA BUNGA";
                    HelloWorld.Instance.SynthesizeSpeech(response);
                    animator.SetBool("IsTalking", true);
                }
                else
                {
                    HelloWorld.Instance.SynthesizeSpeech(response);
                }
                */
                #endregion
            }
        }
        else
        {
            Debug.Log(response);
        }
    }

    private IEnumerator UpdateOutputText(string message)
    {
        output_text.text = message;
        yield return null;
    }

    //choose a random talking animation for naturalistic motion
    public string getRandomTrigger(int option)
    {
        string idx;
        int[] myTalkingAnims = { 0, 1};
        int[] myIdleAnims = { 1, 2, 3 };
        int[] array;
        array = option > 0 ? myTalkingAnims : myIdleAnims;
        int length = array.Length;
        System.Random rand = new System.Random();
        int index = rand.Next(length);
        int chosen_anim = array[index];
        idx = chosen_anim.ToString();
        return idx;

    }




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

    // point to direction relative to users
    void getEnumDirection(Direction to_point)
    {
        switch (to_point)
        {
            case Direction.FRONT:
                animator.SetTrigger("Forward");
                break;
            case Direction.BACK:
                animator.SetTrigger("Behind");
                break;
            case Direction.LEFT:
                animator.SetTrigger("RightTrigger");
                break;
            case Direction.RIGHT:
                animator.SetTrigger("LeftTrigger");
                break;
        }
    }

    IEnumerator TriggerAnimations()
    {
        animator.SetTrigger("WaveTrigger2");
        yield return new WaitForSeconds(9);
        animator.SetTrigger("LeftTrigger");
        yield return new WaitForSeconds(11); 
        animator.SetTrigger("Behind");
        yield return new WaitForSeconds(7f); 
        animator.SetTrigger("RightTrigger");
        yield return new WaitForSeconds(6);
        animator.SetTrigger("Forward");
    }






    Direction getBestDirection(Vector3 target_position)
    {
        float degree_to_point = getDegree(target_position);

        Direction[] directions = (Direction[])Enum.GetValues(typeof(Direction));

        Direction best_direction = Direction.FRONT;
        float lowest_dif = 360;
        foreach (Direction dir in directions)
        {
            // float angle_diff = Mathf.Abs((degree_to_point % 360) - (float)dir);

            float angle_diff = Mathf.Min(Mathf.Abs((degree_to_point % 360) - (float)dir), 360 - Mathf.Abs((degree_to_point % 360) - (float)dir));

            if (angle_diff < lowest_dif)
            {
                lowest_dif = angle_diff;
                best_direction = dir;
            }
        }

        Debug.Log("best_direction: " + best_direction + ", lowest_dif: " + lowest_dif);

        return best_direction;
    }

    private float getDegree(Vector3 target_position)
    {
        Vector3 targetDir = target_position - avator_posi;
        targetDir.y = 0; // Optional: Keep rotation in the horizontal plane only

        // Calculate the rotation needed to look at the target direction
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);

        Vector3 eulerRotation = targetRotation.eulerAngles;
        float yRotation = eulerRotation.y;

        float yRotationNormalized = NormalizeAngle360(yRotation);

        Debug.Log("yRotationNormalized: " + yRotationNormalized);

        return yRotationNormalized;
    }

    private float NormalizeAngle360(float angle)
    {
        return (angle % 360 + 360) % 360;
    }
}
