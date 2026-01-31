using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SubtitlesManager : MonoBehaviour
{
    private static SubtitlesManager Singleton;

    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject DialogueBox;
    [SerializeField] TMP_Text subtitleText;
    [SerializeField] Image speakerAvatar;
    [SerializeField] float charPerSecond = 0.2f;
    [SerializeField] float disappearingTime = 1f;

    [SerializeField] GameObject ToSkipText, ToContinuePress, KeyToPress;
    public static UnityEvent OnDialogClose;

    AudioClip charAudio;
    string textToShow;
    bool shouldDisappearBox = false;
    bool skipToEnd = false;
    int currentCharIndex = 0;
    Timer subtitlesTimer, disappearingTimer;

    //bool bindedToInput = false;

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }

        subtitlesTimer = new Timer(charPerSecond, true, false);
        disappearingTimer = new Timer(disappearingTime, false, false);
        OnDialogClose = new UnityEvent();
    }


    private void Start()
    {
        //LocalPlayerController.GetPlayerInput().actions.actionMaps[0].FindAction("Interact").performed += SkipToEnd;
        enabled = false;
    }

    //private void OnDestroy()
    //{
    //    NetworkPlayer.OnPlayerFinishInit -= SetSkipButton;

    //    if (bindedToInput && LocalPlayerController.GetPlayerInput())
    //    {
    //        LocalPlayerController.GetPlayerInput().actions.actionMaps[0].FindAction("Interact").performed -= SkipToEnd;
    //        bindedToInput = false;
    //    }
    //}

    //private void SetSkipButton(GameObject player)
    //{
    //    LocalPlayerController.GetPlayerInput().actions.actionMaps[0].FindAction("Interact").performed += SkipToEnd;
    //    bindedToInput = true;
    //}

    public void SkipToEnd(InputAction.CallbackContext context)
    {
        if (!enabled)
            return;

        if (shouldDisappearBox && disappearingTimer.isEnd)
        {
            skipToEnd = false;
            shouldDisappearBox = false;
            subtitleText.SetText("");
            subtitlesTimer.SetShouldTick(false);
            DialogueBox.SetActive(false);

            enabled = false;
        }

        skipToEnd = true;
        ToSkipText.SetActive(false);
        KeyToPress.SetActive(false);
    }

    private void Update()
    {
        //if (MenuScript.IsPaused)
        //    return;

        if (shouldDisappearBox)
        {
            CloseDialogBox();
        }
        else if (skipToEnd || subtitlesTimer.Tick(Time.deltaTime))
        {
            if (textToShow.Length <= currentCharIndex)
            {
                disappearingTimer.Reset();
                shouldDisappearBox = true;
                ToSkipText.SetActive(false);
                ToContinuePress.SetActive(false);
                KeyToPress.SetActive(false);
                return;
            }

            char newChar = textToShow[currentCharIndex];

            if (newChar != ' ' && charAudio)
            {
                audioSource.PlayOneShot(charAudio);
            }
            //AudioManager.Singleton.PlayOneShot(charAudio, Vector2.zero);

            if (newChar == '<')
            {
                //add a tag in a single frame
                while (newChar != '>')
                {
                    subtitleText.text += newChar;
                    currentCharIndex++;
                    newChar = textToShow[currentCharIndex];
                }
            }

            subtitleText.text += newChar;
            currentCharIndex++;
        }
    }

    public static void PlayDialogue(string newTextToShow, Sprite newSpeakerAvatar, AudioClip charAudio, string keyboardKeys = "")
    {
        Singleton.skipToEnd = false;

        Singleton.textToShow = newTextToShow + keyboardKeys;

        Singleton.speakerAvatar.sprite = newSpeakerAvatar;

        Singleton.DialogueBox.SetActive(true);
        Singleton.subtitlesTimer.Reset();
        Singleton.subtitleText.SetText("");
        Singleton.currentCharIndex = 0;

        Singleton.shouldDisappearBox = false;
        Singleton.charAudio = charAudio;

        Singleton.ToSkipText.SetActive(true);
        Singleton.ToContinuePress.SetActive(false);
        Singleton.KeyToPress.SetActive(true);

        Singleton.enabled = true;
    }

    private void CloseDialogBox()
    {
        if (disappearingTimer.Tick(Time.deltaTime))
        {
            ToSkipText.SetActive(false);
            ToContinuePress.SetActive(true);
            KeyToPress.SetActive(true);

            OnDialogClose.Invoke();
        }
    }
}
