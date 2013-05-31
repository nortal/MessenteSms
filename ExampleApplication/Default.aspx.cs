/*
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

	This file is from project https://github.com/NortalLTD/MessenteSms, ExampleApplication, file 'Default.aspx.cs'.
*/

using System;
using Nortal.Utilities.MessenteSms;

namespace ExampleApplication
{
	public partial class Default : System.Web.UI.Page
	{
		protected void BalanceCheckingButton_Click(object sender, EventArgs e)
		{
			var smsAgent = new MessenteAgent();
			var messenteResponse = smsAgent.GetAccountBalance();

			AccountBalance.Text = messenteResponse.IsSuccess
				? messenteResponse.SuccessResult + "EUR"
				: "Request failed: " + messenteResponse.FailureReason;
		}

		protected void MessageSendingButton_Click(object sender, EventArgs e)
		{
			var options = new SmsSendingOptions();
			options.Sender = "+37256662995";
			options.DelayedSendTime = DateTime.UtcNow.AddMinutes(1);
			options.DeliveryReportUri = null;

			var smsAgent = new MessenteAgent();
			var messenteResponse = smsAgent.SendMessage(toPhone.Text, messageContent.Text, options);

			MessageSendingResponse.Text = messenteResponse.IsSuccess
				? "Successfully gave message to messente server, sms id: " + messenteResponse.SuccessResult
				: "Sending failed: " + messenteResponse.FailureReason;
		}
	}
}