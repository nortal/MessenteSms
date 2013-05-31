<%--
	Copyright 2013 Imre Pühvel, AS Nortal
	
	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at

		http://www.apache.org/licenses/LICENSE-2.0

	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.

	This file is from project https://github.com/NortalLTD/MessenteSms, file 'Default.aspx'.
--%>

<%@ Page Language="C#" CodeBehind="Default.aspx.cs" Inherits="ExampleApplication.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<h1>Example of a SMS-sending application</h1>
		<hr />

		<asp:Button ID="BalanceCheckingButton" runat="server" Text="Check your account balance" OnClick="BalanceCheckingButton_Click" />
		<div>
			Your Messente account balance:
			<asp:Literal ID="AccountBalance" runat="server" />
		</div>
		<hr />
		<h2>Send SMS</h2>
		Phone number:
		<asp:TextBox ID="toPhone" runat="server" Text="+37256662995" /><br />
		Message:<br />
		<asp:TextBox ID="messageContent" runat="server" TextMode="MultiLine" Columns="40" Rows="4"
			Text="Message supports unicode and long messages." /><br />
		<asp:Button ID="MessageSendingButton" runat="server" Text="Send message" OnClick="MessageSendingButton_Click" /><br />
		<asp:Literal ID="MessageSendingResponse" runat="server" />
	</form>
</body>
</html>
