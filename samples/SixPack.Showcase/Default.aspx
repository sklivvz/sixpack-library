<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Default.aspx.cs" Inherits="SixPack.Showcase._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Fullsix Showcase</title>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<h1>
				Example 1</h1>
			<table>
				<tr>
					<td>
						<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
					</td>
					<td>
						<asp:Button ID="Button1" runat="server" Text="Run" OnClick="Button1_Click" />
					</td>
				</tr>
			</table>
			<asp:Label ID="Label1" runat="server" Text="Insert Data and Click &ldquo;Run&rdquo;"></asp:Label>
			<hr />
			<h1>
				Example 2</h1>
			<table>
				<tr>
					<td>
						<asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" Height="200px" Width="350px">Nulla faucibus mi non ligula. In hac habitasse platea dictumst. Proin ullamcorper. Aliquam varius feugiat lacus. Vivamus consectetuer, est at tempus scelerisque, felis lectus convallis dui, in gravida magna mauris in neque. Mauris egestas tempus tortor. Morbi mollis massa. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos hymenaeos. Aenean dapibus, lectus id accumsan accumsan, velit nulla porttitor ante, a scelerisque odio justo a purus. Duis vehicula. Maecenas congue. Sed pretium sem nec magna. Nullam viverra massa nec pede. Etiam id urna ac dolor venenatis porttitor. Maecenas semper dui ut elit. Etiam faucibus ipsum ut massa. Ut ipsum. Proin malesuada eros quis lectus. Maecenas pellentesque accumsan tellus. Nunc in eros. Nam odio ipsum, cursus facilisis, tincidunt eget, cursus eget, lorem. Pellentesque risus. Maecenas rutrum, libero vel varius ornare, mi dolor bibendum ante, a placerat purus nibh sit amet ligula. In nulla mi, varius ut, gravida sit amet, mattis id, orci. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos hymenaeos. Vivamus luctus lacus vel nunc. Nulla semper gravida ligula. Morbi nunc. Quisque in purus eget purus volutpat commodo. Etiam elit augue, blandit fermentum, sagittis porta, convallis vel, augue. Aenean ac pede. Maecenas dignissim, risus non hendrerit semper, ante leo mollis purus, vel convallis arcu nulla suscipit pede. Curabitur hendrerit. Duis pulvinar lacus eget urna. Nunc lacus. Vivamus at mi at ligula egestas scelerisque. </asp:TextBox>
					</td>
					<td rowspan="4">
						<asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Run" />
					</td>
				</tr>
				<tr>
					<td>
						<asp:TextBox ID="TextBox3" runat="server">150</asp:TextBox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:TextBox ID="TextBox4" runat="server">&hellip;</asp:TextBox>
					</td>
				</tr>
				<tr>
					<td>
						<asp:CheckBox ID="CheckBox1" runat="server" Checked="true" />
					</td>
				</tr>
			</table>
			<asp:Label ID="Label2" runat="server" Text="Insert Data and Click &ldquo;Run&rdquo;"></asp:Label>
			<hr />
			<h1>
				Example 3</h1>
			<asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Run" /><br />
			<asp:Label ID="Label3" runat="server" Text="Click &ldquo;Run&rdquo;"></asp:Label>
		</div>
	</form>
</body>
</html>
