using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RanCyan.Renderers {

    public class KeyDownContentPage : ContentPage {

        public delegate void KeyDownEventHandler(object sender, KeyDownEventArgs e);
        public event KeyDownEventHandler KeyDown;

        public void OnKeyDown(KeyDownEventArgs e) => KeyDown?.Invoke(this, e);

    }
    public class KeyDownEventArgs : EventArgs {
        public string Key { get; set; }
    }

}
