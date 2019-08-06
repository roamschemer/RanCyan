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
    public class RanShikaMainPageViewModel : ViewModelBase
    {
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public ReactiveProperty<string> RanCyanKowashiyaImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniKowashiyaRanCyan.png");
        public ReactiveProperty<string> RanCyanMikoImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniMikoRanCyan.png");
        public ReactiveProperty<string> RanCyanImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.TalkRanCyan.png");
        public ReactiveProperty<string> RanCyanMainImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MainRanCyan.png");

        RandomList ShingenRandomList;
        public ReadOnlyReactiveCollection<Item> ShingenItems { get; }
        public ReactiveCommand<Item> ShingenItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand ShingenRanCommand { get; }
        public ReactiveProperty<string> ShingenLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> ShingenVisible { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> ShingenColor { get; set; } = new ReactiveProperty<string>();

        RandomList KoushinRandomList;
        public ReadOnlyReactiveCollection<Item> KoushinItems { get; }
        public ReactiveCommand<Item> KoushinItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand KoushinRanCommand { get; }
        public ReactiveProperty<string> KoushinLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> KoushinVisible { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> KoushinColor { get; set; } = new ReactiveProperty<string>();

        RandomList SyokugyouRandomList;
        public ReadOnlyReactiveCollection<Item> SyokugyouItems { get; }
        public ReactiveCommand<Item> SyokugyouItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand SyokugyouRanCommand { get; }
        public ReactiveProperty<string> SyokugyouLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> SyokugyouVisible { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> SyokugyouColor { get; set; } = new ReactiveProperty<string>();

        RandomList ToubatsuRandomList;
        public ReadOnlyReactiveCollection<Item> ToubatsuItems { get; }
        public ReactiveCommand<Item> ToubatsuItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand ToubatsuRanCommand { get; }
        public ReactiveProperty<string> ToubatsuLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> ToubatsuVisible { get; set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> ToubatsuColor { get; set; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> Infomation { get; set; } = new ReactiveProperty<string>();
        public FileRead fileRead { get; } = new FileRead();


        public RanShikaMainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            //var shingenItems = new List<Item>()
            //{
            //    new Item { Name = "進言 1" , Ratio=1 },
            //    new Item { Name = "進言 2" , Ratio=1 },
            //    new Item { Name = "進言 3" , Ratio=1 },
            //    new Item { Name = "拒否" , Ratio=1 ,IsSelected=true},
            //    new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            //};
            //ShingenRandomList = new RandomList("RanShikaShingen", shingenItems);
            //ShingenRandomList.DbDataRead();
            //var koushinItems = new List<Item>()
            //{
            //    new Item { Name = "火" , Ratio=1 },
            //    new Item { Name = "水" , Ratio=1 },
            //    new Item { Name = "風" , Ratio=1 },
            //    new Item { Name = "土" , Ratio=1 },
            //    new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            //};
            //KoushinRandomList = new RandomList("RanShikaKoushin", koushinItems, 20, 5000);
            //KoushinRandomList.DbDataRead();
            //var syokugyouItems = new List<Item>()
            //{
            //    new Item { Name = "剣士" , Ratio=1 },
            //    new Item { Name = "薙刀士" , Ratio=1 },
            //    new Item { Name = "弓使い" , Ratio=1 },
            //    new Item { Name = "槍使い" , Ratio=1 },
            //    new Item { Name = "拳法家" , Ratio=1 },
            //    new Item { Name = "壊し屋" , Ratio=1 },
            //    new Item { Name = "大筒士" , Ratio=1 },
            //    new Item { Name = "踊り屋" , Ratio=1 },
            //    new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            //};
            //SyokugyouRandomList = new RandomList("RanShikaSyokugyou", syokugyouItems, 20, 5000);
            //SyokugyouRandomList.DbDataRead();
            //var toubatsuItems = new List<Item>()
            //{
            //    new Item { Name = "鳥居" , Ratio=1 },
            //    new Item { Name = "相翼院" , Ratio=1 },
            //    new Item { Name = "九重楼" , Ratio=1 },
            //    new Item { Name = "白骨城" , Ratio=1 },
            //    new Item { Name = "流水道" , Ratio=1 },
            //    new Item { Name = "鎮魂墓" , Ratio=1 },
            //    new Item { Name = "紅蓮" , Ratio=1 },
            //    new Item { Name = "大江山" , Ratio=1 ,IsSelected=true },
            //    new Item { Name = "地獄" , Ratio=1 ,IsSelected=true },
            //    new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            //};
            //ToubatsuRandomList = new RandomList("RanShikaToubatsu", toubatsuItems, 20, 5000);
            //ToubatsuRandomList.DbDataRead();

            //ViewModel←Model
            ShingenItems = ShingenRandomList.Items.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            ShingenRanCommand = ShingenRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand().AddTo(this.Disposable);
            ShingenLabel = ShingenRandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty().AddTo(this.Disposable);
            ShingenColor = ShingenRandomList.ObserveProperty(x => x.LabelColor).ToReactiveProperty().AddTo(this.Disposable);
            KoushinItems = KoushinRandomList.Items.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            KoushinRanCommand = KoushinRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand().AddTo(this.Disposable);
            KoushinLabel = KoushinRandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty().AddTo(this.Disposable);
            KoushinColor = KoushinRandomList.ObserveProperty(x => x.LabelColor).ToReactiveProperty().AddTo(this.Disposable);
            SyokugyouItems = SyokugyouRandomList.Items.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            SyokugyouRanCommand = SyokugyouRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand().AddTo(this.Disposable);
            SyokugyouLabel = SyokugyouRandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty().AddTo(this.Disposable);
            SyokugyouColor = SyokugyouRandomList.ObserveProperty(x => x.LabelColor).ToReactiveProperty().AddTo(this.Disposable);
            ToubatsuItems = ToubatsuRandomList.Items.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            ToubatsuRanCommand = ToubatsuRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand().AddTo(this.Disposable);
            ToubatsuLabel = ToubatsuRandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty().AddTo(this.Disposable);
            ToubatsuColor = ToubatsuRandomList.ObserveProperty(x => x.LabelColor).ToReactiveProperty().AddTo(this.Disposable);
            Infomation = fileRead.ObserveProperty(x => x.ReadText).ToReactiveProperty().AddTo(this.Disposable);

            //Button
            ShingenItemTapped.Where(_ => !ShingenRandomList.InRundom).Subscribe(x =>
            {
                x.IsSelected = !x.IsSelected;
                ShingenRandomList.DBDataWrite();
            });
            ShingenRanCommand.Subscribe(_ =>
            {
                ShingenVisible.Value = true; KoushinVisible.Value = false; SyokugyouVisible.Value = false; ToubatsuVisible.Value = false;
                ShingenRandomList.RandomAction();
            });
            KoushinItemTapped.Where(_ => !KoushinRandomList.InRundom).Subscribe(x =>
            {
                x.IsSelected = !x.IsSelected;
                KoushinRandomList.DBDataWrite();
            });
            KoushinRanCommand.Subscribe(_ =>
            {
                ShingenVisible.Value = false; KoushinVisible.Value = true; SyokugyouVisible.Value = false; ToubatsuVisible.Value = false;
                KoushinRandomList.RandomAction();
            });
            SyokugyouItemTapped.Where(_ => !SyokugyouRandomList.InRundom).Subscribe(x =>
            {
                x.IsSelected = !x.IsSelected;
                SyokugyouRandomList.DBDataWrite();
            });
            SyokugyouRanCommand.Subscribe(_ =>
            {
                ShingenVisible.Value = false; KoushinVisible.Value = false; SyokugyouVisible.Value = true; ToubatsuVisible.Value = false;
                SyokugyouRandomList.RandomAction();
            });
            ToubatsuItemTapped.Where(_ => !ToubatsuRandomList.InRundom).Subscribe(x =>
            {
                x.IsSelected = !x.IsSelected;
                ToubatsuRandomList.DBDataWrite();
            });
            ToubatsuRanCommand.Subscribe(_ =>
            {
                ShingenVisible.Value = false; KoushinVisible.Value = false; SyokugyouVisible.Value = false; ToubatsuVisible.Value = true;
                ToubatsuRandomList.RandomAction();
            });

            fileRead.GetResourceText("RanCyan.Texts.RanShikaInfomation.txt");
        }

        public override void Destroy()
        {
            this.Disposable.Dispose();
        }
    }
}
