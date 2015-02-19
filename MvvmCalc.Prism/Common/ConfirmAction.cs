using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;

namespace MvvmCalc.Common
{
    /// <summary>
    /// 確認ダイアログを表示するアクション
    /// </summary>
    public class ConfirmAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            // InteractionRequestedEventArgs以外の場合は何もしない
            var args = parameter as InteractionRequestedEventArgs;
            if (args == null)
            {
                return;
            }

            var context = args.Context as Confirmation;
            if (context == null)
            {
                return;
            }

            // メッセージボックスを表示して
            var result = MessageBox.Show(
                args.Context.Content.ToString(),
                "確認",
                MessageBoxButton.OKCancel);

            // ボタンの押された結果をResponseに格納して
            context.Confirmed = result == MessageBoxResult.OK;

            // コールバックを呼ぶ
            args.Callback();
        }
    }
}
