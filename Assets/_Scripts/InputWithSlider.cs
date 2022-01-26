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
    [SerializeField] private Slider _minSlider = null;
    [SerializeField] private Slider _maxSlider = null;
    [SerializeField] private float _minValue = 0f;
    [SerializeField] private float _maxValue = 0f;

    [field:SerializeField]
    public float CurrentValue { get; private set; }
    private float _minSolution = 0f;
    private float _maxSolution = 0f;

    void Start ()
    {
        _inputField.onSubmit.AddListener(OnInputValueChanged);
        _inputField.onDeselect.AddListener(OnInputValueChanged);
        _slider.onValueChanged.AddListener(OnValueChanged);
        SetValue(CurrentValue);
        _slider.minValue = _minValue;
        _slider.maxValue = _maxValue;
        _minSlider.minValue = _minValue;
        _minSlider.maxValue = _maxValue;
        _maxSlider.minValue = _minValue;
        _maxSlider.maxValue = _maxValue;
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
            _slider.minValue = _minValue;
            _slider.maxValue = _maxValue;
            _minSlider.minValue = _minValue;
            _minSlider.maxValue = _maxValue;
            _maxSlider.minValue = _minValue;
            _maxSlider.maxValue = _maxValue;
        }
    }

    public void SetValue (float value)
    {
        _inputField?.SetTextWithoutNotify(value.ToString("0.###").Replace(",", "."));
        _slider?.SetValueWithoutNotify(value);
        CurrentValue = value;
    }

    public void SetSolutionBounds (float min, float max)
    {
        _minSolution = min;
        _maxSolution = max;
        _minSlider.SetValueWithoutNotify(min);
        _maxSlider.SetValueWithoutNotify(_maxValue - max);
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

    private bool CheckLimits (float value)
    {
        return (value > _minValue && value < _maxValue);
    }

}
