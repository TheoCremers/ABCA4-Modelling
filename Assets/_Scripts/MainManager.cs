using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainManager : MonoBehaviour
{
    //public static MainManager Instance;

    [SerializeField] private TextMeshProUGUI _solutionDisplay = null;

    public InputWithSlider InputA;
    public InputWithSlider InputB;
    public InputWithSlider InputC;
    public InputWithSlider InputD;
    public InputWithSlider InputT;
    public InputWithSlider InputPx;

    private List<ACSolution> _solutions = new List<ACSolution>();
    private ACSolution _avgSolution = new ACSolution(0f, 0f);

    private void Awake ()
    {
        RuleScript.inputA = InputA;
        RuleScript.inputB = InputB;
        RuleScript.inputC = InputC;
        RuleScript.inputD = InputD;
        RuleScript.inputT = InputT;
        RuleScript.inputPx = InputPx;

        Screen.fullScreen = true;
    }

    public void AdjustACD ()
    {
        float T = InputT.CurrentValue;
        float Px = InputPx.CurrentValue;
        float iPx = (1f - Px);
        float invPx = 1f / Px;

        float avgA = 0f;
        float avgC = 0f;
        _solutions.Clear();

        for (float A = T; A > 0f; A -= 0.001f)
        {
            for (float C = A; A + C < 2f * T; C += 0.001f)
            {
                float D = (2f * T - 0.5f * A - iPx * C) * invPx;
                float B = 0.25f * (A + D);

                if (A < B && B < C && C < D && D < 1f && B < T && (A + C) < 2f * T)
                {
                    _solutions.Add(new ACSolution(A, C));
                    avgA += A;
                    avgC += C;
                }
            }
        }

        if (_solutions.Count > 0)
        {
            avgA /= (float)_solutions.Count;
            avgC /= (float)_solutions.Count;
            _avgSolution = new ACSolution(avgA, avgC);
        }

        _solutionDisplay.text = $"{_solutions.Count} solution(s) found!";
    }

    public void ShowRandomACSolution ()
    {
        if (_solutions.Count == 0) return;
        float T = InputT.CurrentValue;
        float Px = InputPx.CurrentValue;
        float iPx = (1f - Px);
        float invPx = 1f / Px;

        ACSolution sol = _solutions[Random.Range(0, _solutions.Count)];
        float D = (2f * T - 0.5f * sol.A - iPx * sol.C) * invPx;
        float B = 0.25f * (sol.A + D);

        InputA.SetValue(sol.A);
        InputB.SetValue(B);
        InputC.SetValue(sol.C);
        InputD.SetValue(D);
    }

    public void ShowAverageACSolution ()
    {
        if (_solutions.Count == 0) return;
        float T = InputT.CurrentValue;
        float Px = InputPx.CurrentValue;
        float iPx = (1f - Px);
        float invPx = 1f / Px;

        ACSolution sol = _avgSolution;
        float D = (2f * T - 0.5f * sol.A - iPx * sol.C) * invPx;
        float B = 0.25f * (sol.A + D);

        InputA.SetValue(sol.A);
        InputB.SetValue(B);
        InputC.SetValue(sol.C);
        InputD.SetValue(D);
    }

    public void Exit ()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit ();
#endif
    }
}

public struct ACSolution{
    public float A;
    //public float B;
    public float C;
    //public float D;
    public ACSolution(float a, /*float b,*/ float c/*, float d*/)
    {
        A = a;
        //B = b;
        C = c;
        //D = d;
    }
}