using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Neoxider
{
    [RequireComponent(typeof(CanvasGroup))]
    public class RuleteScreen : MonoBehaviour
    {
        public TutorialController TutorialController;
        [SerializeField] private bool _useOne = true;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _wheelTransform;
        [SerializeField] private RectTransform _arrow;

        [Space]
        [SerializeField] private bool _leftDir = true;
        [SerializeField] private float _speedZVelosity = 45;
        [SerializeField] private float _slowDown = 5;
        private float zVelocity;
        private int mode = 0;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private string[] textButton = { "SPIN", "STOP", "EXIT" };

        [Space]
        [SerializeField] private bool _autoSetItems = true;
        [SerializeField] private GameObject[] _prizeItems;
        [SerializeField] private float _distOnCenter = 200;

        [Space]
        [SerializeField] private bool _debugCheckId = false;
        [SerializeField, Range(0, 360)] private float _zWheel = 0;

        [Space]
        public UnityEvent<int> OnWinIdVariant;

        private bool _isUse;

        private void OnEnable()
        {
            _wheelTransform.rotation = Quaternion.identity;

            mode = 0;

            _text.text = textButton[0];
        }

        private void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            if (mode > 0)
            {
                _wheelTransform.Rotate(Vector3.back * (_leftDir ? zVelocity : -zVelocity) * Time.deltaTime);

                if (mode == 2)
                {
                    zVelocity -= Time.deltaTime * _slowDown;

                    if (zVelocity <= 0f)
                    {
                        mode = 0;

                        if (_canvasGroup != null)
                            _canvasGroup.interactable = true;

                        Result();
                    }
                }
            }
        }

        private void Result()
        {
            int resultId = GetResultId();

            OnWinIdVariant?.Invoke(resultId);
        }

        //public int GetResultId()
        //{
        //    float sizeItem = GetAngleStep();

        //    int resultId = Mathf.FloorToInt((360f - _wheelTransform.rotation.eulerAngles.z - sizeItem / 2) / sizeItem);

        //    return (resultId + _prizeVariants.Length) % _prizeVariants.Length;
        //}

        public int GetResultId()
        {
            if (_arrow == null) return 0;

            float sizeItem = 360f / _prizeItems.Length;
            float wheelAngle = _wheelTransform.rotation.eulerAngles.z;
            float arrowAngle = _arrow.transform.eulerAngles.z;

            float relativeAngle = (wheelAngle - arrowAngle + 360f) % 360f;

            int resultId = Mathf.FloorToInt((relativeAngle + (sizeItem / 2)) / sizeItem);

            return (resultId + _prizeItems.Length) % _prizeItems.Length;
        }

        public void OnClick()
        {
            if (mode == 0 && !_isUse)
            {
                Spin();
            }
            else if(mode == 1)
            {
                Stop();
            }
            else
            {
                if(mode == 0)
                {
                    gameObject.SetActive(false);
                    TutorialController.StartTutor();
                }
            }
        }

        public void Spin()
        {
            if (_prizeItems.Length == 0) return;

            if (mode == 0 && (!_useOne || (_useOne && !_isUse)))
            {
                _isUse = true;

                mode = 1;

                zVelocity = _speedZVelosity;

                _text.text = textButton[1];
            }
        }

        public void Stop()
        {
            if (mode == 0) return;

            if (_canvasGroup != null)
                _canvasGroup.interactable = false;

            mode = 2;

            _text.text = textButton[2];
        }

        private void ArrangePrizes()
        {
            if (_prizeItems == null || _prizeItems.Length != _prizeItems.Length) return;

            int itemCount = _prizeItems.Length;
            float angleStep = 360f / itemCount;

            for (int i = 0; i < itemCount; i++)
            {
                float angle = (itemCount - i) * angleStep + 90;
                float radians = angle * Mathf.Deg2Rad;

                Vector2 position = new Vector2(
                    Mathf.Cos(radians) * _distOnCenter,
                    Mathf.Sin(radians) * _distOnCenter
                );

                _prizeItems[i].transform.localPosition = position;
                _prizeItems[i].transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }
        }

        private float GetAngleStep()
        {
            return 360f / _prizeItems.Length;
        }

        private void OnValidate()
        {
            _wheelTransform.transform.eulerAngles = new Vector3(0, 0, _zWheel);

            if (_debugCheckId)
                print("roulet Id: " + GetResultId());

            _canvasGroup ??= GetComponent<CanvasGroup>();

            if (_autoSetItems)
                ArrangePrizes();
        }
    }
}
