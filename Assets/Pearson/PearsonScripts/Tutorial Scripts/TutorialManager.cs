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
    [SerializeField] private int currentStage;
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
    public int getCurrentStage()
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
        
    }
}
