using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StorySceneRunner : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI dialogueTextBox;
    [SerializeField] Image imageBox;
    [SerializeField] List<DialogueToShow> dialogueList;

    private int currDialogue = -1;
    private float timeToSwapDialogue = 0f;

    [Serializable]
    public class DialogueToShow
    {
        public string text;
        public Sprite image;
        public float timeToShow;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Time.timeSinceLevelLoad > 10f)
        {
            LoadNextScene();
        }*/
        if(Time.time > timeToSwapDialogue)
        {
            ShowNextDialogue();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void ShowNextDialogue()
    {
        currDialogue += 1;
        if(currDialogue == dialogueList.Count)
        {
            LoadNextScene();
        }
        dialogueTextBox.text = dialogueList[currDialogue].text;
        imageBox.sprite = dialogueList[currDialogue].image;
        timeToSwapDialogue = Time.time + dialogueList[currDialogue].timeToShow;
    }
}
