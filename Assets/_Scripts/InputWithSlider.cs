using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class InputWithSlider : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField = null;
    [SerializeField] private Slider _slider = null;
    [SerializeField] private float _minValue = 0f;
    [SerializeField] private float _maxValue = 0f;

    [field:SerializeField]
    public float CurrentValue { get; private set; }

    void Start ()
    {
        _inputField.onSubmit.AddListener(OnInputValueChanged);
        _inputField.onDeselect.AddListener(OnInputValueChanged);
        _slider.onValueChanged.AddListener(OnValueChanged);
        SetValue(CurrentValue);
        _slider.minValue = _minValue;
        _slider.maxValue = _maxValue;
    }

    private void OnDestroy ()
    {
        _inputField.onSubmit.RemoveListener(OnInputValueChanged);
        _inputField.onDeselect.RemoveListener(OnInputValueChanged);
        _slider.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValidate ()
    {
        if (_slider != null)
        {
            SetValue(CurrentValue);
        }
    }

    private void OnInputValueChanged (string stringValue)
    {
        if (float.TryParse(stringValue.Replace(".",","), out float value))
        {
            if (CheckLimits(value))
            {
                OnValueChanged(value);
            }
        }
        else
        {
            OnValueChanged(CurrentValue);
        }
    }

    private void OnValueChanged(float value)
    {
        SetValue(value);
    }

    public void SetValue (float value)
    {
        _inputField?.SetTextWithoutNotify(value.ToString("0.###").Replace(",","."));
        _slider?.SetValueWithoutNotify(value);
        CurrentValue = value;
    }

    private bool CheckLimits (float value)
    {
        return (value > _minValue && value < _maxValue);
    }
}
