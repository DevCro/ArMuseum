using ZXing;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class QrShowAnimal : MonoBehaviour
{
    [SerializeField] private GameObject[] models;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject infoPanel;
    private List<Animal> animals;
    private Animal animal;
    private WebCamTexture camTexture;
    private Rect screenRect;
    private GameObject currentModel;

    private void Start()
    {
        screenRect = new Rect(0, 75, Screen.width, Screen.height - 325);
        camTexture = new WebCamTexture();
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
    }

    private void OnGUI()
    {
        try
        {
            if (camTexture.isPlaying)
            {
                GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);
                IBarcodeReader barcodeReader = new BarcodeReader();
                var result = barcodeReader.Decode(camTexture.GetPixels32(),
                  camTexture.width, camTexture.height);
                if (result != null)
                {
                    if (result.Text == "Dog")
                    {
                        if (currentModel == null)
                        {
                            animal = animals.Where(a => a.name == "Dog").First();
                            currentModel = Instantiate(models[0], new Vector3(0, 0, 0), transform.rotation) as GameObject;
                            enableButtons();
                        }
                        else
                        {
                            camTexture.Stop();
                        }
                    }
                    else if (result.Text == "Cat")
                    {
                        if (currentModel == null)
                        {
                            animal = animals.Where(a => a.name == "Cat").First();
                            currentModel = Instantiate(models[1], new Vector3(0, 0, 0), transform.rotation) as GameObject;
                            enableButtons();
                        }
                        else
                        {
                            camTexture.Stop();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
    }

    private void enableButtons()
    {
        buttons[0].interactable = true;
        buttons[1].interactable = true;
    }

    public void showMainInfo()
    {
        infoPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = animal.mainInfo;
        infoPanel.GetComponent<PanelController>().showPanel();
    }

    public void showShortInfo()
    {
        infoPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = animal.shortInfo;
        infoPanel.GetComponent<PanelController>().showPanel();
    }

    public void scanCode(List<Animal> selectedAnimals)
    {
        animals = selectedAnimals;
        if (camTexture != null)
        {
            camTexture.Play();
        }
    }
}
