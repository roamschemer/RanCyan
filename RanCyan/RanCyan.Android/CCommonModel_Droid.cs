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

namespace RanCyan.Droid
{
    /// <summary>
    /// Androidの共通情報を管理するクラスです。
    /// </summary>
    public class CCommonModel_Droid
    {
        #region プロパティ

        /// <summary>
        /// 共通情報
        /// </summary>
        public static CCommonModel_Droid Instance;

        /// <summary>
        /// 広告バナーのIDを設定します。
        /// </summary>
        public string BannerID { set; get; }

        /// <summary>
        /// 広告バナークラス
        /// </summary>
        public Android.Gms.Ads.InterstitialAd Interstitial { set; get; }

        /// <summary>
        /// 広告バナーのIDを設定します。
        /// </summary>
        public string InterstitialID { set; get; }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CCommonModel_Droid()
        {
        }

        #endregion

        #region メソッド

        /// <summary>
        /// 初期処理をします。
        /// </summary>
        public static void Init()
        {
            Instance = new CCommonModel_Droid();
        }

        /// <summary>
        /// 共通情報のインスタンスを取得します。
        /// </summary>
        /// <returns>共通情報</returns>
        public static CCommonModel_Droid GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// 初期データを設定します。
        /// </summary>
        /// <param name="act">アクティビティ</param>
        public void SetData(Activity act, string sInterstitialID)
        {
            // 広告情報を初期化します。
            Interstitial = new Android.Gms.Ads.InterstitialAd(act);
            Interstitial.AdUnitId = sInterstitialID;
            Android.Gms.Ads.AdListener lisner = new AdListener(Instance);
        }

        /// <summary>
        /// 新しい広告を生成します。
        /// </summary>
        public void RequestNewInterstitial()
        {
            if (!Interstitial.IsLoaded)
            {
                var adRequest = new Android.Gms.Ads.AdRequest.Builder().Build();
                Interstitial.LoadAd(adRequest);
            }
        }

        #endregion

        #region プライベートクラス

        class AdListener : Android.Gms.Ads.AdListener
        {
            CCommonModel_Droid act;

            public AdListener(CCommonModel_Droid t)
            {
                act = t;
            }

            public override void OnAdClosed()
            {
                act.RequestNewInterstitial();
            }
        }

        #endregion
    }
}