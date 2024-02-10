using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static StressProblems;

public class Stone : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro stoneText;

    void Start()
    {
        if (StressProblems.currentProblemIdx >= StressProblems.problems.Length) return;

        string problem = StressProblems.problems[StressProblems.currentProblemIdx];
        stoneText.text = problem;
    }

    void Update()
    {
        
    }
}
