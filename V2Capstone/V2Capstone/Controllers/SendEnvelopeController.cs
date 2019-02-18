using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace V2Capstone.Controllers
{
    public class SendEnvelopeController : Controller
    {
        // Constants need to be set:
        private const string accessToken = "eyJ0eXAiOiJNVCIsImFsZyI6IlJTMjU2Iiwia2lkIjoiNjgxODVmZjEtNGU1MS00Y2U5LWFmMWMtNjg5ODEyMjAzMzE3In0.AQoAAAABAAUABwAAj6ndtZXWSAgAAM_M6_iV1kgCAAwVTa4M4iVIpLqbxyLjeuEVAAEAAAAYAAEAAAAFAAAADQAkAAAAZjBmMjdmMGUtODU3ZC00YTcxLWE0ZGEtMzJjZWNhZTNhOTc4EgABAAAACwAAAGludGVyYWN0aXZlMAAAYnjctZXWSDcA0kMZZq3QFkqlEraw7UFc9Q.lkWi7NGTZHYR6V6-xrcd_Pu0ppCkYbOyzOzQPi_JvbDvNvtQu-YaM3475OQgtMv2ZT6RnCWH-gBfFOD8p4f1zM2On_Y3M5NHXH1Pv-O6_B4aZ1KC9YXoXWUigwOtJ8uDZvaMrmRDI5tzLDIIODZQa8yRy7mlgbdd-FAlKk2Zqsl0SLfVzhNc-GcwHk8S1e5mJMxj-pTiuskWyKPxrBTiMWcwbT4oYxPoOPvrQpLd1_Rwa3auJYE5vXBiiFqdtrae3MGwmIASBjQ7vgbATm5HxoMd7s77sLR59j4je6J9IVGs4ZS1DkNpKJbhXCelN1R3SjKKaiv7Uk4zTmDeTdVG3w";
        private const string accountId = "7898624";
        private const string signerName = "Leum Ruffling";
        private const string signerEmail = "josh18weber@gmail.com";
        private const string docName = "World_Wide_Corp_lorem.pdf";

        // Additional constants
        private const string basePath = "https://demo.docusign.net/restapi";

        public void OnGet()
        {
        }

        public IActionResult OnPostAsync()
        {
            // Send envelope. Signer's request to sign is sent by email.
            // 1. Create envelope request obj
            // 2. Use the SDK to create and send the envelope

            // 1. Create envelope request object
            //    Start with the different components of the request
            //    Create the document object
            Document document = new Document
            {
                DocumentBase64 = Convert.ToBase64String(ReadContent(docName)),
                Name = "Lorem Ipsum",
                FileExtension = "pdf",
                DocumentId = "1"
            };
            Document[] documents = new Document[] { document };

            // Create the signer recipient object 
            Signer signer = new Signer
            {
                Email = signerEmail,
                Name = signerName,
                RecipientId = "1",
                RoutingOrder = "1"
            };

            // Create the sign here tab (signing field on the document)
            SignHere signHereTab = new SignHere
            {
                DocumentId = "1",
                PageNumber = "1",
                RecipientId = "1",
                TabLabel = "Sign Here Tab",
                XPosition = "195",
                YPosition = "147"
            };
            SignHere[] signHereTabs = new SignHere[] { signHereTab };

            // Add the sign here tab array to the signer object.
            signer.Tabs = new Tabs { SignHereTabs = new List<SignHere>(signHereTabs) };
            // Create array of signer objects
            Signer[] signers = new Signer[] { signer };
            // Create recipients object
            Recipients recipients = new Recipients { Signers = new List<Signer>(signers) };
            // Bring the objects together in the EnvelopeDefinition
            EnvelopeDefinition envelopeDefinition = new EnvelopeDefinition
            {
                EmailSubject = "Please sign the document",
                Documents = new List<Document>(documents),
                Recipients = recipients,
                Status = "sent"
            };

            // 2. Use the SDK to create and send the envelope
            ApiClient apiClient = new ApiClient(basePath);
            apiClient.Configuration.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            EnvelopesApi envelopesApi = new EnvelopesApi(apiClient.Configuration);
            EnvelopeSummary results = envelopesApi.CreateEnvelope(accountId, envelopeDefinition);
            ViewData["results"] = $"Envelope status: {results.Status}. Envelope ID: {results.EnvelopeId}";

            return View();
        }

        /// <summary>
        /// This method read bytes content from the project's Resources directory
        /// </summary>
        /// <param name="fileName">resource path</param>
        /// <returns>return bytes array</returns>
        internal static byte[] ReadContent(string fileName)
        {
            byte[] buff = null;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(stream))
                {
                    long numBytes = new FileInfo(path).Length;
                    buff = br.ReadBytes((int)numBytes);
                }
            }

            return buff;
        }
    }
}