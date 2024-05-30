using Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace EverydayHeroes
{
    public class Ruleset : IRuleset
    {
        public string Name => "Everyday Heroes";

        public string RulesetName => "Ruleset.EverydayHeroes";

        public string ImageSource => "https://drive.google.com/thumbnail?id=1ebRVV5mfKLeXjQ1fFVEcNcqxSlLOn8Rp&sz=w1024";

        public string LogoSource => "https://drive.google.com/thumbnail?id=1mYxnrebS9gUUWZJS_KP2Va6CoU4KeNZU&sz=w256";

        public ICharacter NewCharacter()
        {
            var character = new Character();

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EverydayHeroes.Json.Attributes.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var jsonContent = reader.ReadToEnd();
                    character.Attributes = JsonTo.Dictionary<Attribute>(jsonContent);
                }
            }

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EverydayHeroes.Json.Skills.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var jsonContent = reader.ReadToEnd();
                    character.Skills = JsonTo.Dictionary<Skill>(jsonContent);
                }
            }

            return character;
        }

        public IEnumerable<IClass> GetClassList()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EverydayHeroes.Json.Classes.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var jsonContent = reader.ReadToEnd();
                    return JsonTo.IEnumerable<Archetype>(jsonContent);
                }
            }
        }

        public IEnumerable<IFeature> GetFeatureList()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EverydayHeroes.Json.Features.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var jsonContent = reader.ReadToEnd();
                    return JsonTo.IEnumerable<Feature>(jsonContent);
                }
            }
        }

        public IEnumerable<IAbility> GetAbilityList()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EverydayHeroes.Json.Abilities.json"))
            {
                using (var reader = new StreamReader(stream))
                {
                    var jsonContent = reader.ReadToEnd();
                    return JsonTo.IEnumerable<Ability>(jsonContent);
                }
            }
        }
    }
}
