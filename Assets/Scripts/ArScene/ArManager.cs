using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using SimpleJSON;
using System.Linq;

[Serializable]
public class Animal
{
    public string name { get; set; }
    public string mainInfo { get; set; }
    public string shortInfo { get; set; }

    public Animal(string animalName, string animalMainInfo, string animalShortInfo)
    {
        name = animalName;
        mainInfo = animalMainInfo;
        shortInfo = animalShortInfo;
    }
}

public class ArManager : MonoBehaviour
{
    [SerializeField] private List<AssetReference> animalReferences;
    [SerializeField] private QrShowAnimal QrShowAnimal;
    private List<Animal> animalList = new List<Animal>();
    private AssetReference assetReference;

    private void Awake()
    {
        getQuestionAsset();
    }

    public void getQuestionAsset()
    {
        assetReference = animalReferences[0];

        var questionAsset = Addressables.LoadAssetAsync<TextAsset>(assetReference);
        questionAsset.Completed += QuestionAsset_Completed;
    }

    private void QuestionAsset_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<TextAsset> obj)
    {
        var N = JSON.Parse(obj.Result.text);
        foreach (KeyValuePair<string, JSONNode> kvp in (JSONObject)N)
        {
            animalList.Add(new Animal(kvp.Value[0].Value, kvp.Value[1].Value, kvp.Value[2].Value));
        }
        QrShowAnimal.scanCode(animalList);
    }
}
