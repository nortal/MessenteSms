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

	This file is from project https://github.com/NortalLTD/MessenteSms, Nortal.Utilities.MessenteSms, file 'DeliveryReportError.cs'.
*/

namespace Nortal.Utilities.MessenteSms
{
	public enum DeliveryReportError
	{
		None = 0, // No error
		GeneralFatalError = 001, // General fatal error
		SendingExpired = 002, // Sending SMS expired in 6 hours
		InvalidNumber = 003, // Invalid number
		CreditingFailed = 004, // Error crediting Account
		UndeterminedCountry = 005, // Could not determine the destination Country
		SpamBlocked = 006, // Too may identical messages sent to the same number, blocked for 30 minutes
		InvalidSender = 007, // Sender name not allowed
		GeneralTemporaryError = 999, // General temporary error
	}
}
