using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using SimpleJSON;
using System.Linq;

[Serializable]
public class Question
{
    public int id { get; set; }
    public string text { get; set; }

    public Question(int questionId, string questionText)
    {
        id = questionId;
        text = questionText;
    }
}

[Serializable]
public class Answer
{
    public int question_id { get; set; }
    public string text { get; set; }
    public bool correct { get; set; }

    public Answer(int questionId, string questionText, bool isCorrect)
    {
        question_id = questionId;
        text = questionText;
        correct = isCorrect;
    }
}

public class QuizManager : MonoBehaviour
{
    [SerializeField]  private List<AssetReference> questionReferences;
    [SerializeField]  private quizController quizController;
    private static List<Question> questionList = new List<Question>();
    private List<Answer> answerList = new List<Answer>();
    private AssetReference assetReference;
    private bool questionsSet;
    private bool answerSet;
    private int questionsNumber = 10;

    private void Awake()
    {
        getQuestionAsset();
        getAnswerAsset();
    }

    public void getQuestionAsset()
    {
        questionsSet = false;
        assetReference = questionReferences[0];

        var questionAsset = Addressables.LoadAssetAsync<TextAsset>(assetReference);
        questionAsset.Completed += QuestionAsset_Completed;
    }

    public void getAnswerAsset()
    {
        answerSet = false;
        assetReference = questionReferences[1];

        var answerAsset = Addressables.LoadAssetAsync<TextAsset>(assetReference);
        answerAsset.Completed += AnswerAsset_Completed;
    }

    private void QuestionAsset_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<TextAsset> obj)
    {
        var N = JSON.Parse(obj.Result.text);
        foreach (KeyValuePair<string, JSONNode> kvp in (JSONObject)N)
        {
            questionList.Add(new Question(kvp.Value[0], kvp.Value[1].Value));
        }

        questionsSet = true;
        canStartQuiz();
    }

    private void AnswerAsset_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<TextAsset> obj)
    {
        var N = JSON.Parse(obj.Result.text);
        foreach (KeyValuePair<string, JSONNode> kvp in (JSONObject)N)
        {
            answerList.Add(new Answer(kvp.Value[0], kvp.Value[1].Value, kvp.Value[2]));
        }
        answerSet = true;
        canStartQuiz();
    }

    private void canStartQuiz()
    {
        if(!answerSet || !questionsSet)
        {
            return;
        }

        List<Question> selectedQuestions = questionList.OrderBy(qu => Guid.NewGuid()).Take(questionsNumber).ToList();
        List<Answer> selectedAnswers = answerList.Where(p => selectedQuestions.Where(pp => pp.id == p.question_id).Any()).ToList();

        quizController.setupQuiz(selectedQuestions, selectedAnswers);
    }
}
