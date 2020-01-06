using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RanCyan.Models {
    /// <summary>
    /// 各ページそれぞれにおける情報クラス
    /// </summary>
    public class PageInfomation : BindableBase {
        /// <summary>
        /// ページ番号
        /// </summary>
        public int Number {
            get => number;
            set {
                SetProperty(ref number, value);
                PageTitleSet();
            }
        }
        private int number;

        /// <summary>
        /// タイトル
        /// </summary>
        public string PageTitle {
            get => pageTitle;
            set {
                SetProperty(ref pageTitle, value);
                var s = $"{Number.ToString("00")}PageTitle";
                Application.Current.Properties[s] = pageTitle; //永続化
            }
        }
        private string pageTitle;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="number">ページ番号</param>
        public PageInfomation(int number) {
            this.Number = number;
            PageTitleSet();
        }

        /// <summary>
        /// タイトルセッティング
        /// </summary>
        private void PageTitleSet() {
            var s = $"{Number.ToString("00")}PageTitle";
            if (Application.Current.Properties.ContainsKey(s)) {
                PageTitle = Application.Current.Properties[s].ToString();
            } else {
                PageTitle = $"Project{Number.ToString("00")}";
            }
        }
    }
}
