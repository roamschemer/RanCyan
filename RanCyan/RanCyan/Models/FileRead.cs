using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using Prism.Mvvm;

namespace RanCyan.Models {
    public class FileRead : BindableBase {
        /// <summary>
        /// 読み込んだText
        /// </summary>
        public string ReadText {
            get => readText;
            set => SetProperty(ref readText, value);
        }
        private string readText;

        /// <summary>
        /// 埋め込みリソースからのテキスト読み込み
        /// </summary>
        /// <param name="path">読み込み先パス</param>
        /// <returns></returns>
        public void GetResourceText(string path) {
            var assembly = typeof(FileRead).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream(path);
            using (var reader = new StreamReader(stream)) {
                ReadText = reader.ReadToEnd();
            }
        }
    }
}
