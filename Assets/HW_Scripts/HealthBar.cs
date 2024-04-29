using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _mainSlider;
    [SerializeField] private Slider _secondarySlider;
    [SerializeField] private Health _battleHandler;
    [SerializeField] private TextChangeHealth _prefab;
    [SerializeField] private Canvas _parent;

    private Coroutine _coroutine;

    private void Start()
    {
        _mainSlider.maxValue = 1f;
        _mainSlider.value = 1f;

        _secondarySlider.maxValue = 1f;
        _secondarySlider.value = 1f;

        _battleHandler.HealthChanged += CheckHealth;
    }

    private void CheckHealth(float deltaHealth)
    {
        _mainSlider.value = deltaHealth;
        _coroutine = StartCoroutine(SliderDelay());
    }

    private IEnumerator SliderDelay()
    {
        if (_mainSlider.value > _secondarySlider.value)
        {
            _secondarySlider.value = _mainSlider.value;

            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(0.3f);

            while (_mainSlider.value <= _secondarySlider.value)
            {
                _secondarySlider.value -= Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }
        }

        StopCoroutine(_coroutine);
    }
}