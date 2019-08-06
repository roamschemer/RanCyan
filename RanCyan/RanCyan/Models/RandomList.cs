using PCLStorage;
using Prism.Mvvm;
using Reactive.Bindings;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RanCyan.Models
{
    public class Item : BindableBase
    {
        //主キー
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }
        private int id = 0;
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
        public int LoopTimes
        {
            get => loopTimes;
            set => SetProperty(ref loopTimes, value);
        }
        private int loopTimes;

        /// <summary>
        /// 全ループ合計時間(msec)
        /// </summary>
        public int LoopTotalTime
        {
            get => loopTotalTime;
            set => SetProperty(ref loopTotalTime, value);
        }
        private int loopTotalTime;

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
        /// 選択された値
        /// </summary>
        public string DataLabel
        {
            get => dataLabel;
            set => SetProperty(ref dataLabel, value);
        }
        private string dataLabel;

        /// <summary>
        /// ラベルの色
        /// </summary>
        public string LabelColor
        {
            get => labelColor;
            set => SetProperty(ref labelColor, value);
        }
        private string labelColor;

        private string dbPath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbPath">DBテーブル名</param>
        /// <param name="items">ループリスト</param>
        /// <param name="loopNo">ループする回数(回)</param>
        /// <param name="loopTime">全ループ合計時間(msec)</param>
        public RandomList(List<Item> items, int loopNo = 10, int loopTime = 1000)
        {
            Items = new ObservableCollection<Item>(items);
            LoopTimes = loopNo;
            LoopTotalTime = loopTime;
        }

        public void SetDbPath(string dbPath)
        {
            this.dbPath = dbPath;
        }

        /// <summary>
        /// データベースから情報を呼び出して元の状態を復元する
        /// </summary>
        public async void DbDataRead()
        {
            var items = new ObservableCollection<Item>(Items);
            await DbLoad();
            if (Items.Count == 0)
            {
                await DbDeleteAll();
                await DbInsert(items);
                await DbLoad();
            }
        }

        /// <summary>
        /// データベースへ現在の状態を保存する
        /// </summary>
        public async void DBDataWrite()
        {
            await DbDeleteAll();
            await DbInsert(Items);
        }

        /// <summary>
        /// リストの追加
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ratio"></param>
        public async void Insert(string name, int ratio)
        {
            if (name == null) return;
            var item = new Item { Name = name, Ratio = ratio };
            Items.Add(item);
            await DbDeleteAll();
            await DbInsert(Items);
            await DbLoad();
        }

        /// <summary>
        /// リストの更新
        /// </summary>
        public async void UpDate(Item selectedItem, string name, int ratio)
        {
            if (name == null) return;
            if (selectedItem == null) return;
            var item = new Item { Id = selectedItem.Id, Name = name, Ratio = ratio };
            await DbUpDate(item);
            await DbLoad();
        }

        /// <summary>
        /// リストから消す
        /// </summary>
        public async void Delete(Item selectedItem)
        {
            if (selectedItem == null) return;
            var item = new Item { Id = selectedItem.Id };
            await DbDelete(item);
            await DbLoad();
        }

        /// <summary>
        /// データベースの呼出
        /// </summary>
        private async Task DbLoad()
        {
            Items.Clear();
            using (var db = await CreateDb())
            {
                foreach (var x in db.Table<Item>())
                {
                    Items.Add(x);
                }
            }
        }

        /// <summary>
        /// データベースの更新
        /// </summary>
        /// <param name="item">情報</param>
        private async Task DbUpDate(Item item)
        {
            using (var db = await CreateDb())
            {
                db.Update(item);
            }
        }

        /// <summary>
        /// データベースから削除
        /// </summary>
        private async Task DbDelete(Item item)
        {
            using (var db = await CreateDb())
            {
                db.Delete<Item>(item.Id);
            }
        }

        /// <summary>
        /// データベースから全削除
        /// </summary>
        private async Task DbDeleteAll()
        {
            using (var db = await CreateDb())
            {
                db.DeleteAll<Item>();
            }
        }

        /// <summary>
        /// データベースへ追加
        /// </summary>
        /// <param name="items">追加するitems</param>
        /// <returns></returns>
        private async Task DbInsert(ObservableCollection<Item> items)
        {
            using (var db = await CreateDb())
            {
                foreach (var x in items)
                {
                    db.Insert(x);
                }
            }
        }

        /// <summary>
        /// データベースの生成と取得(以下のようにPCLStorageを使わないとUWPで例外が発生した)
        /// </summary>
        /// <returns></returns>
        private async Task<SQLiteConnection> CreateDb()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            var result = await rootFolder.CheckExistsAsync(dbPath);
            if (result == ExistenceCheckResult.NotFound)
            {
                IFile file = await rootFolder.CreateFileAsync(dbPath, CreationCollisionOption.ReplaceExisting);
                var db = new SQLiteConnection(file.Path);
                db.CreateTable<Item>();
                return db;
            }
            else
            {
                IFile file = await rootFolder.CreateFileAsync(dbPath, CreationCollisionOption.OpenIfExists);
                return new SQLiteConnection(file.Path);
            }
        }

        /// <summary>
        /// ランダム選択の実行
        /// </summary>
        public async void RandomAction()
        {

            //シード値を取得(乱数固定化の阻止)
            int seed = Environment.TickCount;
            // Randomクラスのインスタンス生成
            Random rnd = new System.Random(seed);

            //基準となるウェイト時間(msec)
            float oneWaitTime = loopTotalTime / (((loopTimes - 1) * loopTimes) / 2);

            var RaitoSum = Items.Where(x => !x.IsSelected)  //選択している奴は除外した
                                .Sum(x => x.Ratio);         //Ratioの合計値
            if (RaitoSum == 0) return; //レートが0の場合は抽選を回避する
            if (Items.Count(x => !x.IsSelected) <= 1) return; //対象が1以下の場合抽選をの意味がないので回避する
            Items.Where(x => x.IsSelected).Select(x => x.IsHited = false).ToList();//IsHitedは全部Falseにする
            InRundom = true;
            LabelColor = "Black";
            for (int i = 1; i <= loopTimes; i++)
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
                        if (x.IsHited) { DataLabel = x.Name; }
                        lastCount = count;
                    }
                }

                if (i < loopTimes)
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
                        if (i % 2 == 0) { LabelColor = "Red"; } else { LabelColor = "Black"; }
                        await Task.Delay(50);
                    }
                }
            }
            LabelColor = "Red";
            InRundom = false;
        }
    }

}
