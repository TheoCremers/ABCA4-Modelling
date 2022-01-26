using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleScript : MonoBehaviour
{
    [SerializeField] private RuleSelection _rule = RuleSelection.Order;
    [SerializeField] private Image _stateImage = null;

    public static InputWithSlider inputA = null;
    public static InputWithSlider inputB = null;
    public static InputWithSlider inputC = null;
    public static InputWithSlider inputD = null;
    public static InputWithSlider inputT = null;
    public static InputWithSlider inputPx = null;

    private void Update ()
    {
        switch (_rule)
        {
            case RuleSelection.Order:
                _stateImage.color = RuleOrder() ? Color.green : Color.red;
                break;
            case RuleSelection.BT:
                _stateImage.color = RuleBT() ? Color.green : Color.red;
                break;
            case RuleSelection.AC:
                _stateImage.color = RuleAC() ? Color.green : Color.red;
                break;
            case RuleSelection.B:
                _stateImage.color = RuleB() ? Color.green : Color.red;
                break;
            case RuleSelection.Px:
                _stateImage.color = RulePx() ? Color.green : Color.red;
                break;
        }
    }

    private bool RuleOrder ()
    {
        return (inputA.CurrentValue < inputB.CurrentValue) &&
               (inputB.CurrentValue < inputC.CurrentValue) &&
               (inputC.CurrentValue < inputD.CurrentValue);
    }

    private bool RuleBT ()
    {
        return (inputB.CurrentValue < inputT.CurrentValue);
    }

    private bool RuleAC ()
    {
        return (inputA.CurrentValue + inputC.CurrentValue < 2f * inputT.CurrentValue);
    }

    private bool RuleB ()
    {
        return Mathf.Approximately(inputB.CurrentValue,
            0.25f * (inputA.CurrentValue + inputD.CurrentValue));
    }

    private bool RulePx ()
    {
        return Mathf.Approximately(0.5f * inputA.CurrentValue + (1f - inputPx.CurrentValue) * inputC.CurrentValue + inputPx.CurrentValue * inputD.CurrentValue, 2f * inputT.CurrentValue);
    }
}

public enum RuleSelection
{
    Order,
    BT,
    AC,
    B,
    Px
}