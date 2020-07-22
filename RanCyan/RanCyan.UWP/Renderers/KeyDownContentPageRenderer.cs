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
                    var strKey = args.VirtualKey.ToString();

                    switch (strKey) {
                        case "186":
                            strKey = ":";
                            break;
                        case "187":
                            strKey = ";";
                            break;
                        case "188":
                            strKey = ",";
                            break;
                        case "190":
                            strKey = ".";
                            break;
                        case "191":
                            strKey = "/";
                            break;
                        case "192":
                            strKey = "@";
                            break;
                        case "219":
                            strKey = "[";
                            break;
                        case "220":
                            strKey = "\\";
                            break;
                        case "221":
                            strKey = "]";
                            break;
                    }
                    myPage.OnKeyDown(strKey);
                }
            }
        }
    }
}
