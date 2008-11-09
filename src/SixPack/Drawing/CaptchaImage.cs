// CaptchaImage.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Copyright (C) 2008 Marco Cecconi
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

namespace SixPack.Drawing
{
	/// <summary>
	/// Generates CAPTCHA images
	/// </summary>
	public sealed class CaptchaImage : IDisposable
	{
		private readonly Random random = new Random();
		private readonly string text;
		private string familyName;
		private int height;
		private Bitmap image;

		private int width;

		/// <summary>
		/// Initializes a new instance of the <see cref="CaptchaImage"/> class.
		/// </summary>
		/// <param name="text">The s.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		public CaptchaImage(string text, int width, int height)
		{
			this.text = text;
			setDimensions(width, height);
			generateImage();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CaptchaImage"/> class.
		/// </summary>
		/// <param name="text">The s.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="familyName">Name of the family.</param>
		public CaptchaImage(string text, int width, int height, string familyName)
		{
			this.text = text;
			setDimensions(width, height);
			setFamilyName(familyName);
			generateImage();
		}

		/// <summary>
		/// Gets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get { return text; }
		}

		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <value>The image.</value>
		public Bitmap Image
		{
			get { return image; }
		}

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>The width.</value>
		public int Width
		{
			get { return width; }
		}

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public int Height
		{
			get { return height; }
		}

		#region IDisposable Members

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			Dispose(true);
		}

		#endregion

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="CaptchaImage"/> is reclaimed by garbage collection.
		/// </summary>
		~CaptchaImage()
		{
			Dispose(false);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				// dispose managed resources
				image.Dispose();
			}
			// free native resources
		}

		private void setDimensions(int _width, int _height)
		{
			// Check the width and height.
			if (_width <= 0)
				throw new ArgumentOutOfRangeException("_width", _width, "Argument out of range, must be greater than zero.");
			if (_height <= 0)
				throw new ArgumentOutOfRangeException("_height", _height, "Argument out of range, must be greater than zero.");
			width = _width;
			height = _height;
		}

		private void setFamilyName(string _familyName)
		{
			// If the named font is not installed, default to a system font.
			try
			{
				Font font = new Font(familyName, 12F);
				familyName = _familyName;
				font.Dispose();
			}
			catch (ArgumentException)
			{
				familyName = FontFamily.GenericSerif.Name;
			}
		}

		private void generateImage()
		{
			// Create a new 32-bit bitmap image.
			Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

			// Create a graphics object for drawing.
			Graphics g = Graphics.FromImage(bitmap);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle rect = new Rectangle(0, 0, width, height);

			// Fill in the background.
			HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White);
			g.FillRectangle(hatchBrush, rect);

			// Set up the text font.
			SizeF size;
			float fontSize = rect.Height + 1;
			Font font;
			// Adjust the font size until the text fits within the image.
			do
			{
				fontSize--;
				font = new Font(familyName, fontSize, FontStyle.Bold);
				size = g.MeasureString(text, font);
			} while (size.Width > rect.Width);

			// Set up the text format.
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;
			format.LineAlignment = StringAlignment.Center;

			// Create a path using the text and warp it randomly.
			GraphicsPath path = new GraphicsPath();
			path.AddString(text, font.FontFamily, (int) font.Style, font.Size, rect, format);
			float v = 4F;
			PointF[] points =
				{
					new PointF(random.Next(rect.Width)/v, random.Next(rect.Height)/v),
					new PointF(rect.Width - random.Next(rect.Width)/v, random.Next(rect.Height)/v),
					new PointF(random.Next(rect.Width)/v, rect.Height - random.Next(rect.Height)/v),
					new PointF(rect.Width - random.Next(rect.Width)/v, rect.Height - random.Next(rect.Height)/v)
				};
			Matrix matrix = new Matrix();
			matrix.Translate(0F, 0F);
			path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

			// Draw the text.
			hatchBrush = new HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray);
			g.FillPath(hatchBrush, path);

			// Add some random noise.
			int m = Math.Max(rect.Width, rect.Height);
			for (int i = 0; i < (int) (rect.Width*rect.Height/30F); i++)
			{
				int x = random.Next(rect.Width);
				int y = random.Next(rect.Height);
				int w = random.Next(m/50);
				int h = random.Next(m/50);
				g.FillEllipse(hatchBrush, x, y, w, h);
			}

			// Clean up.
			font.Dispose();
			hatchBrush.Dispose();
			g.Dispose();

			// Set the image.
			image = bitmap;
		}
	}
}
