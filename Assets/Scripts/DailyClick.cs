using Neoxider.Shop;
using UnityEngine;
using UnityEngine.UI;

public class DailyClick : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private int _price;
    [SerializeField] private Sprite _sprite;
    [Space(10)]

    [SerializeField] private ButtonBase _button;
    [SerializeField] private RectTransform _transform;
    [SerializeField] private Image _image;
    private GameObject _moneyPrrefab;

    public Sprite Sprite => _sprite;
    public int ID => _id;
    public string Name => _name;
    public int Price => _price;

    private readonly Color DisableColor = new (0, 0, 0, 0.4f);

    private void Start() 
    {
        _moneyPrrefab = Resources.Load<GameObject>("coin");
        _button.OnClick.AddListener(Click);

        if (Stats.Instance == null)
            print("InstanceNull");
        if (Stats.Instance.data == null)
            print("dataNull");
        if (Stats.Instance.data.animalBuy == null)
            print("animalBuyNull");

        _image.color = Stats.Instance.data.animalBuy[_id] ? Color.white : DisableColor;
    }

    private void Click()
    {
        if(Stats.Instance.data.animalBuy[_id])
        {
            if (Stats.Instance.data.animalClick[_id] <= 0)
            {

            }
            else
            {
                Stats.Instance.data.animalClick[_id]--;
                Money.Instance.Add(2);
                ClickFX.Instance.Click(_transform);
                Destroy(Instantiate(_moneyPrrefab, transform.position, Quaternion.identity, transform.root), 1);
            }
        }
        else
        {
            Animals.Instance.OpenPanel(this);
        }
    }

    public void Open()
    {
        Stats.Instance.data.animalBuy[_id] = true;
        _image.color = Color.white;
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        _button ??= GetComponent<ButtonBase>();
        _transform ??= GetComponent<RectTransform>();
        _image ??= GetComponent<Image>();
    }
#endif 
}