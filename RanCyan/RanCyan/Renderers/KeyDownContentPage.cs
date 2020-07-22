using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RanCyan.Renderers {
    public class KeyDownContentPage : ContentPage {

        public static readonly BindableProperty DownKeyProperty =
            BindableProperty.Create(nameof(DownKey), //名前
                                    typeof(string),　//型
                                    typeof(KeyDownContentPage), //自クラス
                                    string.Empty,    //初期値
                                    propertyChanged: (bindable, oldValue, newValue) => //変更があったことを感知するイベントハンドラ
                                    {
                                        ((KeyDownContentPage)bindable).DownKey = newValue;
                                    },
                                    defaultBindingMode: BindingMode.OneWayToSource); //初期バインディング方向
        public object DownKey {
            get => GetValue(DownKeyProperty);
            set => SetValue(DownKeyProperty, value);
        }

        public void OnKeyDown(string key) {
            DownKey = key;
            DownKey = string.Empty; //苦肉の策
        }
    }
}
