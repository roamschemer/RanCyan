using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace RanCyan.Models {
    /// <summary>
    /// メインページに表示されるメニューモデル
    /// </summary>
    public class MenuModel : BindableBase {

        /// <summary>イメージ画像格納先</summary>
        public string ImageAddress { get => imageAddress; set => SetProperty(ref imageAddress, value); }
        private string imageAddress;

        /// <summary>遷移先Viewの名前 または webページアドレス</summary>
        public string ViewAddress { get => viewAddress; set => SetProperty(ref viewAddress, value); }
        private string viewAddress;

        public enum PageTypeEnum{
            /// <summary>抽選ページ</summary>
            Lottery = 0,
            /// <summary>Webページ</summary>
            Web = 1,
            /// <summary>その他のページ</summary>
            Other = 2
        }

        /// <summary>遷移先種類</summary>
        public PageTypeEnum PageType { get => pageType; set => SetProperty(ref pageType, value); }
        private PageTypeEnum pageType;

    }
}
