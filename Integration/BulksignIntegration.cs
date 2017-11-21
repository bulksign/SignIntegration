using System;
using System.IO;
using Bulksign.Api;
using Microsoft.AspNetCore.Hosting.Internal;

namespace WebsiteIntegration.Integration
{
	public class BulksignIntegration
	{
		private const string BulksignAccountEmail = "aaaa";
		private const string BulksignAccountToken = "bbb";


		public string SendDocumentForSigning(string name, string email, string filePath)
		{
			BulkSignApi api = new BulkSignApi();

			BulksignBundle bundle = new BulksignBundle();
			bundle.Name = "Website Integration Sample";
			bundle.DisableNotifications = true; //no email notifications
			

			BulksignRecipient recipient = new BulksignRecipient();
			recipient.Index = 1;
			recipient.Email = email;
			recipient.Name = name;
			recipient.RecipientType = BulksignApiRecipientType.Signer;

			bundle.Recipients = new BulksignRecipient[1] { recipient };


			BulksignDocument document = new BulksignDocument();
			document.FileName = "test.pdf";
			document.ContentBytes = File.ReadAllBytes(filePath);

			bundle.Documents = new BulksignDocument[1] { document };


			BulksignAuthorization auth = new BulksignAuthorization();
			auth.UserEmail = BulksignAccountEmail;
			auth.UserToken = BulksignAccountToken;


			BulksignResult<BulksignSendBundleResult> result = api.SendBundle(auth, bundle);

			if (result.IsSuccessful)
			{
				return api.GetOpenBundleForSigningUrl(result.Response.AccessCodes[0].AccessCode);
			}

			throw new InvalidOperationException(result.ErrorMessage);
		}


	}
}