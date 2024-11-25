using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Utility
{
    public interface IStorage
    {
        public Task UploadCharacterAsync(string rulesetName, ICharacter character);
        public Task UploadCharacterAsync(string rulesetName, ICharacter character, JObject characterFormData);
        public Task<ICharacter> DownloadCharacterAsync(string rulesetName, string characterName);
        public Task<string> DownloadCharacterJsonAsync(string rulesetName, string characterName);
        public Task<List<CharacterSegment>> DownloadCharactersByRulesetAsync(string rulesetName);
        public Task DeleteCharacterAsync(string rulesetName, string characterName);
        public Task<string> UploadImageAsync(string rulesetName, string name, IFormFile image);
        public Task<string> UploadPDFByteArray(string rulesetName, string name, byte[] bytes);
    }
}
