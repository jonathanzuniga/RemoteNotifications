﻿using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageSender;

namespace MessageSender
{
	class MessageSender
	{
		public const string API_KEY = Keys.GCM_SERVER_API_KEY;

		static void Main(string[] args)
		{
			if (args.Length == 0) {
				// Check for null array.
				Console.WriteLine ("Please write the message to send.\nUsage is: MessageSender.exe [message]");
			} else {
				var jGcmData = new JObject ();
				var jData = new JObject ();

				jData.Add ("message", String.Join (Environment.NewLine, args));
				jGcmData.Add ("to", "/topics/global");
				jGcmData.Add ("data", jData);

				var url = new Uri ("https://gcm-http.googleapis.com/gcm/send");

				try {
					using (var client = new HttpClient ()) {
						client.DefaultRequestHeaders.Accept.Add (
							new MediaTypeWithQualityHeaderValue ("application/json"));

						client.DefaultRequestHeaders.TryAddWithoutValidation (
							"Authorization", "key=" + API_KEY);

						Task.WaitAll (client.PostAsync (url,
							new StringContent (jGcmData.ToString (), Encoding.Default, "application/json"))
							.ContinueWith (response => {
							Console.WriteLine (response);
							Console.WriteLine ("Message sent: check the client device notification tray.");
						})
						);
					}
				} catch (Exception e) {
					Console.WriteLine ("Unable to send GCM message:");
					Console.Error.WriteLine (e.StackTrace);
				}
			}
		}
	}
}
