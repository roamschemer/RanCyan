using RanCyan.Models;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;

namespace RanCyan.ViewModels
{
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
}
