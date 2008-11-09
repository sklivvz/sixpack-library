// MailValidationTests.cs 
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

using SixPack.Net.Mail;
using SixPack.Text;
using MbUnit.Framework;

namespace SixPack.UnitTests
{
	[TestFixture]
	public class MailValidationTests
	{
		[RowTest]
		[Row(@"abigail@example.com", true)]
		[Row(@"abigail@example.com ", true)]
		[Row(@" abigail@example.com", true)]
		[Row(@"abigail @example.com", true)]
		[Row(@"*@example.net", true)]
		[Row(@"""\""""@foo.bar", true)]
		[Row(@"fred&barny@example.com", true)]
		[Row(@"---@example.com", true)]
		[Row(@"foo-bar@example.net", true)]
		[Row(@"""127.0.0.1""@[127.0.0.1]", true)]
		[Row(@"Abigail <abigail@example.com>", true)]
		[Row(@"Abigail<abigail@example.com>", true)]
		[Row(@"Abigail<@a,@b,@c:abigail@example.com>", true)]
		[Row(@"""This is a phrase""<abigail@example.com>", true)]
		[Row(@"""Abigail ""<abigail@example.com>", true)]
		[Row(@"""Joe & J. Harvey"" <example @Org>", true)]
		[Row(@"Abigail <abigail @ example.com>", true)]
		[Row(@"Abigail made this <  abigail   @   example  .    com    >", true)]
		//[Row(@"Abigail(the bitch)@example.com", true)]
		[Row(@"Abigail <abigail @ example . (bar) com >", false)]
		//[Row(@"Abigail < (one)  abigail (two) @(three)example . (bar) com (quz) >", true)]
		//[Row(@"Abigail (foo) (((baz)(nested) (comment)) ! ) < (one)  abigail (two) @(three)example . (bar) com (quz) >", true)]
		//[Row(@"Abigail <abigail(fo\(o)@example.com>", true)]
		//[Row(@"Abigail <abigail(fo\)o)@example.com>", true)]
		[Row(@"(foo) abigail@example.com", false)]
		[Row(@"abigail@example.com (foo)", false)]
		[Row(@"""Abi\""gail"" <abigail@example.com>", true)]
		[Row(@"abigail@[example.com]", true)]
		[Row(@"abigail@[exa\[ple.com]", true)]
		[Row(@"abigail@[exa\]ple.com]", true)]
		[Row(@""":sysmail""@  Some-Group. Some-Org", true)]
		//[Row(@"Muhammed.(I am  the greatest) Ali @(the)Vegas.WBA", true)]
		[Row(@"mailbox.sub1.sub2@this-domain", true)]
		[Row(@"sub-net.mailbox@sub-domain.domain", true)]
		[Row(@"name:;", true)]
		[Row(@"':;", true)]
		[Row(@"name:   ;", true)]
		[Row(@"Alfred Neuman <Neuman@BBN-TENEXA>", true)]
		[Row(@"Neuman@BBN-TENEXA", true)]
		[Row(@"""George, Ted"" <Shared@Group.Arpanet>", true)]
		[Row(@"Wilt . (the  Stilt) Chamberlain@NBA.US", false)]
		[Row(@"Cruisers:  Port@Portugal, Jones@SEA;", true)]
		[Row(@"$@[]", true)]
		//[Row(@"*()@[]", true)]
		//[Row(@"""quoted ( brackets"" ( a comment )@example.com", true)]
		[Row(@"Just a string", false)]
		[Row(@"string", false)]
		[Row(@"(comment)", false)]
		[Row(@"()@example.com", false)]
		//[Row(@"fred(&)barny@example.com", false)]
		//[Row(@"fred\ barny@example.com", false)]
		//[Row(@"Abigail <abi gail @ example.com>", false)]
		[Row(@"Abigail <abigail(fo(o)@example.com>", false)]
		[Row(@"Abigail <abigail(fo)o)@example.com>", false)]
		//[Row(@"""Abi""gail"" <abigail@example.com>", false)]
		//[Row(@"abigail@[exa]ple.com]", false)]
		[Row(@"abigail@[exa[ple.com]", false)]
		//[Row(@"abigail@[exaple].com]", false)]
		[Row(@"abigail@", false)]
		[Row(@"@example.com", false)]
		//[Row(@"phrase: abigail@example.com abigail@example.com ;", false)]
		//[Row(@"invalid£char@example.com", false)]
		public void TestRfc2822Validation(string email, bool result)
		{
			Assert.AreEqual(result, EmailAddress.IsValid(email, MailSyntaxValidationMode.Rfc2822));
		}

		[RowTest]
		[Row(@"abigail@example.com", true)]
		[Row(@"abigail@example.com ", false)]
		[Row(@" abigail@example.com", false)]
		[Row(@"abigail @example.com", false)]
		[Row(@"*@example.net", false)]
		[Row(@"""\""""@foo.bar", false)]
		[Row(@"fred&barny@example.com", true)]
		[Row(@"---@example.com", true)]
		[Row(@"foo-bar@example.net", true)]
		[Row(@"""127.0.0.1""@[127.0.0.1]", false)]
		[Row(@"Abigail <abigail@example.com>", false)]
		[Row(@"Abigail<abigail@example.com>", false)]
		[Row(@"Abigail<@a,@b,@c:abigail@example.com>", false)]
		[Row(@"""This is a phrase""<abigail@example.com>", false)]
		[Row(@"""Abigail ""<abigail@example.com>", false)]
		[Row(@"""Joe & J. Harvey"" <example @Org>", false)]
		[Row(@"Abigail <abigail @ example.com>", false)]
		[Row(@"Abigail made this <  abigail   @   example  .    com    >", false)]
		[Row(@"Abigail <abigail @ example . (bar) com >", false)]
		[Row(@"(foo) abigail@example.com", false)]
		[Row(@"abigail@example.com (foo)", false)]
		[Row(@"""Abi\""gail"" <abigail@example.com>", false)]
		[Row(@"abigail@[example.com]", false)]
		[Row(@"abigail@[exa\[ple.com]", false)]
		[Row(@"abigail@[exa\]ple.com]", false)]
		[Row(@""":sysmail""@  Some-Group. Some-Org", false)]
		[Row(@"mailbox.sub1.sub2@this-domain", false)]
		[Row(@"sub-net.mailbox@sub-domain.com", true)]
		[Row(@"name:;", false)]
		[Row(@"':;", false)]
		[Row(@"name:   ;", false)]
		[Row(@"Alfred Neuman <Neuman@BBN-TENEXA>", false)]
		[Row(@"Neuman@BBN-TENEXA", false)]
		[Row(@"""George, Ted"" <Shared@Group.Arpanet>", false)]
		[Row(@"Wilt . (the  Stilt) Chamberlain@NBA.US", false)]
		[Row(@"Cruisers:  Port@Portugal, Jones@SEA;", false)]
		[Row(@"$@[]", false)]
		[Row(@"Just a string", false)]
		[Row(@"string", false)]
		[Row(@"(comment)", false)]
		[Row(@"()@example.com", false)]
		[Row(@"fred(&)barny@example.com", false)]
		[Row(@"fred\ barny@example.com", false)]
		[Row(@"Abigail <abi gail @ example.com>", false)]
		[Row(@"Abigail <abigail(fo(o)@example.com>", false)]
		[Row(@"Abigail <abigail(fo)o)@example.com>", false)]
		[Row(@"""Abi""gail"" <abigail@example.com>", false)]
		[Row(@"abigail@[exa]ple.com]", false)]
		[Row(@"abigail@[exa[ple.com]", false)]
		[Row(@"abigail@[exaple].com]", false)]
		[Row(@"abigail@", false)]
		[Row(@"@example.com", false)]
		[Row(@"phrase: abigail@example.com abigail@example.com ;", false)]
		[Row(@"invalid£char@example.com", false)]
		public void TestSimpleValidation(string email, bool result)
		{
			Assert.AreEqual(result, EmailAddress.IsValid(email, MailSyntaxValidationMode.Simple));
		}

		//[RowTest]
		//[Row("info@fullsix.pt", true)]
		//[Row("some-non-existant-user;;;@fullsix.com", true)]
		//[Row("info@some-non-existant-domain;;;.com", false)]
		//[Row("some-non-existant-user;;;@some-non-existant-domain;;;.com", false)]
		//public void TestDnsLookupSemantics(string email, bool result)
		//{
		//    Assert.AreEqual(result, EmailSemantics.IsValid(email, MailSemanticValidationMode.DnsLookup));
		//}

		//[RowTest]
		//[Row("info@fullsix.com", true)]
		//[Row("info@fullsix.pt", true)]
		//[Row("some-non-existant-user;;;@fullsix.com", true)]
		//[Row("info@some-non-existant-domain;;;.com", false)]
		//[Row("some-non-existant-user;;;@some-non-existant-domain;;;.com", false)]
		//public void TestServerLookupSemantics(string email, bool result)
		//{
		//    Assert.AreEqual(result, EmailSemantics.IsValid(email, MailSemanticValidationMode.ServerLookup));
		//}

		//[RowTest]
		//[Row("info@fullsix.com", false)] // Server does not support VRFY
		//[Row("info@fullsix.pt", true)]
		//[Row("some-non-existant-user4783948397243987@fullsix.pt", true)]
		//// Server will accept user even if user does not exists.
		//[Row("info@some-non-existant-domain;;;.com", false)]
		//[Row("some-non-existant-user4783948397243987@some-non-existant-domain;;;.com", false)]
		//public void TestUserLookupSemantics(string email, bool result)
		//{
		//    Assert.AreEqual(result, EmailSemantics.IsValid(email, MailSemanticValidationMode.UserLookup));
		//}
	}
}
