using RanCyan.Interfaces;
using RanCyan.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(CBannerCtrl_UWP))]
namespace RanCyan.UWP
{
    class CBannerCtrl_UWP : IBannerCtrl
    {
        /// <summary>
        /// バナー広告を表示します。
        /// </summary>
        public void ShowBanner()
        {
        }

        /// <summary>
        /// バナー広告をリセットします。
        /// </summary>
        public void ResetBanner()
        {
        }
    }
}
