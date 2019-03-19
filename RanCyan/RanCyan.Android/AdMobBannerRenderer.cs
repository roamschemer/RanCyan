using RanCyan.Renderers;
using RanCyan.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.Threading.Tasks;

[assembly: ExportRenderer(typeof(AdMobBanner), typeof(AdMobBannerRenderer))]
namespace RanCyan.Droid.Renderers
{
#pragma warning disable 0618 //旧型式です警告回避。

    public class AdMobBannerRenderer : ViewRenderer<AdMobBanner, Android.Gms.Ads.AdView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<AdMobBanner> e)
        {
#if DEBUG
            const string adUnitID = "ca-app-pub-3940256099942544/6300978111";//テスト用バナー
#else
            const string adUnitID = "ca-app-pub-3940256099942544/6300978111";//本物
#endif
            base.OnElementChanged(e);

            if (Control == null)
            {
                var adMobBanner = new Android.Gms.Ads.AdView(Forms.Context);
                adMobBanner.AdSize = Android.Gms.Ads.AdSize.Banner;
                adMobBanner.AdUnitId = adUnitID;

                var requestbuilder = new Android.Gms.Ads.AdRequest.Builder();
                adMobBanner.LoadAd(requestbuilder.Build());

                SetNativeControl(adMobBanner);
            }
        }
    }
#pragma warning restore 0618 //旧型式です警告回避解除。

}