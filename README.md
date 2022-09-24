画像の縦横比を崩さずにリサイズ出来るライブラリです。  
画像変換処理には[SkiaSharp](https://github.com/mono/SkiaSharp)を利用します。


# usage
ImageResizeクラスの静的メソッドResizeを呼び出します。
```C#
byte[] bytes = ImageResize.Resize("ImageFilePath", 640, 480, ResizeMode.Default);
```
他にもいくつかのオーバーロードがあります。
```C#
byte[] bytes1 = ImageResize.Resize(stream, 640, 480, ResizeMode.Uniform, SKColors.Blue);
byte[] bytes2 = ImageResize.Resize(byteArray, 640, 480, ResizeMode.UniformToFill, SKEncodedImageFormat.Png);
byte[] bytes3 = ImageResize.Resize("ImageFilePath", 640, 480, ResizeMode.Default, SKColors.Blue, SKEncodedImageFormat.Jpeg);
byte[] bytes4 = ImageResize.Resize(stream, 640, 480, ResizeMode.Uniform, SKColors.Blue, SKEncodedImageFormat.Webp, 100);
byte[] bytes5 = ImageResize.Resize(byteArray, 640, 480, ResizeMode.UniformToFill, SKColors.Blue, SKEncodedImageFormat.Webp, 100, SKFilterQuality.High);
```
Resizeメソッドのパラメータは以下のとおりです。

args|type|description
---|---|---
source | string<br>byte[]<br>stream<br>[SKCodec](https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skcodec) | 変換元画像を指定します。
width | int |横幅。<br>0を指定した場合、heightを基準に縦横比が変わらないようにします。
height | int |縦幅。<br>0を指定した場合、widthを基準に縦横比が変わらないようにします。
mode | ResizeMode |リサイズモード。**詳細は以下。**
backColor | [SKColor](https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skcolor) |背景色。Uniformで余白ができる場合に適用されます。
imgFmt | [SKEncodedImageFormat](https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skencodedimageformat) |出力する画像フォーマット。<br>SkiaSharpの都合上、jpeg, png, webpのみ指定できます。
quality | int |画像出力時の品質。0～100で指定します。<br>デフォルトは90です。
fq | [SKFilterQuality](https://learn.microsoft.com/ja-jp/dotnet/api/skiasharp.skfilterquality) |SkiaSharpのリサイズメソッドの処理品質。



# ResizeMode


|ResizeMode |description
|---|---
|Default | 何もしません。指定されたサイズ通りに画像の伸縮が発生します。
|Uniform | 縦横比を維持したままリサイズします。<br>元画像と縦横比が異なる場合は余白が出来ます。
|UniformToFill | 縦横比を維持したままリサイズします。<br>元画像と縦横比が異なる場合は一部が欠落します。 

<br>


※WPF/XAMLの[Stretch列挙型](https://learn.microsoft.com/dotnet/api/system.windows.media.stretch?view=windowsdesktop-6.0)をイメージするとわかりやすいかも知れません。


