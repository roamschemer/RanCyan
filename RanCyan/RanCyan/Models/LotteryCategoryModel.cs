using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RanCyan.Models {
    /// <summary>カテゴリークラス</summary>
    public class LotteryCategoryModel : BindableBase {

        /// <summary>カテゴリ名称(ボタンの名称にも使用)</summary>
        public string Name { get => name; set => SetProperty(ref name, value); }
        private string name;

        /// <summary>ループする回数</summary>
        public int NumberOfLoops { get => numberOfLoops; set => SetProperty(ref numberOfLoops, value); }
        private int numberOfLoops = 10;

        /// <summary>全ループ合計時間(msec)</summary>
        public int TotalTimeOfAllLoops { get => totalTimeOfAllLoops; set => SetProperty(ref totalTimeOfAllLoops, value); }
        private int totalTimeOfAllLoops = 1000;

        /// <summary>抽選中</summary>
        public bool InLottery { get => inLottery; set => SetProperty(ref inLottery, value); }
        private bool inLottery;

        /// <summary>抽選モデルのコレクション</summary>
        public ObservableCollection<LotteryModel> LotteryModels { get; private set; }

        /// <summary>コンストラクタ</summary>
        public LotteryCategoryModel() {
            ResetModels();
        }

        /// <summary>
        /// 見本生成
        /// </summary>
        private void ResetModels() {
            var items = Enumerable.Range(0, 6).Select(x => new LotteryModel() { Id = x, Name = $"select{x}" });
            LotteryModels = new ObservableCollection<LotteryModel>(items);
        }

        /// <summary>
        /// 抽選の実施
        /// </summary>
        public async void ToDrawAsync() {
            var raitoSum = LotteryModels.Where(x => !x.IsSelected).Sum(x => x.Ratio);
            if (raitoSum == 0 || LotteryModels.Count(x => !x.IsSelected) < 2) return;
            foreach (var x in LotteryModels) x.IsHited = false;
            var rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            float oneWaitTime = TotalTimeOfAllLoops / (((NumberOfLoops - 1) * NumberOfLoops) / 2); //基準となるウェイト時間(msec)
            InLottery = true;
            //抽選実施
            foreach (var i in Enumerable.Range(0, NumberOfLoops)) {
                var hitCount = rnd.Next(1, raitoSum + 1);
                int lastCount = 0; int count = 0;
                foreach (var x in LotteryModels.Where(x => !x.IsSelected)) {
                    count += x.Ratio;
                    x.IsHited = (lastCount < hitCount && hitCount <= count);
                    lastCount = count;
                }
                if (i < NumberOfLoops) await Task.Delay((int)(oneWaitTime * i)); //少しずつウェイト時間を長くする
            }
            //最後に点滅させる(ここでは選択状態を等間隔でON/OFFするだけ)
            foreach (var x in LotteryModels.Where(x => x.IsHited)) {
                foreach (var i in Enumerable.Range(0, 10)) {
                    x.IsHited = !x.IsHited;
                    await Task.Delay(50);
                }
            }
            InLottery = false;
        }
    }
}
