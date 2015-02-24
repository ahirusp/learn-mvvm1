using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using MvvmCalc.Common;

namespace MvvmCalc.View
{
    /// <summary>
    /// MainView.xaml の相互作用ロジック
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();

            Messenger.Default.Register<DialogMessage>(this, (m) =>
            {
                var result = MessageBox.Show(m.Message, m.Title, MessageBoxButton.OKCancel);
                m.Callback(result == MessageBoxResult.OK);
            });
        }
    }
}
