using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("�ؽ�Ʈ")]
    public TextMeshProUGUI GoalText;
    public TextMeshProUGUI WeightText;
    //public TextMeshProUGUI PlayerSpeedText;
    public TextMeshProUGUI PlayerWeightText;    

    [Header("UI")]
    public int Phase = 1;
    public MapGenerator mapGenerator;    

    [Header("��ũ��Ʈ")]
    public PlayerController playerController;
    public PlayerItemController playerItemController;
    public ExitPlatform exitPlatform;

    public int exitPlatformWeight = 0;
    private const int ExitWeight = 160;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }    

    public void Start()
    {
        SetGoalText();
        SetPlayerWeight(0);
    }
    public void CheckExitPlatformWeight(int currentWeight)
    {
        exitPlatformWeight = currentWeight;
        if (Phase == 2)        
            WeightText.text = "Weight : " + currentWeight + " / 160";

        if (currentWeight >= ExitWeight)
        {
            mapGenerator.OpenExitWall();
            SetPhase(3);
        }        
    }

    public void SetPhase(int phase)
    {
        Phase = phase;
        SetGoalText();
    }

    public int GetPhase()
    {
        return Phase;
    }

    public void SetGoalText()
    {
        switch(Phase)
        {
            case 1:
                GoalText.text = "Find the Exit";
                WeightText.text = "";
                break;
            case 2:
                GoalText.text = "Place the Items on the Exit Platform";
                WeightText.text = "Exit Weight : " + exitPlatformWeight + " / 160";
                break;
            case 3:
                GoalText.text = "Now Escape";
                WeightText.text = "";
                break;
        }
    }
    public void SetPlayerWeight(int weight)
    {        
        if (weight == 0)
            PlayerWeightText.text = "There is nothing now.";
        else if (weight <= 40)
            PlayerWeightText.text = "It's lightweight.";
        else if (weight <= 80)
            PlayerWeightText.text = "It's a reasonable weight.";
        else if (weight <= 120)
            PlayerWeightText.text = "It's a little heavy.";
        else if (weight <= 160)
            PlayerWeightText.text = "It's very heavy!";
        else if (weight > 160)
            PlayerWeightText.text = "I'm carrying an elephant!!";
    }
    public void ChangePlayerWeight(int weight)
    {
        playerController.CalculateMoveSpeed(weight);
        SetPlayerWeight(weight);
    }
}
