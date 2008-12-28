// ImageSizing.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Author: Marco Cecconi
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;

namespace SixPack.Drawing
{
	/// <summary>
	/// This class provides methods for image sizing
	/// </summary>
	public static class ImageSizing
	{
		/// <summary>
		/// Resizes the specified image.
		/// </summary>
		/// <param name="image">The image.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="background">The background color.</param>
		/// <param name="imageResizeMethod">The image resize method.</param>
		/// <returns></returns>
		public static Image Resize(Image image, int width, int height, Color background, ImageResizeMethod imageResizeMethod)
		{
			switch (imageResizeMethod)
			{
				case ImageResizeMethod.FullPadding:
					return fullPaddingResize(image, width, height, background);
				case ImageResizeMethod.ScaleAndCrop:
					return cropResize(image, width, height, background);
				case ImageResizeMethod.HorizontalPadding:
					return paddedHorizontalResize(image, width, height, background);
				case ImageResizeMethod.VerticalPadding:
					return paddedVerticalResize(image, width, height, background);
				case ImageResizeMethod.ByHeight:
					return byHeight(image, height, background);
				case ImageResizeMethod.ByWidth:
					return byWidth(image, width, background);
				case ImageResizeMethod.NoPadding:
					return paddedNoneResize(image, width, height, background);
				default:
					throw new NotImplementedException(
						string.Format(CultureInfo.InvariantCulture, "Resize method {0} is not implemented.", imageResizeMethod));
			}
		}

		private static Image paddedNoneResize(Image image, int width, int height, Color background)
		{
			int oW = image.Width;
			int oH = image.Height;

			int fW, fH;
			float horRatio = width/(float) oW;
			float verRatio = height/(float) oH;

			if (horRatio > verRatio)
			{
				// the least wins
				fH = height;
				fW = (int) (oW*verRatio) + 1;
			}
			else
			{
				fH = (int) (oH*horRatio) + 1;
				fW = width;
			}

			Bitmap b = new Bitmap(fW, fH, PixelFormat.Format24bppRgb);
			b.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			Graphics g = Graphics.FromImage(b);
			g.Clear(background);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(image, new Rectangle(0, 0, fW, fH), new Rectangle(0, 0, oW, oH), GraphicsUnit.Pixel);
			g.Dispose();

			return b;
		}

		private static Image byHeight(Image image, int height, Color background)
		{
			int oW = image.Width;
			int oH = image.Height;

			int fW, fH;
			float verRatio = height/(float) oH;

			// the least wins
			fH = height;
			fW = (int) (oW*verRatio) + 1;

			Bitmap b = new Bitmap(fW, fH, PixelFormat.Format24bppRgb);
			b.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			Graphics g = Graphics.FromImage(b);
			g.Clear(background);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(image, new Rectangle(0, 0, fW, fH), new Rectangle(0, 0, oW, oH), GraphicsUnit.Pixel);
			g.Dispose();

			return b;
		}

		private static Image byWidth(Image image, int width, Color background)
		{
			int oW = image.Width;
			int oH = image.Height;

			int fW, fH;
			float horRatio = width/(float) oW;
			fH = (int) (oH*horRatio) + 1;
			fW = width;

			Bitmap b = new Bitmap(fW, fH, PixelFormat.Format24bppRgb);
			b.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			Graphics g = Graphics.FromImage(b);
			g.Clear(background);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(image, new Rectangle(0, 0, fW, fH), new Rectangle(0, 0, oW, oH), GraphicsUnit.Pixel);
			g.Dispose();

			return b;
		}

		private static Image paddedVerticalResize(Image image, int width, int height, Color background)
		{
			int oW = image.Width;
			int oH = image.Height;

			int fW, fH;
			float horRatio = width/(float) oW;
			float verRatio = height/(float) oH;

			if (horRatio > verRatio)
			{
				// the least wins
				fH = height;
				fW = (int) (oW*verRatio) + 1;
			}
			else
			{
				fH = (int) (oH*horRatio) + 1;
				fW = width;
			}

			Bitmap b = new Bitmap(fW, height, PixelFormat.Format24bppRgb);
			b.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			Graphics g = Graphics.FromImage(b);
			g.Clear(background);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(image, new Rectangle(0, (height - fH)/2, fW, fH), new Rectangle(0, 0, oW, oH), GraphicsUnit.Pixel);
			g.Dispose();

			return b;
		}

		private static Image paddedHorizontalResize(Image image, int width, int height, Color background)
		{
			int oW = image.Width;
			int oH = image.Height;

			int fW, fH;
			float horRatio = width/(float) oW;
			float verRatio = height/(float) oH;

			if (horRatio > verRatio)
			{
				// the least wins
				fH = height;
				fW = (int) (oW*verRatio) + 1;
			}
			else
			{
				fH = (int) (oH*horRatio) + 1;
				fW = width;
			}

			Bitmap b = new Bitmap(width, fH, PixelFormat.Format24bppRgb);
			b.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			Graphics g = Graphics.FromImage(b);
			g.Clear(background);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(image, new Rectangle((width - fW)/2, 0, fW, fH), new Rectangle(0, 0, oW, oH), GraphicsUnit.Pixel);
			g.Dispose();

			return b;
		}

		private static Image fullPaddingResize(Image image, int width, int height, Color background)
		{
			int oW = image.Width;
			int oH = image.Height;

			int fW, fH;
			float horRatio = width/(float) oW;
			float verRatio = height/(float) oH;

			if (horRatio > verRatio)
			{
				// the least wins
				fH = height;
				fW = (int) (oW*verRatio) + 1;
			}
			else
			{
				fH = (int) (oH*horRatio) + 1;
				fW = width;
			}

			Bitmap b = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			b.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			Graphics g = Graphics.FromImage(b);
			g.Clear(background);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(image, new Rectangle((width - fW)/2, (height - fH)/2, fW, fH), new Rectangle(0, 0, oW, oH),
			            GraphicsUnit.Pixel);
			g.Dispose();

			return b;
		}

		private static Image cropResize(Image image, int width, int height, Color background)
		{
			int oW = image.Width;
			int oH = image.Height;

			int fW, fH;
			float horRatio = width/(float) oW;
			float verRatio = height/(float) oH;

			if (horRatio < verRatio)
			{
				// the most wins
				fH = height;
				fW = (int) (oW*verRatio) + 1;
			}
			else
			{
				fH = (int) (oH*horRatio) + 1;
				fW = width;
			}

			Bitmap b = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			b.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			Graphics g = Graphics.FromImage(b);
			g.Clear(background);
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawImage(image, new Rectangle((width - fW)/2, (height - fH)/2, fW, fH), new Rectangle(0, 0, oW, oH),
			            GraphicsUnit.Pixel);
			g.Dispose();

			return b;
		}
	}

	/// <summary>
	/// Represents the resize method to use
	/// </summary>
	public enum ImageResizeMethod
	{
		/// <summary>
		/// Image is scaled proportionally so it is as large as possible while fitting in the destination rectangle.
		/// Padding is added where needed so that the final size of the image corresponds to the passed parameters.
		/// </summary>
		FullPadding,
		/// <summary>
		/// Image is scaled proportionally so it is as large as possible while fitting either horizontally or vertically in the destination rectangle.
		/// The image is then cropped to the size corresponding to the passed parameters.
		/// </summary>
		ScaleAndCrop,
		/// <summary>
		/// Image is scaled proportionally so it is as large as possible while fitting in the destination rectangle.
		/// Padding is added in the horizontal direction if needed so that the final height of the image corresponds to the passed parameters.
		/// The final width of the image can be equal or less than the passed width parameter.
		/// </summary>
		HorizontalPadding,
		/// <summary>
		/// Image is scaled proportionally so it is as large as possible while fitting in the destination rectangle.
		/// Padding is added in the vertical direction if needed so that the final height of the image corresponds to the passed parameters.
		/// The final width of the image can be equal or less than the passed width parameter.
		/// </summary>
		VerticalPadding,
		/// <summary>
		/// Image is scaled proportionally so its width is equal to the passed parameter. Height is ignored.
		/// </summary>
		ByWidth,
		/// <summary>
		/// Image is scaled proportionally so its height is equal to the passed parameter. Width is ignored.
		/// </summary>
		ByHeight,
		/// <summary>
		/// Image is scaled proportionally so it is as large as possible while fitting in the destination rectangle.
		/// Height and width are less than or equal to the passwd parameters.
		/// </summary>
		NoPadding
	}
}
