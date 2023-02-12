using Assets._Scripts._1Data;
using TMPro;
using UnityEngine;

namespace Assets._Scripts._3Managers
{
    public class LogMessage : MonoBehaviour
    {
        private TextMeshProUGUI _textBody;
        private string _message;
        private Logger.LogType _logType;
        private float _lifeTime;
        private float _startTime;
        private bool _isActive;
        //Best practice is to reference transform here as transform.xxx will actually call GetComponent<> behind the scenes
        private RectTransform _rectTransform;

        public void Initialise (string message, Logger.LogType logType, float? lifetime = null)
        {
            _message = message;
            _logType = logType;
            _lifeTime = lifetime ?? Constants.LogMessageLifeTime;
        }

        public void Show()
        {
            _isActive = true;
            _textBody = gameObject.GetComponent<TextMeshProUGUI>();
            _rectTransform = GetComponent<RectTransform>();
            _startTime = Time.time;

            _textBody.text = _message;
            _textBody.color = GetMessageColour(_logType);
            Destroy(gameObject, _lifeTime);
        }

        public void Update()
        {
            if (!_isActive) return;
            var newPos = new Vector2(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y - Time.deltaTime * 25f);
            _rectTransform.anchoredPosition = newPos;
            _textBody.alpha = 1 - (ElapsedTime / _lifeTime);
        }

        private float ElapsedTime => Time.time - _startTime;

        private Color GetMessageColour(Logger.LogType type) => type switch
        {
            Logger.LogType.Debug => Constants.DebugColour,
            Logger.LogType.Error => Constants.ErrorColour,
            _ => Constants.InformationColour
        };
    }
}
