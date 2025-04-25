using UnityEngine;
using Neoxider.Shop;
using TMPro;
using UnityEngine.UI;
using Neoxider;

public class Animals : MonoBehaviour
{
    public static Animals Instance;

    [SerializeField] private GameObject _buyPanel;
    [SerializeField] private GameObject _buyingPanel;
    [SerializeField] private ButtonBase _button;
    [SerializeField] private TextMeshProUGUI[] _textNames;
    [SerializeField] private TextMeshProUGUI _textPrice;
    [SerializeField] private Image _image;

    private int _price;
    private DailyClick _animal;

    private void Awake() => Instance = this;

    private void Start() => _button.OnClick.AddListener(Buy);


    public void OpenPanel(DailyClick animal)
    {
        _animal = animal;

        for (int i = 0; i < _textNames.Length; i++)
        {
            TextMeshProUGUI item = _textNames[i];
            item.text = animal.Name;
            if(i==0)
                item.text = "You are now tre owner of a " + animal.Name;
        }

        _textPrice.text = animal.Price.FormatWithSeparator(".") + "c";
        _image.sprite = animal.Sprite;
        _price = animal.Price;

        _buyPanel.SetActive(true);
        _image.SetNativeSize();
    }



    private void Buy()
    {
        if (Money.Instance.Spend(_price))
        {
            OpenAnimal(_animal);
            _buyPanel.SetActive(false);
            _buyingPanel.SetActive(true);
        }
    }

    public void OpenAnimal(DailyClick animal)
    {
        animal.Open();
    }
}