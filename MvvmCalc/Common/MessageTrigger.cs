using System.Windows.Interactivity;

namespace MvvmCalc.Common
{
    /// <summary>
    /// MessagengerのRaisedイベントを受信するトリガー
    /// </summary>
    public class MessageTrigger : EventTrigger
    {
        protected override string GetEventName()
        {
            return "Raised";
        }
    }
}
