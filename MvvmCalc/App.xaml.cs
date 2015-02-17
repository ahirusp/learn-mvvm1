using System.Windows;
using MvvmCalc.View;

namespace MvvmCalc
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var v = new MainView();
            v.Show();
        }
    }
}
