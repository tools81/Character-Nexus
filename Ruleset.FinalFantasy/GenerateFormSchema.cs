using System.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Utility;

namespace FinalFantasy
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = $"{new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent}/Ruleset.FinalFantasy/Json/";
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = JsonContractResolver.Get(),
            Formatting = Formatting.None
        };

        public static void InitializeSchema()
        {
            try
            {
                var jobs         = LoadJson.Load<Job>(_jsonFilesPath, "Jobs.json");
                var roles        = LoadJson.Load<Role>(_jsonFilesPath, "Roles.json");
                var races        = LoadJson.Load<Race>(_jsonFilesPath, "Races.json");
                var subraces     = LoadJson.Load<Subrace>(_jsonFilesPath, "Subraces.json");
                var attributes   = LoadJson.Load<Attribute>(_jsonFilesPath, "Attributes.json");
                var traits       = LoadJson.Load<Trait>(_jsonFilesPath, "Traits.json");
                var abilities    = LoadJson.Load<Ability>(_jsonFilesPath, "Abilities.json");
                var limitBreaks  = LoadJson.Load<LimitBreak>(_jsonFilesPath, "LimitBreaks.json");
                var items        = LoadJson.Load<Item>(_jsonFilesPath, "Items.json");
                var augmentations = LoadJson.Load<Augmentation>(_jsonFilesPath, "Augmentations.json");

                GenerateCalculatedSchema();
                GenerateDescriptionSchema();
                GenerateRaceSchema(races, subraces, "race", "Race");
                GenerateRoleSchema(roles, jobs, "role", "Role", "job", "Job");
                GenerateAttributeSchema(attributes, "attributes", "Attributes");
                GenerateTraitSchema(traits, "traits", "Traits");
                GenerateAbilitySchema(abilities, "abilities", "Abilities");
                GenerateLimitBreakSchema(limitBreaks, "limitBreak", "Limit Break");
                GenerateItemSchema(items, "items", "Items");
                GenerateAugmentationSchema(augmentations, "augmentation", "Augmentation");

                var schema = new
                {
                    title = "Character Editor",
                    fields = _fields
                };

                string schemaJson = JsonConvert.SerializeObject(schema, Formatting.Indented);

                var schemaPath = _jsonFilesPath + "Character/Form.json";
                Directory.CreateDirectory(Path.GetDirectoryName(schemaPath)!);
                File.WriteAllText(schemaPath, schemaJson);

                Console.WriteLine("Character schema generated and saved to " + schemaPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void GenerateCalculatedSchema()
        {
            _fields.Add(new { name = "hp",           id = "hp",           label = "HP",           type = "number", pinnedStat = true });
            _fields.Add(new { name = "mp",           id = "mp",           label = "MP",           type = "number", pinnedStat = true });
            _fields.Add(new { name = "defense",      id = "defense",      label = "Defense",      type = "number", pinnedStat = true });
            _fields.Add(new { name = "magicDefense", id = "magicDefense", label = "Magic Defense", type = "number", pinnedStat = true });
            _fields.Add(new { name = "vigilance",    id = "vigilance",    label = "Vigilance",    type = "number", pinnedStat = true });
            _fields.Add(new { name = "speed",        id = "speed",        label = "Speed",        type = "number", pinnedStat = true });
        }

        private static void GenerateDescriptionSchema()
        {
            _fields.Add(new { name = "id",      id = "id",      label = "Id",      type = "hidden",   className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "name",    id = "name",    label = "Name",    type = "text",     className = "form-control", @default = "Unknown", tab = "Identity" });
            _fields.Add(new { name = "image",   id = "image",   label = "Image",   type = "image",    className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "level",   id = "level",   label = "Level",   type = "number",   className = "form-control", validation = new { required = true, min = 1, max = 99 }, @default = 1, tab = "Identity" });
            _fields.Add(new { name = "profile", id = "profile", label = "Profile", type = "textarea", className = "form-control", tab = "Identity" });
            _fields.Add(new { name = "size",    id = "size",    label = "Size",    type = "text",     className = "form-control", @default = "Medium", tab = "Identity" });
        }

        private static void GenerateRaceSchema(List<Race> races, List<Subrace> subraces, string raceName, string raceLabel)
        {
            dynamic raceObj = new ExpandoObject();
            raceObj.name = raceName;
            raceObj.label = raceLabel;
            raceObj.type = "select";
            raceObj.className = "form-select";
            raceObj.options = new List<object>();

            foreach (var race in races)
            {
                raceObj.options.Add(new
                {
                    value = race.Name,
                    label = race.Name,
                    description = race.Description
                });
            }

            raceObj.tab = "Origins";
            _fields.Add(raceObj);

            foreach (var race in races)
            {
                var raceSubraces = subraces.Where(s => race.Subraces?.Contains(s.Name) == true).ToList();
                if (raceSubraces.Count == 0) continue;

                dynamic subraceObj = new ExpandoObject();
                subraceObj.name = "subrace";
                subraceObj.label = "Subrace";
                subraceObj.type = "select";
                subraceObj.className = "form-select";
                subraceObj.dependsOn = new { field = raceName, value = race.Name };
                subraceObj.options = new List<object>();

                foreach (var subrace in raceSubraces)
                {
                    subraceObj.options.Add(new
                    {
                        value = subrace.Name,
                        label = subrace.Name,
                        description = subrace.Description
                    });
                }

                subraceObj.tab = "Origins";
                _fields.Add(subraceObj);
            }
        }

        private static void GenerateRoleSchema(List<Role> roles, List<Job> jobs, string roleName, string roleLabel, string jobName, string jobLabel)
        {
            dynamic roleObj = new ExpandoObject();
            roleObj.name = roleName;
            roleObj.label = roleLabel;
            roleObj.type = "select";
            roleObj.className = "form-select";
            roleObj.options = new List<object>();

            foreach (var role in roles)
            {
                roleObj.options.Add(new
                {
                    value = role.Name,
                    label = role.Name,
                    image = role.Image,
                    description = role.Description
                });
            }

            roleObj.tab = "Origins";
            _fields.Add(roleObj);

            foreach (var role in roles)
            {
                var roleJobs = jobs.Where(j => j.Role == role.Name).ToList();
                if (roleJobs.Count == 0) continue;

                dynamic jobObj = new ExpandoObject();
                jobObj.name = jobName;
                jobObj.label = jobLabel;
                jobObj.type = "select";
                jobObj.className = "form-select";
                jobObj.dependsOn = new { field = roleName, value = role.Name };
                jobObj.options = new List<object>();

                foreach (var job in roleJobs)
                {
                    jobObj.options.Add(new
                    {
                        value = job.Name,
                        label = job.Name,
                        description = job.Description,
                        bonusAdjustments = job.BonusAdjustments?.Count > 0
                            ? JsonConvert.SerializeObject(job.BonusAdjustments, _jsonSettings)
                            : null
                    });
                }

                jobObj.tab = "Origins";
                _fields.Add(jobObj);
            }
        }

        private static void GenerateAttributeSchema(List<Attribute> attributes, string name, string label)
        {
            var children = new List<object>();

            foreach (var attribute in attributes)
            {
                children.Add(new
                {
                    name = $"attributes.{attribute.Name}",
                    id = $"attributes.{attribute.Name.ToLower()}",
                    label = attribute.Name,
                    type = "number",
                    className = "form-control",
                    validation = new { required = true, min = 0, max = 20 },
                    @default = attribute.Value
                });
            }

            _fields.Add(new
            {
                type = "group",
                name,
                label,
                children,
                tab = "Attributes"
            });
        }

        private static void GenerateTraitSchema(List<Trait> traits, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var trait in traits)
            {
                obj.options.Add(new
                {
                    value = trait.Name,
                    label = trait.Name,
                    description = trait.Description,
                    category = trait.Category,
                    limitation = trait.Limitation
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj,
                tab = "Features"
            };

            _fields.Add(array);
        }

        private static void GenerateAbilitySchema(List<Ability> abilities, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var ability in abilities)
            {
                var stats = new List<object>();
                if (!string.IsNullOrEmpty(ability.Category)) stats.Add(new { label = "Category:", value = ability.Category,              field = "category" });
                if (!string.IsNullOrEmpty(ability.Target))   stats.Add(new { label = "Target:",   value = ability.Target,                field = "target" });
                if (!string.IsNullOrEmpty(ability.Range))    stats.Add(new { label = "Range:",    value = ability.Range,                 field = "range" });
                if (!string.IsNullOrEmpty(ability.Trigger))  stats.Add(new { label = "Trigger:",  value = ability.Trigger,               field = "trigger" });
                if (!string.IsNullOrEmpty(ability.Check))    stats.Add(new { label = "Check:",    value = ability.Check,                 field = "check" });
                if (!string.IsNullOrEmpty(ability.CR))       stats.Add(new { label = "CR:",       value = ability.CR,                    field = "cr" });
                if (!string.IsNullOrEmpty(ability.Base))     stats.Add(new { label = "Base:",     value = ability.Base,                  field = "base" });
                if (!string.IsNullOrEmpty(ability.Direct))   stats.Add(new { label = "Direct:",   value = ability.Direct,                field = "direct" });
                if (ability.Limitation > 0)                  stats.Add(new { label = "Limit:",    value = ability.Limitation.ToString(), field = "limitation" });

                obj.options.Add(new
                {
                    value = ability.Name,
                    label = ability.Name,
                    image = ability.Image,
                    description = ability.Description,
                    stats = JsonConvert.SerializeObject(stats, _jsonSettings)
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj,
                tab = "Abilities"
            };

            _fields.Add(array);
        }

        private static void GenerateLimitBreakSchema(List<LimitBreak> limitBreaks, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var lb in limitBreaks)
            {
                var stats = new List<object>();
                if (!string.IsNullOrEmpty(lb.Trigger))  stats.Add(new { label = "Trigger:", value = lb.Trigger,                         field = "trigger" });
                if (!string.IsNullOrEmpty(lb.Effect))   stats.Add(new { label = "Effect:",  value = lb.Effect,                          field = "effect" });
                if (lb.Types?.Count > 0)                stats.Add(new { label = "Types:",   value = string.Join(", ", lb.Types),        field = "types" });

                obj.options.Add(new
                {
                    value = lb.Name,
                    label = lb.Name,
                    description = lb.Description,
                    stats = JsonConvert.SerializeObject(stats, _jsonSettings)
                });
            }

            obj.tab = "Abilities";
            _fields.Add(obj);
        }

        private static void GenerateItemSchema(List<Item> items, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var item in items)
            {
                var stats = new List<object>();
                if (!string.IsNullOrEmpty(item.Tier))   stats.Add(new { label = "Tier:",   value = item.Tier,                field = "tier" });
                if (item.Price > 0)                     stats.Add(new { label = "Price:",  value = item.Price.ToString(),    field = "price" });
                if (!string.IsNullOrEmpty(item.Target)) stats.Add(new { label = "Target:", value = item.Target,              field = "target" });
                if (!string.IsNullOrEmpty(item.Range))  stats.Add(new { label = "Range:",  value = item.Range,               field = "range" });
                if (!string.IsNullOrEmpty(item.Base))   stats.Add(new { label = "Base:",   value = item.Base,                field = "base" });

                obj.options.Add(new
                {
                    value = item.Name,
                    label = item.Name,
                    image = item.Image,
                    description = item.Description,
                    stats = JsonConvert.SerializeObject(stats, _jsonSettings)
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj,
                tab = "Equipment"
            };

            _fields.Add(array);
        }

        private static void GenerateAugmentationSchema(List<Augmentation> augmentations, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var aug in augmentations)
            {
                var stats = new List<object>();
                if (aug.Price > 0)                          stats.Add(new { label = "Price:",   value = aug.Price.ToString(),                                                   field = "price" });
                if (!string.IsNullOrEmpty(aug.Trigger))     stats.Add(new { label = "Trigger:", value = aug.Trigger,                                                            field = "trigger" });
                if (aug.Limitation > 0)                     stats.Add(new { label = "Limit:",   value = aug.Limitation.ToString(),                                              field = "limitation" });
                if (aug.BonusCharacteristics?.Count > 0)    stats.Add(new { label = "Bonuses:", value = JsonConvert.SerializeObject(aug.BonusCharacteristics, _jsonSettings),  field = "bonusCharacteristics" });

                obj.options.Add(new
                {
                    value = aug.Name,
                    label = aug.Name,
                    description = aug.Description,
                    stats = JsonConvert.SerializeObject(stats, _jsonSettings)
                });
            }

            obj.tab = "Equipment";
            _fields.Add(obj);
        }
    }
}
