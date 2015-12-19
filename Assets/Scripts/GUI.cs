using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    Text message;
    Text P1Score;
    Text P2Score;


    // Use this for initialization
    void Start()
    {
        message = transform.Find("CenterMessage").GetComponent<Text>();
        message.enabled = false;

        Text P1Score = transform.Find("P1Score").GetComponent<Text>();
        Text P2Score = transform.Find("P2Score").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTempCenterMessage(string text, float seconds)
    {
        message.text = text;
        StartCoroutine(ShowMessage(seconds));
    }

    public void SetPlayerScore(int playerNumber, string newScore)
    {
        if (playerNumber == 1)
            P1Score.text = newScore;
        else
            P2Score.text = newScore;
    }

    IEnumerator ShowMessage(float seconds)
    {
        message.enabled = true;
        yield return new WaitForSeconds(seconds);
        message.enabled = false;

    }
}
