using Neoxider;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Neoxider/" + "Tools/" + nameof(SetText))]
public class SetText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    [SerializeField]
    private string _separator = ".";

    public string startAdd = "";
    public string endAdd = "";

    public bool isAnimation = true;
    [SerializeField] float _maxSize = 1f;
    public float time = 0.3f;
    public bool first = true;
    public UnityEvent OnChange;

    private void Awake()
    {
        first = true;
    }

    public string text
    {
        get => _text.text;
        set => Set(value);
    }

    public void Set(int value)
    {
        Set(value.FormatWithSeparator(_separator));
    }

    public void Set(float value)
    {
        Set(value.FormatWithSeparator(_separator, 2));
    }

    public void Set(string value)
    {
        string text = startAdd + value + endAdd;
        _text.text = text;

        if (!first && gameObject.activeInHierarchy && isAnimation)
        {
            OnChange?.Invoke();

            StopAllCoroutines();
            StartCoroutine(StartAnim());
        }

        first = false;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
    }

    private IEnumerator StartAnim()
    {
        float timer = 0;

        while (timer < time)
        {
            timer += Time.deltaTime;

            transform.localScale = Vector3.one * (_maxSize * (timer / time));

            yield return null;
        }

        transform.localScale = Vector3.one;
    }

    private void OnValidate()
    {
        if (_text == null)
        {
            _text = GetComponent<TMP_Text>();
        }
    }
}
