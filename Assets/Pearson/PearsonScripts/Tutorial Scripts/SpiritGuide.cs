using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TextStages
{
    stage1,
    stage2,
    stage3,
    stage4,
    stage5,
    stage6
}
public class SpiritGuide : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField] private TMP_Text textbox;
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private List<Transform> movepoints;
    [SerializeField] private List<string> TextPrompts;
    [SerializeField] private List<string> Stage1TextPrompts;
    [SerializeField] private List<string> Stage2TextPrompts;
    [SerializeField] private List<string> Stage3TextPrompts;
    [SerializeField] private List<string> Stage4TextPrompts;
    [SerializeField] private List<string> Stage5TextPrompts;
    [SerializeField] private List<string> Stage6TextPrompts;
    [SerializeField] private List<string> Stage7TextPrompts;
    [SerializeField] private List<int> StagesTextPromptsIdx;
    [SerializeField] private string currentTextPrompt;
    [SerializeField] private Transform currentMovePoint;
    [SerializeField] private TextStages CurrentStageTextStages;
    [SerializeField] private bool finishedCurrentPrompt;


    [SerializeField] private float moveSpeed;

    public void SetFinishedCurrentPronpt(bool var)
    {
        finishedCurrentPrompt = var;
    }
    public bool GetFinishedCurrentPronpt()
    {
        return finishedCurrentPrompt;
    }
    public TutorialManager GetTutorialManager()
    {
        return tutorialManager;
    }
    void Start()
    {
        
    }

    void Stage1()
    {
        if(currentMovePoint != movepoints[0])
        {
            currentMovePoint = movepoints[0];
        }

        
        switch (CurrentStageTextStages)
        {
            case TextStages.stage1:
                if (currentTextPrompt != Stage1TextPrompts[0])
                {
                    currentTextPrompt = "";
                    for (int i = 0; i < Stage1TextPrompts[0].Length; i++)
                    {
                        currentTextPrompt += Stage1TextPrompts[0][i];
                        textbox.text = currentTextPrompt;
                    }
                    finishedCurrentPrompt = true;
                }
                break;

            case TextStages.stage2:
                if (currentTextPrompt != Stage1TextPrompts[1])
                {
                    currentTextPrompt = "";
                    for (int i = 0; i < Stage1TextPrompts[1].Length; i++)
                    {
                        currentTextPrompt += Stage1TextPrompts[1][i];
                        textbox.text = currentTextPrompt;
                    }
                }
                break;

            case TextStages.stage3:
                if (currentTextPrompt != Stage1TextPrompts[2])
                {
                    currentTextPrompt = "";
                    for (int i = 0; i < Stage1TextPrompts[2].Length; i++)
                    {
                        currentTextPrompt += Stage1TextPrompts[2][i];
                        textbox.text = currentTextPrompt;
                    }
                }
                break;

            case TextStages.stage4:
                if (currentTextPrompt != Stage1TextPrompts[3])
                {
                    currentTextPrompt = "";
                    for (int i = 0; i < Stage1TextPrompts[3].Length; i++)
                    {
                        currentTextPrompt += Stage1TextPrompts[3][i];
                        textbox.text = currentTextPrompt;
                    }
                }
                break;

            case TextStages.stage5:
                if (currentTextPrompt != Stage1TextPrompts[4])
                {
                    currentTextPrompt = "";
                    for (int i = 0; i < Stage1TextPrompts[4].Length; i++)
                    {
                        currentTextPrompt += Stage1TextPrompts[4][i];
                        textbox.text = currentTextPrompt;
                    }
                }
                break;

            case TextStages.stage6:
                if (currentTextPrompt != Stage1TextPrompts[5])
                {
                    currentTextPrompt = "";
                    for (int i = 0; i < Stage1TextPrompts[5].Length; i++)
                    {
                        currentTextPrompt += Stage1TextPrompts[5][i];
                        textbox.text = currentTextPrompt;
                    }
                }
                break;


        }
    }
    void Stage2()
    {
        if (currentMovePoint != movepoints[1])
        {
            currentMovePoint = movepoints[1];
        }


        switch (CurrentStageTextStages)
        {
            case TextStages.stage1:
                if (currentTextPrompt != Stage2TextPrompts[0])
                {
                    currentTextPrompt = "";
                    for (int i = 0; i < Stage2TextPrompts[0].Length; i++)
                    {
                        currentTextPrompt += Stage2TextPrompts[0][i];
                        textbox.text = currentTextPrompt;
                    }
                }
                break;

            case TextStages.stage2:
                if (currentTextPrompt != Stage2TextPrompts[1])
                {
                    currentTextPrompt = "";
                    for (int i = 0; i < Stage2TextPrompts[1].Length; i++)
                    {
                        currentTextPrompt += Stage2TextPrompts[1][i];
                        textbox.text = currentTextPrompt;
                    }
                }
                break;

            case TextStages.stage3:
                if (currentTextPrompt != Stage2TextPrompts[2])
                {
                    currentTextPrompt = "";
                    for (int i = 0; i < Stage2TextPrompts[2].Length; i++)
                    {
                        currentTextPrompt += Stage2TextPrompts[2][i];
                        textbox.text = currentTextPrompt;
                    }
                }
                break;

            case TextStages.stage4:
                if (currentTextPrompt != Stage2TextPrompts[3])
                {
                    currentTextPrompt = "";
                    for (int i = 0; i < Stage2TextPrompts[3].Length; i++)
                    {
                        currentTextPrompt += Stage2TextPrompts[3][i];
                        textbox.text = currentTextPrompt;
                    }
                }
                break;

            case TextStages.stage5:
                if (currentTextPrompt != Stage2TextPrompts[4])
                {
                    currentTextPrompt = "";
                    for (int i = 0; i < Stage2TextPrompts[4].Length; i++)
                    {
                        currentTextPrompt += Stage2TextPrompts[4][i];
                        textbox.text = currentTextPrompt;
                    }
                }
                break;

            case TextStages.stage6:
                if (currentTextPrompt != Stage2TextPrompts[5])
                {
                    currentTextPrompt = "";
                    for (int i = 0; i < Stage2TextPrompts[5].Length; i++)
                    {
                        currentTextPrompt += Stage2TextPrompts[5][i];
                        textbox.text = currentTextPrompt;
                    }
                }
                break;


        }
    }
    void Stage3()
    {

    }
    void Stage4()
    {

    }
    void Stage5()
    {

    }
    void Stage6()
    {

    }
    void Stage7()
    {

    }
    void Stage8()
    {

    }
    void Stage9()
    {

    }

    void tutorialStates()
    {
        switch (tutorialManager.getCurrentStage())
        {
            case 0:
                Stage1();
                break;

            case 1:
                Stage2();
                break;

            case 2:
                Stage3();
                break;

            case 3:
                Stage4();
                break;

            case 4:
                Stage5();
                break;

            case 5:
                Stage6();
                break;

            case 6:
                Stage7();
                break;

            case 7:
                Stage8();
                break;

            case 8:
                Stage9();
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, currentMovePoint.position, Time.deltaTime * moveSpeed);
    }
}
