using Azure.Storage.Blobs;
using Interfaces;
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
        private static async Task UploadCharacterJsonAsync(BlobContainerClient containerClient, string rulesetName, string characterName, string jsonContent)
        {
            string blobName = $"{rulesetName}.{characterName}.json";
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(jsonContent)), true);
        }

        public static async Task<ICharacter> DownloadCharacterJsonAsync(BlobContainerClient containerClient, string rulesetName, string characterName)
        {
            string blobName = $"{rulesetName}.{characterName}.json";
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            var response = await blobClient.DownloadAsync();
            using var streamReader = new StreamReader(response.Value.Content);
            var jsonContent = await streamReader.ReadToEndAsync();
            return JsonConvert.DeserializeObject<ICharacter>(jsonContent);
        }
    }
}
