using UnityEngine;

public class SlotElement : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _sprite;

    public Sprite Sprite
    {
        get => _sprite; set
        {
            _sprite = value;
            _spriteRenderer.sprite = value;
        }
    }

    private void OnValidate()
    {
        _spriteRenderer ??= GetComponent<SpriteRenderer>();
    }
}
