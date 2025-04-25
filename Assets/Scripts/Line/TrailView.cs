using System.Collections;
using UnityEngine;

public class TrailView : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Coroutine _drawLineProcessCoroutine;
    private readonly WaitForSeconds Interval = new(1f);

    public bool IsActive { get; private set; }

    public void Activate(Vector2[] point)
    {
        IsActive = true;

        if(_drawLineProcessCoroutine != null)
        {
            StopCoroutine(_drawLineProcessCoroutine);
            _drawLineProcessCoroutine = null;
        }
        _drawLineProcessCoroutine = StartCoroutine(DrawLineProcessCoroutine(point));
    }

    private IEnumerator DrawLineProcessCoroutine(Vector2[] pos) 
    {
        for (int i = 1; i < pos.Length; i++)
        {
            while (Vector2.Distance(transform.position, pos[i]) > 0.01f)
            {
                transform.position = Vector2.MoveTowards(transform.position, pos[i], Time.deltaTime * _speed);
                yield return null;
            }
        }

        yield return Interval;
        Release();
    }

    private void Release()
    {
        IsActive = false;
        TrailSystem.OnReturnLine?.Invoke(this);
    }
}