using System.ComponentModel;

namespace MvvmCalc.Common
{
    /// <summary>
    /// ViewModelの基本クラス。INotifyPropertyChangedの実装を提供します。
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// プロパティの変更があった時に発行されます。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var h = this.PropertyChanged;
            if (h != null)
            {
                h(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
