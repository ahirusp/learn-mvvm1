using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MvvmCalc.Common;
using MvvmCalc.Model;

namespace MvvmCalc.ViewModel
{
    /// <summary>
    /// MainViewのViewModel
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// 計算結果にエラーが起きた場合に送信するメッセージのトークン
        /// </summary>
        public static readonly Guid ErrorMessageToken = Guid.NewGuid();

        public const string LhsPropertyName = "Lhs";
        public const string RhsPropertyName = "Rhs";
        public const string AnswerPropertyName = "Answer";

        private string lhs;
        private string rhs;
        private double answer;

        ////private Messenger errorMessenger = new Messenger();

        private CalculateTypeViewModel selectedCalculateType;
        private RelayCommand calculateCommand;

        public MainViewModel()
        {
            this.CalculateTypes = CalculateTypeViewModel.Create().ToArray();

            // プロパティを初期化して妥当性検証を行う
            InitializeProperties();
        }

        /// <summary>
        /// プロパティの初期化を行う。
        /// </summary>
        private void InitializeProperties()
        {
            // 入力値の検証を行う
            this.Lhs = string.Empty;
            this.Rhs = string.Empty;
            this.Answer = default (double);
            this.SelectedCalculateType = this.CalculateTypes.First();
        }

        /// <summary>
        /// 計算方式
        /// </summary>
        public IEnumerable<CalculateTypeViewModel> CalculateTypes { get; private set; }

        /// <summary>
        /// 現在選択されている計算方式
        /// </summary>
        public CalculateTypeViewModel SelectedCalculateType
        {
            get { return this.selectedCalculateType; }
            set
            {
                this.selectedCalculateType = value;
                this.RaisePropertyChanged("SelectedCalculateType");
            }
        }

        /// <summary>
        /// 計算の左辺値
        /// </summary>
        public string Lhs
        {
            get { return this.lhs; }
            set
            {
                if (lhs == value)
                {
                    return;
                }

                var oldValue = lhs;
                this.lhs = value;
                if (!this.IsDouble(value))
                {
                    this.SetError("Lhs", "数字を入力してください");
                }
                else
                {
                    this.ClearError("Lhs");
                }
                // Update bindings, no broadcast
                this.RaisePropertyChanged("Lhs");

                // Update binfings and broadcast change using GalaSoft.MvvmLight.Messenging
                this.RaisePropertyChanged(LhsPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// 計算の右辺値
        /// </summary>
        public string Rhs
        {
            get { return this.rhs; }
            set
            {
                if (rhs == value)
                {
                    return;
                }

                var oldValue = rhs;
                this.rhs = value;
                if (!this.IsDouble(value))
                {
                    this.SetError("Rhs", "数字を入力してください");
                }
                else
                {
                    this.ClearError("Rhs");
                }

                // Update bindings, no broadcast
                this.RaisePropertyChanged("Rhs");

                // Update binfings and broadcast change using GalaSoft.MvvmLight.Messenging
                this.RaisePropertyChanged(RhsPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// 計算結果
        /// </summary>
        public double Answer
        {
            get { return this.answer; }
            set
            {
                if (answer == value)
                {
                    return;
                }

                var oldValue = answer;
                this.answer = value;

                // Update bindings, no broadcast
                this.RaisePropertyChanged("Answer");

                // Update binfings and broadcast change using GalaSoft.MvvmLight.Messenging
                this.RaisePropertyChanged(AnswerPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// 計算処理のコマンド
        /// </summary>
        public RelayCommand CalculateCommand
        {
            get
            {
                if (this.calculateCommand == null)
                {
                    this.calculateCommand = new RelayCommand(CalculateExecute, CanCalculateExecute);
                }

                return this.calculateCommand;
            }
        }

        /// <summary>
        /// 計算処理が実行可能かどうかの判定を行います。
        /// </summary>
        /// <returns>実行可能であればtrue</returns>
        private bool CanCalculateExecute()
        {
            // 現在選択されている計算方法がNone以外かつ入力エラーがなければコマンドの実行が可能
            return this.SelectedCalculateType.CalculateType != CalculateType.None && !this.HasError;
        }

        /// <summary>
        /// 計算処理のコマンドの実行を行います。
        /// </summary>
        private void CalculateExecute()
        {
            // 現在の入力値を元に計算を行う
            var calc = new Calculator();
            this.Answer = calc.Execute(
                 Double.Parse(this.Lhs),
                 Double.Parse(this.Rhs),
                 this.SelectedCalculateType.CalculateType);

            if (IsInvalidAnswer())
            {
                // 計算結果が実数の範囲から外れている場合はViewに通知する
                var message = new DialogMessage("確認", "計算結果が実数の範囲を超えました。入力値を初期化しますか？",
                    result =>
                    {
                        // Viewから入力を初期化すると指定された場合はプロパティの初期化を行う
                        if (!result)
                        {
                            return;
                        }

                        InititalizeProperties();
                    });

                Messenger.Default.Send<DialogMessage>(message);
            }
        }

        /// <summary>
        /// プロパティの初期化を行う。
        /// </summary>
        private void InititalizeProperties()
        {
            this.Lhs = string.Empty;
            this.Rhs = string.Empty;
            this.Answer = default(double);
            this.SelectedCalculateType = this.CalculateTypes.First();
        }

        /// <summary>
        /// Answerが有効な実装値か確認する。
        /// </summary>
        /// <returns>有効な実数の範囲にある場合はtrueを返す</returns>
        private bool IsInvalidAnswer()
        {
            return double.IsInfinity(this.Answer) || double.IsNaN(this.Answer);
        }

        /// <summary>
        /// valueがdouble型に変換できるかどうか検証します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>doubleに変換できる場合はtrueを返す</returns>
        private bool IsDouble(string value)
        {
            var temp = default(double);
            return double.TryParse(value, out temp);
        }
    }
}
