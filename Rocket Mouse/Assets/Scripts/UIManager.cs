using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Animator startButton;
    public Animator settingsBtn;
    public Animator dialog;
    public Animator contentPanel;
    public Animator Gear;

    public Slider volumeSdr;


    private void Start()
    {
        LoadVolume();
    }
    public void ToggleMenu()
    {
        bool isHidden = contentPanel.GetBool("isHidden");
        contentPanel.SetBool("isHidden",!isHidden);
        Gear.SetBool("isHidden", !isHidden);
    }
    public void OpenCloseSettings(bool isOpen)
    {
        startButton.SetBool("isHidden", isOpen);
        settingsBtn.SetBool("isHidden", isOpen);
        dialog.SetBool("isHidden", !isOpen);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }

    //볼륨값 조절 Meun값 SlideBtn 에 추가되어 있음
    public void SaveVolume()
    {
        float volume = volumeSdr.value;
        PlayerPrefs.SetFloat("bgVolume",volume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        float volume = PlayerPrefs.GetFloat("bgVolume");
        volumeSdr.value = volume;
    }
}
