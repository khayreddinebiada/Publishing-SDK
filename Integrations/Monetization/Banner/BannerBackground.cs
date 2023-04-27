using UnityEngine;

namespace Apps
{
    public static class BannerBackground
    {
        internal static Rect BackgroundRect()
        {
            float minH = 0;

#if UNITY_ANDROID || UNITY_EDITOR
            minH = (Screen.width <= 720) ? 80 : 120;
#elif UNITY_IOS
            minH = (SystemInfo.deviceModel.Contains("iPhone")) ? 120 : 80;
#endif

            float h = Mathf.Max(minH, Screen.height * 0.09f);
            return new Rect(new Vector2(0, Screen.height - h), new Vector2(Screen.width, h));
        }
    }
}