using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class QuestionManager : MonoBehaviour
{

    public Text QuestionText, Answer1, Answer2, Answer3, Answer4;

    private int answerSelected;

    private float timer, maxTime = 25.0f, timeRemaining;
    private bool questionAsked, advance;
    private Question asked;
    private List<string> answers;
    private string message = "", transitionMessage = "", feedbackMessage = "";
    private int correct;
    private string totals;
    private string player;
    private bool yourTurn, transition;
    private int attempt;
    private bool crowning, attemptingCrown, feedback, drawn;
    private Vector2 scroll;
    private Touch touch;
    void Start()
    {
        answerSelected = 0;
        drawn = false;
        attempt = -1;
        attemptingCrown = false;
        feedback = false;
        transition = false;
        crowning = false;
        yourTurn = true;
        scroll = new Vector2();
        if (Questions.Instance.CurrentGame.Turn == "1")
        {
            correct = int.Parse(Questions.Instance.CurrentGame.Player1Correct);
            totals = Questions.Instance.CurrentGame.Player1Totals;
        }
        else if (Questions.Instance.CurrentGame.Turn == "2")
        {
            correct = int.Parse(Questions.Instance.CurrentGame.Player2Correct);
            totals = Questions.Instance.CurrentGame.Player2Totals;
        }
        else
        {
            correct = 0;
            totals = "000000";
        }
        asked = null;
        timer = 0;
        questionAsked = false;
        advance = false;
    }

    void Update()
    {

        if (CheckWinState())
        {
            Debug.Log("Win state");
            Questions.Instance.ChangeTurn();
            Application.LoadLevel("ChangeTurns");
        }
        if (questionAsked)
        {
            timer += Time.deltaTime;
            if (timer > maxTime)
            {
                timer = 0;
                questionAsked = false;
                asked = null;
                advance = true;
                message = "OUT OF TIME";
            }
            else
            {
                timeRemaining = maxTime - timer;
            }
        }
    }
    private bool CheckWinState()
    {
        if (totals == Game.WinState)
            return true;
        else
            return false;
    }
    void OnGUI()
    {
        if (Questions.Instance.CurrentGame.Round == "0")
        {
            GUILayout.Label("<b>ACCEPTING INVITATION: INITIAL ROUND</b>");
        }
        else
        {
            GUILayout.Label("<b>ROUND: </b>" + Questions.Instance.CurrentGame.Round);
        }
        if (feedback)
        {
            GUILayout.Label(feedbackMessage);
            GUILayout.Label("EXPLANATION");
            GUILayout.Label(asked.Explanation);
            if (GUILayout.Button("CONTINUE"))
            {
                feedback = false;
            }
        }
        else if (transition)
        {
            if (GUILayout.Button("<b>" + transitionMessage + "</b>\nClick here to continue"))
            {
                transition = false;
            }
        }
        else if ((crowning) && (!attemptingCrown))
        {
            DrawCrowning();
        }
        else
        {
            DrawGameplay();
        }

    }
    private void DrawCrowning()
    {
        GUILayout.Label("<b>CROWNING</b>");
        GUILayout.Label("CHOOSE A CATEGORY");
        if (Questions.Instance.CurrentGame.Turn == "1")
        {
            for (int i = 0; i < Questions.Instance.CurrentGame.Player1Totals.Length; i++)
            {
                if (Questions.Instance.CurrentGame.Player1Totals[i] == '0')
                {
                    if (GUILayout.Button(Questions.Instance.Category[i]))
                    {
                        AskCrowning(i);
                    }
                }
                else if (Questions.Instance.CurrentGame.Player1Totals[i] == '1')
                {
                    GUILayout.Box(Questions.Instance.Category[i] + " *ALREADY OWN CROWN*");
                }
            }
        }
        else if (Questions.Instance.CurrentGame.Turn == "2")
        {
            for (int i = 0; i < Questions.Instance.CurrentGame.Player2Totals.Length; i++)
            {
                if (Questions.Instance.CurrentGame.Player2Totals[i] == '0')
                {
                    if (GUILayout.Button(Questions.Instance.Category[i]))
                    {
                        AskCrowning(i);
                    }
                }
                else if (Questions.Instance.CurrentGame.Player2Totals[i] == '1')
                {
                    GUILayout.Box(Questions.Instance.Category[i] + " *ALREADY OWN CROWN*");
                }
            }
        }
        else
        {
            Debug.Log("Error Crowning. Turn unknown: " + Questions.Instance.CurrentGame.Turn);
            crowning = false;
        }
    }
    public void SelectAnswer(int id)
    {
        answerSelected = id;
        if (id == asked.CorrectIndex)
        {
            Debug.Log("Correct");
        }
        else
        {
            Debug.Log("Wrong");
        }
    }
    private void DrawGameplay()
    {
        if (questionAsked)
        {
            //GUILayout.Box(Questions.Instance.Category[int.Parse(asked.Category)]);
            GUILayout.Label("TIME TO ANSWER: " + timeRemaining.ToString("F0") + " SECONDS");
        }
        GUILayout.Label("CORRECT: " + correct);
        if (advance)
        {
            advance = false;
            Debug.Log("Advancing from Main to Change Turns");
            Application.LoadLevel("ChangeTurns");
        }
        else if (!questionAsked)
        {
            if (GUI.Button(new Rect(0, Screen.height * 0.2f, Screen.width * 0.5f, Screen.height * 0.25f), "TAKE A SPIN"))
            {
                Spin();
            }
        }
        else
        {
            GUILayout.Space(15.0f);
            GUILayout.Box("<b>" + asked.QuestionText + "</b>");
            GUILayout.Space(20.0f);
            float height = 0.2f;
            scroll = GUI.BeginScrollView(new Rect(0, Screen.height * height, Screen.width * 0.5f, Screen.height - (Screen.height * height)), scroll, new Rect(0, Screen.height * height, Screen.width * 0.5f, Screen.height - (Screen.height * height)));
            Answer1.text = answers[0];
            Answer2.text = answers[1];
            Answer3.text = answers[2];
            Answer4.text = answers[3];
            //foreach (string answer in answers)
            //{

                //if (GUI.Button(new Rect(0, Screen.height * height, Screen.width * 0.5f, Screen.height * 0.15f), answer))
                //{
                //    questionAsked = false;
                //    timer = 0;
                //    if (answer.Equals(answers[asked.CorrectIndex - 1]))
                //    {
                //        message = "CORRECT ANSWER";
                //        feedbackMessage = "YOU ANSWERED CORRECTLY";
                //        correct++;
                //        if ((attemptingCrown) && (attempt != -1))
                //        {
                //            if (Questions.Instance.CurrentGame.Turn == "1")
                //            {
                //                Debug.Log("Saving turn 1");
                //                Debug.Log("Attempt: " + attempt);
                //                char[] temp = Questions.Instance.CurrentGame.Player1Totals.ToCharArray();
                //                temp[attempt] = '1';
                //                totals = new string(temp);
                //                Debug.Log("TOTALS: " + totals);
                //                Questions.Instance.CurrentGame.Player1Totals = totals;
                //            }
                //            else if (Questions.Instance.CurrentGame.Turn == "2")
                //            {
                //                Debug.Log("Saving turn 2");
                //                Debug.Log("Attempt: " + attempt);
                //                char[] temp = Questions.Instance.CurrentGame.Player2Totals.ToCharArray();
                //                temp[attempt] = '1';
                //                totals = new string(temp);
                //                Debug.Log("TOTALS: " + totals);
                //                Questions.Instance.CurrentGame.Player2Totals = totals;
                //            }
                //            attempt = -1;
                //            correct = 0;
                //        }
                //        if (correct == 3)
                //        {
                //            yourTurn = true;
                //            crowning = true;
                //            attemptingCrown = false;
                //            transition = false;
                //            feedbackMessage += "\nYOU HAVE ENOUGH CORRECT TO ATTEMPT A CROWN";
                //        }
                //        if (correct > 3)
                //        {
                //            correct = 0;
                //        }
                //        if (Questions.Instance.CurrentGame.Turn == "1")
                //        {
                //            Questions.Instance.CurrentGame.Player1Correct = correct.ToString();
                //        }
                //        else if (Questions.Instance.CurrentGame.Turn == "2")
                //        {
                //            Questions.Instance.CurrentGame.Player2Correct = correct.ToString();
                //        }
                //        Questions.Instance.QuickSave();
                //    }
                //    else
                //    {
                //        feedbackMessage = "QUESTION:\n\n" + asked.QuestionText + "\n\nYOU INCORRECTLY ANSWERED:\n" + answer + "\n\nTHE CORRECT ANSWER WAS:\n" + asked.Answers[asked.CorrectIndex - 1];
                //        if (attemptingCrown)
                //        {
                //            correct = 0;
                //            if (Questions.Instance.CurrentGame.Turn == "1")
                //            {
                //                Questions.Instance.CurrentGame.Player1Correct = correct.ToString();
                //            }
                //            else if (Questions.Instance.CurrentGame.Turn == "2")
                //            {
                //                Questions.Instance.CurrentGame.Player2Correct = correct.ToString();
                //            }
                //        }
                //        message = "WRONG ANSWER";
                //        yourTurn = false;
                //        crowning = false;
                //        attempt = -1;
                //        attemptingCrown = false;
                //        advance = true;
                //        Questions.Instance.ChangeTurn();
                //    }
                //    feedback = true;
                //}
                //height += 0.2f;
            //}
            GUI.EndScrollView();
        }
    }
    private void AskCrowning(int crown)
    {
        Debug.Log("SetAsked crowning");
        attempt = crown;
        SetAsked(crown);
        attemptingCrown = true;
    }
    private void Spin()
    {

        try
        {
            message = "SPINNING";
            int random = Random.Range(0, 7);
            if (random == 6)
            {
                Debug.Log("Choose category (rolled a 6)");
                correct = 3;
                crowning = true;
                attemptingCrown = false;
                transitionMessage = "SPIN RESULTS: AUTOMATIC CROWNING";
                //Choose category
            }
            else if (random == 7)
            {
                Debug.Log("Special (rolled a 7)");
                transitionMessage = "SPIN RESULTS: SUPER SPECIAL ROLL. SHOULD NOT HAPPEN";
                //special
            }
            else
            {
                Debug.Log("SetAsked random");
                SetAsked(random);
                //transitionMessage = "THE CATEGORY IS " + Questions.Instance.Category[random];
            }
            transition = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error spinning: " + e.Message);
            Spin();
        }
    }
    private void SetAsked(int i)
    {

        asked = Questions.Instance.RetrieveQuestion(i);
        Debug.Log("SetAsked (i): " + i + " | " + "Asked: " + asked.ID);
        answers = new List<string>();
        foreach (string answer in asked.Answers)
        {
            answers.Add(answer);
        }
        answers.Sort();
        questionAsked = true;
        message = "ANSWER QUESTION";
    }
}
