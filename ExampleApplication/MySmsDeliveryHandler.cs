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

	This file is from project https://github.com/NortalLTD/MessenteSms, ExampleApplication, file 'MySmsDeliveryHandler.cs'.
*/

using System;
using System.Web;
using Nortal.Utilities.MessenteSms;

namespace ExampleApplication
{
	public class MySmsDeliveryHandler : DeliveryReportHandlerBase
	{
		/// <summary>
		/// Messente will call back to this handler once message is delivered (or when failed to deliver).
		/// </summary>
		/// <param name="context"></param>
		/// <param name="report"></param>
		protected override void ProcessReport(HttpContext context, DeliveryReport report)
		{
			var writer = context.Response.Output;
			writer.WriteLine("Reporting on SMS: " + report.SmsUniqueId);
			switch (report.SmsStatus)
			{
				case DeliveryReportStatus.Sent:
					writer.WriteLine("wait..");
					break;
				case DeliveryReportStatus.Delivered:
					writer.WriteLine("Yay, delivered!!");
					break;
				case DeliveryReportStatus.Failed:
					writer.WriteLine("Damn: " + report.Error);
					break;
			}
		}
	}
}