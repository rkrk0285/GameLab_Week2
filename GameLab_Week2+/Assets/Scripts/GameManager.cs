using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("텍스트")]
    public TextMeshProUGUI GoalText;
    public TextMeshProUGUI WeightText;

    [Header("UI")]
    public int Phase = 1;
    public MapGenerator mapGenerator;    

    [Header("스크립트")]
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
    }
    public void CheckExitPlatformWeight(int currentWeight)
    {
        exitPlatformWeight = currentWeight;
        if (Phase == 2)        
            WeightText.text = "Weight : " + currentWeight + " / 160";

        if (currentWeight >= ExitWeight)
        {
            mapGenerator.OpenExitWall(true);
            SetPhase(3);
        }
        else
        {
            mapGenerator.OpenExitWall(false);
            SetPhase(2);
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
                WeightText.text = "Quota : " + exitPlatformWeight + " / 160";
                break;
            case 3:
                GoalText.text = "Now Escape";
                WeightText.text = "";
                break;
        }
    }

    public void ChangePlayerWeight(int weight)
    {
        playerController.CalculateMoveSpeed(weight);        
    }
}
