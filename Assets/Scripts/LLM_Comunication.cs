using LLMUnity;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class LLM_Comunication : MonoBehaviour
{
    LLM llm;
    public int maxWords = 20;
    string basePrompt;
    string context;

    string AIText;

    //TODO: Erase this
    public int testLives = 5;

    void Start()
    {
        basePrompt = "You are a game boss battling with the player. " +
        "The player has 5 lives and loses 1 each time you hit him or he misses the ball. " +
        "You give angry comments about player progress defeating you. " +
        "You respond in less than " + maxWords.ToString() + " words and " +
        "do not use quotation marks nor describe your actions. ";

        llm = GetComponent<LLM>();
        llm.prompt = basePrompt;

        /*SendMessageToLLM("You are about to do your most powerful attack");
        SendMessageToLLM("Your attack hit the player, leaving him with 2 lives left");
        SendMessageToLLM("You are about to change to last phase, and player is defeating you");*/
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow)) SetContext(testLives++);
        if(Input.GetKeyDown(KeyCode.DownArrow)) SetContext(testLives--);

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SendMessageToLLM("You are about to do your most powerful attack");
        if (Input.GetKeyDown(KeyCode.Alpha2)) 
            SendMessageToLLM("Your attack hit the player, leaving him with 2 lives left");
        if (Input.GetKeyDown(KeyCode.Alpha3)) 
            SendMessageToLLM("You are about to change to last phase, and player is defeating you");
    }

    void SendMessageToLLM(string message)
    {
        AIText = "";
        _ = llm.Chat(basePrompt + context + message, SetAIText, ShowText, false);
    }

    public void SetAIText(string text)
    {
        AIText = text;
    }

    public void ShowText()
    {
        Debug.Log(AIText);
    }

    public void SetContext(int lives)
    {
        context = "For context: ";
        context += "The player has " + lives.ToString() + " out of 5 lives left. ";

    }
}
