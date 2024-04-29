using System.Collections;
using TMPro;
using UnityEngine;

public class TextChangeHealth : MonoBehaviour 
{
    [SerializeField] private float _distanse = 2f;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _drawDelay = 0.1f;

    [SerializeField] private Color redColor = Color.red;
    [SerializeField] private string _damagePrefics = "-";
    [SerializeField] private Color greenColor = Color.green;
    [SerializeField] private string _healingPrefics = "+";

    private Coroutine _coroutine;
    private Vector3 _startPosition;

    public bool IsWork { get; private set; }

    private void OnEnable()
    {
        _startPosition = transform.localPosition;
        _text.alpha = 0f;
        IsWork = false;
    }

    public void ShowDamage(float damage)
    {
        damage = Mathf.Round(damage);   
        _text.SetText($"{_damagePrefics}{damage}");
        _text.color = redColor;
        _coroutine = StartCoroutine(TextAnimation());
    }

    public void ShowHeal(float heal)
    {
        heal = Mathf.Round(heal);
        _text.SetText($"{_healingPrefics}{heal}");
        _text.color = greenColor;
        _coroutine = StartCoroutine(TextAnimation());
    }

    private IEnumerator TextAnimation()
    {
        IsWork = true;
        Vector2 direction = new Vector2(0f, _distanse);
        _text.transform.localPosition = _startPosition;

        float time = 1f;

        while (time > 0f)
        {
            _text.transform.Translate(direction * Time.deltaTime * _speed);

            time = Mathf.Clamp01(time -= Time.deltaTime * _speed);
            _text.alpha = (1f - time);

            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForSeconds(_drawDelay);
        time = 1f;

        while (time > 0f)
        {
            time = Mathf.Clamp01(time -= Time.deltaTime * _speed);
            _text.alpha = time;

            yield return new WaitForSeconds(0.001f);
        }

        IsWork = false;
        StopCoroutine(_coroutine);
    }
}