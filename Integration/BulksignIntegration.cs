using System;
using System.IO;
using Bulksign.Api;

namespace WebsiteIntegration.Integration
{
	public class BulksignIntegration
	{
		private const string BulksignAccountEmail = "aaaa";
		private const string BulksignAccountToken = "bbb";


		public string SendDocumentForSigning(string name, string email, string filePath)
		{
			BulkSignApi api = new BulkSignApi();

			BundleApiModel bundle = new BundleApiModel();
			bundle.Name = "Website Integration Sample";
			bundle.DisableNotifications = true; //no email notifications
			

			RecipientApiModel recipient = new RecipientApiModel();
			recipient.Index = 1;
			recipient.Email = email;
			recipient.Name = name;
			recipient.RecipientType = RecipientTypeApi.Signer;

			bundle.Recipients = new RecipientApiModel[1] { recipient };


			DocumentApiModel document = new DocumentApiModel();
			document.FileName = "test.pdf";
			document.FileContentByteArray = new FileContentByteArray()
			{
				ContentBytes = File.ReadAllBytes(filePath)
			};

			bundle.Documents = new DocumentApiModel[1] { document };


			AuthorizationApiModel auth = new AuthorizationApiModel();
			auth.UserEmail = BulksignAccountEmail;
			auth.UserToken = BulksignAccountToken;


			BulksignResult<SendBundleResultApiModel> result = api.SendBundle(auth, bundle);

			if (result.IsSuccessful)
			{
				return api.GetSignUrlForAccessCode(result.Response.AccessCodes[0].AccessCode);
			}

			throw new InvalidOperationException(result.ErrorMessage);
		}


	}
}