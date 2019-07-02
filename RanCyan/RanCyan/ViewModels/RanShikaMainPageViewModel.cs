using Prism.Commands;
using Prism.Mvvm;
using RanCyan.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace RanCyan.ViewModels
{
    public class RanShikaMainPageViewModel : BindableBase
    {
        public ReactiveProperty<String> RanCyanKowashiyaImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniKowashiyaRanCyan.png");
        public ReactiveProperty<String> RanCyanMikoImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniMikoRanCyan.png");
        public ReactiveProperty<String> RanCyanImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.TalkRanCyan.png");
        public ReactiveProperty<String> RanCyanMainImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MainRanCyan.png");

        public ReadOnlyReactiveCollection<Item> ShingenItems { get; }
        public ReactiveCommand<Item> ShingenItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand ShingenRanCommand { get; }
        public ReactiveProperty<String> ShingenLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> ShingenVisible { get; set; } = new ReactiveProperty<bool>();

        public ReadOnlyReactiveCollection<Item> KoushinItems { get; }
        public ReactiveCommand<Item> KoushinItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand KoushinRanCommand { get; }
        public ReactiveProperty<String> KoushinLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> KoushinVisible { get; set; } = new ReactiveProperty<bool>();

        public ReadOnlyReactiveCollection<Item> SyokugyouItems { get; }
        public ReactiveCommand<Item> SyokugyouItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand SyokugyouRanCommand { get; }
        public ReactiveProperty<String> SyokugyouLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> SyokugyouVisible { get; set; } = new ReactiveProperty<bool>();

        public ReadOnlyReactiveCollection<Item> ToubatsuItems { get; }
        public ReactiveCommand<Item> ToubatsuItemTapped { get; } = new ReactiveCommand<Item>();
        public ReactiveCommand ToubatsuRanCommand { get; }
        public ReactiveProperty<String> ToubatsuLabel { get; set; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> ToubatsuVisible { get; set; } = new ReactiveProperty<bool>();

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
            };
            RandomList shingenRandomList = new RandomList(shingenItems);
            var koushinItems = new List<Item>()
            {
                new Item { Name = "火" , Ratio=1 },
                new Item { Name = "水" , Ratio=1 },
                new Item { Name = "風" , Ratio=1 },
                new Item { Name = "土" , Ratio=1 },
            };
            RandomList koushinRandomList = new RandomList(koushinItems, 20, 5000);
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
            };
            RandomList syokugyouRandomList = new RandomList(syokugyouItems, 20, 5000);
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
            };
            RandomList toubatsuRandomList = new RandomList(toubatsuItems, 20, 5000);

            //ViewModel←Model
            ShingenItems = shingenRandomList.Items.ToReadOnlyReactiveCollection();
            ShingenRanCommand = shingenRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand();
            ShingenLabel = shingenRandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty();
            KoushinItems = koushinRandomList.Items.ToReadOnlyReactiveCollection();
            KoushinRanCommand = koushinRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand();
            KoushinLabel = koushinRandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty();
            SyokugyouItems = syokugyouRandomList.Items.ToReadOnlyReactiveCollection();
            SyokugyouRanCommand = syokugyouRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand();
            SyokugyouLabel = syokugyouRandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty();
            ToubatsuItems = toubatsuRandomList.Items.ToReadOnlyReactiveCollection();
            ToubatsuRanCommand = toubatsuRandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand();
            ToubatsuLabel = toubatsuRandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty();
            Infomation = fileRead.ObserveProperty(x => x.ReadText).ToReactiveProperty();

            //Button
            ShingenItemTapped.Where(_ => !shingenRandomList.InRundom).Subscribe(x => x.IsSelected = !x.IsSelected);
            ShingenRanCommand.Subscribe(_ =>
            {
                ShingenVisible.Value = true; KoushinVisible.Value = false; SyokugyouVisible.Value = false; ToubatsuVisible.Value = false;
                shingenRandomList.RandomAction();
            });
            KoushinItemTapped.Where(_ => !koushinRandomList.InRundom).Subscribe(x => x.IsSelected = !x.IsSelected);
            KoushinRanCommand.Subscribe(_ =>
            {
                ShingenVisible.Value = false; KoushinVisible.Value = true; SyokugyouVisible.Value = false; ToubatsuVisible.Value = false;
                koushinRandomList.RandomAction();
            });
            SyokugyouItemTapped.Where(_ => !syokugyouRandomList.InRundom).Subscribe(x => x.IsSelected = !x.IsSelected);
            SyokugyouRanCommand.Subscribe(_ =>
            {
                ShingenVisible.Value = false; KoushinVisible.Value = false; SyokugyouVisible.Value = true; ToubatsuVisible.Value = false;
                syokugyouRandomList.RandomAction();
            });

            ToubatsuItemTapped.Where(_ => !toubatsuRandomList.InRundom).Subscribe(x => x.IsSelected = !x.IsSelected);
            ToubatsuRanCommand.Subscribe(_ =>
            {
                ShingenVisible.Value = false; KoushinVisible.Value = false; SyokugyouVisible.Value = false; ToubatsuVisible.Value = true;
                toubatsuRandomList.RandomAction();
            });

            fileRead.GetResourceText("RanCyan.Texts.RanShikaInfomation.txt");
        }
    }
}
