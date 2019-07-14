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
    public class GenericRandomMainPageViewModel : ViewModelBase
    {
        private CompositeDisposable Disposable { get; } = new CompositeDisposable();
        public ReactiveProperty<String> RanCyanKowashiyaImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniKowashiyaRanCyan.png");
        public ReactiveProperty<String> RanCyanMikoImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MiniMikoRanCyan.png");
        public ReactiveProperty<String> RanCyanImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.TalkRanCyan.png");
        public ReactiveProperty<String> RanCyanMainImage { get; set; } = new ReactiveProperty<string>("RanCyan.Images.MainRanCyan.png");

        public class RandomSet
        {
            public RandomList RandomList;
            public ReadOnlyReactiveCollection<Item> Items { get; set; }
            public ReactiveCommand<Item> ItemTapped { get; set; } = new ReactiveCommand<Item>();
            public ReactiveCommand RanCommand { get; set; }
            public ReactiveProperty<string> Label { get; set; } = new ReactiveProperty<string>("a");
            public ReactiveProperty<bool> IsVisible { get; set; } = new ReactiveProperty<bool>();
            public ReactiveProperty<string> Color { get; set; } = new ReactiveProperty<string>();
        }
        public RandomSet FastSet { get; set; } = new RandomSet();

        public GenericRandomMainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            var items = new List<Item>()
            {
                new Item { Name = "進言 1" , Ratio=1 },
                new Item { Name = "進言 2" , Ratio=1 },
                new Item { Name = "進言 3" , Ratio=1 },
                new Item { Name = "拒否" , Ratio=1 ,IsSelected=true},
                new Item { Name = "自由" , Ratio=1 ,IsSelected=true},
            };
            FastSet.RandomList = new RandomList("GenericFast", items);
            FastSet.RandomList.DbDataRead();

            //ViewModel←Model
            FastSet.Items = FastSet.RandomList.Items.ToReadOnlyReactiveCollection().AddTo(this.Disposable);
            FastSet.RanCommand = FastSet.RandomList.ObserveProperty(x => !x.InRundom).ToReactiveCommand().AddTo(this.Disposable);
            FastSet.Label = FastSet.RandomList.ObserveProperty(x => x.DataLabel).ToReactiveProperty().AddTo(this.Disposable);
            FastSet.Color = FastSet.RandomList.ObserveProperty(x => x.LabelColor).ToReactiveProperty().AddTo(this.Disposable);
            //Button
            FastSet.ItemTapped.Where(_ => !FastSet.RandomList.InRundom).Subscribe(x =>
            {
                x.IsSelected = !x.IsSelected;
                FastSet.RandomList.DBDataWrite();
            });
            FastSet.RanCommand.Subscribe(_ =>
            {
                FastSet.IsVisible.Value = true;
                FastSet.RandomList.RandomAction();
            });

        }
        public override void Destroy()
        {
            this.Disposable.Dispose();
        }
    }
}
