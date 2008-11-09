// XmlRpc.cs 
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

// XmlRpc.cs created with MonoDevelop
// User: marco at 12:49 PM 10/14/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace SixPack.Net
{
    /// <summary>
    /// Support class for XML remote procedure calls.
    /// </summary>
    public class XmlRpc
    {
        private readonly Uri requestUri;
        private NetworkCredential credentials;
        private XmlRpcRequestMethod method = XmlRpcRequestMethod.Get;
        private HttpWebResponse response;

        private int timeout = 2000;
        private XslCompiledTransform xslTransform;

        /// <summary>
        /// Creates a new instance of the <see cref="XmlRpc"/> class.
        /// </summary>
        /// <param name="requestUri">
        /// The <see cref="Uri"/> that is used for the request.
        /// </param>
        /// <exception cref="ArgumentNullException">The "requestUri" parameter was null.</exception>
        /// <exception cref="ArgumentException">The scheme for the "requestUri" must be either http or https.</exception>
        public XmlRpc(Uri requestUri)
        {
            if (requestUri == null)
                throw new ArgumentNullException("requestUri");
            if (requestUri.Scheme != "http" && requestUri.Scheme != "https")
                throw new ArgumentOutOfRangeException("requestUri",
                                                      "The scheme for requestUri must be either http or https");

            this.requestUri = requestUri;
        }

        /// <value>
        /// Gets the <see cref="Uri"/> that is used for the request.
        /// </value>
        public Uri RequestUri
        {
            get { return requestUri; }
        }

        /// <value>
        /// Gets or sets the request timeout in milliseconds.
        /// </value>
        /// <remarks>The default value is 2000 milliseconds.</remarks>
        public int Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        /// <value>
        /// Gets or sets the HTTP method that is used for the request.
        /// </value>
        /// <remarks>The default value is XmlRpcRequestMethod.Get.</remarks>
        public XmlRpcRequestMethod Method
        {
            get { return method; }
            set { method = value; }
        }

        /// <value>
        /// Gets or sets the <see cref="NetworkCredential"/> that is used for the request.
        /// </value>
        public NetworkCredential Credentials
        {
            get { return credentials; }
            set { credentials = value; }
        }

        /// <value>
        /// Gets or sets the user name that is used for the request.
        /// </value>
        public string UserName
        {
            get
            {
                return credentials == null
                           ?
                               string.Empty
                           :
                               credentials.UserName;
            }
            set
            {
                if (credentials == null)
                    credentials = new NetworkCredential();
                credentials.UserName = value;
            }
        }

        /// <value>
        /// Gets or sets the password that is used for the request.
        /// </value>
        public string Password
        {
            get
            {
                return credentials == null
                           ?
                               string.Empty
                           :
                               credentials.Password;
            }
            set
            {
                if (credentials == null)
                    credentials = new NetworkCredential();
                credentials.Password = value;
            }
        }

        /// <summary>
        /// Gets or sets a XSL transform that will be applied to the response before further processing.
        /// </summary>
        /// <value>The XSL transform.</value>
        public XslCompiledTransform XslTransform
        {
            get { return xslTransform; }
            set { xslTransform = value; }
        }

        private void makeRequest()
        {
#if DEBUG
            Console.WriteLine("Contacting '{0}'", requestUri);
#endif
            HttpWebRequest req = (HttpWebRequest) WebRequest.Create(requestUri);
            req.Timeout = timeout;

            if (method == XmlRpcRequestMethod.Get)
                req.Method = "GET";
            else if (method == XmlRpcRequestMethod.Post)
                req.Method = "POST";

            req.Credentials = credentials;

            response = (HttpWebResponse) req.GetResponse();
        }

        /// <summary>
        /// Makes a HTTP/HTTPS request and returns the result in a <see cref="XmlReader"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="XmlReader"/> containing the response.
        /// </returns>
        public XmlReader GetResponseXml()
        {
            makeRequest();
            if (xslTransform == null)
                return new XmlTextReader(new StreamReader(response.GetResponseStream()));

            XmlReader originalXml = new XmlTextReader(new StreamReader(response.GetResponseStream()));
            StringWriter transformedXml = new StringWriter(CultureInfo.InvariantCulture);

            xslTransform.Transform(originalXml, null, new XmlTextWriter(transformedXml));

            StringReader transformedXmlReader = new StringReader(transformedXml.ToString());
            return new XmlTextReader(transformedXmlReader);
        }

        /// <summary>
        /// Makes a HTTP/HTTPS request and returns the result in a <see cref="HttpWebResponse"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="HttpWebResponse"/> containing the response.
        /// </returns>
        public HttpWebResponse GetResponse()
        {
            makeRequest();
            return response;
        }

        /// <summary>
        /// Makes a HTTP/HTTPS request and returns the result in a <see cref="DataSet"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="DataSet"/> containing the response.
        /// </returns>
        public DataSet GetResponseDataSet()
        {
            DataSet ds = new DataSet();
            ds.Locale = CultureInfo.InvariantCulture;
            ds.ReadXml(GetResponseXml());
            return ds;
        }

        /// <summary>
        /// Makes a HTTP/HTTPS request and returns the result in a deserialized object.
        /// </summary>
        /// <param name="type">
        /// The <see cref="Type"/> to use for deserialization.
        /// </param>
        /// <returns>
        /// A <see cref="System.Object"/> containing the response deserialized to the specified type. 
        /// </returns>
        /// <exception cref="ArgumentNullException">The parameter "type" is null.</exception>
        public object GetResponseObject(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            XmlSerializer ser = new XmlSerializer(type);
            return ser.Deserialize(GetResponseXml());
        }

        /// <summary>
        /// Makes a HTTP/HTTPS request and returns the result in a deserialized object.
        /// </summary>
        /// <param name="types">
        /// An array of <see cref="Type"/> to be used for deserialization. If the first types fails, we will try the
        /// second and so on, until a compatible type is found or the array has been completely parsed.
        /// </param>
        /// <returns>
        /// A <see cref="System.Object"/> containing the response deserialized to the specified type. 
        /// </returns>
        /// <exception cref="ArgumentException">The array is empty.</exception>
        /// <exception cref="XmlException">No compatible type found.</exception>
        public object GetResponseObject(Type[] types)
        {
            int len = types.Length;
            if (len == 0)
                throw new ArgumentException("The array must contain at least one type", "types");

            XmlReader xr = GetResponseXml();

            for (int i = 0; i < len; i++)
            {
                XmlSerializer ser = new XmlSerializer(types[i]);
                if (ser.CanDeserialize(xr))
                    return ser.Deserialize(xr);
            }

            throw new XmlException("No compatible type found");
        }

        /// <summary>
        /// Makes a HTTP/HTTPS request and returns the result in a deserialized object.
        /// </summary>
        /// <param name="serializer">
        /// A <see cref="XmlSerializer"/> to be used for deserialization;
        /// </param>
        /// <returns>
        /// A <see cref="System.Object"/> containing the response deserialized with the custom serializer. 
        /// </returns>
        /// <exception cref="ArgumentNullException">The parameter "serializer" is null.</exception>
        public object GetResponseObject(XmlSerializer serializer)
        {
            if (serializer == null)
                throw new ArgumentNullException("serializer");

            return serializer.Deserialize(GetResponseXml());
        }
    }
}
