Nortal.Utilities.MessenteSMS
====================================

Messente provides worldwide SMS sending services. This project contains a strongly typed .NET client for communicating with Messente API.
Read more about the service and its capabilities from: https://messente.com/.

Implementation
-----------------
Implemented against API documentation v3.6 (as of 22.02.2013) (https://messente.com/pages/api).
Licenced under Apache Licence v2.0.

Getting started
---------------
* Requires Microsoft.Net Framework 2.0
* Activate your Messente API account on the Messente web page
* Reference Nortal.Utilities.MessenteSMS.dll assembly
* Configure the API access credentials in web.config:

		<Nortal.Utilities.MessenteSms.MessenteConnectionSettings>
			<setting name="UserName" serializeAs="String">
				<value>YOUR_API_USER_HERE</value>
			</setting>
			<setting name="Password" serializeAs="String">
				<value>YOUR_API_PASSWORD_HERE</value>
			</setting>
		</Nortal.Utilities.MessenteSms.MessenteConnectionSettings>

* Send SMS:

		var smsAgent = new MessenteAgent();
		var messenteResponse = smsAgent.SendMessage("+37212345678", "my content");
		if (messenteResponse.IsSuccess) { return "Successfully gave message to messente server, sms id: " + messenteResponse.SuccessResult; }
		else { return "Sending failed:" + messenteResponse.FailureReason; }

Check documentation or included sample application for other features.