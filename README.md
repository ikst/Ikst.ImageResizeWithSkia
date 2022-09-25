画像の縦横比を崩さずにリサイズ出来るライブラリです。  
画像変換処理には[SkiaSharp](https://github.com/mono/SkiaSharp)を利用します。


# usage
ImageResizeクラスの静的メソッドResizeを呼び出します。

```C#
byte[] bytes = ImageResize.Resize("ImageFilePath", 200, 100, ResizeMode.Default);
byte[] bytes = ImageResize.Resize("ImageFilePath", 200, 100, ResizeMode.Uniform);
byte[] bytes = ImageResize.Resize("ImageFilePath", 200, 100, ResizeMode.UniformToFill);
```
パラメータにリサイズ時の画像伸縮方法をResizeModeで指定します。

![Original](https://raw.githubusercontent.com/ikst/Ikst.ImageResizeWithSkia/master/README/Original.png)

この250(w)x250(h) の画像を 200(w)x100(h) にリサイズした画像をSampleに示します。


|ResizeMode |Description|Sample
|---|---|---
|Default | 何もしません。指定されたサイズ通りに画像の伸縮が発生します。| ![Default](https://raw.githubusercontent.com/ikst/Ikst.ImageResizeWithSkia/master/README/Default.png)
|Uniform | 縦横比を維持したままリサイズします。<br>元画像と縦横比が異なる場合は余白が出来ます。| ![Uniform](https://raw.githubusercontent.com/ikst/Ikst.ImageResizeWithSkia/master/README/Uniform.png)
|UniformToFill | 縦横比を維持したままリサイズします。<br>元画像と縦横比が異なる場合は一部が欠落します。 | ![UniformToFill](https://raw.githubusercontent.com/ikst/Ikst.ImageResizeWithSkia/master/README/UniformToFill.png)


※WPF/XAMLの[Stretch列挙型](https://learn.microsoft.com/dotnet/api/system.windows.media.stretch?view=windowsdesktop-6.0)をイメージするとわかりやすいかも知れません。

その他Resizeメソッドのパラメータは以下のとおりです。

args|type|required|description
---|---|---|---
source | string<br>byte[]<br>stream<br>[SKCodec](https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skcodec) |true| 変換元画像を指定します。
width | int |true|横幅。<br>0を指定した場合、heightを基準に縦横比が変わらないようにします。
height | int |true|縦幅。<br>0を指定した場合、widthを基準に縦横比が変わらないようにします。
mode | ResizeMode |true|リサイズモード。
backColor | [SKColor](https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skcolor) |false|背景色。Uniformで余白ができる場合に適用されます。
imgFmt | [SKEncodedImageFormat](https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skencodedimageformat) |false|出力する画像フォーマット。<br>SkiaSharpの都合上、jpeg, png, webpのみ指定できます。
quality | int |false|画像出力時の品質。0～100で指定します。<br>デフォルトは90です。
fq | [SKFilterQuality](https://learn.microsoft.com/ja-jp/dotnet/api/skiasharp.skfilterquality) |false|SkiaSharpのリサイズメソッドの処理品質。






