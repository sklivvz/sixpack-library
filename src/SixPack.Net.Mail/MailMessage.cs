// MailMessage.cs 
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
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net.Configuration;
using System.Web;
using System.Web.Configuration;
using ADODB;
using CDO;
using SixPack.Diagnostics;
using Stream=System.IO.Stream;

namespace SixPack.Net.Mail
{
	/// <summary>
	/// Façade for CDO.Message.
	/// </summary>
	/// <example>
	/// // Create new order
	/// Order o = new Order(UserCart,PageUser,0,Language);
	/// 
	/// // Define basic email properties
	/// MailMessage mm = new MailMessage();
	/// mm.Subject="test";
	/// mm.From="marco.cecconi@fullsix.com";
	/// mm.To="tas@fullsix.com";
	/// mm.SMTPServer="beinteractive04";
	/// mm.SMTPPort=25;
	/// mm.Subject="New Mail Message XML XSL class test email :-P";
	/// 
	/// // Let's use an xml/xsl transformation - not compulsory
	/// 
	/// // XSL
	/// StreamReader sr_stylesheet = new StreamReader(Server.MapPath("xsl/EN/MailOrder.xsl"));
	/// XPathNavigator xpn_stylesheet = new XPathDocument(sr_stylesheet).CreateNavigator();
	/// 
	/// // XML
	/// XmlTextReader xtr_content = new XmlTextReader(new StringReader(o.ToXMLDocument()));
	/// XPathDocument xpd_content = new XPathDocument(xtr_content);			
	/// 
	/// // Output of the process
	/// MemoryStream ms_output = new MemoryStream();
	/// XmlTextWriter xw_output = new XmlTextWriter(ms_output,System.Text.Encoding.UTF8);
	/// 
	/// // Do the transformation
	/// XslTransform xslt = new XslTransform();
	/// xslt.Load(xpn_stylesheet,null,null);
	/// xslt.Transform(xpd_content, null, xw_output, null);
	/// 
	/// // Create the email body
	/// ms_output.Position=0;
	/// mm.CreateMHTMLBody(ms_output,MHTMLFlags.SuppressNone);
	/// 
	/// // Close the streams
	/// xtr_content.Close();
	/// sr_stylesheet.Close();
	/// xw_output.Close();		
	/// 	
	/// // Create a call back image in the message (the HTMLBody has "[$TAG]" somewhere...)
	/// Hashtable foo = new Hashtable();
	/// foo["TAG"] = @"<img src="http://www.fullsix.com/User-Open-Mail.aspx"/>";
	/// mm.ApplyTemplate(foo);
	/// 
	/// // Add an attachment
	/// mm.AttachFile(new Uri(@"http://www.downloadcoolspyware.com/fullsix-user-tracker.zip"));
	/// 
	/// // Send the message
	/// mm.Send();
	/// </example>
	public class MailMessage
	{
		#region Constants

		private const int CDOSENDUSING = 2;
		private const string CDOSENDUSINGMETHOD = @"http://schemas.microsoft.com/cdo/configuration/sendusing";
		private const string CDOSMTPSERVER = @"http://schemas.microsoft.com/cdo/configuration/smtpserver";
		private const string CDOSMTPSERVERPORT = @"http://schemas.microsoft.com/cdo/configuration/smtpserverport";

		#endregion

		private readonly Message msg;
		private string smtpPort;
		private string smtpServer;
		private string templateTagFormat = "[${0}]";

		#region Properties

		/// <summary>
		/// Gets or sets the From address.
		/// </summary>
		/// <value>From.</value>
		public string From
		{
			get { return msg.From; }
			set { msg.From = value; }
		}

		/// <summary>
		/// Gets or sets the subject.
		/// </summary>
		/// <value>The subject.</value>
		public string Subject
		{
			get { return msg.Subject; }
			set { msg.Subject = value; }
		}

		/// <summary>
		/// Gets or sets the to address.
		/// </summary>
		/// <value>To.</value>
		public string To
		{
			get { return msg.To; }
			set { msg.To = value; }
		}

		/// <summary>
		/// Gets or sets the CC address.
		/// </summary>
		/// <value>The CC.</value>
		public string CC
		{
			get { return msg.CC; }
			set { msg.CC = value; }
		}

		/// <summary>
		/// Gets or sets the BCC address.
		/// </summary>
		/// <value>The BCC.</value>
		public string BCC
		{
			get { return msg.BCC; }
			set { msg.BCC = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to auto generate the text body.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if you want the class to auto generate the text body; otherwise, <c>false</c>.
		/// </value>
		public bool AutoGenerateTextBody
		{
			get { return msg.AutoGenerateTextBody; }
			set { msg.AutoGenerateTextBody = value; }
		}

		/// <summary>
		/// Gets or sets the SMTP server.
		/// </summary>
		/// <value>The SMTP server.</value>
		public string SmtpServer
		{
			get { return smtpServer; }
			set { smtpServer = value; }
		}

		/// <summary>
		/// Gets or sets the SMTP port.
		/// </summary>
		/// <value>The SMTP port.</value>
		public int SmtpPort
		{
			get { return int.Parse(smtpPort, CultureInfo.InvariantCulture); }
			set { smtpPort = value.ToString(CultureInfo.InvariantCulture); }
		}

		/// <summary>
		/// Gets or sets the template tag format.
		/// </summary>
		/// <value>The template tag format.</value>
		public string TemplateTagFormat
		{
			get { return templateTagFormat; }
			set { templateTagFormat = value; }
		}

		/// <summary>
		/// Gets or sets the HTML body.
		/// </summary>
		/// <value>The HTML body.</value>
		public string HtmlBody
		{
			get { return msg.HTMLBody; }
			set { msg.HTMLBody = value; }
		}

		/// <summary>
		/// Gets or sets the text body.
		/// </summary>
		/// <value>The text body.</value>
		public string TextBody
		{
			get { return msg.TextBody; }
			set { msg.TextBody = value; }
		}

		// Added by rfiel 20071022
		/// <summary>
		/// Gets or sets the charset of the message.
		/// </summary>
		/// <value>The char set.</value>
		public string CharSet
		{
			get { return msg.HTMLBodyPart.Charset; }
			set { msg.HTMLBodyPart.Charset = value; }
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="MailMessage"/> class.
		/// </summary>
		public MailMessage()
		{
			msg = new Message();
		}

		/// <summary>
		/// Creates the MHTML body of the message by spidering a URL.
		/// </summary>
		/// <param name="uri">The URI of the source page.</param>
		/// <param name="suppressionOptions">Specifies the kind of spidering.</param>
		public void CreateMhtmlBody(Uri uri, MhtmlSuppress suppressionOptions)
		{
			if (uri == null)
				throw new ArgumentNullException("uri");
			msg.CreateMHTMLBody(uri.ToString(), (CdoMHTMLFlags)(int)suppressionOptions, string.Empty, string.Empty);
		}

		/// <summary>
		/// Creates the MHTML body of the message by spidering a page passed via Stream.
		/// </summary>
		/// <param name="bodyStream">The Stream containing the page.</param>
		/// <param name="suppressionOptions">Specifies the kind of spidering.</param>
		public void CreateMhtmlBody(Stream bodyStream, MhtmlSuppress suppressionOptions)
		{
			string tempfile = dumpStreamToTempFile(bodyStream, ".html");
			try
			{
				CreateMhtmlBody(new Uri(String.Format(CultureInfo.InvariantCulture, "file://{0}", tempfile)), suppressionOptions);
			}
			catch (ApplicationException exception)
			{
				Log.Instance.HandleException(exception);
			}
			finally
			{
				File.Delete(tempfile);
			}
		}

		private static string dumpStreamToTempFile(Stream stream, string extension)
		{
			string tempfile = Path.GetTempFileName();
			if (!string.IsNullOrEmpty(extension))
			{
				File.Move(tempfile, tempfile + extension);
				tempfile += extension;
			}
			Stream outputStream = File.OpenWrite(tempfile);
			//Log.Instance.Add(String.Format("Sending new order email (Dumping stream to tempfile (file opened) ({0})) ", tempfile),LogLevel.Info);
			BufferedStream inputBuffer = new BufferedStream(stream);
			BufferedStream outputBuffer = new BufferedStream(outputStream);
			byte[] buf = new byte[4096];
			int read;
			while ((read = inputBuffer.Read(buf, 0, buf.Length)) > 0)
				outputBuffer.Write(buf, 0, read);

			//Log.Instance.Add(String.Format("Sending new order email (Dumping stream to tempfile (file written) ({0})) ", tempfile),LogLevel.Info);
			outputBuffer.Flush();
			//Log.Instance.Add(String.Format("Sending new order email (Dumping stream to tempfile (file flushed) ({0})) ", tempfile),LogLevel.Info);
			outputBuffer.Close();
			inputBuffer.Close();
			//Log.Instance.Add(String.Format("Sending new order email (Dumping stream to tempfile (file closed) ({0})) ", tempfile),LogLevel.Info);
			return tempfile;
		}

		private static string dumpStreamToTempFile(Stream stream)
		{
			return dumpStreamToTempFile(stream, null);
		}

		/// <summary>
		/// Attaches a file to the message.
		/// </summary>
		/// <param name="attachStream">The Stream containing the file to be attached.</param>
		/// <param name="fileName">The destination filename</param>
		public void AttachFile(Stream attachStream, string fileName)
		{
			if (attachStream == null)
				throw new ArgumentNullException("attachStream");

			string tempfile = dumpStreamToTempFile(attachStream);
			if (!string.IsNullOrEmpty(fileName))
			{
				string fullPath = Path.Combine(Path.GetDirectoryName(tempfile), fileName);

				// If the destination file exists, delete it first
				if (File.Exists(fullPath))
					File.Delete(fullPath);
				File.Move(tempfile, fullPath);
				tempfile = fullPath;
			}
			try
			{
				AttachFile(new Uri(String.Format(CultureInfo.InvariantCulture, "file://{0}", tempfile)));
			}
			finally
			{
				File.Delete(tempfile);
			}
		}

		/// <summary>
		/// Attaches a file to the message.
		/// </summary>
		/// <param name="uri">The URI pointing to the file to be attached.</param>
		public void AttachFile(Uri uri)
		{
			if (uri == null)
				throw new ArgumentNullException("uri");
			msg.AddAttachment(uri.ToString(), string.Empty, string.Empty);
		}

		/// <summary>
		/// Applies the contents to a template loaded in the body of the message.
		/// </summary>
		/// <param name="tagData"></param>
		public void ApplyTemplate(IDictionary tagData)
		{
			if (tagData == null)
				throw new ArgumentNullException("tagData");
			string bar = msg.TextBody;
			string baz = msg.HTMLBody;

			foreach (string foo in tagData.Keys)
			{
				string tag = String.Format(CultureInfo.InvariantCulture, templateTagFormat, foo);
				bar = bar.Replace(tag, tagData[foo].ToString());
				baz = baz.Replace(tag, tagData[foo].ToString());
			}
			msg.HTMLBody = baz;
			msg.TextBody = bar;
		}

		/// <summary>
		/// Sends the message.
		/// </summary>
		public void Send()
		{
			// Configure the mail server
			Configuration iConfg;
			Fields oFields;
			Field oField;
			iConfg = msg.Configuration;
			oFields = iConfg.Fields;
			oField = oFields[CDOSENDUSINGMETHOD];
			oField.Value = CDOSENDUSING;

            
            //These can be empty so let´s check and get information from web.config, if needed
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

            //We have to guarantee that at least it's possible to read from the properties or get information from system.net settings
            if (
                (!String.IsNullOrEmpty(smtpServer) && !String.IsNullOrEmpty(smtpPort))
                ||
                (settings != null && settings.Smtp != null && settings.Smtp.Network != null && settings.Smtp.Network.Host != null)
            )
            {
                oField = oFields[CDOSMTPSERVER];
                oField.Value = smtpServer ?? settings.Smtp.Network.Host;
                oField = oFields[CDOSMTPSERVERPORT];
                oField.Value = smtpPort ?? settings.Smtp.Network.Port.ToString(CultureInfo.InvariantCulture);

                oFields.Update();
                msg.Configuration = iConfg;

                Log.Instance.AddFormat(
                    "[MailMessage] Sending from: \"{0}\", to \"{1}\", CC \"{2}\", BCC \"{3}\" using server \"{4}:{5}\" with charset: \"{6}\"",
                    new object[]
                        {
                            msg.From,
                            msg.To,
                            msg.CC,
                            msg.BCC,
                            oFields[CDOSMTPSERVER].Value,
                            oFields[CDOSMTPSERVERPORT].Value,
                            CharSet
                        },
                    LogLevel.Debug);

                // Send the message
                msg.Send();
            }
            else
            {
                throw new ArgumentException(
                    "Missing SMTP configuration.\n Check if SMTP properties are set or have a valid System.Net configuration in web.config");
            }
		}
	}

	/// <summary>
	/// These flags specify how the spidering of a page should be done.
	/// </summary>
	[Flags]
	public enum MhtmlSuppress
	{
		/// <summary>
		/// Download all resources from the Web page (default).
		/// </summary>
		None = 0,
		/// <summary>
		/// Do not download resources from &lt;IMG [DYN]SRC=...&gt; or &lt;BODY BACKGROUND=...&gt; tags.
		/// </summary>
		Images = 1,
		/// <summary>
		/// Do not download resources from &lt;BGSOUND SRC=...&gt; tags.
		/// </summary>
		BackgroundSounds = 2,
		/// <summary>
		/// Do not download resources from &lt;FRAME SRC=...&gt; tags.
		/// </summary>
		Frames = 4,
		/// <summary>
		/// Do not download resources from &lt;OBJECT CODEBASE=...&gt; tags.
		/// </summary>
		Objects = 8,
		/// <summary>
		/// Do not download resources from &lt;LINK HREF=...&gt; tags.
		/// </summary>
		StyleSheets = 16,
		/// <summary>
		/// Do not download any resources referred to from within the page.
		/// </summary>
		All = StyleSheets | Objects | Frames | BackgroundSounds | Images
	}
}
