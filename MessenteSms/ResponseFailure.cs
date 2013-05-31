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

	This file is from project https://github.com/NortalLTD/MessenteSms, Nortal.Utilities.MessenteSms, file 'ResponseFailure.cs'.
*/

namespace Nortal.Utilities.MessenteSms
{
	public enum ResponseFailure
	{
		Other = 0,
		//error codes declared by Messente API:
		AccessRestricted = 101, //Access is restricted, wrong credentials. Check username and password values. 
		ParametersWrongOrMissing = 102, //Check that all required parameters are present.
		InvalidIPAddress = 103, // The IP address you made the request from, is not in the API settings whitelist. 
		CountryNotFound = 104, // Country was not found. 
		CountryNotSupported = 105, //This country is not supported 
		InvalidFormatProvided = 106, //Invalid format provided. Only json or xml is allowed.
		InvalidSenderName = 111, //Invalid Sender parameter "from" is invalid. You have not activated this Sender Name from Messente.com 
		ServerFailure = 209, //try again after a few seconds or try api3.messente.com backup server.

		//non-messente error codes:
		Suppressed = 2,
	}
}
