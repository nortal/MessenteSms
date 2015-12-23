Nortal.Utilities.MessenteSMS
====================================

Messente provides worldwide SMS sending services. This project contains a strongly typed .NET client for communicating with Messente API.
Read more about the service and its capabilities from: https://messente.com/.

Implementation
-----------------
Implemented against API documentation v3.6 (as of 22.02.2013) (https://messente.com/pages/api).
Requires Microsoft.Net Framework 2.0 and a Messente API account.

Licenced under Apache Licence v2.0.

Getting started
---------------

* Reference Nortal.Utilities.MessenteSMS.dll assembly -or- install package from Nuget using command:

	Install-Package Nortal.Utilities.MessenteSms

* Configure the API access credentials in web.config:


		<configSections>
			<sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
				<section name="Nortal.Utilities.MessenteSms.MessenteConnectionSettings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
			</sectionGroup>
		</configSections>
		<applicationSettings>
			<Nortal.Utilities.MessenteSms.MessenteConnectionSettings>
				<setting name="UserName" serializeAs="String">
					<value>**YOUR_API_USER_HERE**</value>
				</setting>
				<setting name="Password" serializeAs="String">
					<value>**YOUR_API_PASSWORD_HERE**</value>
				</setting>
			</Nortal.Utilities.MessenteSms.MessenteConnectionSettings>
		</applicationSettings>

* Send your SMS messages:

		var smsAgent = new MessenteAgent();
		var result = smsAgent.SendMessage("+37212345678", "my content");
		return result.IsSuccess ? "Yay!" : result.FailureReason;

Check documentation or included sample application for other features like delayed sending, delivery reports, account balance, etc.
