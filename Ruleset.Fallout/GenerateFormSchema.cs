using System.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Utility;

namespace Fallout
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = $"{new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent}/Ruleset.Fallout/Json/";
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
                string jsonOriginsData     = File.ReadAllText(_jsonFilesPath + "Origins.json");
                string jsonTraitsData      = File.ReadAllText(_jsonFilesPath + "Traits.json");
                string jsonAttributesData  = File.ReadAllText(_jsonFilesPath + "Attributes.json");
                string jsonSkillsData      = File.ReadAllText(_jsonFilesPath + "Skills.json");
                string jsonPerksData       = File.ReadAllText(_jsonFilesPath + "Perks.json");
                string jsonPacksData       = File.ReadAllText(_jsonFilesPath + "Packs.json");
                string jsonWeaponsData     = File.ReadAllText(_jsonFilesPath + "Weapons.json");
                string jsonArmorsData      = File.ReadAllText(_jsonFilesPath + "Armors.json");
                string jsonRobotArmorsData = File.ReadAllText(_jsonFilesPath + "Armor_Robot.json");
                string jsonClothingsData   = File.ReadAllText(_jsonFilesPath + "Clothing.json");
                string jsonConsomeablesData = File.ReadAllText(_jsonFilesPath + "Consumeables.json");
                string jsonItemsData       = File.ReadAllText(_jsonFilesPath + "Item.json");

                List<Origin>?    origins    = JsonConvert.DeserializeObject<List<Origin>>(jsonOriginsData);
                List<Trait>?     traits     = JsonConvert.DeserializeObject<List<Trait>>(jsonTraitsData);
                List<Attribute>? attributes = JsonConvert.DeserializeObject<List<Attribute>>(jsonAttributesData);
                List<Skill>?     skills     = JsonConvert.DeserializeObject<List<Skill>>(jsonSkillsData);
                List<Perk>?      perks      = JsonConvert.DeserializeObject<List<Perk>>(jsonPerksData);
                List<Pack>?      packs      = JsonConvert.DeserializeObject<List<Pack>>(jsonPacksData);
                List<Weapon>?    weapons    = JsonConvert.DeserializeObject<List<Weapon>>(jsonWeaponsData);
                List<Armor>?     armors     = JsonConvert.DeserializeObject<List<Armor>>(jsonArmorsData);
                List<ArmorRobot>? robotArmors = JsonConvert.DeserializeObject<List<ArmorRobot>>(jsonRobotArmorsData);
                List<Clothing>?  clothings  = JsonConvert.DeserializeObject<List<Clothing>>(jsonClothingsData);
                List<Consumeable>? consumeables = JsonConvert.DeserializeObject<List<Consumeable>>(jsonConsomeablesData);
                List<Item>?      items      = JsonConvert.DeserializeObject<List<Item>>(jsonItemsData);

                if (origins    == null) { Console.WriteLine("Unable to read Origins.json. Aborting...");      return; }
                if (traits     == null) { Console.WriteLine("Unable to read Traits.json. Aborting...");       return; }
                if (attributes == null) { Console.WriteLine("Unable to read Attributes.json. Aborting...");   return; }
                if (skills     == null) { Console.WriteLine("Unable to read Skills.json. Aborting...");       return; }
                if (perks      == null) { Console.WriteLine("Unable to read Perks.json. Aborting...");        return; }
                if (packs      == null) { Console.WriteLine("Unable to read Packs.json. Aborting...");        return; }
                if (weapons    == null) { Console.WriteLine("Unable to read Weapons.json. Aborting...");      return; }
                if (armors     == null) { Console.WriteLine("Unable to read Armors.json. Aborting...");       return; }
                if (robotArmors == null) { Console.WriteLine("Unable to read Armor_Robot.json. Aborting..."); return; }
                if (clothings  == null) { Console.WriteLine("Unable to read Clothing.json. Aborting...");     return; }
                if (consumeables == null) { Console.WriteLine("Unable to read Consumeables.json. Aborting..."); return; }
                if (items      == null) { Console.WriteLine("Unable to read Item.json. Aborting...");         return; }

                GenerateDescriptionSchema();

                _fields.Add(new { type = "divider" });

                GenerateOriginSchema(origins, "origin", "Origin");
                GenerateTraitSchema(traits, "traits", "Traits");
                GenerateAttributeSchema(attributes, "attributes", "Attributes");
                GenerateSkillSchema(skills, "skills", "Skills");
                GeneratePerkSchema(perks, "perks", "Perks");
                GeneratePackSchema(packs, "pack", "Pack");
                GenerateWeaponSchema(weapons, "weapons", "Weapons");
                GenerateArmorSchema(armors, "armors", "Armor");
                GenerateRobotArmorSchema(robotArmors, "robotArmors", "Robot Armor");
                GenerateClothingSchema(clothings, "clothings", "Clothing");
                GenerateConsomeableSchema(consumeables, "consumeables", "Consumeables");
                GenerateItemSchema(items, "items", "Items");

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
                className = "form-control"
            });
            _fields.Add(new
            {
                name = "healthPoints",
                id = "healthPoints",
                label = "Health Points",
                type = "hidden",
                className = "form-control",
                calculation = "2 + ([attributes.Endurance] * 2)",
                @default = 10
            });
            _fields.Add(new
            {
                name = "carryWeight",
                id = "carryWeight",
                label = "Carry Weight",
                type = "hidden",
                className = "form-control",
                calculation = "150 + ([attributes.Strength] * 10)",
                @default = 150
            });
            _fields.Add(new
            {
                name = "meleeDamage",
                id = "meleeDamage",
                label = "Melee Damage",
                type = "hidden",
                className = "form-control",
                calculation = "Math.max(1, Math.floor([attributes.Strength] / 2))",
                @default = 1
            });
            _fields.Add(new
            {
                name = "initiative",
                id = "initiative",
                label = "Initiative",
                type = "hidden",
                className = "form-control",
                calculation = "[attributes.Agility] + [attributes.Perception]",
                @default = 0
            });
            _fields.Add(new
            {
                name = "damageResistance",
                id = "damageResistance",
                label = "Damage Resistance",
                type = "hidden",
                className = "form-control",
                @default = 0
            });
            _fields.Add(new
            {
                name = "defense",
                id = "defense",
                label = "Defense",
                type = "hidden",
                className = "form-control",
                @default = 0
            });
            _fields.Add(new
            {
                name = "name",
                id = "name",
                label = "Name",
                type = "text",
                className = "form-control",
                @default = "Unknown"
            });
            _fields.Add(new
            {
                name = "image",
                id = "image",
                label = "Image",
                type = "image",
                className = "form-control"
            });
        }

        private static void GenerateOriginSchema(List<Origin> origins, string name, string label)
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
                    image = origin.Image,
                    description = origin.Description,
                    userChoices = origin.UserChoices?.Count > 0
                        ? JsonConvert.SerializeObject(origin.UserChoices, _jsonSettings)
                        : null
                });
            }

            _fields.Add(obj);
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
                    bonusAdjustments = trait.BonusAdjustments?.Count > 0
                        ? JsonConvert.SerializeObject(trait.BonusAdjustments, _jsonSettings)
                        : null,
                    userChoices = trait.UserChoices?.Count > 0
                        ? JsonConvert.SerializeObject(trait.UserChoices, _jsonSettings)
                        : null
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj
            };

            _fields.Add(array);
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
                    image = attribute.Image,
                    type = "number",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 1,
                        max = attribute.Max
                    },
                    @default = attribute.Value
                });
            }

            _fields.Add(new
            {
                type = "group",
                name,
                label,
                children
            });
        }

        private static void GenerateSkillSchema(List<Skill> skills, string name, string label)
        {
            var children = new List<object>();

            foreach (var skill in skills)
            {
                children.Add(new
                {
                    name = $"skills.{skill.Name.ReplaceWhitespace("")}",
                    id = $"skills.{skill.Name.ToLower().ReplaceWhitespace("")}",
                    label = skill.Name,
                    image = skill.Image,
                    type = "number",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 0,
                        max = skill.Max
                    },
                    @default = skill.Value
                });
            }

            _fields.Add(new
            {
                type = "group",
                name,
                label,
                children
            });
        }

        private static void GeneratePerkSchema(List<Perk> perks, string name, string label)
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
                    image = perk.Image,
                    description = perk.Description,
                    quantity = perk.Max,
                    prerequisites = perk.Prerequisites?.Count > 0
                        ? JsonConvert.SerializeObject(perk.Prerequisites, _jsonSettings)
                        : null
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj
            };

            _fields.Add(array);
        }

        private static void GeneratePackSchema(List<Pack> packs, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var pack in packs)
            {
                obj.options.Add(new
                {
                    value = pack.Name,
                    label = pack.Name,
                    description = pack.Description,
                    userChoices = pack.UserChoices?.Count > 0
                        ? JsonConvert.SerializeObject(pack.UserChoices, _jsonSettings)
                        : null
                });
            }

            _fields.Add(obj);
        }

        private static void GenerateWeaponSchema(List<Weapon> weapons, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var weapon in weapons)
            {
                obj.options.Add(new
                {
                    value = weapon.Name,
                    label = weapon.Name,
                    image = weapon.Image,
                    description = weapon.Description
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj
            };

            _fields.Add(array);
        }

        private static void GenerateArmorSchema(List<Armor> armors, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var armor in armors)
            {
                obj.options.Add(new
                {
                    value = armor.Name,
                    label = armor.Name,
                    description = armor.Description
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj
            };

            _fields.Add(array);
        }

        private static void GenerateRobotArmorSchema(List<ArmorRobot> robotArmors, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var armor in robotArmors)
            {
                obj.options.Add(new
                {
                    value = armor.Name,
                    label = armor.Name,
                    description = armor.Description
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj
            };

            _fields.Add(array);
        }

        private static void GenerateClothingSchema(List<Clothing> clothings, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var clothing in clothings)
            {
                obj.options.Add(new
                {
                    value = clothing.Name,
                    label = clothing.Name,
                    description = clothing.Description
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj
            };

            _fields.Add(array);
        }

        private static void GenerateConsomeableSchema(List<Consumeable> consumeables, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var consumeable in consumeables)
            {
                obj.options.Add(new
                {
                    value = consumeable.Name,
                    label = consumeable.Name,
                    description = consumeable.Description
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj
            };

            _fields.Add(array);
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
                obj.options.Add(new
                {
                    value = item.Name,
                    label = item.Name,
                    description = item.Description
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj
            };

            _fields.Add(array);
        }
    }
}
