#if UNITY_IOS
namespace apps.ios
{
    public interface IGDPRWindow
    {
        void Show(System.Action onAccepted);
    }
}
#endif