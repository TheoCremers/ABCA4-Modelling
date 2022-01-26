using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuleScript : MonoBehaviour
{
    [SerializeField] private int _rule = 0;
    [SerializeField] private Image _stateImage = null;

    public static InputWithSlider inputA = null;
    public static InputWithSlider inputB = null;
    public static InputWithSlider inputC = null;
    public static InputWithSlider inputD = null;
    public static InputWithSlider inputT = null;
    //public static InputWithSlider inputPx = null;
    public static InputWithSlider inputM = null;

    private void Update ()
    {
        switch (_rule)
        {
            case 1:
                _stateImage.color = RuleOne() ? Color.green : Color.red;
                break;
            case 2:
                _stateImage.color = RuleTwo() ? Color.green : Color.red;
                break;
            case 3:
                _stateImage.color = RuleThree() ? Color.green : Color.red;
                break;
            case 4:
                _stateImage.color = RuleFour() ? Color.green : Color.red;
                break;
            case 5:
                _stateImage.color = RuleFive() ? Color.green : Color.red;
                break;
            case 6:
                _stateImage.color = RuleSix() ? Color.green : Color.red;
                break;
            default:
                break;
        }
    }

    private bool RuleOne ()
    {
        return (inputA.CurrentValue <= inputB.CurrentValue) &&
               (inputB.CurrentValue <= inputC.CurrentValue) &&
               (inputC.CurrentValue <= inputD.CurrentValue);
    }

    private bool RuleTwo ()
    {
        return 2f * inputB.CurrentValue < inputT.CurrentValue;
    }

    private bool RuleThree ()
    {
        return inputA.CurrentValue + inputC.CurrentValue < inputT.CurrentValue;
    }

    private bool RuleFour ()
    {
        return (inputA.CurrentValue + inputD.CurrentValue) * inputM.CurrentValue < inputT.CurrentValue;
    }

    private bool RuleFive ()
    {
        return 2f * inputC.CurrentValue >= inputT.CurrentValue;
    }

    private bool RuleSix ()
    {
        return inputA.CurrentValue + inputD.CurrentValue >= inputT.CurrentValue;
    }

    #region oldrules
    //private bool RuleOrder ()
    //{
    //    return (inputA.CurrentValue < inputB.CurrentValue) &&
    //           (inputB.CurrentValue < inputC.CurrentValue) &&
    //           (inputC.CurrentValue < inputD.CurrentValue);
    //}
    //private bool RuleBT ()
    //{
    //    return (inputB.CurrentValue < inputT.CurrentValue);
    //}

    //private bool RuleAC ()
    //{
    //    return (inputA.CurrentValue + inputC.CurrentValue < 2f * inputT.CurrentValue);
    //}

    //private bool RuleB ()
    //{
    //    return Mathf.Approximately(inputB.CurrentValue,
    //        0.25f * (inputA.CurrentValue + inputD.CurrentValue));
    //}

    //private bool RulePx ()
    //{
    //    return Mathf.Approximately(0.5f * inputA.CurrentValue + (1f - inputPx.CurrentValue) * inputC.CurrentValue + inputPx.CurrentValue * inputD.CurrentValue, 2f * inputT.CurrentValue);
    //}
    #endregion
}

public enum RuleSelection
{
    Order,
    BT,
    AC,
    B,
    Px
}