using Neoxider.Shop;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SpinController : MonoBehaviour
{
    [SerializeField] private CheckSpin _checkSpin = new();
    [SerializeField] private BetsData _betsData;
    [SerializeField] private SpritesData _allSpritesData;

    [Space, Header("Settings")]
    [SerializeField] private bool _priceOnLine = true;
    [SerializeField] private int _countRowElements = 3;
    [SerializeField] private Row[] _rows;
    [SerializeField] private Sprite[,] _spritesEnd;
    [SerializeField] private bool _isSingleSpeed = true;
    [Range(0f, 1f)] public float chanseWin = 0.5f;

    [Space, Header("Visual")]
    [SerializeField] private float _delaySpinRoll = 0.2f;
    [SerializeField] private SpeedControll _speedControll = new();
    [SerializeField] private Vector2 _space = Vector2.one;
    [SerializeField] private VisualSlotLines _lineSlot = new();

    [Space, Header("Text"), Space]
    [SerializeField] private TMP_Text _textCountLine;

    [Space, Header("Events")]
    public UnityEvent OnStartSpin;

    [Space]
    public UnityEvent<int> OnWin;
    public static System.Action<int[]> OnWinLines;
    public UnityEvent OnLose;

    [Space]
    public UnityEvent<string> OnChangeBet;
    public UnityEvent<string> OnChangeMoneyWin;

    [Space, Header("Debug")]
    [SerializeField] private bool firstWin = true;
    [SerializeField, Min(1)] private int _countLine = 1;
    [SerializeField, Min(0)] private int _betsId = 0;

    private int price;

    private void Start()
    {
        _betsId = 0;

        for (int i = 0; i < _rows.Length; i++)
        {
            _rows[i].SetSprites(_allSpritesData.sprites[0]);
        }

        SetPrice();

        _lineSlot.LineActiv(false);
    }

    private void SetPrice()
    {
        int linePrice = _betsData.bets[_betsId];
        price = _priceOnLine ? _countLine * linePrice : linePrice;

        if (_textCountLine != null)
            _textCountLine.text = _countLine.ToString();

        OnChangeBet?.Invoke(price.ToString());

        int[] sequence = Enumerable.Range(0, _countLine).ToArray();
        _lineSlot.LineActiv(sequence);
    }

    public void AddLine()
    {
        if (IsStop())
        {
            _countLine++;

            if (_countLine > _lineSlot.lines.Length)
            {
                _countLine = 1;
            }

            SetPrice();
        }
    }

    public void RemoveLine()
    {
        if (IsStop())
        {
            _countLine--;

            if (_countLine < 1)
            {
                _countLine = _lineSlot.lines.Length;
            }

            SetPrice();
        }
    }

    public void SetMaxBet()
    {
        if (IsStop())
        {
            _betsId = _betsData.bets.Length - 1;

            SetPrice();
        }
    }

    public void AddBet()
    {
        if (IsStop())
        {
            _betsId++;

            if (_betsId >= _betsData.bets.Length)
            {
                _betsId = 0;
            }

            SetPrice();
        }
    }

    public void RemoveBet()
    {
        if (IsStop())
        {
            _betsId--;

            if (_betsId < 0)
            {
                _betsId = _betsData.bets.Length - 1;
            }

            SetPrice();
        }
    }

    private void Win(int[] lines, float[] mult)
    {
        _lineSlot.LineActiv(lines);
        float moneyWin = 0;

        for (int i = 0; i < mult.Length; i++)
        {
            moneyWin += mult[i] * _betsData.bets[_betsId];
        }

        moneyWin = math.max(1, moneyWin);

        OnChangeMoneyWin?.Invoke(((int)moneyWin).ToString());

        OnWin?.Invoke((int)moneyWin);
        OnWinLines?.Invoke(lines);
        Money.Instance.Add((int)moneyWin);
    }

    private void Lose()
    {
        OnChangeMoneyWin?.Invoke(0.ToString());

        OnLose?.Invoke();
    }

    public void StartSpin()
    {
        if (IsStop())
        {
            bool freeSpin = false;

            if(Stats.Instance.data.freeSpin > 0)
            {
                Stats.Instance.SpendFreeSpin();
                freeSpin = true;
                _betsId = 0;
                SetPrice();
            }

            if (freeSpin || Money.Instance.Spend(price))
            {
                OnChangeMoneyWin?.Invoke("");
                StartCoroutine(StartSpinCoroutine());
                OnStartSpin?.Invoke();
            }
        }
    }

    private IEnumerator StartSpinCoroutine()
    {
        var delay = new WaitForSeconds(_delaySpinRoll);

        _lineSlot.LineActiv(false);

        CreateRandomSprites();

        for (int x = 0; x < _rows.Length; x++)
        {
            Sprite[] rowSprite = new Sprite[_countRowElements];

            for (int y = 0; y < _countRowElements; y++)
            {
                rowSprite[y] = _spritesEnd[x, y];
            }

            _rows[x].Spin(_allSpritesData.sprites, rowSprite);
            yield return delay;
        }
    }

    private void CreateRandomSprites()
    {
        _spritesEnd = new Sprite[_rows.Length, _countRowElements];

        for (int x = 0; x < _spritesEnd.GetLength(0); x++)
        {
            for (int y = 0; y < _spritesEnd.GetLength(1); y++)
            {
                Sprite randSprite = _allSpritesData.sprites[Random.Range(0, _allSpritesData.sprites.Length)];
                _spritesEnd[x, y] = randSprite;
            }
        }

        if (firstWin || Random.Range(0, 1f) < chanseWin)
        {
            firstWin = false;
            print(nameof(Win));
            _checkSpin.SetWin(_spritesEnd, _allSpritesData.sprites, _countLine);
        }
        else
        {
            print(nameof(Lose));
            _checkSpin.SetLose(_spritesEnd, _checkSpin.GetWinningLines(_spritesEnd, _countLine), _allSpritesData.sprites, _countLine);
        }

    }

    public bool IsStop()
    {
        if (!TrailSystem.Instance.IsNotActiveLine()) return false;

        for (int i = 0; i < _rows.Length; i++)
        {
            if (_rows[i].is_spinning)
                return false;
        }

        return true;
    }

    private void CheckWin()
    {
        if (IsStop())
        {
            int[] lines = _checkSpin.GetWinningLines(_spritesEnd, _countLine);
            int countWin = lines.Length;
            float[] mult = _checkSpin.GetMultiplayers(_spritesEnd, _countLine);

            if (countWin > 0)
                Win(lines, mult);
            else if (countWin == 0)
                Lose();
        }
    }

    private void OnEnable()
    {
        foreach (var item in _rows)
        {
            item.OnStop.AddListener(CheckWin);
        }
    }

    private void OnDisable()
    {
        foreach (var item in _rows)
        {
            item.OnStop.RemoveListener(CheckWin);
        }
    }

    private void OnValidate()
    {
        _rows ??= GetComponentsInChildren<Row>(true);

        if (_rows != null)
        {
            for (int i = 0; i < _rows.Length; i++)
            {
                if (i > 0)
                {
                    Vector3 pos = _rows[i].transform.position;
                    pos.x = _rows[i - 1].transform.position.x + _space.x;
                    _rows[i].transform.position = pos;
                }

                _rows[i].spaceY = _space.y;

                if (_isSingleSpeed)
                    _rows[i].speedControll = _speedControll;

                if (_allSpritesData != null)
                    _rows[i].SetSprites(_allSpritesData.sprites[0]);

                _rows[i].OnValidate();
            }
        }
    }
}
