using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using SimpleJSON;
using System.Collections;
using TMPro;

public class LocManager : MonoBehaviour
{
    private static LocManager instance;
    public List<AssetReference> locReferences;
    private AssetReference assetReference;
    private static Dictionary<string, string> locDict = new Dictionary<string, string>();
    private static bool translationsSet;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            getLocAsset();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void getLocAsset()
    {
        translationsSet = false;
        locDict.Clear();
        if (PlayerPrefs.GetInt("Language", 0) == 0)
        {
            assetReference = locReferences[0];
        }
        else
        {
            assetReference = locReferences[1];
        }

        var locAsset = Addressables.LoadAssetAsync<TextAsset>(assetReference);
        locAsset.Completed += LocAsset_Completed;
    }

    private void LocAsset_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<TextAsset> obj)
    {
        var N = JSON.Parse(obj.Result.text);
        foreach (KeyValuePair<string, JSONNode> kvp in (JSONObject)N)
        {
            locDict.Add(kvp.Key, kvp.Value.Value);
        }
        translationsSet = true;
    }

    public static IEnumerator getTranslation(string key, TextMeshProUGUI text)
    {
        while (translationsSet == false)
        {
            yield return null;
        }
        text.text = locDict[key];
    }
}
