using RanCyan.Renderers;
using RanCyan.UWP.Renderers;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(KeyDownContentPage), typeof(KeyDownContentPageRenderer))]
namespace RanCyan.UWP.Renderers {
    public class KeyDownContentPageRenderer : PageRenderer {
        KeyDownContentPage myPage = null;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Page> e) {
            base.OnElementChanged(e);

            if (e.OldElement != null) {
                myPage = null;
                Unloaded -= ImageViewRenderer_Unloaded;
                Loaded -= ImageViewRenderer_Loaded;
            }

            if (e.NewElement != null) {
                myPage = (KeyDownContentPage)e.NewElement;
                Unloaded += ImageViewRenderer_Unloaded;
                Loaded += ImageViewRenderer_Loaded;
            }

        }

        private void ImageViewRenderer_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            Dispatcher.AcceleratorKeyActivated += Dispatcher_AcceleratorKeyActivated;
        }

        private void ImageViewRenderer_Unloaded(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            Dispatcher.AcceleratorKeyActivated -= Dispatcher_AcceleratorKeyActivated;
        }

        private void Dispatcher_AcceleratorKeyActivated(Windows.UI.Core.CoreDispatcher sender, Windows.UI.Core.AcceleratorKeyEventArgs args) {
            if (args.EventType == Windows.UI.Core.CoreAcceleratorKeyEventType.KeyDown) {
                if (myPage != null) {
                    string resKey(string s) {
                        if (s == "186") return ":";
                        if (s == "187") return ";";
                        if (s == "188") return ",";
                        if (s == "189") return "-";
                        if (s == "190") return ".";
                        if (s == "191") return "/";
                        if (s == "192") return "@";
                        if (s == "219") return "[";
                        if (s == "221") return "]";
                        if (s == "222") return "^";
                        if (s == "226") return "\\";
                        return s;
                    }
                    var e = new KeyDownEventArgs {
                        Key = resKey(args.VirtualKey.ToString())
                    };
                    myPage.OnKeyDown(e);
                }
            }
        }
    }
}
