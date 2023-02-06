using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class OptionMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    private Resolution[] resolutions;

    public TMPro.TMP_Dropdown resolutionDropdown;

    private CanvasGroup Canvas;

    private void Start()
    {
        resolutions = Screen.resolutions;
        
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " " + resolutions[i].refreshRate + "hz"; 
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.width && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                //ToggleCanvasGroup(Canvas, isActive = true);
            }
        }
    }
    private void ToggleCanvasGroup(CanvasGroup canvas, bool isActive)
    {
        canvas.alpha = isActive ? 1f : 0f;
        canvas.interactable = isActive;
        canvas.blocksRaycasts = isActive;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
        // audioMixer.
    }

    public void SetFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is closing");
    }
}
