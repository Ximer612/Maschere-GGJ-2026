using UnityEngine;
using UnityEngine.Events;

public class SubtitleTriggerer : MonoBehaviour
{
    [SerializeField] Sprite speakerAvatar;
    [SerializeField][Multiline] string dialogueToPlay;
    [SerializeField] AudioClip charAudio;
    [SerializeField] string keyboardKeys = "";
    [SerializeField] protected bool hasPlayedOnce = false;
    [SerializeField] UnityEvent CloseCallback;

    [SerializeField] protected bool playOnlyOnce = true;

    public void TriggerSubtitle()
    {
        if (playOnlyOnce)
        {
            if (!hasPlayedOnce)
            {
                hasPlayedOnce = true;
                PlayDialogue();
            }
        }
        else
            PlayDialogue();
    }

    [ContextMenu("DebugTriggerSubtitle")]
    private void PlayDialogue()
    {
        SubtitlesManager.PlayDialogue(dialogueToPlay, speakerAvatar, charAudio, keyboardKeys);
        SubtitlesManager.OnDialogClose.AddListener(CloseAction);
    }

    private void CloseAction()
    {
        CloseCallback.Invoke();
        SubtitlesManager.OnDialogClose.RemoveListener(CloseAction);
    }

    public void SetDialogueToPlay(string newDialogue) => dialogueToPlay = newDialogue;

}
