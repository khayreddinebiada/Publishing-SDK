using Apps.KPIs;
using UnityEngine;

namespace Apps
{
    public class IntegrationHelper : MonoBehaviour
    {
        private void LateUpdate()
        {
            PlayTimeInfo.FramePassed(Time.unscaledDeltaTime);
            PlayTimeInfo.SendNewAchievementEvent();
        }

        private void OnApplicationPause(bool pause)
        {
            AppsIntegration.OnApplicationPause(pause);
        }
    }
}
