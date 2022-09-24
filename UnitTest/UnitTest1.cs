using System.Security.Cryptography.X509Certificates;
using Ikst.ImageResizeWithSkia;
using System.IO;
using SkiaSharp;
using System.Drawing;
using System.Xml.Linq;
using Xunit;
using System.Reflection;
using System.Diagnostics;
using Xunit.Abstractions;

namespace UnitTest
{
    public class UnitTest1
    {

        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
            Debug.WriteLine("-TestInitialize");
        }

        [Fact]
        public void IrregularCheck()
        {
            var testImg = CreateTestImage(500, 500, SKEncodedImageFormat.Jpeg);

            output.WriteLine($"âÊëúèoóÕêÊÅFJPGÅAPNGÅAWEBPà»äOÇÕçÏê¨Ç≈Ç´Ç»Ç¢");
            Assert.Throws<NullReferenceException>(() => {
                ImageResize.Resize(testImg, 100, 100, ResizeMode.Default, SKEncodedImageFormat.Bmp);
            });

            Assert.Throws<ArgumentNullException>(() => {
                ImageResize.Resize(testImg, 0, 0, ResizeMode.Default, SKEncodedImageFormat.Jpeg);
            });

            // 1Ç≈Ç‡ïœä∑Ç≈Ç´ÇÈ
            ImageResize.Resize(testImg, 1, 1, ResizeMode.Default, SKEncodedImageFormat.Jpeg);
            ImageResize.Resize(testImg, 1, 1, ResizeMode.Uniform, SKEncodedImageFormat.Jpeg);
            ImageResize.Resize(testImg, 1, 1, ResizeMode.UniformToFill, SKEncodedImageFormat.Jpeg);
            ImageResize.Resize(testImg, 0, 1, ResizeMode.Default, SKEncodedImageFormat.Jpeg);
            ImageResize.Resize(testImg, 0, 1, ResizeMode.Uniform, SKEncodedImageFormat.Jpeg);
            ImageResize.Resize(testImg, 0, 1, ResizeMode.UniformToFill, SKEncodedImageFormat.Jpeg);
            ImageResize.Resize(testImg, 1, 0, ResizeMode.Default, SKEncodedImageFormat.Jpeg);
            ImageResize.Resize(testImg, 1, 0, ResizeMode.Uniform, SKEncodedImageFormat.Jpeg);
            ImageResize.Resize(testImg, 1, 0, ResizeMode.UniformToFill, SKEncodedImageFormat.Jpeg);

        }

        [Fact]
        public void OverLoadCheck()
        {
            var testImg = CreateTestImage(500, 500, SKEncodedImageFormat.Jpeg);

            ImageResize.Resize(testImg, 111, 222, ResizeMode.Default);
            ImageResize.Resize(testImg, 111, 222, ResizeMode.Default, SKColors.Blue);
            ImageResize.Resize(testImg, 111, 222, ResizeMode.Default, SKEncodedImageFormat.Png);
            ImageResize.Resize(testImg, 111, 222, ResizeMode.Default, SKColors.Blue, SKEncodedImageFormat.Webp);
            ImageResize.Resize(testImg, 111, 222, ResizeMode.Default, SKColors.Blue, SKEncodedImageFormat.Webp, 100);
            ImageResize.Resize(testImg, 111, 222, ResizeMode.Default, SKColors.Blue, SKEncodedImageFormat.Webp, 100, SKFilterQuality.High);

            var tmpFile = Path.GetTempFileName();
            File.WriteAllBytes(tmpFile, testImg);

            ImageResize.Resize(tmpFile, 111, 222, ResizeMode.Default);
            ImageResize.Resize(tmpFile, 111, 222, ResizeMode.Default, SKColors.Blue);
            ImageResize.Resize(tmpFile, 111, 222, ResizeMode.Default, SKEncodedImageFormat.Png);
            ImageResize.Resize(tmpFile, 111, 222, ResizeMode.Default, SKColors.Blue, SKEncodedImageFormat.Webp);
            ImageResize.Resize(tmpFile, 111, 222, ResizeMode.Default, SKColors.Blue, SKEncodedImageFormat.Webp, 100);
            ImageResize.Resize(tmpFile, 111, 222, ResizeMode.Default, SKColors.Blue, SKEncodedImageFormat.Webp, 100, SKFilterQuality.High);

            if (File.Exists(tmpFile)) File.Delete(tmpFile);


        }

        [Fact]
        public void ResizeCheckSmall()
        {
            var square = CreateTestImage(500, 500, SKEncodedImageFormat.Jpeg);
            var portrait = CreateTestImage(500, 750, SKEncodedImageFormat.Jpeg);
            var landscape = CreateTestImage(750, 500, SKEncodedImageFormat.Jpeg);

            // ê≥ï˚å`
            using SKBitmap img1 = SKBitmap.Decode(ImageResize.Resize(square, 111, 222, ResizeMode.Default));
            Assert.Equal(111, img1.Width);
            Assert.Equal(222, img1.Height);
            using SKBitmap img2 = SKBitmap.Decode(ImageResize.Resize(square, 222, 111, ResizeMode.Default));
            Assert.Equal(222, img2.Width);
            Assert.Equal(111, img2.Height);
            using SKBitmap img3 = SKBitmap.Decode(ImageResize.Resize(square, 333, 333, ResizeMode.Default));
            Assert.Equal(333, img3.Width);
            Assert.Equal(333, img3.Height);
            using SKBitmap img4 = SKBitmap.Decode(ImageResize.Resize(square, 200, 0, ResizeMode.Default));
            Assert.Equal(200, img4.Width);
            Assert.Equal(200, img4.Height);
            using SKBitmap img5 = SKBitmap.Decode(ImageResize.Resize(square, 0, 200, ResizeMode.Default));
            Assert.Equal(200, img5.Width);
            Assert.Equal(200, img5.Height);

            // â°í∑
            using SKBitmap img6 = SKBitmap.Decode(ImageResize.Resize(portrait, 111, 222, ResizeMode.Default));
            Assert.Equal(111, img6.Width);
            Assert.Equal(222, img6.Height);
            using SKBitmap img7 = SKBitmap.Decode(ImageResize.Resize(portrait, 222, 111, ResizeMode.Default));
            Assert.Equal(222, img7.Width);
            Assert.Equal(111, img7.Height);
            using SKBitmap img8 = SKBitmap.Decode(ImageResize.Resize(portrait, 333, 333, ResizeMode.Default));
            Assert.Equal(333, img8.Width);
            Assert.Equal(333, img8.Height);
            using SKBitmap img9 = SKBitmap.Decode(ImageResize.Resize(portrait, 200, 0, ResizeMode.Default));
            Assert.Equal(200, img9.Width);
            Assert.Equal( (int)(((float)750/(float)500)*200), img9.Height); // widthÇ©ÇÁheightÇåvéZ
            using SKBitmap img10 = SKBitmap.Decode(ImageResize.Resize(portrait, 0, 200, ResizeMode.Default));
            Assert.Equal( (int)(((float)500/(float)750)*200), img10.Width); // heightÇ©ÇÁwidthÇåvéZ
            Assert.Equal(200, img10.Height);

            // ècí∑
            using SKBitmap img11 = SKBitmap.Decode(ImageResize.Resize(landscape, 111, 222, ResizeMode.Default));
            Assert.Equal(111, img11.Width);
            Assert.Equal(222, img11.Height);
            using SKBitmap img12 = SKBitmap.Decode(ImageResize.Resize(landscape, 222, 111, ResizeMode.Default));
            Assert.Equal(222, img12.Width);
            Assert.Equal(111, img12.Height);
            using SKBitmap img13 = SKBitmap.Decode(ImageResize.Resize(landscape, 333, 333, ResizeMode.Default));
            Assert.Equal(333, img13.Width);
            Assert.Equal(333, img13.Height);
            using SKBitmap img14 = SKBitmap.Decode(ImageResize.Resize(landscape, 200, 0, ResizeMode.Default));
            Assert.Equal(200, img14.Width);
            Assert.Equal((int)(((float)500 / (float)750) * 200), img14.Height); // widthÇ©ÇÁheightÇåvéZ
            using SKBitmap img15 = SKBitmap.Decode(ImageResize.Resize(landscape, 0, 200, ResizeMode.Default));
            Assert.Equal((int)(((float)750 / (float)500) * 200), img15.Width); // heightÇ©ÇÁwidthÇåvéZ
            Assert.Equal(200, img15.Height);

        }

        [Fact]
        public void ResizeCheckLarge()
        {
            var square = CreateTestImage(50, 50, SKEncodedImageFormat.Jpeg);
            var portrait = CreateTestImage(50, 75, SKEncodedImageFormat.Jpeg);
            var landscape = CreateTestImage(75, 50, SKEncodedImageFormat.Jpeg);

            // ê≥ï˚å`
            using SKBitmap img1 = SKBitmap.Decode(ImageResize.Resize(square, 111, 222, ResizeMode.Default));
            Assert.Equal(111, img1.Width);
            Assert.Equal(222, img1.Height);
            using SKBitmap img2 = SKBitmap.Decode(ImageResize.Resize(square, 222, 111, ResizeMode.Default));
            Assert.Equal(222, img2.Width);
            Assert.Equal(111, img2.Height);
            using SKBitmap img3 = SKBitmap.Decode(ImageResize.Resize(square, 333, 333, ResizeMode.Default));
            Assert.Equal(333, img3.Width);
            Assert.Equal(333, img3.Height);
            using SKBitmap img4 = SKBitmap.Decode(ImageResize.Resize(square, 200, 0, ResizeMode.Default));
            Assert.Equal(200, img4.Width);
            Assert.Equal(200, img4.Height);
            using SKBitmap img5 = SKBitmap.Decode(ImageResize.Resize(square, 0, 200, ResizeMode.Default));
            Assert.Equal(200, img5.Width);
            Assert.Equal(200, img5.Height);

            // â°í∑
            using SKBitmap img6 = SKBitmap.Decode(ImageResize.Resize(portrait, 111, 222, ResizeMode.Default));
            Assert.Equal(111, img6.Width);
            Assert.Equal(222, img6.Height);
            using SKBitmap img7 = SKBitmap.Decode(ImageResize.Resize(portrait, 222, 111, ResizeMode.Default));
            Assert.Equal(222, img7.Width);
            Assert.Equal(111, img7.Height);
            using SKBitmap img8 = SKBitmap.Decode(ImageResize.Resize(portrait, 333, 333, ResizeMode.Default));
            Assert.Equal(333, img8.Width);
            Assert.Equal(333, img8.Height);
            using SKBitmap img9 = SKBitmap.Decode(ImageResize.Resize(portrait, 200, 0, ResizeMode.Default));
            Assert.Equal(200, img9.Width);
            Assert.Equal((int)(((float)75 / (float)50) * 200), img9.Height); // widthÇ©ÇÁheightÇåvéZ
            using SKBitmap img10 = SKBitmap.Decode(ImageResize.Resize(portrait, 0, 200, ResizeMode.Default));
            Assert.Equal((int)(((float)50 / (float)75) * 200), img10.Width); // heightÇ©ÇÁwidthÇåvéZ
            Assert.Equal(200, img10.Height);

            // ècí∑
            using SKBitmap img11 = SKBitmap.Decode(ImageResize.Resize(landscape, 111, 222, ResizeMode.Default));
            Assert.Equal(111, img11.Width);
            Assert.Equal(222, img11.Height);
            using SKBitmap img12 = SKBitmap.Decode(ImageResize.Resize(landscape, 222, 111, ResizeMode.Default));
            Assert.Equal(222, img12.Width);
            Assert.Equal(111, img12.Height);
            using SKBitmap img13 = SKBitmap.Decode(ImageResize.Resize(landscape, 333, 333, ResizeMode.Default));
            Assert.Equal(333, img13.Width);
            Assert.Equal(333, img13.Height);
            using SKBitmap img14 = SKBitmap.Decode(ImageResize.Resize(landscape, 200, 0, ResizeMode.Default));
            Assert.Equal(200, img14.Width);
            Assert.Equal((int)(((float)50 / (float)75) * 200), img14.Height); // widthÇ©ÇÁheightÇåvéZ
            using SKBitmap img15 = SKBitmap.Decode(ImageResize.Resize(landscape, 0, 200, ResizeMode.Default));
            Assert.Equal((int)(((float)75 / (float)50) * 200), img15.Width); // heightÇ©ÇÁwidthÇåvéZ
            Assert.Equal(200, img15.Height);

        }


        [Fact]
        public void FormatCheck()
        {

            var jpgImg = CreateTestImage(500, 500, SKEncodedImageFormat.Jpeg);
            var pngImg = CreateTestImage(500, 500, SKEncodedImageFormat.Png);
            var webpImg = CreateTestImage(500, 500, SKEncodedImageFormat.Webp);

            // -------------------------------------------------
            // âÊëúÉtÉHÅ[É}ÉbÉgÇéwíËÇµÇ»Ç¢èÍçáÅAÉ\Å[ÉXâÊëúÇÃÉtÉHÅ[É}ÉbÉgÇ∆ïœÇÌÇÁÇ»Ç¢Ç±Ç∆
            // -------------------------------------------------
            using SKCodec skc1 = SKCodec.Create(new MemoryStream(ImageResize.Resize(jpgImg, 200, 200, ResizeMode.Default)));
            Assert.Equal(SKEncodedImageFormat.Jpeg, skc1.EncodedFormat);


            using SKCodec skc2 = SKCodec.Create(new MemoryStream(ImageResize.Resize(pngImg, 200, 200, ResizeMode.Default)));
            Assert.Equal(SKEncodedImageFormat.Png, skc2.EncodedFormat);

            using SKCodec skc3 = SKCodec.Create(new MemoryStream(ImageResize.Resize(webpImg, 200, 200, ResizeMode.Default)));
            Assert.Equal(SKEncodedImageFormat.Webp, skc3.EncodedFormat);

            // -------------------------------------------------
            // éwíËÇµÇΩâÊëúÉtÉHÅ[É}ÉbÉgÇ…Ç»ÇÈÇ±Ç∆
            // -------------------------------------------------
            using SKCodec skc4 = SKCodec.Create(new MemoryStream(ImageResize.Resize(jpgImg, 200, 200, ResizeMode.Default, SKEncodedImageFormat.Png)));
            Assert.Equal(SKEncodedImageFormat.Png, skc4.EncodedFormat);
            using SKCodec skc5 = SKCodec.Create(new MemoryStream(ImageResize.Resize(jpgImg, 200, 200, ResizeMode.Default, SKEncodedImageFormat.Webp)));
            Assert.Equal(SKEncodedImageFormat.Webp, skc5.EncodedFormat);

            using SKCodec skc6 = SKCodec.Create(new MemoryStream(ImageResize.Resize(pngImg, 200, 200, ResizeMode.Default, SKEncodedImageFormat.Jpeg)));
            Assert.Equal(SKEncodedImageFormat.Jpeg, skc6.EncodedFormat);
            using SKCodec skc7 = SKCodec.Create(new MemoryStream(ImageResize.Resize(pngImg, 200, 200, ResizeMode.Default, SKEncodedImageFormat.Webp)));
            Assert.Equal(SKEncodedImageFormat.Webp, skc7.EncodedFormat);

            using SKCodec skc8 = SKCodec.Create(new MemoryStream(ImageResize.Resize(webpImg, 200, 200, ResizeMode.Default, SKEncodedImageFormat.Jpeg)));
            Assert.Equal(SKEncodedImageFormat.Jpeg, skc8.EncodedFormat);
            using SKCodec skc9 = SKCodec.Create(new MemoryStream(ImageResize.Resize(webpImg, 200, 200, ResizeMode.Default, SKEncodedImageFormat.Png)));
            Assert.Equal(SKEncodedImageFormat.Png, skc9.EncodedFormat);

        }




        [Fact]
        public void VisuallyCheckTheGeneratedImage()
        {

            // èoóÕêÊÇÃÉfÉBÉåÉNÉgÉäÇçÏê¨
            var execDir = Directory.GetParent(Assembly.GetExecutingAssembly().Location);
            string outputDir = string.Empty;
            if (execDir is not null)
            {
                outputDir = Path.Combine(execDir.FullName, "img");
            }
            else
            {
                throw new Exception("é¿çsÉtÉHÉãÉ_ÇÃéÊìæÇ…é∏îs");
            }
            

            //var outputDir = Path.Combine(execDir.FullName, "img");
            if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
            output.WriteLine($"âÊëúèoóÕêÊÅF{outputDir}");

            // ÉeÉXÉgâÊëú
            var square = CreateTestImage(500, 500, SKEncodedImageFormat.Jpeg);
            var portrait = CreateTestImage(500, 750, SKEncodedImageFormat.Jpeg);
            var landscape = CreateTestImage(750, 500, SKEncodedImageFormat.Jpeg);

            // ÉeÉXÉgê›íË
            SKColor color = SKColor.Parse("80FF0000"); //SKColors.Blue;
            SKEncodedImageFormat fmt = SKEncodedImageFormat.Png;
            int quality = 50;
            SKFilterQuality fq = SKFilterQuality.High;

            // ----------------ê≥ï˚å`----------------
            File.WriteAllBytes(Path.Combine(outputDir, "ê≥ï˚Source.png"), square);
            File.WriteAllBytes(Path.Combine(outputDir, "ê≥ï˚Å®ê≥ï˚_Default.png"), ImageResize.Resize(square, 200, 200, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ê≥ï˚Å®â°í∑_Default.png"), ImageResize.Resize(square, 200, 100, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ê≥ï˚Å®ècí∑_Default.png"), ImageResize.Resize(square, 100, 200, ResizeMode.Default, color, fmt, quality, fq));
                                                             
            File.WriteAllBytes(Path.Combine(outputDir, "ê≥ï˚Å®ê≥ï˚_Uniform.png"), ImageResize.Resize(square, 200, 200, ResizeMode.Uniform, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ê≥ï˚Å®â°í∑_Uniform.png"), ImageResize.Resize(square, 200, 100, ResizeMode.Uniform, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ê≥ï˚Å®ècí∑_Uniform.png"), ImageResize.Resize(square, 100, 200, ResizeMode.Uniform, color, fmt, quality, fq));

            File.WriteAllBytes(Path.Combine(outputDir, "ê≥ï˚Å®ê≥ï˚_UniformToFill.png"), ImageResize.Resize(square, 200, 200, ResizeMode.UniformToFill, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ê≥ï˚Å®â°í∑_UniformToFill.png"), ImageResize.Resize(square, 200, 100, ResizeMode.UniformToFill, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ê≥ï˚Å®ècí∑_UniformToFill.png"), ImageResize.Resize(square, 100, 200, ResizeMode.UniformToFill, color, fmt, quality, fq));

            File.WriteAllBytes(Path.Combine(outputDir, "ê≥ï˚Å®â°éwíËñ≥Çµ.png"), ImageResize.Resize(square, 0, 100, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ê≥ï˚Å®ècéwíËñ≥Çµ.png"), ImageResize.Resize(square, 100, 0, ResizeMode.Default, color, fmt, quality, fq));

            // ----------------ècí∑----------------
            File.WriteAllBytes(Path.Combine(outputDir, "ècí∑Source.png"), portrait);
            File.WriteAllBytes(Path.Combine(outputDir, "ècí∑Å®ê≥ï˚_Default.png"), ImageResize.Resize(portrait, 200, 200, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ècí∑Å®â°í∑_Default.png"), ImageResize.Resize(portrait, 200, 100, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ècí∑Å®ècí∑_Default.png"), ImageResize.Resize(portrait, 100, 200, ResizeMode.Default, color, fmt, quality, fq));
                                                             
            File.WriteAllBytes(Path.Combine(outputDir, "ècí∑Å®ê≥ï˚_Uniform.png"), ImageResize.Resize(portrait, 200, 200, ResizeMode.Uniform, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ècí∑Å®â°í∑_Uniform.png"), ImageResize.Resize(portrait, 200, 100, ResizeMode.Uniform, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ècí∑Å®ècí∑_Uniform.png"), ImageResize.Resize(portrait, 100, 200, ResizeMode.Uniform, color, fmt, quality, fq));
                                                           
            File.WriteAllBytes(Path.Combine(outputDir, "ècí∑Å®ê≥ï˚_UniformToFill.png"), ImageResize.Resize(portrait, 200, 200, ResizeMode.UniformToFill, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ècí∑Å®â°í∑_UniformToFill.png"), ImageResize.Resize(portrait, 200, 100, ResizeMode.UniformToFill, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ècí∑Å®ècí∑_UniformToFill.png"), ImageResize.Resize(portrait, 100, 200, ResizeMode.UniformToFill, color, fmt, quality, fq));

            File.WriteAllBytes(Path.Combine(outputDir, "ècí∑Å®â°éwíËñ≥Çµ.png"), ImageResize.Resize(portrait, 0, 100, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "ècí∑Å®ècéwíËñ≥Çµ.png"), ImageResize.Resize(portrait, 100, 0, ResizeMode.Default, color, fmt, quality, fq));

            // ----------------â°í∑----------------
            File.WriteAllBytes(Path.Combine(outputDir, "â°í∑Source.png"), landscape);
            File.WriteAllBytes(Path.Combine(outputDir, "â°í∑Å®ê≥ï˚_Default.png"), ImageResize.Resize(landscape, 200, 200, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "â°í∑Å®â°í∑_Default.png"), ImageResize.Resize(landscape, 200, 100, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "â°í∑Å®ècí∑_Default.png"), ImageResize.Resize(landscape, 100, 200, ResizeMode.Default, color, fmt, quality, fq));
                                                         
            File.WriteAllBytes(Path.Combine(outputDir, "â°í∑Å®ê≥ï˚_Uniform.png"), ImageResize.Resize(landscape, 200, 200, ResizeMode.Uniform, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "â°í∑Å®â°í∑_Uniform.png"), ImageResize.Resize(landscape, 200, 100, ResizeMode.Uniform, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "â°í∑Å®â°í∑_Uniform.png"), ImageResize.Resize(landscape, 100, 200, ResizeMode.Uniform, color, fmt, quality, fq));
                                                       
            File.WriteAllBytes(Path.Combine(outputDir, "â°í∑Å®ê≥ï˚_UniformToFill.png"), ImageResize.Resize(landscape, 200, 200, ResizeMode.UniformToFill, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "â°í∑Å®â°í∑_UniformToFill.png"), ImageResize.Resize(landscape, 200, 100, ResizeMode.UniformToFill, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "â°í∑Å®ècí∑_UniformToFill.png"), ImageResize.Resize(landscape, 100, 200, ResizeMode.UniformToFill, color, fmt, quality, fq));

            File.WriteAllBytes(Path.Combine(outputDir, "â°í∑Å®â°éwíËñ≥Çµ.png"), ImageResize.Resize(landscape, 0, 100, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "â°í∑Å®ècéwíËñ≥Çµ.png"), ImageResize.Resize(landscape, 100, 0, ResizeMode.Default, color, fmt, quality, fq));
        }


        private byte[] CreateTestImage(int width, int height, SKEncodedImageFormat fmt)
        {
            var info = new SKImageInfo(width, height);
            using SKSurface surface = SKSurface.Create(info);
            using SKCanvas canvas = surface.Canvas;
            canvas.Clear(SKColors.Green);

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 50,
                Color = SKColors.Red
            };

            canvas.DrawCircle(width / 2, height / 2, width / 4, paint);
            canvas.DrawRect(0, 0, width, height, paint);

            using SKImage skImg = surface.Snapshot();
            using SKData skData = skImg.Encode(fmt, 90);

            return skData.ToArray();
        }
    }
}