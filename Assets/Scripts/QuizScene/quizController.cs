using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class quizController : MonoBehaviour
{
    [SerializeField] private GameObject answerButtonPrefab;
    [SerializeField] private Transform gameOverPanel;
    private TextMeshProUGUI questionText;
    private Transform answersHolder;
    private List<Question> questions;
    private List<Answer> answers;
    private int round;
    private bool canAnswer;
    private int correctAnswers;

    private void Awake()
    {
        questionText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        answersHolder = transform.GetChild(1);
    }


    public void setupQuiz(List<Question> selectedQuestions, List<Answer> selectedAnswers)
    {
        questions = selectedQuestions;
        answers = selectedAnswers;

        StartCoroutine(startRound());
    }

    private IEnumerator startRound()
    {
        questionText.text = "Round: " + (round + 1) + "\n<b>" + questions[round].text + "</b>";

        if(answersHolder.childCount > 0)
        {
            foreach (Transform child in answersHolder)
            {
                Destroy(child.gameObject);
            }

        }

        foreach (Answer answer in answers.Where(p => p.question_id == questions[round].id).ToList())
        {
            yield return new WaitForSeconds(0.5f);
            GameObject answerObject = Instantiate(answerButtonPrefab, new Vector3(0f, 0, 0), Quaternion.identity, answersHolder) as GameObject;
            answerObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = answer.text;
            answerObject.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(answerChoosen(answer.correct, answerObject)); });
        }
        canAnswer = true;
    }

    private IEnumerator answerChoosen(bool correct, GameObject button)
    {
        if (canAnswer)
        {
            canAnswer = false;
            if (correct)
            {
                button.GetComponent<Image>().DOColor(Color.green, 0.5f);
                correctAnswers += 1;
            }
            else
            {
                button.GetComponent<Image>().DOColor(Color.red, 0.5f);
            }
            yield return new WaitForSeconds(3);

            if (round + 1 < questions.Count)
            {
                round += 1;
                StartCoroutine(startRound());
            }
            else
            {
                gameOverPanel.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "<b>" + (correctAnswers / (float)questions.Count) * 100 + "%</b>\n";
                gameOverPanel.GetComponent<PanelController>().showPanel();
            }
        }
        yield return null;

    }

}
