using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public interface IStorage
    {
        public Task UploadCharacterAsync(string rulesetName, ICharacter character);
        public Task<ICharacter> DownloadCharacterAsync(string rulesetName, string characterName);
        public Task<List<CharacterSegment>> DownloadCharactersByRulesetAsync(string rulesetName);
        public Task DeleteCharacterAsync(string rulesetName, string characterName);
    }
}
