using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public bool swapMusic = false;
    public AudioSource audioSource;
    public AudioClip music1,musci2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        swapMusic = !swapMusic;

        audioSource.Stop();
        audioSource.clip = swapMusic ? music1 : musci2;
        audioSource.Play();
    }
}
