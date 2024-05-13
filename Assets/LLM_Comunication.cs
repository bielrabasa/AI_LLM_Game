using LLMUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LLM_Comunication : MonoBehaviour
{
    public LLM llm;
    string AIText;

    void Start()
    {
        SendMessageToLLM("I'm winning, what do you say about that?");
    }

    void SendMessageToLLM(string message)
    {
        AIText = "Hmm...";
        ShowText();
        _ = llm.Chat(message, SetAIText, ShowText);
    }

    public void SetAIText(string text)
    {
        AIText = text;
    }

    public void ShowText()
    {
        Debug.Log(AIText);
    }
}
