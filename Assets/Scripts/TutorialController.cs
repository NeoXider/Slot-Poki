using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private float _blockingRaycastTime;
    [Space(20)]

    [SerializeField] private RectTransform _handRect;
    [SerializeField] private Animator _animator;
    [SerializeField] private ButtonBase _button;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _buttonImage;
    [SerializeField] private GameObject _panelSpin, _panelBuy;
    [SerializeField] private GameObject _blokingRaycast;

    [Space(20)]
    [SerializeField] private ButtonBase[] _buttons;

    
    private Coroutine _tutorialProcessCoroutine;
    private bool _onClick;

    private void Start()
    {
        //StartTutor();
    }

    public void StartTutor()
    {
        if (Stats.Instance.data.StepTutorial < _buttons.Length)
        {
            _button.OnClick.AddListener(() => { _onClick = true; });
            _tutorialProcessCoroutine = StartCoroutine(TutorialProcessCoroutine());
        }
    }

    private IEnumerator TutorialProcessCoroutine()
    {
        _onClick = false;

        int id = Stats.Instance.data.StepTutorial;

        if(id == 2 && !_panelSpin.activeInHierarchy) id = 1;
        if(id == 3 && !_panelSpin.activeInHierarchy) id = 4;
        if (id == 5 && !_panelBuy.activeInHierarchy) id = 4;

        if (id == 3 && _panelSpin.activeInHierarchy)      
        {
            _blokingRaycast.SetActive(true);
            _animator.Play("Exit");
            yield return new WaitForSeconds(_blockingRaycastTime);
            _blokingRaycast.SetActive(false);
        }


        if(id >= _buttons.Length)
        {
            _animator.Play("Exit");
            yield break;
        }

        _button.OnClick.AddListener(_buttons[id].OnClick.Invoke);
        var buttonImage = _buttons[id].gameObject.GetComponent<Image>();

        //позиция и размер кнопки
        _buttonImage.sprite = buttonImage.sprite;
        _buttonImage.rectTransform.anchorMin = buttonImage.rectTransform.anchorMin;
        _buttonImage.rectTransform.anchorMax = buttonImage.rectTransform.anchorMax;
        _buttonImage.rectTransform.anchoredPosition = buttonImage.rectTransform.anchoredPosition;
        _buttonImage.rectTransform.sizeDelta = buttonImage.rectTransform.sizeDelta;

        //позиция тыкалки
        _handRect.position = _buttonImage.rectTransform.position;

        //настройка текста
        if(buttonImage.gameObject.GetComponentInChildren<TextMeshProUGUI>() != null)
        {
            TextMeshProUGUI text = buttonImage.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            _text.gameObject.SetActive(true);
            _text.text = text.text;
            _text.fontSize = text.fontSize;
            _text.fontMaterial = text.fontMaterial;
            _text.lineSpacing = text.lineSpacing;
            _text.rectTransform.anchoredPosition = text.rectTransform.anchoredPosition;
        }
        else _text.gameObject.SetActive(false);


        _animator.Play("Enter");

        while(!_onClick) yield return null;

        _button.OnClick.RemoveListener(_buttons[id].OnClick.Invoke);

        Stats.Instance.data.StepTutorial = id + 1;

        ReleaseCoroutine();
    }

    private void ReleaseCoroutine()
    {
        if (_tutorialProcessCoroutine != null)
        {
            StopCoroutine(_tutorialProcessCoroutine);
            _tutorialProcessCoroutine = null;
        }
        _tutorialProcessCoroutine = StartCoroutine(TutorialProcessCoroutine());
    }
}