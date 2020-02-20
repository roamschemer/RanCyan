using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace RanCyan.Models {
    /// <summary>コアクラス</summary>
    public class CoreModel : BindableBase {

        /// <summary>透過用背景色</summary>
        public string BackColor { get => backColor; set => SetProperty(ref backColor, value); }
        private string backColor;

        /// <summary>乱ちゃん表示幅</summary>
        public int ImageGridWidth { get => imageGridWidth; set => SetProperty(ref imageGridWidth, value); }
        private int imageGridWidth;
        
    }
}
