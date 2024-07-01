using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace AzureBlobStorage
{
    public class Storage : IStorage
    {
        private readonly IConfiguration _configuration;

        public Storage(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task UploadCharacterAsync(string rulesetName, ICharacter character)
        {
            string blobName = $"{character.Name}.json";
            BlobContainerClient blobContainerClient = await RetrieveBlobClient(rulesetName.FormatAzureCompliance());
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(character)), true));

            var characters = await DownloadCharactersByRulesetAsync(rulesetName.FormatAzureCompliance());
            characters.Add(character.CharacterSegment);

            blobName = $"characters.json";
            blobClient = blobContainerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(characters)), true));
        }

        public async Task<ICharacter> DownloadCharacterAsync(string rulesetName, string characterName)
        {
            string blobName = $"{characterName}.json";
            BlobContainerClient blobContainerClient = await RetrieveBlobClient(rulesetName.FormatAzureCompliance());
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var response = await blobClient.DownloadAsync();
            using var streamReader = new StreamReader(response.Value.Content);
            var jsonContent = await streamReader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<ICharacter>(jsonContent);
        }

        public async Task<List<CharacterSegment>> DownloadCharactersByRulesetAsync(string rulesetName)
        {
            Console.WriteLine($"Ruleset for blob: {rulesetName}");
            string blobName = "characters.json";

            try 
            {
                BlobContainerClient blobContainerClient = await RetrieveBlobClient(rulesetName.FormatAzureCompliance());
                Console.WriteLine($"Blob container name: {blobContainerClient.Name}");
                var blobClient = blobContainerClient.GetBlobClient(blobName);
                Console.WriteLine($"Blob client name: {blobClient.Name}"); 

                var response = await blobClient.DownloadAsync();
                using var streamReader = new StreamReader(response.Value.Content);
                var jsonContent = await streamReader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<List<CharacterSegment>>(jsonContent);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Blob exception: {ex.Message}");
                return new List<CharacterSegment>();
            }            
        }

        public async Task DeleteCharacterAsync(string rulesetName, string characterName)
        {
            BlobContainerClient blobContainerClient = await RetrieveBlobClient(rulesetName.FormatAzureCompliance());

            // List blobs and delete those with names starting with the prefix.
            await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, characterName))
            {
                var blobClient = blobContainerClient.GetBlobClient(blobItem.Name);
                await blobClient.DeleteIfExistsAsync();
                Console.WriteLine($"Deleted blob: {blobItem.Name}");
            }

                var characters = await DownloadCharactersByRulesetAsync(rulesetName.FormatAzureCompliance());
            characters.Remove(characters.Where(c => c.Name == characterName).FirstOrDefault());

            var blobName = "characters.json";
            var blobCharactersClient = blobContainerClient.GetBlobClient(blobName);

            await blobCharactersClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(characters)), true));
        }

        public async Task<string> UploadImageAsync(string rulesetName, ICharacter character, string filePath)
        {
            string blobName = $"{character.Name}-img{Path.GetExtension(filePath)}";
            BlobContainerClient blobContainerClient = await RetrieveBlobClient(rulesetName.FormatAzureCompliance());
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            await blobContainerClient.CreateIfNotExistsAsync();

            using FileStream uploadFileStream = File.OpenRead(filePath);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();

            return blobClient.Uri.ToString();
        }

        private async Task<BlobContainerClient> RetrieveBlobClient(string containerName)
        {
            string connectionString = _configuration["azure-blob-storage-connection-string"];
            
            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient((Debugger.IsAttached ? "dev-" : string.Empty) + containerName);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            return blobContainerClient;
        }
    }
}
