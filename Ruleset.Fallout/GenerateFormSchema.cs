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

        private static readonly string _nonRobotOrigins = "Brotherhood Initiate, Ghoul, Super Mutant, Survivor, Vault Dweller";
        private static readonly string _robotOrigins = "Mister Handy";

        private static readonly Dictionary<string, string> _imageTokens = new()
        {
            ["bottlecap"] = "https://characternexus.blob.core.windows.net/resources/Fallout/bottlecap.png"
        };

        private static readonly Regex _tokenRegex = new(@"\{\{(\w+)\}\}");

        private static string ReplaceTokens(string? text)
        {
            if (string.IsNullOrEmpty(text)) return text ?? string.Empty;
            return _tokenRegex.Replace(text, m =>
                _imageTokens.TryGetValue(m.Groups[1].Value.ToLower(), out var url)
                    ? $"<img src=\"{url}\" alt=\"{m.Groups[1].Value}(s)\" class=\"inline-icon\">"
                    : m.Value);
        }

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
                string jsonWeaponModsData  = File.ReadAllText(_jsonFilesPath + "Weapon_Mods.json");
                string jsonArmorsData      = File.ReadAllText(_jsonFilesPath + "Armors.json");
                string jsonArmorModsData   = File.ReadAllText(_jsonFilesPath + "Armor_Mods.json");
                string jsonRobotArmorsData = File.ReadAllText(_jsonFilesPath + "Armor_Robot.json");
                string jsonRobotModsData   = File.ReadAllText(_jsonFilesPath + "Robot_Mods.json");
                string jsonRobotWeaponsData = File.ReadAllText(_jsonFilesPath + "Weapon_Robot.json");
                string jsonClothingsData   = File.ReadAllText(_jsonFilesPath + "Clothing.json");
                string jsonConsomeablesData = File.ReadAllText(_jsonFilesPath + "Consumeables.json");
                string jsonItemsData       = File.ReadAllText(_jsonFilesPath + "Item.json");
                string jsonAmmosData       = File.ReadAllText(_jsonFilesPath + "Ammo.json");

                List<Origin>?    origins    = JsonConvert.DeserializeObject<List<Origin>>(jsonOriginsData);
                List<Trait>?     traits     = JsonConvert.DeserializeObject<List<Trait>>(jsonTraitsData);
                List<Attribute>? attributes = JsonConvert.DeserializeObject<List<Attribute>>(jsonAttributesData);
                List<Skill>?     skills     = JsonConvert.DeserializeObject<List<Skill>>(jsonSkillsData);
                List<Perk>?      perks      = JsonConvert.DeserializeObject<List<Perk>>(jsonPerksData);
                List<Pack>?      packs      = JsonConvert.DeserializeObject<List<Pack>>(jsonPacksData);
                List<Weapon>?    weapons    = JsonConvert.DeserializeObject<List<Weapon>>(jsonWeaponsData);
                List<WeaponMod>? weaponMods = JsonConvert.DeserializeObject<List<WeaponMod>>(jsonWeaponModsData);
                List<Armor>?     armors     = JsonConvert.DeserializeObject<List<Armor>>(jsonArmorsData);
                List<ArmorMod>?  armorMods  = JsonConvert.DeserializeObject<List<ArmorMod>>(jsonArmorModsData);
                List<ArmorRobot>? robotArmors = JsonConvert.DeserializeObject<List<ArmorRobot>>(jsonRobotArmorsData);
                List<RobotMod>?  robotMods   = JsonConvert.DeserializeObject<List<RobotMod>>(jsonRobotModsData);
                List<WeaponRobot>? robotWeapons = JsonConvert.DeserializeObject<List<WeaponRobot>>(jsonRobotWeaponsData);
                List<Clothing>?  clothings  = JsonConvert.DeserializeObject<List<Clothing>>(jsonClothingsData);
                List<Consumeable>? consumeables = JsonConvert.DeserializeObject<List<Consumeable>>(jsonConsomeablesData);
                List<Item>?      items      = JsonConvert.DeserializeObject<List<Item>>(jsonItemsData);
                List<Ammo>?      ammos      = JsonConvert.DeserializeObject<List<Ammo>>(jsonAmmosData);

                if (origins    == null) { Console.WriteLine("Unable to read Origins.json. Aborting...");      return; }
                if (traits     == null) { Console.WriteLine("Unable to read Traits.json. Aborting...");       return; }
                if (attributes == null) { Console.WriteLine("Unable to read Attributes.json. Aborting...");   return; }
                if (skills     == null) { Console.WriteLine("Unable to read Skills.json. Aborting...");       return; }
                if (perks      == null) { Console.WriteLine("Unable to read Perks.json. Aborting...");        return; }
                if (packs      == null) { Console.WriteLine("Unable to read Packs.json. Aborting...");        return; }
                if (weapons    == null) { Console.WriteLine("Unable to read Weapons.json. Aborting...");      return; }
                if (weaponMods == null) { Console.WriteLine("Unable to read Weapon_Mods.json. Aborting..."); return; }
                if (armors     == null) { Console.WriteLine("Unable to read Armors.json. Aborting...");       return; }
                if (armorMods  == null) { Console.WriteLine("Unable to read Armor_Mods.json. Aborting...");   return; }
                if (robotArmors == null) { Console.WriteLine("Unable to read Armor_Robot.json. Aborting..."); return; }
                if (robotMods  == null) { Console.WriteLine("Unable to read Robot_Mods.json. Aborting...");  return; }
                if (robotWeapons == null) { Console.WriteLine("Unable to read Weapon_Robot.json. Aborting..."); return; }
                if (clothings  == null) { Console.WriteLine("Unable to read Clothing.json. Aborting...");     return; }
                if (consumeables == null) { Console.WriteLine("Unable to read Consumeables.json. Aborting..."); return; }
                if (items      == null) { Console.WriteLine("Unable to read Item.json. Aborting...");         return; }
                if (ammos      == null) { Console.WriteLine("Unable to read Ammo.json. Aborting...");         return; }

                GenerateDescriptionSchema();

                _fields.Add(new { type = "divider" });

                GenerateOriginSchema(origins, "origin", "Origin");
                GenerateTraitSchema(traits, "traits", "Traits");
                GenerateAttributeSchema(attributes, "attributes", "Attributes");
                GenerateSkillSchema(skills, "skills", "Skills");
                GeneratePerkSchema(perks, "perks", "Perks");
                GeneratePackSchema(packs, origins, "pack", "Pack");
                GenerateRobotModSchema(robotMods, "robotMods", "Mods");
                GenerateRobotWeaponSchema(robotWeapons, "robotWeapons", "Weapons");
                GenerateWeaponSchema(weapons, weaponMods, "weapons", "Weapons");
                GenerateArmorSchema(armors, armorMods, "armors", "Armor");
                GenerateRobotArmorSchema(robotArmors, "robotArmors", "Armor");
                GenerateClothingSchema(clothings, "clothings", "Clothing");
                GenerateConsumeableSchema(consumeables, "consumeables", "Consumeables");
                GenerateItemSchema(items, "items", "Items");
                GenerateAmmoSchema(ammos, "ammos", "Ammo");

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
                className = "form-control",
                tab = "Identity"
            });
            _fields.Add(new
            {
                name = "healthPoints",
                id = "healthPoints",
                label = "Health Points",
                type = "number",
                className = "form-control",
                calculation = "2 + ([attributes.Endurance] * 2)",
                pinnedStat = true
            });
            _fields.Add(new
            {
                name = "initiative",
                id = "initiative",
                label = "Initiative",
                type = "number",
                className = "form-control",
                calculation = "[attributes.Agility] + [attributes.Perception]",
                pinnedStat = true
            }); 
            _fields.Add(new
            {
                name = "defense",
                id = "defense",
                label = "Defense",
                type = "number",
                className = "form-control",
                pinnedStat = true
            });
            _fields.Add(new
            {
                name = "damageResistance",
                id = "damageResistance",
                label = "Damage Resistance",
                type = "number",
                className = "form-control",
                pinnedStat = true
            });
            _fields.Add(new
            {
                name = "meleeDamage",
                id = "meleeDamage",
                label = "Melee Damage",
                type = "number",
                className = "form-control",
                calculation = "Math.max(1, Math.floor([attributes.Strength] / 2))",
                pinnedStat = true
            });                      
            _fields.Add(new
            {
                name = "carryWeight",
                id = "carryWeight",
                label = "Carry Weight",
                type = "number",
                className = "form-control",
                calculation = "150 + ([attributes.Strength] * 10)",
                pinnedStat = true
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
                name = "level",
                id = "level",
                label = "Level",
                type = "number",
                className = "form-control",
                @default = 1,
                tab = "Identity"
            });
            _fields.Add(new
            {
                name = "caps",
                id = "caps",
                label = "Caps",
                type = "number",
                className = "form-control",
                @default = 0,
                tab = "Identity"
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

            obj.tab = "Origins";
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
                component = obj,
                tab = "Origins"
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
                children,
                tab = "Attributes"
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
                children,
                tab = "Skills"
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
                    quantity = perk.Rank,
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
                component = obj,
                tab = "Features"
            };

            _fields.Add(array);
        }

        private static void GeneratePackSchema(List<Pack> packs, List<Origin> origins, string name, string label)
        {
            foreach (var origin in origins)
            {
                var originPacks = packs.Where(p => p.Origin == origin.Name).ToList();
                if (originPacks.Count == 0) continue;

                dynamic obj = new ExpandoObject();
                obj.name = name;
                obj.label = label;
                obj.type = "select";
                obj.className = "form-select";
                obj.dependsOn = new { field = "origin", value = origin.Name };
                obj.options = new List<object>();

                foreach (var pack in originPacks)
                {
                    obj.options.Add(new
                    {
                        value = pack.Name,
                        label = pack.Name,
                        description = pack.Description,
                        bonusAdjustments = pack.BonusAdjustments.Count > 0
                            ? JsonConvert.SerializeObject(pack.BonusAdjustments, _jsonSettings)
                            : null,
                        bonusCharacteristics = pack.BonusCharacteristics.Count > 0
                            ? JsonConvert.SerializeObject(pack.BonusCharacteristics, _jsonSettings)
                            : null,
                        userChoices = pack.UserChoices?.Count > 0
                            ? JsonConvert.SerializeObject(pack.UserChoices, _jsonSettings)
                            : null
                    });
                }

                obj.tab = "Features";
                _fields.Add(obj);
            }
        }

        private static void GenerateWeaponSchema(List<Weapon> weapons, List<WeaponMod> weaponMods, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "modifiableitem";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var weapon in weapons)
            {
                var weaponStats = new List<object>
                {
                    new { label = "DMG:",    value = weapon.Damage.ToString(),  field = "damage" },
                    new { label = "Type:",   value = weapon.DamageType,         field = "damageType" },
                    new { label = "Rate:",   value = weapon.FireRate.ToString(), field = "fireRate" },
                    new { label = "Range:",  value = weapon.Range,              field = "range" },
                    new { label = "Weight:", value = weapon.Weight.ToString(),   field = "weight" }
                };
                if (!string.IsNullOrEmpty(weapon.Ammunition))
                    weaponStats.Add(new { label = "Ammo:",     value = weapon.Ammunition,                   field = "ammunition" });
                if (weapon.Qualities?.Count > 0)
                    weaponStats.Add(new { label = "Qualities:", value = string.Join(", ", weapon.Qualities), field = "qualities" });
                if (weapon.Effects?.Count > 0)
                    weaponStats.Add(new { label = "Effects:",   value = string.Join(", ", weapon.Effects),   field = "effects" });

                obj.options.Add(new
                {
                    value = weapon.Name,
                    label = weapon.Name,
                    cost = ReplaceTokens($"{weapon.Cost} {{{{bottlecap}}}}"),
                    image = weapon.Image,
                    description = weapon.Description,
                    modSets = weapon.ModSets?.Count > 0
                        ? JsonConvert.SerializeObject(weapon.ModSets.Select(m => $"{weapon.WeaponType}.{m}").ToList(), _jsonSettings)
                        : null,
                    stats = JsonConvert.SerializeObject(weaponStats, _jsonSettings)
                });
            }

            obj.modOptions = weaponMods.Select(mod => (object)new
            {
                value = $"{mod.WeaponType}.{mod.Name}",
                label = $"{mod.Prefix} ({mod.Slot})",
                prefix = mod.Prefix,
                slot = mod.Slot,
                description = BuildModDescription(mod),
                bonusAdjustments = mod.BonusAdjustments?.Count > 0
                    ? JsonConvert.SerializeObject(mod.BonusAdjustments, _jsonSettings)
                    : null,
                bonusCharacteristics = mod.BonusCharacteristics?.Count > 0
                    ? JsonConvert.SerializeObject(mod.BonusCharacteristics, _jsonSettings)
                    : null,
                prerequisites = mod.Prerequisites?.Count > 0
                    ? JsonConvert.SerializeObject(mod.Prerequisites, _jsonSettings)
                    : null
            }).ToList();

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj,
                dependsOn =
                        new
                        {
                            field = "origin",
                            value = _nonRobotOrigins
                        },
                tab = "Equipment"
            };

            _fields.Add(array);
        }

        private static void GenerateArmorSchema(List<Armor> armors, List<ArmorMod> armorMods, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "modifiableitem";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var armor in armors)
            {
                var armorStats = new List<object>
                {
                    new { label = "Phys Resist:",     value = armor.ResistancePhysical.ToString(),  field = "resistancePhysical" },
                    new { label = "Eng Resist:",     value = armor.ResistanceEnergy.ToString(),    field = "resistanceEnergy" },
                    new { label = "Rad Resist:",     value = armor.ResistanceRadiation.ToString(), field = "resistanceRadiation" },
                    new { label = "Weight:", value = armor.Weight.ToString(),              field = "weight" }
                };
                if (armor.Locations?.Count > 0)
                    armorStats.Add(new { label = "Locations:", value = string.Join(", ", armor.Locations), field = "locations" });

                // Expand set names to individual mod values for frontend filtering
                var applicableModValues = armorMods
                    .Where(m => armor.ModSets?.Contains(m.Set) == true)
                    .Select(m => $"{m.Set}.{m.Name}")
                    .ToList();

                obj.options.Add(new
                {
                    value = armor.Name,
                    label = armor.Name,
                    cost = ReplaceTokens($"{armor.Cost} {{{{bottlecap}}}}"),
                    description = armor.Description,
                    modSets = applicableModValues.Count > 0
                        ? JsonConvert.SerializeObject(applicableModValues, _jsonSettings)
                        : null,
                    stats = JsonConvert.SerializeObject(armorStats, _jsonSettings)
                });
            }

            obj.modOptions = armorMods.Select(mod => (object)new
            {
                value = $"{mod.Set}.{mod.Name}",
                label = $"{mod.Prefix} ({string.Join("/", mod.Locations ?? [])})",
                prefix = mod.Prefix,
                slot = string.Join("/", mod.Locations ?? []),
                description = BuildModDescription(mod),
                bonusAdjustments = mod.BonusAdjustments?.Count > 0
                    ? JsonConvert.SerializeObject(mod.BonusAdjustments, _jsonSettings)
                    : null,
                bonusCharacteristics = mod.BonusCharacteristics?.Count > 0
                    ? JsonConvert.SerializeObject(mod.BonusCharacteristics, _jsonSettings)
                    : null,
                prerequisites = mod.Prerequisites?.Count > 0
                    ? JsonConvert.SerializeObject(mod.Prerequisites, _jsonSettings)
                    : null
            }).ToList();

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj,
                dependsOn =
                        new
                        {
                            field = "origin",
                            value = _nonRobotOrigins
                        },
                tab = "Equipment"
            };

            _fields.Add(array);
        }

        private static void GenerateRobotModSchema(List<RobotMod> robotMods, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var mods in robotMods)
            {
                obj.options.Add(new
                {
                    value = mods.Name,
                    label = mods.Name,
                    cost = ReplaceTokens($"{mods.Cost} {{{{bottlecap}}}}"),
                    description = mods.Description
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj,
                dependsOn =
                        new
                        {
                            field = "origin",
                            value = _robotOrigins
                        },
                tab = "Equipment"
            };

            _fields.Add(array);
        }

        private static void GenerateRobotWeaponSchema(List<WeaponRobot> weapons, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var weapon in weapons)
            {
                var weaponStats = new List<object>
                {
                    new { label = "DMG:",    value = weapon.Damage.ToString(),  field = "damage" },
                    new { label = "Type:",   value = weapon.DamageType,         field = "damageType" },
                    new { label = "Weight:", value = weapon.Weight.ToString(),   field = "weight" }
                };
                if (weapon.FireRate > 0)
                    weaponStats.Add(new { label = "Rate:",  value = weapon.FireRate.ToString(), field = "fireRate" });
                if (!string.IsNullOrEmpty(weapon.Range))
                    weaponStats.Add(new { label = "Range:", value = weapon.Range,               field = "range" });
                if (!string.IsNullOrEmpty(weapon.Ammunition))
                    weaponStats.Add(new { label = "Ammo:",      value = weapon.Ammunition,                    field = "ammunition" });
                if (weapon.Qualities?.Count > 0)
                    weaponStats.Add(new { label = "Qualities:", value = string.Join(", ", weapon.Qualities),  field = "qualities" });
                if (weapon.Effects?.Count > 0)
                    weaponStats.Add(new { label = "Effects:",   value = string.Join(", ", weapon.Effects),    field = "effects" });

                obj.options.Add(new
                {
                    value = weapon.Name,
                    label = weapon.Name,
                    cost = ReplaceTokens($"{weapon.Cost} {{{{bottlecap}}}}"),
                    image = weapon.Image,
                    description = weapon.Description,
                    installedMods = weapon.InstalledMods?.Count > 0
                        ? JsonConvert.SerializeObject(weapon.InstalledMods, _jsonSettings)
                        : null,
                    stats = JsonConvert.SerializeObject(weaponStats, _jsonSettings)
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj,
                dependsOn = new
                {
                    field = "origin",
                    value = _robotOrigins
                },
                tab = "Equipment"
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
                    cost = ReplaceTokens($"{armor.Cost} {{{{bottlecap}}}}"),
                    description = armor.Description
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj,
                dependsOn =
                        new
                        {
                            field = "origin",
                            value = _robotOrigins
                        },
                tab = "Equipment"
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
                    cost = ReplaceTokens($"{clothing.Cost} {{{{bottlecap}}}}"),
                    description = clothing.Description
                });
            }

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = obj,
                dependsOn =
                        new
                        {
                            field = "origin",
                            value = _nonRobotOrigins
                        },
                tab = "Equipment"
            };

            _fields.Add(array);
        }

        private static void GenerateConsumeableSchema(List<Consumeable> consumeables, string name, string label)
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
                    cost = ReplaceTokens($"{consumeable.Cost} {{{{bottlecap}}}}"),
                    quantity = consumeable.Quantity,
                    description = consumeable.Description
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
                    cost = ReplaceTokens($"{item.Cost} {{{{bottlecap}}}}"),
                    quantity = item.Quantity,
                    description = item.Description
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

        private static void GenerateAmmoSchema(List<Ammo> ammos, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var ammo in ammos)
            {
                obj.options.Add(new
                {
                    value = ammo.Name,
                    label = ammo.Name,
                    cost = ReplaceTokens($"{ammo.Cost} {{{{bottlecap}}}}"),
                    quantity = ammo.Quantity,
                    description = ammo.Description
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

        private static string BuildModDescription(ArmorMod mod) =>
            BuildModDescription(mod.Description, mod.BonusAdjustments, mod.BonusCharacteristics);

        private static string BuildModDescription(RobotMod mod) =>
            BuildModDescription(mod.Description, mod.BonusAdjustments, mod.BonusCharacteristics);

        private static string BuildModDescription(WeaponMod mod) =>
            BuildModDescription(mod.Description, mod.BonusAdjustments, mod.BonusCharacteristics);

        private static string BuildModDescription(string? description, List<BonusAdjustment>? adjustments, List<BonusCharacteristic>? characteristics)
        {
            if (!string.IsNullOrWhiteSpace(description))
                return description;

            var parts = new List<string>();

            foreach (var adj in adjustments ?? [])
            {
                var typeName = CamelCaseToTitle(!string.IsNullOrEmpty(adj.Name) ? adj.Name : adj.Type);
                var sign = adj.Value >= 0 ? "+" : "";
                parts.Add($"{sign}{adj.Value} {typeName}");
            }

            foreach (var ch in characteristics ?? [])
            {
                var typeName = CamelCaseToTitle(ch.Type);
                parts.Add($"Adds {ch.Value} ({typeName})");
            }

            return string.Join(", ", parts);
        }

        private static string CamelCaseToTitle(string camelCase)
        {
            if (string.IsNullOrEmpty(camelCase)) return camelCase;
            var spaced = Regex.Replace(camelCase, "([A-Z])", " $1").Trim();
            return char.ToUpper(spaced[0]) + spaced[1..];
        }
    }
}
