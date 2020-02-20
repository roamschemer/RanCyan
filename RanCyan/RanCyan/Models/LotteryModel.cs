using Prism.Mvvm;
using SQLite;

namespace RanCyan.Models {
    /// <summary>抽選モデル(SQLiteで永続化される)</summary>
    public class LotteryModel : BindableBase {

        /// <summary>主キー</summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get => id; set => SetProperty(ref id, value); }
        private int id = 0;

        /// <summary>ラベル名称</summary>
        public string Name { get => name; set => SetProperty(ref name, value); }
        private string name;

        /// <summary>割合</summary>
        public int Ratio { get => ratio; set => SetProperty(ref ratio, value); }
        private int ratio;

        /// <summary>除外状態</summary>
        public bool IsSelected { get => isSelected; set => SetProperty(ref isSelected, value); }
        private bool isSelected;

        /// <summary>選択された</summary>
        public bool IsHited { get => isHited; set => SetProperty(ref isHited, value); }
        private bool isHited;

    }
}
