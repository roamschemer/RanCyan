using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace RanCyan.Interfaces
{
    /// <summary>
    /// バナーを操作するインターフェースです。
    /// </summary>
    public interface IBannerCtrl
    {
        /// <summary>
        /// バナーを表示します。
        /// </summary>
        void ShowBanner();

        /// <summary>
        /// バナーをリセットします。
        /// </summary>
        void ResetBanner();
    }
}
