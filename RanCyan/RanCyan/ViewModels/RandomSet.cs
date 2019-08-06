using RanCyan.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RanCyan.ViewModels
{
    public class RandomSet
    {
        public RandomList RandomList;
        /// <summary>ランダムアイテムコレクション</summary>
        public ReadOnlyReactiveCollection<Item> Items { get; set; }
        /// <summary>ランダムアイテムタップイベント</summary>
        public ReactiveCommand<Item> ItemTapped { get; set; } = new ReactiveCommand<Item>();
        /// <summary>ランダムボタン</summary>
        public ReactiveCommand RanCommand { get; set; }
        /// <summary>ランダムボタンのテキスト</summary>
        public ReactiveProperty<string> RanCommandButtonText { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Label { get; set; } = new ReactiveProperty<string>();
        /// <summary>選択結果ラベルの表示</summary>
        public ReactiveProperty<bool> IsVisible { get; set; } = new ReactiveProperty<bool>();
        /// <summary>選択結果ラベルの色</summary>
        public ReactiveProperty<string> Color { get; set; } = new ReactiveProperty<string>();
        /// <summary>ループする回数(回)</summary>
        public ReactiveProperty<int> LoopTimes { get; set; } = new ReactiveProperty<int>(10);
        /// <summary>ループする回数(回)リスト</summary>
        public ObservableCollection<int> LoopTimesList { get; set; }
        /// <summary>全ループ合計時間(msec)</summary>
        public ReactiveProperty<int> LoopTotalTime { get; set; } = new ReactiveProperty<int>(1000);
        /// <summary>全ループ合計時間(msec)リスト</summary>
        public ObservableCollection<int> LoopTotalTimeList { get; set; }
        public ReactiveProperty<Item> SelectedItem { get; set; } = new ReactiveProperty<Item>();
        /// <summary>Raitoのリスト</summary>
        public ObservableCollection<int> RatioList { get; set; }
        /// <summary>追加するRatio</summary>
        public ReactiveProperty<int> InputRatio { get; set; } = new ReactiveProperty<int>(1);
        /// <summary>追加するName</summary>
        public ReactiveProperty<string> InputName { get; set; } = new ReactiveProperty<string>();
        /// <summary>リストに追加</summary>
        public ReactiveCommand InsertTapped { get; set; } = new ReactiveCommand();
        /// <summary>リストを更新</summary>
        public ReactiveCommand UpDateTapped { get; set; } = new ReactiveCommand();
        /// <summary>リストから排除</summary>
        public ReactiveCommand DeleteTapped { get; set; } = new ReactiveCommand();



    }
}
