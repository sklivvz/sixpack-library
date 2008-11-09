// CachedSink.cs 
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
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Text;
using SixPack.Diagnostics;

namespace SixPack.ComponentModel
{
    /// <summary>
    /// Experimental
    /// </summary>
    public class CachedSink : IMessageSink
    {
        private readonly IMessageSink nextSink;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedSink"/> class.
        /// </summary>
        /// <param name="nextSink">The next sink.</param>
        public CachedSink(IMessageSink nextSink)
        {
            this.nextSink = nextSink;
            //Log.Instance.AddFormat("CachedSink ctor (nextSink={1}, cacheTime={0})", cacheTime, nextSink);
        }

        #region IMessageSink Members

        /// <summary>
        /// Gets the next message sink in the sink chain.
        /// </summary>
        /// <value></value>
        /// <returns>The next message sink in the sink chain.</returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller makes the call through a reference to the interface and does not have infrastructure permission. </exception>
        /// <PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/></PermissionSet>
        public IMessageSink NextSink
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
            get { return nextSink; }
        }

        /// <summary>
        /// Synchronously processes the given message.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>
        /// A reply message in response to the request.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller makes the call through a reference to the interface and does not have infrastructure permission. </exception>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public IMessage SyncProcessMessage(IMessage msg)
        {

#if DEBUG
            Log.Instance.AddFormat("CachedSink SyncProcessMessage (msg={0})", msg);
#endif
            IMethodCallMessage methodMessage = msg as IMethodCallMessage;
            if (methodMessage != null)
            {

                object[] attributes =
                    methodMessage.MethodBase.GetCustomAttributes(Type.GetType("SixPack.ComponentModel.CachedMethodAttribute"), true);
                if (attributes.Length > 0)
                {
                    int cacheTime = ((CachedMethodAttribute)attributes[0]).CacheTime;
                    if (cacheTime > 0)
                    {
                        //Log.Instance.AddFormat("CachedSink SyncProcessMessage (methodMessage={0})", methodMessage);
                        StringBuilder sb =
                            new StringBuilder(methodMessage.TypeName.GetHashCode().ToString("X", CultureInfo.InvariantCulture));
                        sb.Append(methodMessage.MethodName.GetHashCode().ToString("X", CultureInfo.InvariantCulture));
                        sb.AppendFormat(" {0}.{1}", methodMessage.TypeName.Split(',')[0], methodMessage.MethodName);
                        foreach (object o in methodMessage.Args)
                            sb.AppendFormat(CultureInfo.InvariantCulture, " <{0}>", o.GetHashCode());

                        return
                            CacheController.GetFromCache(sb.ToString(), methodMessage, nextSink,
                                                         cacheTime);
                    }
                }
            }

            return nextSink.SyncProcessMessage(msg) as IMethodReturnMessage;

        }

        /// <summary>
        /// Asynchronously processes the given message.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <param name="replySink">The reply sink for the reply message.</param>
        /// <returns>
        /// Returns an <see cref="T:System.Runtime.Remoting.Messaging.IMessageCtrl"></see> interface that provides a way to control asynchronous messages after they have been dispatched.
        /// </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller makes the call through a reference to the interface and does not have infrastructure permission. </exception>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            return nextSink.AsyncProcessMessage(msg, replySink);
        }

        #endregion
    }
}
