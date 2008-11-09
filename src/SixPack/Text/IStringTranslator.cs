// IStringTranslator.cs 
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

/*
 * Made by: Nuno Lourenço
 * Create date: 2008/03/01
 * Modified date: 2008/04/17
 */
namespace SixPack.Text
{
    /// <summary>
    /// Interface for a class implementing a translation operator a-la Perl tr/// operator.
    /// </summary>
    public interface IStringTranslator
    {
        /// <summary>
        /// Gets the initial alphabet.
        /// </summary>
        /// <value>The initial alphabet.</value>
        string InitialAlphabet { get; }

        /// <summary>
        /// Gets the final alphabet.
        /// </summary>
        /// <value>The final alphabet.</value>
        string FinalAlphabet { get; }

        /// <summary>
        /// Translates the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        string Translate(string text);
    }
}
