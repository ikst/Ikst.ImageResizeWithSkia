using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ikst.ImageResizeWithSkia
{
    /// <summary>
    /// 変換モード
    /// </summary>
    public enum ResizeMode
    {
        /// <summary>何もしない。画像の引き伸ばしが発生する。</summary>
        Default = 1,

        /// <summary>縦横比を維持して元画像を伸縮する。元画像と縦横比が異なる場合、余白ができる。</summary>
        Uniform,

        /// <summary>縦横比を維持して元画像を伸縮する。元画像と縦横比が異なる場合であっても余白はできないが元画像の一部が欠落する。</summary>
        UniformToFill

    }
}
