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
    public InputWithSlider InputM;

    private List<ABCDSolution> _solutions = new List<ABCDSolution>();
    private ABCDSolution _avgSolution = new ABCDSolution(0f, 0f, 0f, 0f);
    const float DELTA = 0.01f;
    const float LIMIT = 100000000;

    private void Awake ()
    {
        RuleScript.inputA = InputA;
        RuleScript.inputB = InputB;
        RuleScript.inputC = InputC;
        RuleScript.inputD = InputD;
        RuleScript.inputT = InputT;
        RuleScript.inputM = InputM;

        Screen.fullScreen = true;
    }

    public void AdjustABCD ()
    {
        float T = InputT.CurrentValue;
        float M = InputM.CurrentValue;

        double avgA = 0f;
        double avgB = 0f;
        double avgC = 0f;
        double avgD = 0f;
        float minA = -1f;
        float minB = float.MaxValue;
        float minC = float.MaxValue;
        float minD = float.MaxValue;
        float maxA = float.MinValue;
        float maxB = float.MinValue;
        float maxC = float.MinValue;
        float maxD = float.MinValue;
        _solutions.Clear();

        int solutions = 0;
        float A = -DELTA;
        while (solutions < LIMIT)
        {
            A += DELTA;
            // check rules for max A
            if (2f * A >= T || 2f * A * M >= T) { break; }

            float localMinB = -1f;
            float localMaxB = float.MinValue;
            float B = A - DELTA;
            while (true)
            {
                B += DELTA;
                // check rules for max B
                if (2f * B >= T || A + B >= T || (A + B) * M >= T) { break; }

                float localMinC = -1f;
                float localMaxC = float.MinValue;
                float C = B - DELTA;
                while (true)
                {
                    C += DELTA;
                    // check rules for max C
                    if (A + C >= T || (A + C) * M >= T) { break; }

                    // check rules for min C
                    if (2f * C < T) { continue; }

                    float localMinD = -1f;
                    float localMaxD = float.MinValue;
                    float D = C - DELTA;
                    while(true)
                    {
                        D += DELTA;
                        // check rules for max D
                        if ((A + D) * M >= T) { break; }

                        // check rules for min D
                        if (A + D < T) { continue; }

                        // solution found!
                        _solutions.Add(new ABCDSolution(A, B, C, D));

                        avgA += A;
                        avgB += B;
                        avgC += C;
                        avgD += D;

                        if (minA < 0f) { minA = A; }
                        maxA = A;

                        if (localMinB < 0f) { localMinB = B; }
                        localMaxB = B;

                        if (localMinC < 0f) { localMinC = C; }
                        localMaxC = C;

                        if (localMinD < 0f) { localMinD = D; }
                        localMaxD = D;


                        solutions++;
                    }
                    if (localMinD >= 0f && localMinD < minD) {  minD = localMinD; }
                    if (localMaxD > maxD) {  maxD = localMaxD; }
                }
                if (localMinC >= 0f && localMinC < minC) { minC = localMinC; }
                if (localMaxC > maxC) { maxC = localMaxC; }
            }
            if (localMinB >= 0f && localMinB < minB) { minB = localMinB; }
            if (localMaxB > maxB) { maxB = localMaxB; }
        }

        if (_solutions.Count > 0)
        {
            avgA /= (float) solutions;
            avgB /= (float) solutions;
            avgC /= (float) solutions;
            avgD /= (float) solutions;
            _avgSolution = new ABCDSolution((float) avgA, (float) avgB, (float) avgC, (float) avgD);
            InputA.SetSolutionBounds(minA, maxA);
            InputB.SetSolutionBounds(minB, maxB);
            InputC.SetSolutionBounds(minC, maxC);
            InputD.SetSolutionBounds(minD, maxD);
            Debug.Log($"A: {minA}-{maxA}  B: {minB}-{maxB}  C: {minC}-{maxC}  D: {minD}-{maxD}");
        }

        _solutionDisplay.text = $"{solutions} solution(s) found!";
    }

    public void ShowRandomACSolution ()
    {
        if (_solutions.Count == 0) return;
        ABCDSolution sol = _solutions[Random.Range(0, _solutions.Count)];
        InputA.SetValue(sol.A);
        InputB.SetValue(sol.B);
        InputC.SetValue(sol.C);
        InputD.SetValue(sol.D);
    }

    public void ShowAverageACSolution ()
    {
        if (_solutions.Count == 0) return;
        InputA.SetValue(_avgSolution.A);
        InputB.SetValue(_avgSolution.B);
        InputC.SetValue(_avgSolution.C);
        InputD.SetValue(_avgSolution.D);
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

public struct ABCDSolution{
    public float A;
    public float B;
    public float C;
    public float D;
    public ABCDSolution(float a, float b, float c, float d)
    {
        A = a;
        B = b;
        C = c;
        D = d;
    }
}