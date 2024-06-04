using LLMUnity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LLM_Comunication : MonoBehaviour
{
    LLM llm;
    public int maxWords = 20;
    string basePrompt;
    string context;

    public TMP_Text ui;

    public bool isWriting = false;

    private void Awake()
    {
        isWriting = false;

        llm = GetComponent<LLM>();
        Debug.Log(llm);

        basePrompt = "You are a game boss battling with the player. " +
        "The player has 5 lives and loses 1 each time you hit him or he misses the ball. " +
        "You give angry comments about player progress defeating you. " +
        "You respond in less than " + maxWords.ToString() + " words and " +
        "do not use quotation marks nor describe your actions. ";

        llm.prompt = basePrompt;
    }

    public void BossTalk(int lives, int level)
    {
        SetContext(lives, level);

        string m = "";
        switch (level)
        {
            case 0:
                m = "The player just challenged you to battle.";
                break;
            case 1:
                m = "The player is damaging you a bit.";
                break;
            case 2:
                m = "You are being defeated, the player is about to kill you, but you are about to attack.";
                break;
            case 3:
                m = "You are almost dead, but you throw your last attack.";
                break;
            case -1:
                m = "You defeated the player.";
                break;
            case 4:
                m = "You have been defeated by the player.";
                break;
            }
    
        Debug.Log(m);
        SendMessageToLLM(m);
    }

    void SendMessageToLLM(string message)
    {
        isWriting = true;

        ui.text = "";
        _ = llm.Chat(basePrompt + context + message, SetAIText, TextCompleted, false);
    }

    void SetAIText(string text)
    {
        ui.text = text;
    }

    void TextCompleted()
    {
        StartCoroutine(Erase());
    }

    IEnumerator Erase()
    {
        isWriting = false;
        yield return new WaitForSeconds(2);
        ui.text = "";
    }

    public void SetContext(int lives, int level)
    {
        context = "For context: ";
        context += "The player has " + lives.ToString() + " out of 5 lives left. ";
        context += "The player has beaten " + level.ToString() + 
            " levels out of 4 and will beat you when beats all of them. ";
    }
}
