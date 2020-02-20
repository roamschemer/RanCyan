using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RanCyan.Models {
    /// <summary>ページクラス</summary>
    public class PageModel : BindableBase {

        /// <summary>ページタイトル</summary>
        public string PageTitle { get => pageTitle; set => SetProperty(ref pageTitle, value); }
        private string pageTitle;

        public ObservableCollection<CategoryModel> CategoryModels { get; }

    }
}
