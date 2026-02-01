using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUi;
    [SerializeField] private AudioMixer generalMixer;
    public AudioMixerGroup musicGroup, sfxGroup;

    void Start()
    {
        Resume(); 
    }

    public void OnEscMenu(InputAction.CallbackContext context)
    {
        Debug.Log("Help");
        if (context.started)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        pauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SetGeneral(float sliderValue)
    {
        generalMixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSfxs(float sliderValue)
    {
        sfxGroup.audioMixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusics(float sliderValue)
    {
        musicGroup.audioMixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}