using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace RanCyan.Models {
    /// <summary>
    /// メインページに表示されるメニューモデル
    /// </summary>
    public class MenuModel : BindableBase {

        /// <summary>リストに並ぶタイトル</summary>
        [Obsolete("削除予定")]
        public string Title { get => title; set => SetProperty(ref title, value); }
        private string title;

        /// <summary>イメージ画像格納先</summary>
        public string ImageAddress { get => imageAddress; set => SetProperty(ref imageAddress, value); }
        private string imageAddress;

        /// <summary>遷移先Viewの名前 または webページアドレス</summary>
        public string ViewAddress { get => viewAddress; set => SetProperty(ref viewAddress, value); }
        private string viewAddress;

        /// <summary>抽選ページのindex</summary>
        public int? LotteryPageIndex { get => lotteryPageIndex; set => SetProperty(ref lotteryPageIndex, value); }
        private int? lotteryPageIndex = null;

        /// <summary>遷移先がWebページ</summary>
        public bool IsWebPage { get => isWebPage; set => SetProperty(ref isWebPage, value); }
        private bool isWebPage;
    }
}
