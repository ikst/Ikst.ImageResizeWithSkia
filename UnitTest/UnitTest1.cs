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

            output.WriteLine($"�摜�o�͐�FJPG�APNG�AWEBP�ȊO�͍쐬�ł��Ȃ��B");
            // https://github.com/mono/SkiaSharp/issues/1130#issuecomment-583161267
            Assert.Throws<NullReferenceException>(() => {
                ImageResize.Resize(testImg, 100, 100, ResizeMode.Default, SKEncodedImageFormat.Avif);
            });

            Assert.Throws<ArgumentNullException>(() => {
                ImageResize.Resize(testImg, 0, 0, ResizeMode.Default, SKEncodedImageFormat.Jpeg);
            });

            // 1�ł��ϊ��ł���
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

            // �����`
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

            // ����
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
            Assert.Equal( (int)(((float)750/(float)500)*200), img9.Height); // width����height���v�Z
            using SKBitmap img10 = SKBitmap.Decode(ImageResize.Resize(portrait, 0, 200, ResizeMode.Default));
            Assert.Equal( (int)(((float)500/(float)750)*200), img10.Width); // height����width���v�Z
            Assert.Equal(200, img10.Height);

            // �c��
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
            Assert.Equal((int)(((float)500 / (float)750) * 200), img14.Height); // width����height���v�Z
            using SKBitmap img15 = SKBitmap.Decode(ImageResize.Resize(landscape, 0, 200, ResizeMode.Default));
            Assert.Equal((int)(((float)750 / (float)500) * 200), img15.Width); // height����width���v�Z
            Assert.Equal(200, img15.Height);

        }

        [Fact]
        public void ResizeCheckLarge()
        {
            var square = CreateTestImage(50, 50, SKEncodedImageFormat.Jpeg);
            var portrait = CreateTestImage(50, 75, SKEncodedImageFormat.Jpeg);
            var landscape = CreateTestImage(75, 50, SKEncodedImageFormat.Jpeg);

            // �����`
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

            // ����
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
            Assert.Equal((int)(((float)75 / (float)50) * 200), img9.Height); // width����height���v�Z
            using SKBitmap img10 = SKBitmap.Decode(ImageResize.Resize(portrait, 0, 200, ResizeMode.Default));
            Assert.Equal((int)(((float)50 / (float)75) * 200), img10.Width); // height����width���v�Z
            Assert.Equal(200, img10.Height);

            // �c��
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
            Assert.Equal((int)(((float)50 / (float)75) * 200), img14.Height); // width����height���v�Z
            using SKBitmap img15 = SKBitmap.Decode(ImageResize.Resize(landscape, 0, 200, ResizeMode.Default));
            Assert.Equal((int)(((float)75 / (float)50) * 200), img15.Width); // height����width���v�Z
            Assert.Equal(200, img15.Height);

        }


        [Fact]
        public void FormatCheck()
        {

            var jpgImg = CreateTestImage(500, 500, SKEncodedImageFormat.Jpeg);
            var pngImg = CreateTestImage(500, 500, SKEncodedImageFormat.Png);
            var webpImg = CreateTestImage(500, 500, SKEncodedImageFormat.Webp);

            // -------------------------------------------------
            // �摜�t�H�[�}�b�g���w�肵�Ȃ��ꍇ�A�\�[�X�摜�̃t�H�[�}�b�g�ƕς��Ȃ�����
            // -------------------------------------------------
            using SKCodec skc1 = SKCodec.Create(new MemoryStream(ImageResize.Resize(jpgImg, 200, 200, ResizeMode.Default)));
            Assert.Equal(SKEncodedImageFormat.Jpeg, skc1.EncodedFormat);


            using SKCodec skc2 = SKCodec.Create(new MemoryStream(ImageResize.Resize(pngImg, 200, 200, ResizeMode.Default)));
            Assert.Equal(SKEncodedImageFormat.Png, skc2.EncodedFormat);

            using SKCodec skc3 = SKCodec.Create(new MemoryStream(ImageResize.Resize(webpImg, 200, 200, ResizeMode.Default)));
            Assert.Equal(SKEncodedImageFormat.Webp, skc3.EncodedFormat);

            // -------------------------------------------------
            // �w�肵���摜�t�H�[�}�b�g�ɂȂ邱��
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

            // �o�͐�̃f�B���N�g�����쐬
            var execDir = Directory.GetParent(Assembly.GetExecutingAssembly().Location);
            string outputDir = string.Empty;
            if (execDir is not null)
            {
                outputDir = Path.Combine(execDir.FullName, "img");
            }
            else
            {
                throw new Exception("���s�t�H���_�̎擾�Ɏ��s");
            }
            

            //var outputDir = Path.Combine(execDir.FullName, "img");
            if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
            output.WriteLine($"�摜�o�͐�F{outputDir}");

            // �e�X�g�摜
            var square = CreateTestImage(250, 250, SKEncodedImageFormat.Jpeg);
            var portrait = CreateTestImage(250, 300, SKEncodedImageFormat.Jpeg);
            var landscape = CreateTestImage(300, 250, SKEncodedImageFormat.Jpeg);

            // �e�X�g�ݒ�
            SKColor color = SKColor.Parse("330000ff"); //SKColors.Blue;
            SKEncodedImageFormat fmt = SKEncodedImageFormat.Png;
            int quality = 50;
            SKFilterQuality fq = SKFilterQuality.High;

            // ----------------�����`----------------
            File.WriteAllBytes(Path.Combine(outputDir, "����Source.png"), square);
            File.WriteAllBytes(Path.Combine(outputDir, "����������_Default.png"), ImageResize.Resize(square, 200, 200, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "����������_Default.png"), ImageResize.Resize(square, 200, 100, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�������c��_Default.png"), ImageResize.Resize(square, 100, 200, ResizeMode.Default, color, fmt, quality, fq));
                                                             
            File.WriteAllBytes(Path.Combine(outputDir, "����������_Uniform.png"), ImageResize.Resize(square, 200, 200, ResizeMode.Uniform, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "����������_Uniform.png"), ImageResize.Resize(square, 200, 100, ResizeMode.Uniform, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�������c��_Uniform.png"), ImageResize.Resize(square, 100, 200, ResizeMode.Uniform, color, fmt, quality, fq));

            File.WriteAllBytes(Path.Combine(outputDir, "����������_UniformToFill.png"), ImageResize.Resize(square, 200, 200, ResizeMode.UniformToFill, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "����������_UniformToFill.png"), ImageResize.Resize(square, 200, 100, ResizeMode.UniformToFill, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�������c��_UniformToFill.png"), ImageResize.Resize(square, 100, 200, ResizeMode.UniformToFill, color, fmt, quality, fq));

            File.WriteAllBytes(Path.Combine(outputDir, "���������w�薳��.png"), ImageResize.Resize(square, 0, 100, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�������c�w�薳��.png"), ImageResize.Resize(square, 100, 0, ResizeMode.Default, color, fmt, quality, fq));

            // ----------------�c��----------------
            File.WriteAllBytes(Path.Combine(outputDir, "�c��Source.png"), portrait);
            File.WriteAllBytes(Path.Combine(outputDir, "�c��������_Default.png"), ImageResize.Resize(portrait, 200, 200, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�c��������_Default.png"), ImageResize.Resize(portrait, 200, 100, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�c�����c��_Default.png"), ImageResize.Resize(portrait, 100, 200, ResizeMode.Default, color, fmt, quality, fq));
                                                             
            File.WriteAllBytes(Path.Combine(outputDir, "�c��������_Uniform.png"), ImageResize.Resize(portrait, 200, 200, ResizeMode.Uniform, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�c��������_Uniform.png"), ImageResize.Resize(portrait, 200, 100, ResizeMode.Uniform, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�c�����c��_Uniform.png"), ImageResize.Resize(portrait, 100, 200, ResizeMode.Uniform, color, fmt, quality, fq));
                                                           
            File.WriteAllBytes(Path.Combine(outputDir, "�c��������_UniformToFill.png"), ImageResize.Resize(portrait, 200, 200, ResizeMode.UniformToFill, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�c��������_UniformToFill.png"), ImageResize.Resize(portrait, 200, 100, ResizeMode.UniformToFill, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�c�����c��_UniformToFill.png"), ImageResize.Resize(portrait, 100, 200, ResizeMode.UniformToFill, color, fmt, quality, fq));

            File.WriteAllBytes(Path.Combine(outputDir, "�c�������w�薳��.png"), ImageResize.Resize(portrait, 0, 100, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�c�����c�w�薳��.png"), ImageResize.Resize(portrait, 100, 0, ResizeMode.Default, color, fmt, quality, fq));

            // ----------------����----------------
            File.WriteAllBytes(Path.Combine(outputDir, "����Source.png"), landscape);
            File.WriteAllBytes(Path.Combine(outputDir, "����������_Default.png"), ImageResize.Resize(landscape, 200, 200, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "����������_Default.png"), ImageResize.Resize(landscape, 200, 100, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�������c��_Default.png"), ImageResize.Resize(landscape, 100, 200, ResizeMode.Default, color, fmt, quality, fq));
                                                         
            File.WriteAllBytes(Path.Combine(outputDir, "����������_Uniform.png"), ImageResize.Resize(landscape, 200, 200, ResizeMode.Uniform, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "����������_Uniform.png"), ImageResize.Resize(landscape, 200, 100, ResizeMode.Uniform, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "����������_Uniform.png"), ImageResize.Resize(landscape, 100, 200, ResizeMode.Uniform, color, fmt, quality, fq));
                                                       
            File.WriteAllBytes(Path.Combine(outputDir, "����������_UniformToFill.png"), ImageResize.Resize(landscape, 200, 200, ResizeMode.UniformToFill, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "����������_UniformToFill.png"), ImageResize.Resize(landscape, 200, 100, ResizeMode.UniformToFill, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�������c��_UniformToFill.png"), ImageResize.Resize(landscape, 100, 200, ResizeMode.UniformToFill, color, fmt, quality, fq));

            File.WriteAllBytes(Path.Combine(outputDir, "���������w�薳��.png"), ImageResize.Resize(landscape, 0, 100, ResizeMode.Default, color, fmt, quality, fq));
            File.WriteAllBytes(Path.Combine(outputDir, "�������c�w�薳��.png"), ImageResize.Resize(landscape, 100, 0, ResizeMode.Default, color, fmt, quality, fq));
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