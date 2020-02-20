using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace RanCyan.Models {
    /// <summary>コアクラス</summary>
    public class CoreModel : BindableBase {

        /// <summary>透過用背景色</summary>
        public string BackColor { get => backColor; set => SetProperty(ref backColor, value); }
        private string backColor;

        /// <summary>選択されたモデル</summary>
        public LotteryPageModel LotteryPageModel { get => lotteryPageModel; set => SetProperty(ref lotteryPageModel, value); }
        private LotteryPageModel lotteryPageModel;

        /// <summary>ページモデルのコレクション</summary>
        public ObservableCollection<LotteryPageModel> LotteryPageModels { get; private set; }

        /// <summary>コンストラクタ</summary>
        public CoreModel() {
            ResetModels();
        }

        /// <summary>選択されたモデルを保有する</summary>
        /// <param name="i">選択するモデルのindex</param>
        public void SelectModel(int index) => LotteryPageModel = LotteryPageModels[index];

        /// <summary>
        /// 見本作成する
        /// </summary>
        private void ResetModels() {
            var items = Enumerable.Range(0, 5).Select(x => new LotteryPageModel() { Title = $"NewPage{x}" });
            LotteryPageModels = new ObservableCollection<LotteryPageModel>(items);
        }

        /// <summary>
        /// 新規にLotteryPageModelを追加する
        /// </summary>
        public void CleateNewLotteryPageModel() => LotteryPageModels.Add(new LotteryPageModel() { Title = $"NewPage{LotteryPageModels.Count()}" });

        /// <summary>
        /// 指定したindexのLotteryPageModelを削除する
        /// </summary>
        /// <param name="index">消去するモデルのindex</param>
        public void DeleteLotteryPageModel(int index) => LotteryPageModels.RemoveAt(index);

    }
}
