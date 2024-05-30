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

namespace AzureBlobStorage
{

    public class Storage : IStorage
    {
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
            string blobName = $"{characterName}.json";
            BlobContainerClient blobContainerClient = await RetrieveBlobClient(rulesetName.FormatAzureCompliance());
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            await blobClient.DeleteIfExistsAsync();

            var characters = await DownloadCharactersByRulesetAsync(rulesetName.FormatAzureCompliance());
            characters.Remove(characters.Where(c => c.Name == characterName).FirstOrDefault());

            blobName = "characters.json";
            blobClient = blobContainerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(characters)), true));
        }

        private static async Task<BlobContainerClient> RetrieveBlobClient(string containerName)
        {
            string _connectionString = "";
            
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient((Debugger.IsAttached ? "dev-" : string.Empty) + containerName);
            await blobContainerClient.CreateIfNotExistsAsync();
            return blobContainerClient;
        }
    }
}
