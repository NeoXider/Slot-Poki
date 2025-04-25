using UnityEngine;

public class ClickFX : MonoBehaviour
{
    public static ClickFX Instance;
    [SerializeField] private RectTransform _transform;
    [SerializeField] private ParticleSystem _particle;

    private void Awake()
    {
        Instance = this;
    }
    public void Click(RectTransform pos)
    {
        _transform.position = pos.position;
        _particle.Play();
    }
}