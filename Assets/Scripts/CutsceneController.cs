using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CutsceneController : MonoBehaviour
{
    public CutsceneStep[] sentences;
    public TextMeshProUGUI text;
    public int currStep = 0;
    public int nextSceneIndex;
    public float defaultTextSpeed;

    private bool isWritingText;
    private bool skipText;
    private SpeakerController currentSpeaker;


    /* Prospective flow:
     * 
     * Cutscenes are scripted by "steps"
     * Step types: 
     *      [Display text] the text to be displayed, speed to display at, speaker name if applicable.
     *      [character move] includes enter, exit, switch sides, center, etc.
     *      [pause] float seconds
     *      
     *      every step has the bool:
     *      bool startNextStep (if true, immediately calls step when this step is finished, otherwise waits for user input)
     * 
     * consider inserting escape tokens that indicate things like pausing or speeding up. Like '@' followed by a number sets the tempo. This will allow for fine control of conversation pacing. (at speed)
     * Another option: '*' followed by a number indicates a pause. useful after sentences. (wait seconds)
     * Also consider a way to make this easier on you. You can also just have pacing as an option for text in a step, then break the text into different steps depending on mid-sentence pacing.
     */


    private void Start()
    {
        Step();
    }

    //Steps forward in the cutscene
    public void Step()
    {
        //TODO: Obviously this needs to change.
        if (isWritingText)
        {
            skipText = true;
            return;
        }
        DisplayNextText();
    }

    void DisplayNextText()
    {
        if (currStep >= sentences.Length)
        {
            LevelLoader.Instance.LoadLevelWithIndex(nextSceneIndex);
            return;
        }
        text.text = "";
        CutsceneStep step = sentences[currStep];
        SpeakerController speaker = step.speaker;

        // Only need to specify the speaker if there is a new speaker
        if (speaker == null)
        {
            speaker = currentSpeaker;
        }

        float textSpeed = defaultTextSpeed;
        if (step.customSpeed)
        {
            textSpeed = step.textSpeed;
        }

        if (step.doesCharacterExit)
        {
            speaker.Exit();
        }
        else if (step.doesCharacterEnter)
        {
            sentences[currStep].speaker.Enter();
        } else if (currentSpeaker != speaker)
        {
            if (currentSpeaker != null)
            {
                currentSpeaker.Exit();
            }
            currentSpeaker = speaker;
            currentSpeaker.Enter();
        }
        currentSpeaker = speaker;
        StartCoroutine(SlowTypeText(textSpeed));
        print(currentSpeaker.name);
    }

    private IEnumerator SlowTypeText(float textSpeed)
    {
        isWritingText = true;
        int charIndex = 0;
        while (charIndex < sentences[currStep].text.Length && isWritingText)
        {
            yield return new WaitForSeconds(textSpeed);
            if (skipText)
            {
                print(skipText);
                charIndex = sentences[currStep].text.Length;
                isWritingText = false;
                skipText = false;
                text.text = sentences[currStep].text;
            }
            else
            {
                text.text += sentences[currStep].text[charIndex];
                charIndex++;
            }
        }
        skipText = false;
        isWritingText = false;
        currStep++;
    }

}

