using Engine.Data;
using UnityEngine;

namespace Apps.KPIs
{
    public static class PlayTimeInfo
    {
        private const string _dataKey = "Playtime";
        private const int _range = 60;

        private static readonly FieldKey<float> _playtime = new FieldKey<float>(_dataKey, DataSaveInfo.FileName, 0.0f);

        private static float _playTime;
        private static float _playTimeData
        {
            get
            {
                if (_playtime.hasValue)
                    return _playtime.value;

                return 0;
            }
            set
            {
                _playtime.value = value;
            }
        }

        private static float _lastRefreshingTime;

        private static float _lastPlayTimeEventSent;


        public static float PlayTime => _playTime;
        public static string TimeRange
        {
            get
            {
                int index = Mathf.FloorToInt(_playTime / _range);
                return $"{index}min";
            }
        }

        public static void Initialize()
        {
            _playTime = _playTimeData;
        }

        public static void FramePassed(float deltaTime)
        {
            _playTime += deltaTime;

            if (_lastRefreshingTime + 10 <= _playTime)
            {
                _playTimeData = _lastRefreshingTime = _playTime;
            }
        }

        public static void SendNewAchievementEvent()
        {
            int time = Mathf.FloorToInt(_playTime / _range);
            if (time != _lastPlayTimeEventSent)
            {
                _lastPlayTimeEventSent = time;
                EventsLogger.CustomEvent($"KPI:PlayTime{TimeRange}");
            }
        }
    }
}