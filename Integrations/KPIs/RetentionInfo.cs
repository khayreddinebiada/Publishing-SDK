using Engine.Data;
using System;
using UnityEngine;

namespace Apps.KPIs
{
    public static class RetentionInfo
    {
        private const string _keyName = "FirstLogin";
        private static readonly FieldKey<string> _firstLoginField = new FieldKey<string>(_keyName, DataSaveInfo.FileName);


        private static DateTime _firstLogin;
        private static int _retentionDay;

        public static int RetentionDay
        {
            get
            {
                return _retentionDay;
            }
        } 

        public static void Initialize()
        {
            Login();

            DefineRetention();
        }

        private static void Login()
        {
            if (!_firstLoginField.hasValue)
            {
                _firstLogin = DateNow();
                _firstLoginField.value = _firstLogin.ToString();
                return;
            }

            if (!DateTime.TryParse(_firstLoginField.value, out _firstLogin))
            {
                Debug.LogError("The login is not available to Parse Datetime!...");
                _firstLogin = DateNow();
                _firstLoginField.value = _firstLogin.ToString();
            }
        }

        private static void DefineRetention()
        {
            TimeSpan span = DateTime.UtcNow.Subtract(_firstLogin);
            _retentionDay = Mathf.FloorToInt((int)span.TotalDays);
        }

        public static void SendLoginEvent()
        {
            EventsLogger.CustomEvent($"KPI:Retention{RetentionDay}");
        }

        public static DateTime DateNow()
        {
            DateTime now = DateTime.UtcNow;
            return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
        }
    }
}