using UnityEngine;

public class SubtitleTriggerer : MonoBehaviour
{
    [SerializeField] Sprite speakerAvatar;
    [SerializeField][Multiline] string dialogueToPlay;
    [SerializeField] AudioClip charAudio;
    [SerializeField] string keyboardKeys = "";
    [SerializeField] protected bool hasPlayedOnce = false;

    protected bool playOnlyOnce = true;

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
    }

    public void SetDialogueToPlay(string newDialogue) => dialogueToPlay = newDialogue;

}
