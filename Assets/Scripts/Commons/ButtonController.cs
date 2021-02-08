using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void setLanguage(int lang)
    {
        PlayerPrefs.SetInt("Language", lang);
    }

    public void closePanelAndReloadScene(PanelController panel)
    {
        panel.closeFunction = delegate
        {
            buttonClickScene("MainScene");
        };
        panel.closePanel();
        GameObject.FindGameObjectWithTag("LocManager").GetComponent<LocManager>().getLocAsset();
    }

    public void buttonClickScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void closeApp()
    {
        Application.Quit();
    }
}
