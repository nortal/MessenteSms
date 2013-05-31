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

	This file is from project https://github.com/NortalLTD/MessenteSms, Nortal.Utilities.MessenteSms, file 'DeliveryReport.cs'.
*/

using System;
using System.Web;

namespace Nortal.Utilities.MessenteSms
{
	public class DeliveryReport
	{
		public DeliveryReport(String smsUniqueId, DeliveryReportStatus status, DeliveryReportError error)
		{
			this.SmsUniqueId = smsUniqueId;
			this.SmsStatus = status;
			this.Error = error;
		}

		public String SmsUniqueId { get; protected set; }
		public DeliveryReportStatus SmsStatus { get; protected set; }
		public DeliveryReportError Error { get; protected set; }

		public override string ToString()
		{
			return String.Format("{0}: {1} {2}", SmsUniqueId, SmsStatus, Error);
		}

		protected static class ParameterNames
		{
			public const String SmsUniqueId = @"sms_unique_id";
			public const String Status = @"status";
			public const String ErrorCode = @"err";
		}

		public static DeliveryReport CreateFrom(HttpRequest request)
		{
			if (request == null) { throw new ArgumentNullException("request"); }

			var smsUniqueId = request.Params[ParameterNames.SmsUniqueId];
			var smsStatus = MapDeliveryStatus(request.Params[ParameterNames.Status]);
			var error = MapErrorCode(request.Params[ParameterNames.ErrorCode]);
			return new DeliveryReport(smsUniqueId, smsStatus, error);
		}

		#region Map delivery parts from raw input:
		protected static DeliveryReportError MapErrorCode(string rawError)
		{
			if (String.IsNullOrEmpty(rawError)) { return DeliveryReportError.None; }

			int errorCode;
			if (Int32.TryParse(rawError, out errorCode)) { return (DeliveryReportError)errorCode; }
			throw new NotSupportedException("Delivery error code is in incorrect format: '" + rawError + "'.");
		}

		protected static DeliveryReportStatus MapDeliveryStatus(String rawStatus)
		{
			if (String.IsNullOrEmpty(rawStatus)) { return DeliveryReportStatus.Unspecified; }
			switch (rawStatus.Trim())
			{
				case "DELIVERED": return DeliveryReportStatus.Delivered;
				case "FAILED": return DeliveryReportStatus.Failed;
				case "SENT": return DeliveryReportStatus.Sent;
				default:
					throw new NotSupportedException("Unexpected delivery status value: " + rawStatus + ".");
			}
		}
		#endregion
	}
}
