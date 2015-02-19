using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.ViewModel;

namespace MvvmCalc.Common
{
    /// <summary>
    /// ViewModelの基本クラス。INotifyPropertyChangedの実装を提供します。
    /// </summary>
    public class ViewModelBase : NotificationObject, IDataErrorInfo
    {
        private ErrorsContainer<string> errors;

        protected ErrorsContainer<string> Errors
        {
            get
            {
                if (this.errors == null)
                {
                    this.errors = new ErrorsContainer<string>(s => this.RaisePropertyChanged(() => HasError));
                }
                return errors;
            }
        }

        /// <summary>
        /// プロパティのエラーメッセージを取得する。
        /// </summary>
        /// <param name="columnName">プロパティ名</param>
        /// <returns>エラーメッセージ。エラーがないときにはnullを返す。</returns>
        public string this[string columnName]
        {
            get { return this.Errors.GetErrors(columnName).FirstOrDefault(); }
        }

        /// <summary>
        /// 使用しないため未実装
        /// </summary>
        public string Error
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// エラーがある場合にtrueを返す。
        /// </summary>
        public bool HasError
        {
            get { return this.Errors.HasErrors; }
        }
    }
}
