using UnityEngine;

namespace Apps.Adsview
{
    internal class GUIBox : GUIView
    {
        protected override void Draw()
        {
            GUI.Box(_rect, _content);
        }
    }
}
