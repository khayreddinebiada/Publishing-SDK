using UnityEngine;

namespace Apps
{
    public class BannerBackgroundBehaviour : MonoBehaviour
    {
        private GUIStyle _style = null;

        private void OnGUI()
        {
            if (_style == null || IsNull(_style.normal.background))
            {
                _style = new GUIStyle(GUI.skin.box);
                Texture2D texture = MakeTex(2, 2, new Color(1f, 1f, 1f, 1f));
                _style.normal.background = texture;
            }

            if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
                GUI.Box(BannerBackground.BackgroundRect(), "", _style);
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private static bool IsNull(Object @object)
        {
            return @object == null || @object.Equals(null);
        }
    }
}