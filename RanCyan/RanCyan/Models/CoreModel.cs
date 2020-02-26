using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RanCyan.Models {
    /// <summary>コアクラス</summary>
    public class CoreModel : BindableBase {

        /// <summary>透過用背景色</summary>
        public string BackColor { get => backColor; set => SetProperty(ref backColor, value); }
        private string backColor;

        /// <summary>乱ちゃんの表示画像</summary>
        public string RanCyanImage { get => ranCyanImage; set => SetProperty(ref ranCyanImage, value); }
        private string ranCyanImage;

        /// <summary>
        /// 乱ちゃん画像の表示アクションフラグ
        /// </summary>
        public bool IsImageActive { get => isImageActive; set => SetProperty(ref isImageActive, value); }
        private bool isImageActive;

        /// <summary>選択された抽選ページ</summary>
        public LotteryPageModel SelectionLotteryPageModel { get => selectionLotteryPageModel; set => SetProperty(ref selectionLotteryPageModel, value); }
        private LotteryPageModel selectionLotteryPageModel;

        /// <summary>ページモデルのコレクション</summary>
        public ObservableCollection<LotteryPageModel> LotteryPageModels { get; private set; }

        /// <summary>コンストラクタ</summary>
        public CoreModel() {
            ResetModels();
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

        /// <summary>
        /// 乱ちゃんの画像を抽選画像に差し替えた後、ちょっと待ってから待機画像に差し替える
        /// </summary>
        public async void LotteryRancyanImageAsync() {
            RanCyanImage = "RanCyan.Images.3D_Jamp1.gif";
            await Task.Delay(4000);
            WaitingRancyanImage();
        }

        /// <summary>
        /// 乱ちゃんの画像を待機画像に差し替える
        /// </summary>
        public void WaitingRancyanImage() {
            var images = new[] {
                "RanCyan.Images.3D_Taiki2.gif",
                "RanCyan.Images.3D_Taiki5.gif",
                "RanCyan.Images.3D_Taiki6.gif",
                "RanCyan.Images.3D_Taiki7.gif",
                "RanCyan.Images.3D_Taiki8.gif",
            };
            var rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            RanCyanImage = images[rnd.Next(0, images.Count())];
        }

        /// <summary>
        /// 乱ちゃんの立ち絵を消す(初期状態)
        /// </summary>
        public void StartRancyanImage() {
            RanCyanImage = "";
            IsImageActive = false;
        }
    }
}
