using System.Collections.Generic;
using System.ComponentModel;

namespace MvvmCalc.Common
{
    /// <summary>
    /// ViewModelの基本クラス。INotifyPropertyChangedの実装を提供します。
    /// </summary>
    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase, IDataErrorInfo
    {
        /// <summary>
        /// プロパティに紐づいたエラーメッセージを返します。
        /// </summary>
        private Dictionary<string, string> errors = new Dictionary<string, string>();

        /// <summary>
        /// columnNameで指定したプロパティのエラーを返します。
        /// </summary>
        /// <param name="columnName">プロパティ名</param>
        /// <returns>エラーメッセージ</returns>
        public string this[string columnName]
        {
            get
            {
                if (this.errors.ContainsKey(columnName))
                {
                    return this.errors[columnName];
                }
                return "";
            }
        }

        public string Error
        {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>
        /// プロパティにエラーメッセージを設定します。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        protected void SetError(string propertyName, string errorMessage)
        {
            this.errors[propertyName] = errorMessage;
            this.RaisePropertyChanged("HasError");
        }

        /// <summary>
        /// プロパティのエラーをクリアします。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        protected void ClearError(string propertyName)
        {
            if (this.errors.ContainsKey(propertyName))
            {
                this.errors.Remove(propertyName);
                this.RaisePropertyChanged("HasError");
            }
        }

        /// <summary>
        /// すべてのエラーをクリアします。
        /// </summary>
        protected void ClearErrors()
        {
            this.errors.Clear();
            this.RaisePropertyChanged("HasError");
        }

        public bool HasError
        {
            get
            {
                return this.errors.Count != 0;
            }
        }
    }
}
