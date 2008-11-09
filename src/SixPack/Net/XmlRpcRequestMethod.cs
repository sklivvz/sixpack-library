// XmlRpcRequestMethod.cs 
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

// XmlRpcRequestMethod.cs created with MonoDevelop
// User: marco at 10:59 AM 11/3/2008
//

namespace SixPack.Net
{
    /// <summary>
    /// Represents the HTTP method to use to make an XML remote procedure call request.
    /// </summary>
    public enum XmlRpcRequestMethod
    {
        /// <summary>
        /// Make the request using "GET"
        /// </summary>
        Get,
        /// <summary>
        /// Make the request using "POST"
        /// </summary>
        Post,
    }
}
