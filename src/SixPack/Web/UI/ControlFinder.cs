// ControlFinder.cs 
//
//  Copyright (C) 2009 Marco Cecconi
//  Author: Marco Cecconi <marco.cecconi@gmail.com>
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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using SixPack.Collections.Generic;
using SixPack.Text;

namespace SixPack.Web.UI
{
	/// <summary>
	/// A class that perform searches in control trees
	/// </summary>
	public class ControlFinder
	{
		private readonly Control root;

		/// <summary>
		/// Initializes a new instance of the <see cref="ControlFinder"/> class.
		/// </summary>
		/// <param name="rootControl">The root control.</param>
		public ControlFinder(Control rootControl)
		{
			root = rootControl;
		}
		
		/// <summary>
		/// Searches the control tree recursively and returns the first 
		/// case insensitive partial match of the expression 
		/// </summary>
		/// <param name="expression">
		/// The search expression <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="Control"/> that matches the expression
		/// </returns>
		public Control FindFirst(string expression)
		{
			return FindFirst(expression, TextSearchOptions.CaseInsensitive | TextSearchOptions.Partial);
		}
		
		/// <summary>
		/// Searches the control tree recursively and returns the first match 
		/// </summary>
		/// <param name="expression">
		/// The search expression <see cref="System.String"/>
		/// </param>
		/// <param name="searchOptions">
		/// The <see cref="TextSearchOptions"/>
		/// </param>
		/// <returns>
		/// A <see cref="Control"/> that matches the expression
		/// </returns>		
		public Control FindFirst(string expression, TextSearchOptions searchOptions)
		{
			return FindFirst(expression, searchOptions, true);
		}
		
		/// <summary>
		/// Searches the control tree and returns the first match 
		/// </summary>
		/// <param name="expression">
		/// The search expression <see cref="System.String"/>
		/// </param>
		/// <param name="searchOptions">
		/// The <see cref="TextSearchOptions"/>
		/// </param>
		/// <param name="recursive">
		/// Search in all the subtree if true
		/// </param>
		/// <returns>
		/// A <see cref="Control"/> that matches the expression
		/// </returns>
		public Control FindFirst(string expression, TextSearchOptions searchOptions, bool recursive)
		{
			IFullList<Control> res = Find(expression, searchOptions, recursive, true);
			if (res.Count > 0)
				return res[0];
			return null;
		}

		/// <summary>
		/// Finds the specified expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <returns></returns>
		public IFullList<Control> Find(string expression)
		{
			return Find(expression, TextSearchOptions.CaseInsensitive | TextSearchOptions.Partial);
		}

		/// <summary>
		/// Finds the specified expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <param name="searchOptions">The search options.</param>
		/// <returns></returns>
		public IFullList<Control> Find(string expression, TextSearchOptions searchOptions)
		{
			return Find(expression, searchOptions, true);
		}

		/// <summary>
		/// Finds the specified expression.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <param name="searchOptions">The search options.</param>
		/// <param name="recursive">if set to <c>true</c> [recursive].</param>
		/// <returns></returns>
		public IFullList<Control> Find(string expression, TextSearchOptions searchOptions, bool recursive)
		{
			return Find(expression, searchOptions, recursive, false);
		}
		
		private IFullList<Control> Find(string expression, TextSearchOptions searchOptions, bool recursive, bool stopAtFirst)
		{
			bool caseInsensitive = (searchOptions & TextSearchOptions.CaseInsensitive) != 0;

			IFullList<Control> result = new FullList<Control>();

			if ((searchOptions & TextSearchOptions.Regex) == 0)
			{
				bool partial = (searchOptions & TextSearchOptions.Partial) != 0;
				// text
				if (caseInsensitive)
				{
					// case insensitive text
					expression = expression.ToUpperInvariant();

					if (partial)
					{
						// case insensitive partial text
						doFind(root, result, delegate(Control c)
						                     	{
						                     		string test = c.ID;
						                     		if (string.IsNullOrEmpty(test))
						                     			return false;
						                     		test = test.ToUpperInvariant();
						                     		return (test.Contains(expression));
						                     	}, recursive, stopAtFirst);
					}
					else
					{
						// case insensitive exact text
						doFind(root, result,
						       delegate(Control c) { return (string.Compare(c.ID, expression, StringComparison.OrdinalIgnoreCase) == 0); },
						       recursive, stopAtFirst);
					}
				}
				else
				{
					// case sensitive text
					if (partial)
					{
						// case sensitive partial text
						doFind(root, result, delegate(Control c) { return (c.ID.Contains(expression)); }, recursive, stopAtFirst);
					}
					else
					{
						// case sensitive exact text
						doFind(root, result,
						       delegate(Control c) { return (string.Compare(c.ID, expression, StringComparison.Ordinal) == 0); },
						       recursive, stopAtFirst);
					}
				}
			}
			else
			{
				// regex
				// we silently ignore the partial flag
				RegexOptions opts = caseInsensitive ? RegexOptions.IgnoreCase : RegexOptions.None;
				doFind(root, result, delegate(Control c) { return (Regex.IsMatch(c.ID, expression, opts)); }, recursive, stopAtFirst);
			}

			return result;
		}

		private static void doFind(Control target, ICollection<Control> result, ControlSieve matches, bool recursive, bool stopAtFirst)
		{
			if (target == null || !target.HasControls())
				return;
			foreach (Control c in target.Controls)
			{
				if (stopAtFirst && result.Count > 0)
					break;
				if (matches(c))
				{
					result.Add(c);
					if (stopAtFirst)
						break;
				}
				if (recursive)
				{
					doFind(c, result, matches, true, stopAtFirst);
				}
			}
		}

		/// <summary>
		/// Finds all controls matching the specified expression 
		/// using case insensitive partial matching
		/// in the specified control and its subtree. 
		/// </summary>
		/// <param name="rootControl">
		/// The root <see cref="Control"/>
		/// </param>
		/// <param name="expression">
		/// The expression
		/// </param>
		/// <returns>
		/// </returns>
		public static IFullList<Control> FindInControl(Control rootControl, string expression)
		{
			return FindInControl(rootControl, expression, TextSearchOptions.CaseInsensitive | TextSearchOptions.Partial);
		}
		
		/// <summary>
		/// Finds all controls matching the specified expression 
		/// in the specified control and its subtree. 
		/// </summary>
		/// <param name="rootControl">
		/// The root <see cref="Control"/>
		/// </param>
		/// <param name="expression">
		/// The expression
		/// </param>
		/// <param name="searchOptions">
		/// The <see cref="TextSearchOptions"/>
		/// </param>
		/// <returns>
		/// </returns>
		public static IFullList<Control> FindInControl(Control rootControl, string expression, TextSearchOptions searchOptions)
		{
			return FindInControl(rootControl, expression, searchOptions, true);
		}
		
		/// <summary>
		/// Finds all controls matching the specified expression 
		/// </summary>
		/// <param name="rootControl">
		/// The root <see cref="Control"/>
		/// </param>
		/// <param name="expression">
		/// The expression
		/// </param>
		/// <param name="searchOptions">
		/// The <see cref="TextSearchOptions"/>
		/// </param>
		/// <param name="recursive">
		/// Recurse subtree if true
		/// </param>
		/// <returns>
		/// </returns>
		public static IFullList<Control> FindInControl(Control rootControl, string expression, TextSearchOptions searchOptions, bool recursive)
		{
			return new ControlFinder(rootControl).Find(expression, searchOptions, recursive);
		}

		/// <summary>
		/// Searches the control tree recursively and returns the first 
		/// case insensitive partial match of the expression 
		/// </summary>
		/// <param name="rootControl">
		/// The root <see cref="Control"/>
		/// </param>
		/// <param name="expression">
		/// The search expression <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// </returns>
		public static Control FindFirstInControl(Control rootControl, string expression)
		{
			return FindFirstInControl(rootControl, expression, TextSearchOptions.CaseInsensitive | TextSearchOptions.Partial);
		}
		
		/// <summary>
		/// Searches the control tree recursively and returns the first match 
		/// </summary>
		/// <param name="rootControl">
		/// The root <see cref="Control"/>
		/// </param>
		/// <param name="expression">
		/// The search expression <see cref="System.String"/>
		/// </param>
		/// <param name="searchOptions">
		/// The <see cref="TextSearchOptions"/>
		/// </param>
		/// <returns>
		/// </returns>		
		public static Control FindFirstInControl(Control rootControl, string expression, TextSearchOptions searchOptions)
		{
			return FindFirstInControl(rootControl, expression, searchOptions, true);
		}
		
		/// <summary>
		/// Searches the control tree and returns the first match 
		/// </summary>
		/// <param name="rootControl">
		/// The root <see cref="Control"/>
		/// </param>
		/// <param name="expression">
		/// The search expression <see cref="System.String"/>
		/// </param>
		/// <param name="searchOptions">
		/// The <see cref="TextSearchOptions"/>
		/// </param>
		/// <param name="recursive">
		/// Search in all the subtree if true
		/// </param>
		/// <returns>
		/// </returns>
		public static Control FindFirstInControl(Control rootControl, string expression, TextSearchOptions searchOptions, bool recursive)
		{
			return new ControlFinder(rootControl).FindFirst(expression, searchOptions, recursive);
		}

	}

	/// <summary>
	/// A delegate implementing a filter for controls
	/// </summary>
	internal delegate bool ControlSieve(Control target);
}