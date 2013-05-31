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

	This file is from project https://github.com/NortalLTD/MessenteSms, Nortal.Utilities.MessenteSms, file 'MessenteAgent.cs'.
*/

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Nortal.Utilities.MessenteSms
{
	/// <summary>
	/// Entrypoint for client-initiated communication to Messente SMS services.
	/// </summary>
	public class MessenteAgent
	{
		/// <summary>
		/// Initializes Messente communication agent with connection configuration from config-files.
		/// </summary>
		public MessenteAgent()
		{
			InitializeSettings(null, null);
		}

		/// <summary>
		/// Initializes Messente communication agent.
		/// </summary>
		/// <param name="connectionSettings">explicit Messente connectivity parameters</param>
		public MessenteAgent(IMessenteConnectionSettings connectionSettings)
			: this(connectionSettings, null)
		{
			if (connectionSettings == null) { throw new ArgumentNullException("connectionSettings"); }

			InitializeSettings(connectionSettings, defaultSmsSendingOptions: null);
		}

		/// <summary>
		/// Initializes Messente communication agent
		/// </summary>
		/// <param name="connectionSettings">explicit Messente connectivity parameters</param>
		/// <param name="defaultSmsSendingOptions">default sms sending options</param>
		public MessenteAgent(IMessenteConnectionSettings connectionSettings, ISmsSendingOptions defaultSmsSendingOptions)
		{
			if (connectionSettings == null) { throw new ArgumentNullException("connectionSettings"); }
			if (defaultSmsSendingOptions == null) { throw new ArgumentNullException("defaultSmsSendingOptions"); }

			InitializeSettings(connectionSettings, defaultSmsSendingOptions);
		}

		private void InitializeSettings(IMessenteConnectionSettings connectionSettings, ISmsSendingOptions defaultSmsSendingOptions)
		{
			this.ConnectionSettings = connectionSettings ?? MessenteConnectionSettings.Default;
			ValidateConnectionConfiguration();

			this.SmsSendingOptions = defaultSmsSendingOptions ?? new SmsSendingOptions();
		}

		#region configuration
		/// <summary>
		/// 
		/// </summary>
		public IMessenteConnectionSettings ConnectionSettings { get; protected set; }

		private void ValidateConnectionConfiguration()
		{
			var settings = this.ConnectionSettings;
			if (settings.MainUrl == null) { throw new InvalidOperationException("Cannot use SMS service: Main Url not configured."); }
			if (String.IsNullOrEmpty(settings.UserName)) { throw new InvalidOperationException("Cannot use SMS service: user name not configured."); }
			if (String.IsNullOrEmpty(settings.Password)) { throw new InvalidOperationException("Cannot use SMS service: password not configured."); }
			//allowing backup url to be not set.
		}

		public ISmsSendingOptions SmsSendingOptions { get; protected set; }

		#endregion

		private static String PreparePhoneNumber(String phoneNumber)
		{
			//No reference to documentation but it seems Messente only accepts plus and numbers. Clean up some common unsuitable chars:
			phoneNumber = phoneNumber
				.Replace(" ", String.Empty)
				.Replace("(", String.Empty)
				.Replace(")", String.Empty);
			return phoneNumber;
		}

		#region specific API method calls

		/// <summary>
		/// Checks current account and for successful requests returns remaining balance in euros:
		/// <seealso cref="https://messente.com/pages/api#balance"/>
		/// </summary>
		/// <returns></returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Call to external logic.")]
		public MessenteResponse GetAccountBalance()
		{
			StringBuilder postData = new StringBuilder(100);
			AppendAuthenticationParameters(postData);
			return SendRequest("get_balance/", postData.ToString());
		}

		/// <summary>
		/// Fetches pricelist for given account in give country as JSON.
		/// https://messente.com/pages/api#prices
		/// </summary>
		/// <param name="forCountry">2-letter country code</param>
		/// <returns></returns>
		public MessenteResponse GetAccountPrices(String forCountry)
		{
			if (forCountry == null) { throw new ArgumentNullException("forCountry"); }
			if (forCountry.Length != 2) { throw new ArgumentException("Country must be two letter countryname", "forCountry"); }

			StringBuilder postData = new StringBuilder(200);
			AppendAuthenticationParameters(postData);
			AppendPostParameter(postData, "country", forCountry);

			return SendRequest("prices/", postData.ToString());
		}

		/// <summary>
		/// Sends SMS.
		/// https://messente.com/pages/api#sms
		/// </summary>
		/// <param name="toPhoneNumber">reciepient mobile number</param>
		/// <param name="message">SMS text content</param>
		/// <returns>Successful responses contain unique SMS id for deliver reports.</returns>
		public MessenteResponse SendMessage(String toPhoneNumber, String message)
		{
			return SendMessage(toPhoneNumber, message, options: null);
		}

		/// <summary>
		/// Sends SMS.
		/// https://messente.com/pages/api#sms
		/// </summary>
		/// <param name="toPhoneNumber">reciepient mobile number</param>
		/// <param name="message">SMS text content</param>
		/// <param name="options">options to use instead of defaults</param>
		/// <returns>Successful responses contain unique SMS id for deliver reports.</returns>
		public MessenteResponse SendMessage(String toPhoneNumber, String message, ISmsSendingOptions options)
		{
			if (toPhoneNumber == null) { throw new ArgumentNullException("toPhoneNumber"); }
			if (message == null) { throw new ArgumentNullException("message"); }

			options = options ?? this.SmsSendingOptions;
			toPhoneNumber = PreparePhoneNumber(toPhoneNumber);

			StringBuilder postData = new StringBuilder(200);
			AppendAuthenticationParameters(postData);
			AppendPostParameter(postData, "to", toPhoneNumber, urlEncode: true);
			AppendPostParameter(postData, "text", message, urlEncode: true);

			//optional parameters:
			if (options.DeliveryReportUri != null) { AppendPostParameter(postData, "dlr-url", options.DeliveryReportUri.ToString(), urlEncode: true); }
			if (options.Sender != null) { AppendPostParameter(postData, "from", options.Sender, urlEncode: true); }

			if (options.DelayedSendTime != null)
			{
				var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
				var secondsSinceEpoch = Convert.ToInt64((options.DelayedSendTime.Value - epoch).TotalSeconds);
				postData.Append("&time_to_send=").Append(secondsSinceEpoch); // in unix timestamp.
			}

			return SendRequest("send_sms/", postData.ToString());
		}

		/// <summary>
		/// Queries Messente for delivery report by unique SMS id.
		/// </summary>
		/// <param name="smsId"></param>
		/// <returns></returns>
		public MessenteResponse CheckDeliveryStatus(String smsId)
		{
			if (smsId == null) { throw new ArgumentNullException("smsId"); }

			StringBuilder postData = new StringBuilder(200);
			AppendAuthenticationParameters(postData);
			AppendPostParameter(postData, "sms_unique_id", smsId);

			return SendRequest("get_dlr_response/", postData);
		}

		#endregion

		#region low level message-agnostic tools.

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
		protected static void AppendPostParameter(StringBuilder builder, String parameterName, String value, Boolean urlEncode = true)
		{
			if (builder == null) { throw new ArgumentNullException("builder"); }
			if (urlEncode) { value = HttpUtility.UrlEncode(value); }
			builder
				.Append("&" + parameterName + "=")
				.Append(value);
		}

		protected void AppendAuthenticationParameters(StringBuilder builder)
		{
			AppendPostParameter(builder, "username", this.ConnectionSettings.UserName);
			AppendPostParameter(builder, "password", this.ConnectionSettings.Password);
		}

		protected MessenteResponse SendRequest(String apiMethod, StringBuilder rawHttpPostContent)
		{
			String content = rawHttpPostContent != null ? rawHttpPostContent.ToString() : null;
			return SendRequest(apiMethod, content);
		}

		protected MessenteResponse SendRequest(String apiMethod, String rawHttpPostContent)
		{
			Boolean retry = false;
			MessenteResponse result = null;
			//try main server
			try
			{
				result = SendRequestHttpLevel(this.ConnectionSettings.MainUrl, apiMethod, rawHttpPostContent);
				retry = result.FailureReason == ResponseFailure.ServerFailure;
			}
			catch (WebException exception)
			{
				retry = exception.Status == WebExceptionStatus.ProtocolError
					&& (int)((HttpWebResponse)(exception.Response)).StatusCode >= 500;
				if (!retry) { throw; }
			}

			// on some error types we can retry on backup server:
			if (retry && ConnectionSettings.BackupUrl != null)
			{
				result = SendRequestHttpLevel(this.ConnectionSettings.BackupUrl, apiMethod, rawHttpPostContent);
			}
			Debug.Assert(result != null);
			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		private MessenteResponse SendRequestHttpLevel(Uri targetService, String apiMethod, String rawHttpPostContent)
		{
			if (targetService == null) { throw new ArgumentNullException("targetService"); }
			if (apiMethod == null) { throw new ArgumentNullException("apiMethod"); }
			if (rawHttpPostContent == null) { throw new ArgumentNullException("rawHttpPostContent"); }

			if (ConnectionSettings.SuppressSmsSending) { return MessenteResponse.SuppressedResponse; }
			ValidateConnectionConfiguration(); //intentionally after suppression check to allow suppressed environments be without configuration.

			UriBuilder commandUriBuilder = new UriBuilder(targetService);
			commandUriBuilder.Path += apiMethod;

			HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(commandUriBuilder.ToString());
			httpRequest.Headers.Add(HttpRequestHeader.ContentEncoding, "utf-8");

			var encoding = new UTF8Encoding();
			byte[] data = encoding.GetBytes(rawHttpPostContent.ToString());

			httpRequest.Method = "POST";
			httpRequest.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
			httpRequest.ContentLength = data.Length;

			using (var requestStream = httpRequest.GetRequestStream()) { requestStream.Write(data, 0, data.Length); }

			// HTTP request + handle HTTP errors:
			HttpWebResponse httpResponse = null;
			httpResponse = (HttpWebResponse)httpRequest.GetResponse();

			// analyze response and return:
			using (var responseStream = httpResponse.GetResponseStream())
			using (var responseReader = new StreamReader(responseStream, encoding))
			{
				String response = responseReader.ReadToEnd();
				return new MessenteResponse(response);
			}
		}
		#endregion

	}
}
