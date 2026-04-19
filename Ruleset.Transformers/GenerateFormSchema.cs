using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Utility;

namespace Transformers
{
    public class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = $"{new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent}/Ruleset.Transformers/Json/";
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
                string jsonFactionsData = File.ReadAllText(_jsonFilesPath + "Factions.json");
                var factions = JsonConvert.DeserializeObject<List<Faction>>(jsonFactionsData);
                if (factions == null) { Console.WriteLine("Unable to read Factions.json. Aborting..."); Console.Read(); return; }

                string jsonOriginsData = File.ReadAllText(_jsonFilesPath + "Origins.json");
                var origins = JsonConvert.DeserializeObject<List<Origin>>(jsonOriginsData);
                if (origins == null) { Console.WriteLine("Unable to read Origins.json. Aborting..."); Console.Read(); return; }

                string jsonRolesData = File.ReadAllText(_jsonFilesPath + "Roles.json");
                var roles = JsonConvert.DeserializeObject<List<Role>>(jsonRolesData);
                if (roles == null) { Console.WriteLine("Unable to read Roles.json. Aborting..."); Console.Read(); return; }

                string jsonFocusesData = File.ReadAllText(_jsonFilesPath + "Focuses.json");
                var focuses = JsonConvert.DeserializeObject<List<Focus>>(jsonFocusesData);
                if (focuses == null) { Console.WriteLine("Unable to read Focuses.json. Aborting..."); Console.Read(); return; }

                string jsonInfluencesData = File.ReadAllText(_jsonFilesPath + "Influences.json");
                var influences = JsonConvert.DeserializeObject<List<Influence>>(jsonInfluencesData);
                if (influences == null) { Console.WriteLine("Unable to read Influences.json. Aborting..."); Console.Read(); return; }

                string jsonHangUpsData = File.ReadAllText(_jsonFilesPath + "Hang-Ups.json");
                var hangUps = JsonConvert.DeserializeObject<List<HangUp>>(jsonHangUpsData);
                if (hangUps == null) { Console.WriteLine("Unable to read Hang-Ups.json. Aborting..."); Console.Read(); return; }

                string jsonEssencesData = File.ReadAllText(_jsonFilesPath + "Essences.json");
                var essences = JsonConvert.DeserializeObject<List<Essence>>(jsonEssencesData);
                if (essences == null) { Console.WriteLine("Unable to read Essences.json. Aborting..."); Console.Read(); return; }

                string jsonPerksData = File.ReadAllText(_jsonFilesPath + "Perks.json");
                var perks = JsonConvert.DeserializeObject<List<Perk>>(jsonPerksData);
                if (perks == null) { Console.WriteLine("Unable to read Perks.json. Aborting..."); Console.Read(); return; }

                string jsonSkillsData = File.ReadAllText(_jsonFilesPath + "Skills.json");
                var skills = JsonConvert.DeserializeObject<List<Skill>>(jsonSkillsData);
                if (skills == null) { Console.WriteLine("Unable to read Skills.json. Aborting..."); Console.Read(); return; }

                string jsonSpecializationsData = File.ReadAllText(_jsonFilesPath + "Specializations.json");
                var specializations = JsonConvert.DeserializeObject<List<Specialization>>(jsonSpecializationsData);
                if (specializations == null) { Console.WriteLine("Unable to read Specializations.json. Aborting..."); Console.Read(); return; }

                var choiceLookup = new Dictionary<string, List<IBaseJson>>(StringComparer.OrdinalIgnoreCase)
                {
                    ["skills"]          = skills.Cast<IBaseJson>().ToList(),
                    ["specialization"] = specializations.Cast<IBaseJson>().ToList(),
                };

                GenerateDescriptionSchema();
                GenerateFactionsSchema(factions, "faction", "Faction");
                GenerateInfluencesSchema(influences, "influences", "Influences");
                GenerateOriginSchema(origins, "origin", "Origin", choiceLookup);
                GenerateRoleSchema(roles, "role", "Role", choiceLookup);
                GenerateFocusSchema(focuses, roles, "focus", "Focus", choiceLookup);
                GeneratePerksSchema(perks, "perks", "Perks");            
                GenerateHangUpsSchema(hangUps, "hangups", "Hang-Ups");
                GenerateEssencesSchema(essences, "essences", "Essences");                
                GenerateSkillsSchema(skills, specializations, "specialization", "Specializations");

                var schema = new
                {
                    title = "Hero Editor",
                    fields = _fields
                };

                string schemaJson = JsonConvert.SerializeObject(schema, Formatting.Indented);

                var schemaPath = _jsonFilesPath + "Character/Form.json";
                File.WriteAllText(schemaPath, schemaJson);

                Console.WriteLine("Character schema generated and saved to " + schemaPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void GenerateDescriptionSchema()
        {
            _fields.Add(new
            {
                name = "id",
                id = "id",
                label = "Id",
                type = "hidden",
                className = "form-control",
                tab = "Identity"
            });           
            _fields.Add(new
            {
                name = "name",
                id = "name",
                label = "Name",
                type = "text",
                className = "form-control",
                @default = "Unknown",
                tab = "Identity"
            });
            _fields.Add(new
            {
                name = "image",
                id = "image",
                label = "Image",
                type = "image",
                className = "form-control",
                tab = "Identity"
            });
            _fields.Add(new
            {
                name = "description",
                id = "description",
                label = "Description",
                type = "textarea",
                className = "form-control",
                tab = "Identity"
            });
            _fields.Add(new
            {
                name = "notes",
                id = "notes",
                label = "Notes",
                type = "textarea",
                className = "form-control",
                tab = "Identity"
            });
            _fields.Add(new
            {
                name = "level",
                id = "level",
                label = "Level",
                type = "number",
                className = "form-control",
                validation = new { required = true, min = 1, max = 20 },
                @default = 1,
                tab = "Identity"
            });
        }

        private static void GenerateFactionsSchema(List<Faction> factions, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var faction in factions)
            {
                obj.options.Add(new
                {
                    value = faction.Name,
                    label = faction.Name,
                    description = faction.Description,
                    image = faction.Image
                });
            }

            obj.tab = "Origins";
            _fields.Add(obj);
        }

        private static void GenerateOriginSchema(List<Origin> origins, string name, string label, Dictionary<string, List<IBaseJson>> choiceLookup)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var origin in origins)
            {
                obj.options.Add(new
                {
                    value = origin.Name,
                    label = origin.Name,
                    description = origin.Description,
                    bonusAdjustments = JsonConvert.SerializeObject(origin.BonusAdjustments, _jsonSettings),
                    userChoices = JsonConvert.SerializeObject(FormSchemaExtensions.EnrichUserChoices(origin.UserChoices, choiceLookup), _jsonSettings)
                });
            }

            obj.tab = "Origins";
            _fields.Add(obj);
        }

        private static void GenerateRoleSchema(List<Role> roles, string name, string label, Dictionary<string, List<IBaseJson>> choiceLookup)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var role in roles)
            {
                obj.options.Add(new
                {
                    value = role.Name,
                    label = role.Name,
                    description = role.Description,
                    bonusAdjustments = JsonConvert.SerializeObject(role.BonusAdjustments, _jsonSettings),
                    bonusCharacteristics = JsonConvert.SerializeObject(role.BonusCharacteristics, _jsonSettings),
                    userChoices = JsonConvert.SerializeObject(FormSchemaExtensions.EnrichUserChoices(role.UserChoices, choiceLookup), _jsonSettings)
                });
            }

            obj.tab = "Origins";
            _fields.Add(obj);
        }

        private static void GenerateFocusSchema(List<Focus> focuses, List<Role> roles, string name, string label, Dictionary<string, List<IBaseJson>> choiceLookup)
        {
            foreach (var role in roles)
            {
                dynamic obj = new ExpandoObject();

                obj.name = $"focus.{role.Name.ToLower()}";
                obj.label = "Focus";
                obj.type = "select";
                obj.className = "form-select";
                obj.options = new List<object>();

                foreach (var foc in focuses.Where(f => f.Role.ToLower() == role.Name.ToLower()))
                {
                    if (foc.BonusCharacteristics != null) UpdateBonusCharacteristicValues(foc.BonusCharacteristics);

                    obj.options.Add(
                        new
                        {
                            value = foc.Name,
                            label = foc.Name,
                            description = foc.Description,
                            bonusCharacteristics = JsonConvert.SerializeObject(foc.BonusCharacteristics, _jsonSettings),
                            BonusAdjustments = JsonConvert.SerializeObject(foc.BonusAdjustments, _jsonSettings),
                            userChoices = JsonConvert.SerializeObject(FormSchemaExtensions.EnrichUserChoices(foc.UserChoices, choiceLookup), _jsonSettings)
                        }
                    );

                    obj.dependsOn =
                        new
                        {
                            field = "role",
                            value = role.Name
                        };
                }

                obj.tab = "Origins";
                _fields.Add(obj);
            }
        }

        private static void GenerateInfluencesSchema(List<Influence> influences, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var influence in influences)
            {
                obj.options.Add(new
                {
                    value = influence.Name,
                    label = influence.Name,
                    description = influence.Description
                    //perk = influence.Perk,
                    //suggestion = influence.Suggestion
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

        private static void GenerateHangUpsSchema(List<HangUp> hangUps, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var hangUp in hangUps)
            {
                obj.options.Add(new
                {
                    value = hangUp.Name,
                    label = hangUp.Name,
                    description = hangUp.Description
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

        private static void GenerateEssencesSchema(List<Essence> essences, string name, string label)
        {
            var children = new List<object>();

            foreach (var essence in essences)
            {
                children.Add(new
                {
                    name = $"essences.{essence.Name}",
                    id = $"essences.{essence.Name.ToLower()}",
                    label = essence.Name,
                    type = "number",
                    className = "form-control",
                    validation = new { required = true, min = 1, max = 15 },
                    @default = 1
                });
            }

            var group = new
            {
                type = "group",
                name,
                label,
                children,
                tab = "Attributes"
            };

            _fields.Add(group);
        }

        private static void GeneratePerksSchema(List<Perk> perks, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var perk in perks)
            {
                obj.options.Add(new
                {
                    value = perk.Name,
                    label = perk.Name,
                    description = perk.Description
                    //perkType = perk.Type
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

        private static void GenerateSkillsSchema(List<Skill> skills,
            List<Specialization> specializations, string specName, string specLabel)
        {
            foreach (var skill in skills)
            {
                _fields.Add(new { type = "divider", tab = "Skills" });

                _fields.Add(new
                {
                    name = $"skills.{skill.Name}",
                    id = $"skills.{skill.Name.ToLower()}",
                    label = skill.Name,
                    description = skill.Description,
                    essenceType = skill.Type,
                    type = "number",
                    className = "form-control",
                    validation = new { required = true, min = 0, max = 6 },
                    @default = 0,
                    tab = "Skills"
                });

                var skillSpecializations = specializations.Where(s => s.Type == skill.Name).ToList();
                if (skillSpecializations.Count > 0)
                {
                    dynamic specObj = new ExpandoObject();
                    specObj.name = $"{specName}.{skill.Name}";
                    specObj.id = $"{specName}.{skill.Name}";
                    specObj.label = $"{skill.Name} {specLabel}";
                    specObj.type = "select";
                    specObj.className = "form-select";
                    specObj.options = new List<object>();

                    foreach (var spec in skillSpecializations)
                    {
                        specObj.options.Add(new
                        {
                            value = spec.Name,
                            label = spec.Name,
                            description = spec.Description
                        });
                    }

                    dynamic specArray = new
                    {
                        name = (string)specObj.name,
                        label = (string)specObj.label,
                        type = "array",
                        component = specObj,
                        tab = "Skills"
                    };

                    _fields.Add(specArray);
                }
            }
        }

        private static void UpdateBonusCharacteristicValues(List<BonusCharacteristic>? characteristics)
        {
            foreach (var bonus in characteristics)
            {
                if (bonus.Type.StartsWith("skill.", StringComparison.OrdinalIgnoreCase))
                {
                    bonus.Value = $"{bonus.Type}.{bonus.Value}";
                }
            };
        }
    }
}
