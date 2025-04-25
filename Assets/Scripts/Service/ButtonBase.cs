using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    #region Sub-Classes
    [System.Serializable]
    public class UIButtonEvent : UnityEvent<PointerEventData.InputButton> { }
    #endregion

    [SerializeField] private bool _interactable = true;
    [SerializeField] private Image _targetGraphic;
    [SerializeField] private Sprite _enableSprite;
    [SerializeField] private Sprite _disableSprite;

    public UnityEvent OnClick;
    public bool Interactable
    { 
        get => _interactable; 
        set
        {
            _interactable = value;
            _targetGraphic.sprite = value ? _enableSprite : _disableSprite;
        } 
    }

    private RectTransform _rectTransform;
    private readonly Vector2 PressedSize = new(0.9f, 0.9f);
    private const float ResizeDuration = 0.2f;
    private Vector2 _currentSize;
    private Coroutine _resizeCoroutine; 

    private void Awake()
    {
        _rectTransform ??= GetComponent<RectTransform>();
        _currentSize = _rectTransform.localScale;
    }
    private void OnEnable()
    {
        _rectTransform.localScale = _currentSize;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!Interactable) return;

        if (_resizeCoroutine != null)
            StopCoroutine(_resizeCoroutine);

        _resizeCoroutine = StartCoroutine(ResizeButton(PressedSize));
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (!Interactable) return;

        if (_resizeCoroutine != null)
            StopCoroutine(_resizeCoroutine);

        _resizeCoroutine = StartCoroutine(ResizeButton(_currentSize));
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!Interactable) return;

        OnClick?.Invoke();
        AudioController.onClick?.Invoke();
    }

    private IEnumerator ResizeButton(Vector2 targetSize)
    {
        Vector2 initialSize = _rectTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < ResizeDuration)
        {
            _rectTransform.localScale = Vector2.Lerp(initialSize, targetSize, elapsedTime / ResizeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _rectTransform.localScale = targetSize;
    }
}