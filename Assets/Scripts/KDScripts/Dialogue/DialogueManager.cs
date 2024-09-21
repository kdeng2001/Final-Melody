using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Params")]
    private float typingSpeed = 0.02f;
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;
    private TextMeshProUGUI[] choicesText;

    public Story currentStory;
    public Coroutine displayLineCoroutine { get; private set; }
    public bool canContinueToNextLine { get; private set; }
    [SerializeField] private GameObject canContinueIcon;
    public bool dialogueIsPlaying { get; private set; }
    public bool displayingChoices { get; private set; }

    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    
    public static DialogueManager Instance;
    [Header("Persistent Dialogue Variables")]
    private DialogueVariables dialogueVariables;

    // variable for the load_globals.ink JSON
    [Header("Load Globals JSON")]
    [SerializeField] public TextAsset loadGlobalsJSON;

    public delegate void HasEnteredDialogueMode();
    public HasEnteredDialogueMode hasEnteredDialogueMode;
    public delegate void HasExitedDialogueMode();
    public HasExitedDialogueMode hasExitedDialogueMode;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            dialogueVariables = gameObject.AddComponent<DialogueVariables>();
            dialogueVariables.SetUpVariables(loadGlobalsJSON);        
            layoutAnimator = dialoguePanel.GetComponent<Animator>();
            dialoguePanel.SetActive(false);
            displayingChoices = false;
            dialogueIsPlaying = false;
            displayLineCoroutine = null;
            canContinueToNextLine = false;
            //get all of the choices text
            choicesText = new TextMeshProUGUI[choices.Length];
            int index = 0;
            // enable and initialize the choices up to the amount of choices for this line of dialogue
            foreach (GameObject choice in choices)
            {
                choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
                choice.SetActive(false);
                index++;
            }
            DontDestroyOnLoad(gameObject);
        }
    }


    public void EnterDialogueMode(TextAsset inkJson)
    {
        currentStory = new Story(inkJson.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        dialogueVariables.StartListening(currentStory);

        Player.Instance.PauseMovement();
        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play("default");

        hasEnteredDialogueMode?.Invoke();
        StartCoroutine(DelayOneFrame());

    }

    private IEnumerator DelayOneFrame()
    {
        yield return new WaitForEndOfFrame();
        canContinueToNextLine = true;
        ContinueStory();
    }

    public void ContinueStory()
    {
        // if there is more text in the story
        if(currentStory.canContinue) { HandleContinueStory(); }
        // if there is no more text in the story
        else { HandleFinishStory(); }
    }

    private void HandleContinueStory()
    {
        // if coroutine is still running (text is still displaying character by character)
        if (displayLineCoroutine != null)
        {
            // fully load the text
            StopCoroutine(displayLineCoroutine);
            ReachEndOfLine();
            return;
        }
        // if coroutine is finished (text is fully displayed)
        else
        {
            // start new coroutine with new text
            // display choices if any for this dialogue line
            // handle tags
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            DisplayChoices();
            HandleTags(currentStory.currentTags);
        }
    }

    private void HandleFinishStory()
    {
        if (!canContinueToNextLine)
        {
            // fully load the text
            StopCoroutine(displayLineCoroutine);
            ReachEndOfLine();
            return;
        }
        else
        {
            // finish the story
            ExitDialogueMode();
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        canContinueIcon.SetActive(false);
        dialogueText.text = "";
        canContinueToNextLine = false;
        bool isAddingRichTextTag = false;
        foreach(char letter in line.ToCharArray())
        {
            if(letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                dialogueText.text += letter;
                if(letter == '>') 
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        ReachEndOfLine();
    }

    private void ReachEndOfLine()
    {
        displayLineCoroutine = null;
        canContinueToNextLine = true;
        dialogueText.text = currentStory.currentText;
        canContinueIcon.SetActive(true);
    }

    private void HandleTags(List<string> currentTags)
    {
        // loop through each tag and handle it accordingly
        foreach(string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');     
            if(splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }                
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    public void ExitDialogueMode()
    {
        dialogueVariables.StopListening(currentStory);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        Player.Instance.UnpauseMovement();
        hasExitedDialogueMode?.Invoke();
    }

    private void DisplayChoices()
    {
        displayingChoices = false;
        List<Choice> currentChoices = currentStory.currentChoices;
        // defensive check to make sure UI can support the number of choices coming in
        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. NUmber of choices given: " 
                + currentChoices.Count);
        }
        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
            displayingChoices = true;
        }
        // go through the remaining choices the UI supports and make sure they are hidden
        for(int i=index; i<choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear selected GameObject first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        if(!canContinueToNextLine) { return; }
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}
