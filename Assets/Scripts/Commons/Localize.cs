using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]

public class Localize : MonoBehaviour
{
    [SerializeField] string translationKey;

    void Start()
    {
        StartCoroutine(LocManager.getTranslation(translationKey, GetComponent<TextMeshProUGUI>()));
    }
}
