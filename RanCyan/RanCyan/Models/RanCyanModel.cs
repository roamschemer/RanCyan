using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RanCyan.Models {
    /// <summary>乱ちゃんに関するモデル</summary>
    public class RanCyanModel : BindableBase {
        /// <summary>乱ちゃんの表示画像</summary>
        public ImageSource RanCyanImage { get => ranCyanImage; set => SetProperty(ref ranCyanImage, value); }
        private ImageSource ranCyanImage;

        /// <summary>
        /// 乱ちゃん画像の表示アクションフラグ
        /// </summary>
        public bool IsImageActive { get => isImageActive; set => SetProperty(ref isImageActive, value); }
        private bool isImageActive;

        /// <summary>乱ちゃん挙動中</summary>
        private bool isRancyanLottery;

        /// <summary>
        /// 乱ちゃんの画像を抽選画像に差し替えた後、ちょっと待ってから待機画像に差し替える
        /// </summary>
        public async void LotteryRancyanImageAsync() {
            if (isRancyanLottery) return;
            isRancyanLottery = true;
            var assembly = typeof(App).GetTypeInfo().Assembly;
            RanCyanImage = ImageSource.FromResource("RanCyan.Images.3D_Jamp1.gif", assembly);
            await Task.Delay(4000);
            WaitingRancyanImage();
            isRancyanLottery = false;
        }

        /// <summary>
        /// 乱ちゃんの画像を待機画像に差し替える
        /// </summary>
        public void WaitingRancyanImage() {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var images = new[] {
                ImageSource.FromResource("RanCyan.Images.3D_Taiki2.gif", assembly),
                ImageSource.FromResource("RanCyan.Images.3D_Taiki5.gif", assembly),
                ImageSource.FromResource("RanCyan.Images.3D_Taiki6.gif", assembly),
                ImageSource.FromResource("RanCyan.Images.3D_Taiki7.gif", assembly),
                ImageSource.FromResource("RanCyan.Images.3D_Taiki8.gif", assembly),
            };
            var rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            RanCyanImage = images[rnd.Next(0, images.Count())];
            IsImageActive = true;
        }

        /// <summary>
        /// 乱ちゃんの立ち絵を消す(初期状態)
        /// </summary>
        public void StartRancyanImage() {
            RanCyanImage = null;
            IsImageActive = false;
        }

    }
}
