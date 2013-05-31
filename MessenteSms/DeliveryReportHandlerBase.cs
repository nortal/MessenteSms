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

	This file is from project https://github.com/NortalLTD/MessenteSms, Nortal.Utilities.MessenteSms, file 'DeliveryReportHandlerBase.cs'.
*/

using System;
using System.Web;

namespace Nortal.Utilities.MessenteSms
{
	public abstract class DeliveryReportHandlerBase : IHttpHandler
	{
		#region IHttpHandler Members

		public virtual Boolean IsReusable { get { return true; } }

		/// <summary>
		/// Captures DLR request
		/// </summary>
		/// <param name="context"></param>
		public void ProcessRequest(HttpContext context)
		{
			if (context == null) { throw new ArgumentNullException("context"); }

			var report = DeliveryReport.CreateFrom(context.Request);
			ProcessReport(context, report);
		}
		#endregion

		protected abstract void ProcessReport(HttpContext context, DeliveryReport report);
	}
}
