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

	This file is from project https://github.com/NortalLTD/MessenteSms, Nortal.Utilities.MessenteSms, file 'DeliveryReportStatus.cs'.
*/

namespace Nortal.Utilities.MessenteSms
{
	/// <summary>
	/// SMS statuses as reported by Messente DLR requests.
	/// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32", Justification="Cannot foresee set of values ever needing Int32.")]
	public enum DeliveryReportStatus : byte
	{
		/// <summary>
		/// Delivery status could not be determined.
		/// </summary>
		Unspecified = 0,
		/// <summary>
		/// SMS was sent but status about the delivery is not present yet
		/// </summary>
		Sent,
		/// <summary>
		/// SMS was successfully delivered. This is final status.
		/// </summary>
		Delivered,
		/// <summary>
		/// SMS could not be delivered. This is final status.
		/// </summary>
		Failed,
	}
}
