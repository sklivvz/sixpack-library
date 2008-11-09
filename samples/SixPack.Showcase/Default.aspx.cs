// Default.aspx.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
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
using System.Threading;
using System.Web.UI;
using SixPack.ComponentModel;
using SixPack.Text;

namespace SixPack.Showcase
{
	public partial class _Default : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			//Example 1

			if (SyntacticValidator.IsValidPTPostalCode(TextBox1.Text))
				Label1.Text = "OK";
			else
				Label1.Text = "KO";
		}

		protected void Button2_Click(object sender, EventArgs e)
		{
			// Example 2
			Label2.Text = TextUtilities.Clip(TextBox2.Text, int.Parse(TextBox3.Text), TextBox4.Text, CheckBox1.Checked);
		}

		protected void Button3_Click(object sender, EventArgs e)
		{
			MyTime myTime = new MyTime();
			Label3.Text = string.Empty;
			for (int i = 0; i < 10; i++)
			{
				Label3.Text +=
					String.Format("{0:HH:mm:ss:fffff}&nbsp;&raquo;&nbsp;{1:HH:mm:ss:fffff}<br />
", DateTime.Now, myTime.Get());
				Thread.Sleep(600);
			}
		}
	}

	[Cached]
	public class MyTime : ContextBoundObject
	{
		[CachedMethod(1)]
		public DateTime Get()
		{
			return DateTime.Now;
		}
	}
}
