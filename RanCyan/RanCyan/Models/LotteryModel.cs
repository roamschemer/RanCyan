using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;


namespace RanCyan.Models {
    /// <summary>抽選モデル</summary>
    public class LotteryModel : BindableBase {

        /// <summary>名称</summary>
        public string Name { get => name; set => SetProperty(ref name, value); }
        private string name;

        /// <summary>割合</summary>
        public int Ratio { get => ratio; set => SetProperty(ref ratio, value); }
        private int ratio = 1;

        /// <summary>除外状態</summary>
        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
        private bool isSelected;

        /// <summary>抽選された</summary>
        [JsonIgnore]
        public bool IsHited { get => isHited; set => SetProperty(ref isHited, value); }
        private bool isHited;

        /// <summary>レートのリスト</summary>
        [JsonIgnore]
        public ObservableCollection<int> RatioItems { get; }

        public LotteryModel() {
            RatioItems = new ObservableCollection<int>(Enumerable.Range(1, 100));
        }
        /// <summary>
        /// 選択状態を切り替える
        /// </summary>
        public void SelectionState() => IsSelected = !IsSelected;

    }
}
