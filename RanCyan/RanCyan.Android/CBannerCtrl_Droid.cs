using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RanCyan.Droid;
using RanCyan.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(CBannerCtrl_Droid))]
namespace RanCyan.Droid
{
    /// <summary>
    /// 広告バナーを表示するクラスです。
    /// </summary>
    public class CBannerCtrl_Droid : IBannerCtrl
    {
        #region メソッド

        /// <summary>
        /// バナー広告を表示します。
        /// </summary>
        public void ShowBanner()
        {
            Android.Gms.Ads.InterstitialAd interstitial = CCommonModel_Droid.GetInstance().Interstitial;
            if (interstitial.IsLoaded)
            {
                interstitial.Show();
            }
        }

        /// <summary>
        /// バナー広告をリセットします。
        /// </summary>
        public void ResetBanner()
        {
            CCommonModel_Droid.GetInstance().RequestNewInterstitial();
        }
        #endregion
    }
}