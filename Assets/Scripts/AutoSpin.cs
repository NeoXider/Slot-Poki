using UnityEngine;

public class AutoSpin : MonoBehaviour
{
    [SerializeField] private SpinController _spinController;
    [SerializeField] private ButtonBase _buttonAuto;
    [SerializeField] private ButtonBase _buttonSpin;
    [SerializeField] private ParticleSystem _particle;
    private bool _isAuto;

    private void Start()
    {
        _spinController.OnLose.AddListener(StartAutoSpin);
        TrailSystem.OnEndSpin += StartAutoSpin;
        _buttonAuto.OnClick.AddListener(SwitchAuto);
        _isAuto = false;
    }

    private void StartAutoSpin()
    {
        if (!_isAuto) return;
        _spinController.StartSpin();
    }

    private void SwitchAuto()
    {
        _isAuto = !_isAuto;

        if(_isAuto) _particle.Play();    
        else _particle.Stop();

        if (_spinController.IsStop() && _isAuto) _spinController.StartSpin();
    }
}