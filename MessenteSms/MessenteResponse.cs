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

	This file is from project https://github.com/NortalLTD/MessenteSms, Nortal.Utilities.MessenteSms, file 'MessenteResponse.cs'.
*/

using System;
using System.Net;

namespace Nortal.Utilities.MessenteSms
{
	public sealed class MessenteResponse
	{
		//constructors:
		public MessenteResponse(String rawResponse) { this.RawResponse = rawResponse; }
		private MessenteResponse(Boolean isSuppressed) { this.IsSuppressed = isSuppressed; }

		//content:
		public String RawResponse { get; private set; }
		public Boolean IsSuppressed { get; private set; }

		#region static response object creation
		private static MessenteResponse SuppressedResponseInstance = new MessenteResponse(isSuppressed: true);
		public static MessenteResponse SuppressedResponse { get { return SuppressedResponseInstance; } }
		#endregion

		private static class MessagePrefixes
		{
			internal const String OK = "OK ";
			internal const String Error = "ERROR ";
			internal const String Failed = "FAILED ";
		}

		public Boolean IsSuccess
		{
			get
			{
				if (IsSuppressed || RawResponse == null || RawResponse.StartsWith(MessagePrefixes.Error) || RawResponse.StartsWith(MessagePrefixes.Failed)) { return false; }
				return true;
			}
		}

		public String SuccessResult
		{
			get
			{
				if (!IsSuccess) { return null; }
				return GetContentAfterPrefix(MessagePrefixes.OK) ?? this.RawResponse;
			}
		}

		public ResponseFailure? FailureReason
		{
			get
			{
				if (IsSuccess) { return null; }
				if (this.IsSuppressed) { return ResponseFailure.Suppressed; }

				String returnCodeAsString = GetContentAfterPrefix(MessagePrefixes.Error) ?? GetContentAfterPrefix(MessagePrefixes.Failed);
				int code;
				if (Int32.TryParse(returnCodeAsString, out code)
					&& Enum.IsDefined(typeof(ResponseFailure), code))
				{
					return (ResponseFailure)code;
				}
				return ResponseFailure.Other;
			}
		}

		private String GetContentAfterPrefix(String prefix)
		{
			if (this.RawResponse != null && this.RawResponse.StartsWith(prefix))
			{
				return this.RawResponse.Substring(prefix.Length);
			}
			return null;
		}

		public override string ToString()
		{
			return this.RawResponse;
		}
	}
}
