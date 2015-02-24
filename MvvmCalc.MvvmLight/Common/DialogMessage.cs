using System;

namespace MvvmCalc.Common
{
    public class DialogMessage
    {
        public DialogMessage(string title, string message, Action<bool> callback)
        {
            Title = title;
            Message = message;
            Callback = callback;
        }

        public string Title { get; private set; }
        public string Message { get; private set; }
        public Action<bool> Callback { get; private set; }
    }
}
