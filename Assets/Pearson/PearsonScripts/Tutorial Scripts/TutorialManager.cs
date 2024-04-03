using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialStages
{
    stage1,
    stage2,
    stage3,
    stage4,
    stage5,
    stage6,
    stage7,
    stage8,
    stage9
}
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialStages currentStage;
    [SerializeField] private UnNetworkPlayer player;
    [SerializeField] private SpiritGuide guide;
    [SerializeField] private float delayTimer, delayTimerReset;
    [SerializeField] private bool currentInfoFinish;
    [SerializeField] private bool tpFinish;
    [SerializeField] private bool nextDialogue;
    [SerializeField] private int tutorialActionNum;

    public void setTutorialActionNum(int val)
    {
        tutorialActionNum = val;
    }

    public int getTutorialActionNum()
    {
        return tutorialActionNum;
    }
    public TutorialStages getCurrentStage()
    {
        return currentStage;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Stage1()
    {
        if(currentInfoFinish)
        {
            currentInfoFinish = false;
        }
    }
    void Stage2()
    {

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
        switch(currentStage)
        {
            case TutorialStages.stage1:
                Stage1();
                break;

            case TutorialStages.stage2:
                Stage2();
                break;

            case TutorialStages.stage3:
                Stage3();
                break;

            case TutorialStages.stage4:
                Stage4();
                break;

            case TutorialStages.stage5:
                Stage5();
                break;

            case TutorialStages.stage6:
                Stage6();
                break;

            case TutorialStages.stage7:
                Stage7();
                break;

            case TutorialStages.stage8:
                Stage8();
                break;

            case TutorialStages.stage9:
                Stage9();
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
