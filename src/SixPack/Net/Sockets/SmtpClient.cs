// SmtpClient.cs 
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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SixPack.Net.Sockets
{
	/// <summary>
	/// This class provides a simple SMTP client, based on RFC 821
	/// See: http://www.ietf.org/rfc/rfc0821.txt
	/// </summary>
	public sealed class SmtpClient : IDisposable
	{
		private int port;
		private IPAddress server;
		private Socket socket;

		/// <summary>
		/// Initializes a new instance of the <see cref="SmtpClient"/> class.
		/// </summary>
		/// <param name="smtpServer">The SMTP server.</param>
		public SmtpClient(IPAddress smtpServer)
			: this(smtpServer, 25)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SmtpClient"/> class.
		/// </summary>
		/// <param name="server">The server.</param>
		/// <param name="port">The port.</param>
		public SmtpClient(IPAddress server, int port)
		{
			this.server = server;
			this.port = port;
		}

		/// <summary>
		/// Gets or sets the server.
		/// </summary>
		/// <value>The server.</value>
		public IPAddress Server
		{
			get { return server; }
			set { server = value; }
		}

		/// <summary>
		/// Gets or sets the port.
		/// </summary>
		/// <value>The port.</value>
		public int Port
		{
			get { return port; }
			set { port = value; }
		}

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		/// <summary>
		/// Connects this instance to the SMTP server.
		/// </summary>
		public SmtpResponse Connect()
		{
			IPEndPoint ipEndPoint = new IPEndPoint(server, port);
			socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect(ipEndPoint);
			return receiveAndParse();
		}

		/// <summary>
		/// HELLO (HELO)
		/// This command is used to identify the sender-SMTP to the
		/// receiver-SMTP.  The argument field contains the host name of
		/// the sender-SMTP.
		/// The receiver-SMTP identifies itself to the sender-SMTP in
		/// the connection greeting reply, and in the response to this
		/// command.
		/// This command and an OK reply to it confirm that both the
		/// sender-SMTP and the receiver-SMTP are in the initial state,
		/// that is, there is no transaction in progress and all state
		/// tables and buffers are cleared.
		/// </summary>
		/// <returns></returns>
		public SmtpResponse Hello()
		{
			send(string.Format(CultureInfo.InvariantCulture, "HELO {0}", System.Net.Dns.GetHostName()));
			return receiveAndParse();
		}

		/// <summary>
		/// MAIL (MAIL)
		/// This command is used to initiate a mail transaction in which
		/// the mail data is delivered to one or more mailboxes.  The
		/// argument field contains a reverse-path.
		/// The reverse-path consists of an optional list of hosts and
		/// the sender mailbox.  When the list of hosts is present, it
		/// is a "reverse" source route and indicates that the mail was
		/// relayed through each host on the list (the first host in the
		/// list was the most recent relay).  This list is used as a
		/// source route to return non-delivery notices to the sender.
		/// As each relay host adds itself to the beginning of the list,
		/// it must use its name as known in the IPCE to which it is
		/// relaying the mail rather than the IPCE from which the mail
		/// came (if they are different).  In some types of error
		/// reporting messages (for example, undeliverable mail
		/// notifications) the reverse-path may be null (see Example 7).
		/// This command clears the reverse-path buffer, the
		/// forward-path buffer, and the mail data buffer; and inserts
		/// the reverse-path information from this command into the
		/// reverse-path buffer.
		/// </summary>
		/// <param name="from">From parameter.</param>
		public SmtpResponse Mail(string from)
		{
			send(string.Format(CultureInfo.InvariantCulture, "MAIL FROM:{0}", from));
			return receiveAndParse();
		}

		/// <summary>
		/// RECIPIENT (RCPT)
		/// 
		/// This command is used to identify an individual recipient of
		/// the mail data; multiple recipients are specified by multiple
		/// use of this command.
		/// 
		/// The forward-path consists of an optional list of hosts and a
		/// required destination mailbox.  When the list of hosts is
		/// present, it is a source route and indicates that the mail
		/// must be relayed to the next host on the list.  If the
		/// receiver-SMTP does not implement the relay function it may
		/// user the same reply it would for an unknown local user
		/// (550).
		/// 
		/// When mail is relayed, the relay host must remove itself from
		/// the beginning forward-path and put itself at the beginning
		/// of the reverse-path.  When mail reaches its ultimate
		/// destination (the forward-path contains only a destination
		/// mailbox), the receiver-SMTP inserts it into the destination
		/// mailbox in accordance with its host mail conventions.
		/// </summary>
		/// <param name="to">To parameter.</param>
		public SmtpResponse Recipient(string to)
		{
			send(string.Format(CultureInfo.InvariantCulture, "RCPT TO:{0}", to));
			return receiveAndParse();
		}

		/// <summary>
		/// DATA (DATA)
		/// The receiver treats the lines following the command as mail
		/// data from the sender.  This command causes the mail data
		/// from this command to be appended to the mail data buffer.
		/// The mail data may contain any of the 128 ASCII character
		/// codes.
		/// The mail data is terminated by a line containing only a
		/// period, that is the character sequence "&lt;CRLF&gt;.&lt;CRLF&gt;" (see
		/// Section 4.5.2 on Transparency).  This is the end of mail
		/// data indication.
		/// The end of mail data indication requires that the receiver
		/// must now process the stored mail transaction information.
		/// This processing consumes the information in the reverse-path
		/// buffer, the forward-path buffer, and the mail data buffer,
		/// and on the completion of this command these buffers are
		/// cleared.  If the processing is successful the receiver must
		/// send an OK reply.  If the processing fails completely the
		/// receiver must send a failure reply.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public SmtpResponse Data(string text)
		{
			send("DATA");
			SmtpResponse smtpResponse = receiveAndParse();
			if (!(smtpResponse.ResponseCode == SmtpResponseCode.StartMailInput))
				return smtpResponse;
			send(text);
			send("\r\n.");
			return receiveAndParse();
		}

		/// <summary>
		/// SEND (SEND)
		/// 
		/// This command is used to initiate a mail transaction in which
		/// the mail data is delivered to one or more terminals.  The
		/// argument field contains a reverse-path.  This command is
		/// successful if the message is delivered to a terminal.
		/// 
		/// The reverse-path consists of an optional list of hosts and
		/// the sender mailbox.  When the list of hosts is present, it
		/// is a "reverse" source route and indicates that the mail was
		/// relayed through each host on the list (the first host in the
		/// list was the most recent relay).  This list is used as a
		/// source route to return non-delivery notices to the sender.
		/// As each relay host adds itself to the beginning of the list,
		/// it must use its name as known in the IPCE to which it is
		/// relaying the mail rather than the IPCE from which the mail
		/// came (if they are different).
		/// 
		/// This command clears the reverse-path buffer, the
		/// forward-path buffer, and the mail data buffer; and inserts
		/// the reverse-path information from this command into the
		/// reverse-path buffer.
		/// </summary>
		/// <returns></returns>
		public SmtpResponse Send(string from)
		{
			send(string.Format(CultureInfo.InvariantCulture, "SEND FROM:{0}", from));
			return receiveAndParse();
		}

		/// <summary>
		/// SEND OR MAIL (SOML)
		/// 
		/// This command is used to initiate a mail transaction in which
		/// the mail data is delivered to one or more terminals or
		/// mailboxes. For each recipient the mail data is delivered to
		/// the recipient's terminal if the recipient is active on the
		/// host (and accepting terminal messages), otherwise to the
		/// recipient's mailbox.  The argument field contains a
		/// reverse-path.  This command is successful if the message is
		/// delivered to a terminal or the mailbox.
		/// 
		/// The reverse-path consists of an optional list of hosts and
		/// the sender mailbox.  When the list of hosts is present, it
		/// is a "reverse" source route and indicates that the mail was
		/// relayed through each host on the list (the first host in the
		/// list was the most recent relay).  This list is used as a
		/// source route to return non-delivery notices to the sender.
		/// As each relay host adds itself to the beginning of the list,
		/// it must use its name as known in the IPCE to which it is
		/// relaying the mail rather than the IPCE from which the mail
		/// came (if they are different).
		/// 
		/// This command clears the reverse-path buffer, the
		/// forward-path buffer, and the mail data buffer; and inserts
		/// the reverse-path information from this command into the
		/// reverse-path buffer.
		/// </summary>
		/// <returns></returns>
		public SmtpResponse SendOrMail(string from)
		{
			send(string.Format(CultureInfo.InvariantCulture, "SOML FROM:{0}", from));
			return receiveAndParse();
		}

		/// <summary>
		/// SEND AND MAIL (SAML)
		/// 
		/// This command is used to initiate a mail transaction in which
		/// the mail data is delivered to one or more terminals and
		/// mailboxes. For each recipient the mail data is delivered to
		/// the recipient's terminal if the recipient is active on the
		/// host (and accepting terminal messages), and for all
		/// recipients to the recipient's mailbox.  The argument field
		/// contains a reverse-path.  This command is successful if the
		/// message is delivered to the mailbox.
		/// 
		/// The reverse-path consists of an optional list of hosts and
		/// the sender mailbox.  When the list of hosts is present, it
		/// is a "reverse" source route and indicates that the mail was
		/// relayed through each host on the list (the first host in the
		/// list was the most recent relay).  This list is used as a
		/// source route to return non-delivery notices to the sender.
		/// As each relay host adds itself to the beginning of the list,
		/// it must use its name as known in the IPCE to which it is
		/// relaying the mail rather than the IPCE from which the mail
		/// came (if they are different).
		/// 
		/// This command clears the reverse-path buffer, the
		/// forward-path buffer, and the mail data buffer; and inserts
		/// the reverse-path information from this command into the
		/// reverse-path buffer.
		/// </summary>
		/// <returns></returns>
		public SmtpResponse SendAndMail(string from)
		{
			send(string.Format(CultureInfo.InvariantCulture, "SAML FROM:{0}", from));
			return receiveAndParse();
		}

		/// <summary>
		/// RESET (RSET)
		/// 
		/// This command specifies that the current mail transaction is
		/// to be aborted.  Any stored sender, recipients, and mail data
		/// must be discarded, and all buffers and state tables cleared.
		/// The receiver must send an OK reply.
		/// </summary>
		/// <returns></returns>
		public SmtpResponse Reset()
		{
			send("RSET");
			return receiveAndParse();
		}

		/// <summary>
		/// VERIFY (VRFY)
		/// This command asks the receiver to confirm that the argument
		/// identifies a user.  If it is a user name, the full name of
		/// the user (if known) and the fully specified mailbox are
		/// returned.
		/// This command has no effect on any of the reverse-path
		/// buffer, the forward-path buffer, or the mail data buffer.
		/// </summary>
		/// <param name="argument">The argument.</param>
		/// <returns></returns>
		public SmtpResponse Verify(string argument)
		{
			send(string.Format(CultureInfo.InvariantCulture, "VRFY {0}", argument));
			return receiveAndParse();
		}

		/// <summary>
		/// EXPAND (EXPN)
		/// This command asks the receiver to confirm that the argument
		/// identifies a mailing list, and if so, to return the
		/// membership of that list.  The full name of the users (if
		/// known) and the fully specified mailboxes are returned in a
		/// multiline reply.
		/// This command has no effect on any of the reverse-path
		/// buffer, the forward-path buffer, or the mail data buffer.
		/// </summary>
		/// <param name="argument">The argument.</param>
		/// <returns></returns>
		public string Expand(string argument)
		{
			send(string.Format(CultureInfo.InvariantCulture, "EXPN {0}", argument));
			return receive();
		}

		/// <summary>
		/// HELP (HELP)
		/// This command causes the receiver to send helpful information
		/// to the sender of the HELP command.  The command may take an
		/// argument (e.g., any command name) and return more specific
		/// information as a response.
		/// This command has no effect on any of the reverse-path
		/// buffer, the forward-path buffer, or the mail data buffer.
		/// </summary>
		/// <param name="argument">The argument.</param>
		/// <returns></returns>
		public string Help(string argument)
		{
			if (argument != null)
				send(string.Format(CultureInfo.InvariantCulture, "HELP {0}", argument));
			else
				send("HELP");
			return receive();
		}

		/// <summary>
		/// NOOP (NOOP)
		/// 
		/// This command does not affect any parameters or previously
		/// entered commands.  It specifies no action other than that
		/// the receiver send an OK reply.
		/// 
		/// This command has no effect on any of the reverse-path
		/// buffer, the forward-path buffer, or the mail data buffer.
		/// </summary>
		/// <returns></returns>
		public SmtpResponse Noop()
		{
			send("NOOP");
			return receiveAndParse();
		}

		/// <summary>
		/// QUIT (QUIT)
		/// 
		/// This command specifies that the receiver must send an OK
		/// reply, and then close the transmission channel.
		/// 
		/// The receiver should not close the transmission channel until
		/// it receives and replies to a QUIT command (even if there was
		/// an error).  The sender should not close the transmission
		/// channel until it send a QUIT command and receives the reply
		/// (even if there was an error response to a previous command).
		/// If the connection is closed prematurely the receiver should
		/// act as if a RSET command had been received (canceling any
		/// pending transaction, but not undoing any previously
		/// completed transaction), the sender should act as if the
		/// command or transaction in progress had received a temporary
		/// error (4xx).
		/// </summary>
		/// <returns></returns>
		public SmtpResponse Quit()
		{
			send("QUIT");
			return receiveAndParse();
		}

		/// <summary>
		/// TURN (TURN)
		/// 
		/// This command specifies that the receiver must either (1)
		/// send an OK reply and then take on the role of the
		/// sender-SMTP, or (2) send a refusal reply and retain the role
		/// of the receiver-SMTP.
		/// 
		/// If program-A is currently the sender-SMTP and it sends the
		/// TURN command and receives an OK reply (250) then program-A
		/// becomes the receiver-SMTP.  Program-A is then in the initial
		/// state as if the transmission channel just opened, and it
		/// then sends the 220 service ready greeting.
		/// 
		/// If program-B is currently the receiver-SMTP and it receives
		/// the TURN command and sends an OK reply (250) then program-B
		/// becomes the sender-SMTP.  Program-B is then in the initial
		/// state as if the transmission channel just opened, and it
		/// then expects to receive the 220 service ready greeting.
		/// 
		/// To refuse to change roles the receiver sends the 502 reply.
		/// </summary>
		/// <returns></returns>
		public SmtpResponse Turn()
		{
			send("TURN");
			return receiveAndParse();
		}

		private SmtpResponse receiveAndParse()
		{
			return new SmtpResponse(receive());
		}

		private void send(string message)
		{
			//Log.Instance.AddFormat("Request: {0}", message);

			byte[] messageBytes = Encoding.ASCII.GetBytes(string.Format(CultureInfo.InvariantCulture, "{0}\r\n", message));
			socket.Send(messageBytes, 0, messageBytes.Length, SocketFlags.None);
		}

		private string receive()
		{
			byte[] bytes = new byte[1024];
			while (socket.Available == 0)
				Thread.Sleep(100);

			socket.Receive(bytes, 0, socket.Available, SocketFlags.None);

			return Encoding.ASCII.GetString(bytes);
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
				if (socket != null)
				{
					if (socket.Connected)
					{
						socket.Disconnect(false);
					}
					socket.Close();
					((IDisposable)socket).Dispose();
				}
			}
			// free native resources
		}
	}

	/// <summary>
	/// Encapsulates a SMTP response
	/// </summary>
	public class SmtpResponse
	{
		private readonly string message;
		private SmtpResponseCode responseCode;
		private string responseText;

		internal SmtpResponse(string message)
		{
			this.message = message;
			Parse();
		}

		//internal string Message
		//{
		//    get { return message; }
		//    set { message = value; }
		//}

		/// <summary>
		/// Gets the response code.
		/// </summary>
		/// <value>The response code.</value>
		public SmtpResponseCode ResponseCode
		{
			get { return responseCode; }
		}

		/// <summary>
		/// Gets the response text as provided from the SMTP server.
		/// </summary>
		/// <value>The response text.</value>
		public string ResponseText
		{
			get { return responseText; }
		}

		internal void Parse()
		{
			responseCode = (SmtpResponseCode) int.Parse(message.Substring(0, 3), CultureInfo.InvariantCulture);
			responseText = message.Substring(3);
			//Log.Instance.AddFormat("Response: {0}", message.Trim());
		}
	}

	/// <summary>
	/// Encapsulates SMTP Response Codes
	/// </summary>
	public enum SmtpResponseCode : int
	{
		/// <summary>
		/// No result code
		/// </summary>
		None = 0,
		///<summary>
		/// System status, or system help reply
		///</summary>
		SystemStatus = 211,
		///<summary>
		/// Help message
		///</summary>
		HelpMessage = 214,
		///<summary>
		/// &lt;domain&gt; Service ready
		///</summary>
		ServiceReady = 220,
		///<summary>
		/// &lt;domain&gt; Service closing transmission channel
		///</summary>
		ServiceClosing = 221,
		///<summary>
		/// Requested mail action okay, completed
		///</summary>
		Completed = 250,
		///<summary>
		/// User not local; will forward to &lt;forward-path&gt;
		///</summary>
		UserNotLocalForward = 251,
		///<summary>
		/// Cannot VRFY user; try RCPT to attempt delivery (or try finger)
		///</summary>
		CannotVerify = 252,
		///<summary>
		/// Start mail input; end with &lt;CRLF&gt;.&lt;CRLF&gt;
		///</summary>
		StartMailInput = 354,
		///<summary>
		/// &lt;domain&gt; Service not available, closing transmission channel
		///</summary>
		ServiceNotAvailable = 421,
		///<summary>
		/// Requested mail action not taken: mailbox unavailable
		///</summary>
		MailboxUnavailableMail = 450,
		///<summary>
		/// Requested action aborted: local error in processing
		///</summary>
		ErrorInProcessing = 451,
		///<summary>
		/// Requested action not taken: insufficient system storage
		///</summary>
		InsufficientSystemStorage = 452,
		///<summary>
		/// Syntax error, command unrecognized
		///</summary>
		CommandUnrecognized = 500,
		///<summary>
		/// Syntax error in parameters or arguments
		///</summary>
		SyntaxError = 501,
		///<summary>
		/// Command not implemented
		///</summary>
		NotImplemented = 502,
		///<summary>
		/// Bad sequence of commands
		///</summary>
		BadSequence = 503,
		///<summary>
		/// Command parameter not implemented
		///</summary>
		ParameterNotImplemented = 504,
		///<summary>
		/// Requested action not taken: mailbox unavailable
		///</summary>
		MailboxUnavailable = 550,
		///<summary>
		/// User not local; please try &lt;forward-path&gt;
		///</summary>
		UserNotLocalTry = 551,
		///<summary>
		/// Requested mail action aborted: exceeded storage allocation
		///</summary>
		ExceededStorageAllocation = 552,
		///<summary>
		/// Requested action not taken: mailbox name not allowed
		///</summary>
		MailboxNameNotAllowed = 553,
		///<summary>
		/// Transaction failed
		///</summary>
		TransactionFailed = 554
	}
}
