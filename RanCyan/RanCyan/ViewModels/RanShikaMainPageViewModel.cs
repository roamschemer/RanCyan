using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace RanCyan.ViewModels
{
    public class RanShikaMainPageViewModel : BindableBase, IDisposable
    {
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public ReactiveProperty<String> RanCyanKowashiyaImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniKowashiyaRanCyan.png");
        public ReactiveProperty<String> RanCyanMikoImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniMikoRanCyan.png");
        public ReactiveProperty<String> RanCyanImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.TalkRanCyan.png");
        public ReactiveProperty<String> RanCyanMainImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MainRanCyan.png");

        public ReadOnlyReactiveCollection<Item> ShingenItems { get; }
        public ReactiveCommand<Item> ShingenItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand ShingenRanCommand { get; }
        public ReactiveProperty<String> ShingenLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> ShingenVisible { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> ShingenColor { get; set; } = new ReactiveProperty<string>();

        public ReadOnlyReactiveCollection<Item> KoushinItems { get; }
        public ReactiveCommand<Item> KoushinItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand KoushinRanCommand { get; }
        public ReactiveProperty<String> KoushinLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> KoushinVisible { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> KoushinColor { get; set; } = new ReactiveProperty<string>();

        public ReadOnlyReactiveCollection<Item> SyokugyouItems { get; }
        public ReactiveCommand<Item> SyokugyouItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand SyokugyouRanCommand { get; }
        public ReactiveProperty<String> SyokugyouLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> SyokugyouVisible { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> SyokugyouColor { get; set; } = new ReactiveProperty<string>();

        public ReadOnlyReactiveCollection<Item> ToubatsuItems { get; }
        public ReactiveCommand<Item> ToubatsuItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand ToubatsuRanCommand { get; }
        public ReactiveProperty<String> ToubatsuLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> ToubatsuVisible { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> ToubatsuColor { get; set; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> Infomation { get; set; } = new ReactiveProperty<string>();
        public FileRead fileRead { get; } = new FileRead();

        public RanShikaMainPageViewModel()
        {
            var shingenItems = new List<Item>()
            {
                new Item { Name = "進言 1" , Ratio=1 },
                new Item { Name = "進言 2" , Ratio=1 },
                new Item { Name = "進言 3" , Ratio=1 },
                new Item { Name = "拒否" , Ratio=1 ,IsSelected=true},
                new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            };
            RandomList shingenRandomList = new RandomList("RanShikaShingen", shingenItems);
            shingenRandomList.DbDataRead();
            var koushinItems = new List<Item>()
            {
                new Item { Name = "火" , Ratio=1 },
                new Item { Name = "水" , Ratio=1 },
                new Item { Name = "風" , Ratio=1 },
                new Item { Name = "土" , Ratio=1 },
                new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            };
            RandomList koushinRandomList = new RandomList("RanShikaKoushin", koushinItems, 20, 5000);
            koushinRandomList.DbDataRead();
            var syokugyouItems = new List<Item>()
            {
                new Item { Name = "剣士" , Ratio=1 },
                new Item { Name = "薙刀士" , Ratio=1 },
                new Item { Name = "弓使い" , Ratio=1 },
                new Item { Name = "槍使い" , Ratio=1 },
                new Item { Name = "拳法家" , Ratio=1 },
                new Item { Name = "壊し屋" , Ratio=1 },
                new Item { Name = "大筒士" , Ratio=1 },
                new Item { Name = "踊り屋" , Ratio=1 },
                new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            };
            RandomList syokugyouRandomList = new RandomList("RanShikaSyokugyou", syokugyouItems, 20, 5000);
            syokugyouRandomList.DbDataRead();
            var toubatsuItems = new List<Item>()
            {
                new Item { Name = "鳥居" , Ratio=1 },
                new Item { Name = "相翼院" , Ratio=1 },
                new Item { Name = "九重楼" , Ratio=1 },
                new Item { Name = "白骨城" , Ratio=1 },
                new Item { Name = "流水道" , Ratio=1 },
                new Item { Name = "鎮魂墓" , Ratio=1 },
                new Item { Name = "紅蓮" , Ratio=1 },
                new Item { Name = "大江山" , Ratio=1 ,IsSelected=true },
                new Item { Name = "地獄" , Ratio=1 ,IsSelected=true },
                new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            };
            RandomList toubatsuRandomList = new RandomList("RanShikaToubatsu", toubatsuItems, 20, 5000);
            toubatsuRandomList.DbDataRead();

            //ViewModel←Model
            ShingenItems = shingenRandomList.Items.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            ShingenRanCommand = shingenRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand().AddTo(this.Disposable);
            ShingenLabel = shingenRandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty().AddTo(this.Disposable);
            ShingenColor = shingenRandomList.ObserveProperty(x => x.LabelColor).ToReactiveProperty().AddTo(this.Disposable);
            KoushinItems = koushinRandomList.Items.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            KoushinRanCommand = koushinRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand().AddTo(this.Disposable);
            KoushinLabel = koushinRandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty().AddTo(this.Disposable);
            KoushinColor = koushinRandomList.ObserveProperty(x => x.LabelColor).ToReactiveProperty().AddTo(this.Disposable);
            SyokugyouItems = syokugyouRandomList.Items.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            SyokugyouRanCommand = syokugyouRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand().AddTo(this.Disposable);
            SyokugyouLabel = syokugyouRandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty().AddTo(this.Disposable);
            SyokugyouColor = syokugyouRandomList.ObserveProperty(x => x.LabelColor).ToReactiveProperty().AddTo(this.Disposable);
            ToubatsuItems = toubatsuRandomList.Items.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            ToubatsuRanCommand = toubatsuRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand().AddTo(this.Disposable);
            ToubatsuLabel = toubatsuRandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty().AddTo(this.Disposable);
            ToubatsuColor = toubatsuRandomList.ObserveProperty(x => x.LabelColor).ToReactiveProperty().AddTo(this.Disposable);
            Infomation = fileRead.ObserveProperty(x => x.ReadText).ToReactiveProperty().AddTo(this.Disposable);

            //Button
            ShingenItemTapped.Where(_ => !shingenRandomList.InRundom).Subscribe(x =>
            {
                x.IsSelected = !x.IsSelected;
                shingenRandomList.DBDataWrite();
            });
            ShingenRanCommand.Subscribe(_ =>
            {
                ShingenVisible.Value = true; KoushinVisible.Value = false; SyokugyouVisible.Value = false; ToubatsuVisible.Value = false;
                shingenRandomList.RandomAction();
            });
            KoushinItemTapped.Where(_ => !koushinRandomList.InRundom).Subscribe(x =>
            {
                x.IsSelected = !x.IsSelected;
                koushinRandomList.DBDataWrite();
            });
            KoushinRanCommand.Subscribe(_ =>
            {
                ShingenVisible.Value = false; KoushinVisible.Value = true; SyokugyouVisible.Value = false; ToubatsuVisible.Value = false;
                koushinRandomList.RandomAction();
            });
            SyokugyouItemTapped.Where(_ => !syokugyouRandomList.InRundom).Subscribe(x =>
            {
                x.IsSelected = !x.IsSelected;
                syokugyouRandomList.DBDataWrite();
            });
            SyokugyouRanCommand.Subscribe(_ =>
            {
                ShingenVisible.Value = false; KoushinVisible.Value = false; SyokugyouVisible.Value = true; ToubatsuVisible.Value = false;
                syokugyouRandomList.RandomAction();
            });
            ToubatsuItemTapped.Where(_ => !toubatsuRandomList.InRundom).Subscribe(x =>
            {
                x.IsSelected = !x.IsSelected;
                toubatsuRandomList.DBDataWrite();
            });
            ToubatsuRanCommand.Subscribe(_ =>
            {
                ShingenVisible.Value = false; KoushinVisible.Value = false; SyokugyouVisible.Value = false; ToubatsuVisible.Value = true;
                toubatsuRandomList.RandomAction();
            });

            fileRead.GetResourceText("RanCyan.Texts.RanShikaInfomation.txt");
        }
        public void Dispose()
        {
            this.Disposable.Dispose();
        }
    }
}
