using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ikst.ImageResizeWithSkia
{

    /// <summary>
    /// 画像変換
    /// </summary>
    public class ImageResize
    {

        /// <see cref="Resize(string, int, int, ResizeMode, SKColor, SKEncodedImageFormat, int, SKFilterQuality)"/>
        public static byte[] Resize(string filePath, int width, int height, ResizeMode mode)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return Resize(fs, width, height, mode);
            }
        }
        public static byte[] Resize(byte[] bytes, int width, int height, ResizeMode mode)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Resize(ms, width, height, mode);
            }
        }
        public static byte[] Resize(Stream st, int width, int height, ResizeMode mode)
        {
            using (SKCodec sKCodec = SKCodec.Create(st))
            {
                return Resize(sKCodec, width, height, mode);
            }
        }
        public static byte[] Resize(SKCodec sKCodec, int width, int height, ResizeMode mode)
        {
            var backColor = SKColors.Transparent;
            var fmt = sKCodec.EncodedFormat;
            return Resize(sKCodec, width, height, mode, backColor, fmt);
        }



        public static byte[] Resize(string filePath, int width, int height, ResizeMode mode, SKColor backColor)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return Resize(fs, width, height, mode, backColor);
            }
        }
        public static byte[] Resize(byte[] bytes, int width, int height, ResizeMode mode, SKColor backColor)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Resize(ms, width, height, mode, backColor);
            }
        }
        public static byte[] Resize(Stream st, int width, int height, ResizeMode mode, SKColor backColor)
        {
            using (SKCodec sKCodec = SKCodec.Create(st))
            {
                return Resize(sKCodec, width, height, mode, backColor);
            }
        }
        public static byte[] Resize(SKCodec sKCodec, int width, int height, ResizeMode mode, SKColor backColor)
        {
            var fmt = sKCodec.EncodedFormat;
            return Resize(sKCodec, width, height, mode, backColor, fmt);
        }


        public static byte[] Resize(string filePath, int width, int height, ResizeMode mode, SKEncodedImageFormat fmt)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return Resize(fs, width, height, mode, fmt);
            }
        }
        public static byte[] Resize(byte[] bytes, int width, int height, ResizeMode mode, SKEncodedImageFormat fmt)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Resize(ms, width, height, mode, fmt);
            }
        }
        public static byte[] Resize(Stream st, int width, int height, ResizeMode mode, SKEncodedImageFormat fmt)
        {
            using (SKCodec sKCodec = SKCodec.Create(st))
            {
                return Resize(sKCodec, width, height, mode, fmt);
            }
        }
        public static byte[] Resize(SKCodec sKCodec, int width, int height, ResizeMode mode, SKEncodedImageFormat fmt)
        {
            var backColor = SKColors.Transparent;
            return Resize(sKCodec, width, height, mode, backColor, fmt);
        }


        public static byte[] Resize(string filePath, int width, int height, ResizeMode mode, SKColor backColor, SKEncodedImageFormat fmt, int quality = 90, SKFilterQuality fq = SKFilterQuality.Medium)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return Resize(fs, width, height, mode, backColor, fmt, quality, fq);
            }
        }
        public static byte[] Resize(byte[] bytes, int width, int height, ResizeMode mode, SKColor backColor, SKEncodedImageFormat fmt, int quality = 90, SKFilterQuality fq = SKFilterQuality.Medium)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Resize(ms, width, height, mode, backColor, fmt, quality, fq);
            }
        }
        public static byte[] Resize(Stream st, int width, int height, ResizeMode mode, SKColor backColor, SKEncodedImageFormat fmt, int quality = 90, SKFilterQuality fq = SKFilterQuality.Medium)
        {
            using (SKCodec sKCodec = SKCodec.Create(st))
            {
                return Resize(sKCodec, width, height, mode, backColor, fmt, quality, fq);
            }
        }


        /// <summary>
        /// 画像をリサイズします
        /// </summary>
        /// <param name="sKCodec"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        /// <param name="backColor"></param>
        /// <param name="imgFmt"></param>
        /// <param name="quality"></param>
        /// <param name="fq"></param>
        /// <returns></returns>
        public static byte[] Resize(SKCodec sKCodec, int width, int height, ResizeMode mode, SKColor backColor, SKEncodedImageFormat imgFmt, int quality = 90, SKFilterQuality fq = SKFilterQuality.Medium)
        {

            using (SKBitmap srcBitmap = SKBitmap.Decode(sKCodec))
            {

                // width,heightのどちらかで0を指定された場合は、もう片方を基準に縦横比を維持したサイズとみなす
                if (width == 0 || height == 0)
                {
                    if (width == 0) width = (int)(height * ((float)srcBitmap.Width / (float)srcBitmap.Height));
                    if (height == 0) height = (int)(width * ((float)srcBitmap.Height / (float)srcBitmap.Width));
                    mode = ResizeMode.Default;
                }

                SKImage ski;
                if (mode == ResizeMode.Default)
                {
                    using (SKBitmap scaledBitmap = srcBitmap.Resize(new SKImageInfo(width, height), fq))
                    {
                        ski = SKImage.FromBitmap(scaledBitmap);
                    }
                }
                else
                {
                    bool toFill = false;
                    if (mode == ResizeMode.UniformToFill) toFill = true;

                    var info = new SKImageInfo(width, height);
                    using (SKSurface surface = SKSurface.Create(info))
                    using (SKCanvas canvas = surface.Canvas)
                    {

                        // Transparentでない場合のみ適用する
                        if (backColor != SKColors.Transparent)  canvas.Clear(backColor);

                        var s = ConvertUniformSize(srcBitmap.Width, srcBitmap.Height, width, height, toFill);
                        using (SKBitmap scaledBitmap = srcBitmap.Resize(new SKImageInfo((int)s.Width, (int)s.Height), fq))
                        {

                            float x = (width - s.Width) / 2;
                            float y = (height - s.Height) / 2;
                            canvas.DrawBitmap(scaledBitmap, x, y);

                            ski = surface.Snapshot();
                        }
                    }
                }

                using (SKData data = ski.Encode(imgFmt, quality))
                {
                    ski.Dispose();
                    return data.ToArray();
                }
            }

            // SKImage.Encodeの画像フォーマットのデフォルトはpng
            // https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skimage.encode?view=skiasharp-2.88
        }


        private static SKSize ConvertUniformSize(float sourceW, float sourceH, float convW, float convH, bool toFill)
        {
            float rate;

            if ((convH / convW) <= (sourceH / sourceW))
            {
                if (toFill)
                {
                    rate = convW / sourceW;
                }
                else
                {
                    rate = convH / sourceH;
                }

            }
            else
            {
                if (toFill)
                {
                    rate = convH / sourceH;
                }
                else
                {
                    rate = convW / sourceW;
                }
            }

            return new SKSize((sourceW * rate), (sourceH * rate));
        }

    }
}
