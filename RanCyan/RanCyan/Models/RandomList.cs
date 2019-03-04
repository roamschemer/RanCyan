using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RanCyan.Models
{
    public class Item : BindableBase
    {
        /// <summary>
        /// ラベル名称
        /// </summary>
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        private string name;

        /// <summary>
        /// 除外状態
        /// </summary>
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }
        private bool isSelected;

        /// <summary>
        /// 割合 
        /// </summary>
        public int Ratio
        {
            get => ratio;
            set => SetProperty(ref ratio, value);
        }
        private int ratio;

        /// <summary>
        /// ランダム選択された
        /// </summary>
        public bool IsHited
        {
            get => isHited;
            set => SetProperty(ref isHited, value);
        }
        private bool isHited;
        
    }

    public class RandomList : BindableBase
    {
        /// <summary>
        /// リスト
        /// </summary>
        public ObservableCollection<Item> Items { get; }

        /// <summary>
        /// ループする回数(回)
        /// </summary>
        public int LoopNo
        {
            get => loopNo;
            set => SetProperty(ref loopNo, value);
        }
        private int loopNo;

        /// <summary>
        /// 全ループ合計時間(msec)
        /// </summary>
        public int LoopTime
        {
            get => loopTime;
            set => SetProperty(ref loopTime, value);
        }
        private int loopTime;

        /// <summary>
        /// ランダム実行中
        /// </summary>
        public bool InRundom
        {
            get => inRundom;
            set => SetProperty(ref inRundom, value);
        }
        private bool inRundom;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="items">ループリスト</param>
        /// <param name="loopNo">ループする回数(回)</param>
        /// <param name="loopTime">全ループ合計時間(msec)</param>
        public RandomList(List<Item> items, int loopNo = 10, int loopTime = 1000)
        {
            Items = new ObservableCollection<Item>(items);
            LoopNo = loopNo;
            LoopTime = loopTime;
        }

        public async void RandomAction()
        {

            //シード値を取得(乱数固定化の阻止)
            int seed = Environment.TickCount;
            // Randomクラスのインスタンス生成
            Random rnd = new System.Random(seed);

            //基準となるウェイト時間(msec)
            float oneWaitTime = loopTime / (((loopNo - 1) * loopNo) / 2);

            var RaitoSum = Items.Where(x => !x.IsSelected)  //選択している奴は除外した
                                .Sum(x => x.Ratio);         //Ratioの合計値
            if (RaitoSum == 0) return; //レートが0の場合は抽選を回避する
            if (Items.Count(x => !x.IsSelected) <= 1) return; //対象が1以下の場合抽選をの意味がないので回避する
            Items.Where(x => x.IsSelected).Select(x => x.IsHited = false).ToList();//IsHitedは全部Falseにする
            InRundom = true;
            for (int i = 1; i <= loopNo; i++)
            {
                // listの数からダンダムで値を取得
                var hitCount = rnd.Next(1, RaitoSum + 1);
                // 得た数値に該当するIsHitをTrueに。それ以外はFalseにする。
                int lastCount = 0;
                int count = 0;
                foreach (var x in Items)
                {
                    if (!x.IsSelected)
                    {
                        count = count + x.Ratio;
                        x.IsHited = (lastCount < hitCount && hitCount <= count);
                        lastCount = count;
                    }
                }

                if (i < loopNo)
                {
                    //少しずつウェイト時間を長くする
                    await Task.Delay((int)(oneWaitTime * i));
                }
            }

            //最後に点滅させる
            foreach (var x in Items)
            {
                if (x.IsHited)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        x.IsHited = !x.IsHited;
                        await Task.Delay(50);
                    }
                }
            }
            InRundom = false;
        }
    }

}
