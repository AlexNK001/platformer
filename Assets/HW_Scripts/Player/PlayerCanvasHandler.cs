using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasHandler : MonoBehaviour
{
    [SerializeField] private PlayerInventory _wallet;
    [SerializeField] private TMP_Text _coinCount;
    [SerializeField] private TMP_Text _potionCount;
    [SerializeField] private TMP_Text _zoneName;
    [SerializeField] private string _text = "X";
    [SerializeField] private Animator _animator;
    [SerializeField] private Image _image;

    private Color _defaultPotionCountColor;
    private Color _defaultPotionLineColor;
    private Color _defaultPotionIconColor;

    private void Awake()
    {
        _wallet.CoinCountChanged += CheckCoinCount;
        _wallet.HealingPotionCountChanged += CheckHealingPotion;

        _defaultPotionCountColor = _coinCount.color;
        _defaultPotionLineColor = _coinCount.outlineColor;
        _defaultPotionIconColor = _image.color;
    }

    private void CheckCoinCount(int coinCount)
    {
        string coin = $"{_text}{coinCount}";
        _coinCount.SetText(coin);
    }

    private void CheckHealingPotion(int healingPotion)
    {
        _animator.SetTrigger("Enter");
        string potion = $"{_text}{healingPotion}";
        _potionCount.SetText(potion);

        if (healingPotion > 0)
        {
            _potionCount.faceColor = _defaultPotionCountColor;
            _potionCount.outlineColor = _defaultPotionLineColor;
            _image.color = _defaultPotionIconColor;
        }
        else
        {
            _potionCount.faceColor = new Color(0f, 0f, 0f, 0.5f);
            _potionCount.outlineColor = new Color(0f, 0f, 0f, 0.5f);
            _image.color = new Color(0f, 0f, 0f, 0.5f);
        }
    }
}