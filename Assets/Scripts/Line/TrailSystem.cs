using UnityEngine;
using System;

[Serializable]
public class CoordinateLine
{
    public Vector2[] Position;
}

public class TrailSystem : MonoBehaviour
{
    public static TrailSystem Instance;
    public static Action<TrailView> OnReturnLine;
    public static Action OnEndSpin;

    [SerializeField] private TrailView[] _trails;
    [SerializeField] private CoordinateLine[] _coordinate;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SpinController.OnWinLines += Activate;
        OnReturnLine += Release;
    }

    private void Activate(int[] lines)
    {
        Debug.Log(lines[0]);
        for(int i = 0; i < lines.Length; i++)
        {
            _trails[lines[i]].transform.position = _coordinate[lines[i]].Position[0];
            _trails[lines[i]].gameObject.SetActive(true);
            _trails[lines[i]].Activate(_coordinate[lines[i]].Position);
        }
    }

    private void Release(TrailView trail)
    {
        trail.gameObject.SetActive(false);
        if (IsNotActiveLine()) OnEndSpin?.Invoke();
    }

    public bool IsNotActiveLine()
    {
        for (int i = 0; i < _trails.Length; i++)
        {
            if (_trails[i].IsActive) return false;
        }

        return true;
    }
}