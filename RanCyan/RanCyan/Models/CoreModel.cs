using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RanCyan.Models {
    /// <summary>コアクラス</summary>
    public class CoreModel : BindableBase {

        /// <summary>透過用背景色のリスト</summary>
        public ObservableCollection<string> ImageBackColorList { get; }

        /// <summary>透過用背景色</summary>
        public string BackColor {
            get => backColor;
            set {
                SetProperty(ref backColor, value);
                Application.Current.Properties[$"{GetType().Name}/{nameof(BackColor)}"] = backColor;
            }
        }
        private string backColor;

        /// <summary>抽選リスト部分の幅リスト</summary>
        public ObservableCollection<int> SelectionViewWidthList { get; }

        /// <summary>抽選リスト部分の幅</summary>
        public int SelectionViewWidth {
            get => selectionViewWidth; set {
                SetProperty(ref selectionViewWidth, value);
                Application.Current.Properties[$"{GetType().Name}/{nameof(SelectionViewWidth)}"] = selectionViewWidth;
            }
        }
        private int selectionViewWidth;

        /// <summary>乱ちゃんの情報</summary>
        public RanCyanModel RanCyanModel { get => ranCyanModel; set => SetProperty(ref ranCyanModel, value); }
        private RanCyanModel ranCyanModel;

        /// <summary>選択された抽選ページ</summary>
        public LotteryPageModel SelectionLotteryPageModel { get => selectionLotteryPageModel; set => SetProperty(ref selectionLotteryPageModel, value); }
        private LotteryPageModel selectionLotteryPageModel;

        /// <summary>ページモデルのコレクション</summary>
        public ObservableCollection<LotteryPageModel> LotteryPageModels { get; private set; }


        /// <summary>コンストラクタ</summary>
        public CoreModel() {
            ResetModels();
            ImageBackColorList = new ObservableCollection<string>() { "White", "Blue", "Lime", "DodgerBlue", "CornflowerBlue", "Chartreuse", "ForestGreen", "Yellow" };
            SelectionViewWidthList = new ObservableCollection<int>(Enumerable.Range(4, 7).Select(x => x * 100));
            RanCyanModel = new RanCyanModel();
            ConfigRead();
        }

        /// <summary>アプリ設定の読み込み</summary>
        private void ConfigRead() {
            string s = $"{GetType().Name}/{nameof(BackColor)}";
            BackColor = (Application.Current.Properties.ContainsKey(s)) ? Application.Current.Properties[s].ToString() : ImageBackColorList.First();
            s = $"{GetType().Name}/{nameof(SelectionViewWidth)}";
            SelectionViewWidth = (Application.Current.Properties.ContainsKey(s)) ? (int)Application.Current.Properties[s] : SelectionViewWidthList.First();
        }

        /// <summary>選択されたモデルを保有する</summary>
        /// <param name="model">選択されたモデル</param>
        public void SelectModel(LotteryPageModel model) => SelectionLotteryPageModel = model;

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
