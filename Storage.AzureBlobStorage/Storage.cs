using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Utility.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBlobStorage
{
    public class Storage : IStorage
    {
        public async Task UploadCharacterAsync(string rulesetName, ICharacter character)
        {
            string blobName = $"{rulesetName}.{character.Name}.json";
            BlobClient blobClient = RetrieveBlobClient(blobName);
            await blobClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(character)), true));
        }

        public async Task<ICharacter> DownloadCharacterAsync(string rulesetName, string characterName)
        {
            string blobName = $"{rulesetName}.{characterName}.json";
            BlobClient blobClient = RetrieveBlobClient(blobName);
            var response = await blobClient.DownloadAsync();
            using var streamReader = new StreamReader(response.Value.Content);
            var jsonContent = await streamReader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<ICharacter>(jsonContent);
        }

        public async Task DeleteCharacterAsync(string rulesetName, string characterName)
        {
            string blobName = $"{rulesetName}.{characterName}.json";
            BlobClient blobClient = RetrieveBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }

        private static BlobClient RetrieveBlobClient(string blobName)
        {
            string _connectionString = "your_connection_string_here"; // Retrieve this from your Azure portal
            string containerName = "your_container_name_here";
            
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            return blobContainerClient.GetBlobClient(blobName);
        }
    }
}
