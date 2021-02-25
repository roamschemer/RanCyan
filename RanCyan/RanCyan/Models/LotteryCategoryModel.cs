using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RanCyan.Models {
    /// <summary>カテゴリークラス</summary>
    public class LotteryCategoryModel : BindableBase {

        /// <summary>カテゴリ名称(ボタンの名称にも使用)</summary>
        public string Title { get => title; set => SetProperty(ref title, value); }
        private string title;

        /// <summary>ループする回数</summary>
        public int NumberOfLoops { get => numberOfLoops; set => SetProperty(ref numberOfLoops, value); }
        private int numberOfLoops = 10;

        /// <summary>全ループ合計時間(msec)</summary>
        public int TotalTimeOfAllLoops { get => totalTimeOfAllLoops; set => SetProperty(ref totalTimeOfAllLoops, value); }
        private int totalTimeOfAllLoops = 1000;

        /// <summary>抽選中</summary>
        [JsonIgnore]
        public bool InLottery { get => inLottery; set => SetProperty(ref inLottery, value); }
        private bool inLottery;

        /// <summary>ラベルの情報</summary>
        [JsonIgnore]
        public LotteryLabelModel LotteryLabelModel { get => lotteryLabelModel; set => SetProperty(ref lotteryLabelModel, value); }
        private LotteryLabelModel lotteryLabelModel;

        /// <summary>抽選モデルのコレクション</summary>
        public ObservableCollection<LotteryModel> LotteryModels { get => lotteryModels; set => SetProperty(ref lotteryModels, value); }
        private ObservableCollection<LotteryModel> lotteryModels;

        /// <summary>抽選ラベルのコレクション(複数回抽選用)</summary>
        [JsonIgnore]
        public ObservableCollection<LotteryLabelModel> LotteryLabelModels { get => lotteryLabelModels; set => SetProperty(ref lotteryLabelModels, value); }
        private ObservableCollection<LotteryLabelModel> lotteryLabelModels;


        /// <summary>ループする回数選択リスト</summary>
        [JsonIgnore]
        public ObservableCollection<int> NumberOfLoopsSelectList { get; }

        /// <summary>全ループ合計時間(msec)選択リスト</summary>
        [JsonIgnore]
        public ObservableCollection<int> TotalTimeOfAllLoopsSelectList { get; }

        /// <summary>数値追加選択リスト</summary>
        [JsonIgnore]
        public ObservableCollection<int> RangeList { get; }

        /// <summary>数値追加選択レンジFrom</summary>
        [JsonIgnore]
        public int RangeFrom { get => rangeFrom; set => SetProperty(ref rangeFrom, value); }
        private int rangeFrom = 1;

        /// <summary>数値追加選択レンジTo</summary>
        [JsonIgnore]
        public int RangeTo { get => rangeTo; set => SetProperty(ref rangeTo, value); }
        private int rangeTo = 1;

        /// <summary>コンストラクタ</summary>
        public LotteryCategoryModel() {
            NumberOfLoopsSelectList = new ObservableCollection<int>(Enumerable.Range(1, 50).Select(x => x * 10));
            TotalTimeOfAllLoopsSelectList = new ObservableCollection<int>(Enumerable.Range(1, 50).Select(x => x * 500));
            RangeList = new ObservableCollection<int>(Enumerable.Range(0, 1000));
            LotteryLabelModel = new LotteryLabelModel();
            LotteryLabelModels = new ObservableCollection<LotteryLabelModel>();
        }

        /// <summary>見本生成</summary>
        public void ResetModels() {
            var items = Enumerable.Range(0, 1).Select(x => new LotteryModel() { Name = $"select{x}" });
            LotteryModels = new ObservableCollection<LotteryModel>(items);
        }

        /// <summary>抽選モデルの前移動</summary>
        public void Up(LotteryModel m) {
            var item = LotteryModels.Select((model, index) => (model, index)).First(x => x.model == m);
            if (item.index == 0) return;
            LotteryModels.Move(item.index, item.index - 1);
        }

        /// <summary>抽選モデルの後移動</summary>
        public void Down(LotteryModel m) {
            var item = LotteryModels.Select((model, index) => (model, index)).First(x => x.model == m);
            if (item.index == LotteryModels.Count - 1) return;
            LotteryModels.Move(item.index, item.index + 1);
        }

        /// <summary>抽選モデルの削除</summary>
        public void Clear(LotteryModel m) {
            if (LotteryModels.Count == 1) return;
            LotteryModels.Remove(m);
        }

        /// <summary>抽選モデルの作成</summary>
        public void Create() => LotteryModels.Add(new LotteryModel() { Name = $"select{LotteryModels.Count}" });

        /// <summary>クリップボードに存在する項目を全部張り付ける</summary>
        public async void ClipboardGet() {
            if (!Clipboard.HasText) return;
            var words = await Clipboard.GetTextAsync();
            var names = words.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            LotteryModels.Clear();
            foreach (var x in names) {
                LotteryModels.Add(new LotteryModel() { Name = x });
            }
        }

        /// <summary>連番追加</summary>
        public void RangeInput() {
            if (RangeFrom > RangeTo) return;
            LotteryModels.Clear();
            foreach (var x in Enumerable.Range(RangeFrom, RangeTo - RangeFrom + 1)) {
                LotteryModels.Add(new LotteryModel() { Name = x.ToString() });
            }
        }

        /// <summary>
        /// 抽選の実施
        /// </summary>
        public async Task ToDrawAsync(LotteryPageModel pageModel, LotteryLabelModel lotteryLabelModel = null) {
            if (lotteryLabelModel == null) lotteryLabelModel = LotteryLabelModel;
            var raitoSum = LotteryModels.Where(x => !x.IsSelected).Sum(x => x.Ratio);
            if (raitoSum == 0 || LotteryModels.Count(x => !x.IsSelected) < 2 || InLottery) return;
            pageModel.SelectionLotteryCategoryModel = this;
            InLottery = true;
            lotteryLabelModel.Color = "Black";
            foreach (var x in LotteryModels) x.IsHited = false;
            var rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            float oneWaitTime = TotalTimeOfAllLoops / (((NumberOfLoops - 1) * NumberOfLoops) / 2); //基準となるウェイト時間(msec)
            //抽選実施
            foreach (var i in Enumerable.Range(0, NumberOfLoops)) {
                var hitCount = rnd.Next(1, raitoSum + 1);
                int lastCount = 0; int count = 0;
                foreach (var x in LotteryModels.Where(x => !x.IsSelected)) {
                    count += x.Ratio;
                    x.IsHited = (lastCount < hitCount && hitCount <= count);
                    if (x.IsHited) lotteryLabelModel.Text = x.Name;
                    lastCount = count;
                }
                if (i < NumberOfLoops) await Task.Delay((int)(oneWaitTime * i)); //少しずつウェイト時間を長くする
            }
            //最後に点滅させる
            lotteryLabelModel.Color = "Red";
            foreach (var x in LotteryModels.Where(x => x.IsHited)) {
                foreach (var i in Enumerable.Range(0, 10)) {
                    x.IsHited = !x.IsHited;
                    lotteryLabelModel.Text = x.IsHited ? x.Name : string.Empty;
                    await Task.Delay(50);
                }
            }
            InLottery = false;
        }

        /// <summary>
        /// 抽選の実施
        /// </summary>
        /// <param name="lotteryNumber">抽選回数</param>
        public async Task ToDrawAsync(LotteryPageModel pageModel, int lotteryNumber) {
            LotteryLabelModels.Clear();
            foreach (var i in Enumerable.Range(0, lotteryNumber)) {
                var lotterLabelModel = new LotteryLabelModel();
                lotterLabelModel.Number = i + 1;
                LotteryLabelModels.Add(lotterLabelModel);
                await ToDrawAsync(pageModel, lotterLabelModel);
            }
        }
    }
}
