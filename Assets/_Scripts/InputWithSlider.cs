using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[ExecuteInEditMode]
public class InputWithSlider : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField = null;
    [SerializeField] private Slider _slider = null;
    [SerializeField] private Slider _minSlider = null;
    [SerializeField] private Slider _maxSlider = null;
    [SerializeField] private Button _lockButton = null;
    [SerializeField] private Image _lockImage = null;
    [SerializeField] private Sprite _lockSprite = null;
    [SerializeField] private Sprite _unlockSprite = null;
    [SerializeField] private float _minValue = 0f;
    [SerializeField] private float _maxValue = 0f;

    [field:SerializeField]
    public float CurrentValue { get; private set; }
    public bool Locked { get; private set; }
    public event Action<float> OnValueChanged;

    private float _minSolution = 0f;
    private float _maxSolution = 0f;

    void Start ()
    {
        _inputField.onSubmit.AddListener(OnInputValueChanged);
        _inputField.onDeselect.AddListener(OnInputValueChanged);
        _slider.onValueChanged.AddListener(OnInputValueChanged);
        _lockButton.onClick.AddListener(OnLockButtonClicked);
        SetValue(CurrentValue);
        _slider.minValue = _minValue;
        _slider.maxValue = _maxValue;
        _minSlider.minValue = _minValue;
        _minSlider.maxValue = _maxValue;
        _maxSlider.minValue = _minValue;
        _maxSlider.maxValue = _maxValue;
        _lockImage.sprite = Locked ? _lockSprite : _unlockSprite;
    }

    private void OnDestroy ()
    {
        _inputField.onSubmit.RemoveListener(OnInputValueChanged);
        _inputField.onDeselect.RemoveListener(OnInputValueChanged);
        _slider.onValueChanged.RemoveListener(OnInputValueChanged);
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
        _inputField?.SetTextWithoutNotify(value.ToString("0.##").Replace(",", "."));
        _slider?.SetValueWithoutNotify(value);
        CurrentValue = value;
    }

    public void SetSolutionBounds (float min = float.MinValue, float max = float.MaxValue)
    {
        _minSolution = Mathf.Max(min, _minValue);
        _maxSolution = Mathf.Min(max, _maxValue);
        _minSlider.SetValueWithoutNotify(min);
        _maxSlider.SetValueWithoutNotify(_maxValue - max);
    }

    private void OnInputValueChanged (string stringValue)
    {
        if (float.TryParse(stringValue.Replace(".",","), out float value))
        {
            if (CheckLimits(value))
            {
                OnInputValueChanged(value);
            }
        }
        else
        {
            OnInputValueChanged(CurrentValue);
        }
    }

    private void OnLockButtonClicked ()
    {
        Locked = !Locked;
        _lockImage.sprite = Locked ? _lockSprite : _unlockSprite;
        _lockButton.image.color = Locked ? Color.cyan : Color.white;
        _slider.interactable = !Locked;
        _inputField.interactable = !Locked;
    }

    private void OnInputValueChanged(float value)
    {
        OnValueChanged?.Invoke(value);
        SetValue(value);
    }

    private bool CheckLimits (float value)
    {
        return (value > _minValue && value < _maxValue);
    }

}
