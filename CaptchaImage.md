http://sites.google.com/site/sixpacklibrary/_/rsrc/1237036619924/Home/sixpack-icon.gif?height=48&width=48

This is an example of using CaptchaImage. Note that this uses the new extensions of CaptchaImage in 1.1-beta2.

Typically, you would generate a random sequence of characters, store it in session, then use this class to serve the image from a handler.

On submit, you would then compare the value input by the user with the value in session.

Stating the obvious: do NOT use a parameter to pass the correct CAPTCHA value to the handler... that would make the CAPTCHA useless!

A not-so-obvious note: mono does not support the WarpPath method in System.Drawing that we use to scramble the text. The class works around this by distorting the image in other ways under Unix environments. This is not as effective as the Win32 method, unfortunately.

In future versions of the library we might create another CaptchaImage class based on ImageMagick, so we can provide a consistent user experience on both platforms.

```
using System;
using SixPack.Drawing;
using System.Drawing;
using System.Drawing.Imaging;

namespace sixpackexamples
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			CaptchaImage captcha = new CaptchaImage("foo-bar", 300, 100, "Times New Roman");
			captcha.BackgroundDark = TangoPalette.SkyBlue1;
			captcha.BackgroundLight = TangoPalette.SkyBlue3;
			captcha.ForegroundDark = TangoPalette.SkyBlue2;
			captcha.ForegroundLight = TangoPalette.SkyBlue1;
			captcha.FontStyle = FontStyle.Italic;
			Image image = captcha.Generate();
			Bitmap bitmap = new Bitmap(image);
			bitmap.Save("foobar.png", ImageFormat.Png);			
		}
	}
}
```

This is a typical result under mono:
<p align='center'><img src='http://sixpack-library.googlecode.com/svn/wiki/foobar.png' /></p>