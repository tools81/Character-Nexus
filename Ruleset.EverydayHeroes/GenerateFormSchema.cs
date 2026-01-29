using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Utility;

namespace EverydayHeroes
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = $"{new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent}/Ruleset.EverydayHeroes/Json/";
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
                string jsonArchetypesData = File.ReadAllText(_jsonFilesPath + "Archetypes.json");
                List<Archetype> archetypes = JsonConvert.DeserializeObject<List<Archetype>>(jsonArchetypesData);

                if (archetypes == null)
                {
                    Console.WriteLine($"Unable to read archetypes json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonClassesData = File.ReadAllText(_jsonFilesPath + "Classes.json");
                List<Class> classes = JsonConvert.DeserializeObject<List<Class>>(jsonClassesData);

                if (classes == null)
                {
                    Console.WriteLine($"Unable to read classes json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonBackgroundsData = File.ReadAllText(_jsonFilesPath + "Backgrounds.json");
                List<Background> backgrounds = JsonConvert.DeserializeObject<List<Background>>(jsonBackgroundsData);

                if (backgrounds == null)
                {
                    Console.WriteLine($"Unable to read backgrounds json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonProfessionsData = File.ReadAllText(_jsonFilesPath + "Professions.json");
                List<Profession> professions = JsonConvert.DeserializeObject<List<Profession>>(jsonProfessionsData);

                if (professions == null)
                {
                    Console.WriteLine($"Unable to read professions json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonAttributesData = File.ReadAllText(_jsonFilesPath + "Attributes.json");
                List<Attribute> attributes = JsonConvert.DeserializeObject<List<Attribute>>(jsonAttributesData);

                if (attributes == null)
                {
                    Console.WriteLine($"Unable to read attributes json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonSkillsData = File.ReadAllText(_jsonFilesPath + "Skills.json");
                List<Skill> skills = JsonConvert.DeserializeObject<List<Skill>>(jsonSkillsData);

                if (skills == null)
                {
                    Console.WriteLine($"Unable to read skills json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonFeatsData = File.ReadAllText(_jsonFilesPath + "Feats.json");
                List<Feat> feats = JsonConvert.DeserializeObject<List<Feat>>(jsonFeatsData);

                if (feats == null)
                {
                    Console.WriteLine($"Unable to read feats json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonTalentsData = File.ReadAllText(_jsonFilesPath + "Talents.json");
                List<Talent> talents = JsonConvert.DeserializeObject<List<Talent>>(jsonTalentsData);

                if (talents == null)
                {
                    Console.WriteLine($"Unable to read talents json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonPlansData = File.ReadAllText(_jsonFilesPath + "Plans.json");
                List<Plan> plans = JsonConvert.DeserializeObject<List<Plan>>(jsonPlansData);

                if (plans == null)
                {
                    Console.WriteLine($"Unable to read plans json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonTricksData = File.ReadAllText(_jsonFilesPath + "Tricks.json");
                List<Trick> tricks = JsonConvert.DeserializeObject<List<Trick>>(jsonTricksData);

                if (tricks == null)
                {
                    Console.WriteLine($"Unable to read tricks json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonPacksData = File.ReadAllText(_jsonFilesPath + "Packs.json");
                List<Pack> packs = JsonConvert.DeserializeObject<List<Pack>>(jsonPacksData);

                if (packs == null)
                {
                    Console.WriteLine($"Unable to read packs json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonItemsData = File.ReadAllText(_jsonFilesPath + "Items.json");
                List<Item> items = JsonConvert.DeserializeObject<List<Item>>(jsonItemsData);

                if (items == null)
                {
                    Console.WriteLine($"Unable to read items json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonWeaponsData = File.ReadAllText(_jsonFilesPath + "Weapons.json");
                List<Weapon> weapons = JsonConvert.DeserializeObject<List<Weapon>>(jsonWeaponsData);

                if (weapons == null)
                {
                    Console.WriteLine($"Unable to read weapons json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonArmorsData = File.ReadAllText(_jsonFilesPath + "Armors.json");
                List<Armor> armors = JsonConvert.DeserializeObject<List<Armor>>(jsonArmorsData);

                if (armors == null)
                {
                    Console.WriteLine($"Unable to read armors json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonVehiclesData = File.ReadAllText(_jsonFilesPath + "Vehicles.json");
                List<Vehicle> vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(jsonVehiclesData);

                if (vehicles == null)
                {
                    Console.WriteLine($"Unable to read vehicles json file. Aborting...");
                    Console.Read();
                    return;
                }

                AddDescriptionFields();

                GenerateAttributeSchema(attributes, "attributes", "Ability Scores");
                GenerateArchetypeSchema(archetypes, "archetype", "Archetype");
                GenerateClassSchema(classes, archetypes, "class", "Class");
                GenerateBackgroundSchema(backgrounds, "background", "Background");
                GenerateProfessionSchema(professions, "profession", "Profession");
                GenerateSkillSchema(skills, "skill", "Skills");
                GenerateFeatSchema(feats, "feat", "Feats");
                GenerateTalentSchema(talents, "talent", "Talents");
                GeneratePlanSchema(plans, "plan", "Plans");
                GenerateTrickSchema(tricks, "trick", "Tricks");
                GeneratePackSchema(packs, "pack", "Packs");
                GenerateItemSchema(items, "item", "Items");
                GenerateWeaponSchema(weapons, "weapon", "Weapons");
                GenerateArmorSchema(armors, "armor", "Armors");
                GenerateVehicleSchema(vehicles, "vehicle", "Vehicles");

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

        private static void AddDescriptionFields()
        {
            _fields.Add(
                new
                {
                    name = "id",
                    id = "id",
                    label = "Id",
                    type = "hidden",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "name",
                    id = "name",
                    label = "Name",
                    type = "text",
                    className = "form-control",
                    @default = "Unknown"
                });
            _fields.Add(
                new
                {
                    name = "image",
                    id = "image",
                    label = "Image",
                    type = "image",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "height",
                    id = "height",
                    label = "Height",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "weight",
                    id = "weight",
                    label = "Weight",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "skin",
                    id = "skin",
                    label = "Skin",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "eyes",
                    id = "eyes",
                    label = "Eyes",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "hair",
                    id = "hair",
                    label = "Hair",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "age",
                    id = "age",
                    label = "Age",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "maritalstatus",
                    id = "maritalstatus",
                    label = "Marital Status",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "pronouns",
                    id = "pronouns",
                    label = "Pronouns",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "biography",
                    id = "biography",
                    label = "Biography",
                    type = "textarea",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "notes",
                    id = "notes",
                    label = "Notes",
                    type = "textarea",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "level",
                    id = "level",
                    label = "Level",
                    type = "number",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 1,
                        max = 20
                    },
                    @default = 1
                });            
        }

        private static void GenerateAttributeSchema(List<Attribute> attributes, string name, string label)
        {
            _fields.Add(new
            {
                type = "divider"
            });
            
            var children = new List<object>();

            foreach (var attribute in attributes)
            {
                children.Add(new
                {
                    name = $"attribute.{attribute.Name}",
                    id = $"attribute.{attribute.Name.ToLower()}",
                    label = attribute.Name,
                    type = "number",
                    className = "form-control",
                    image = attribute.Image,
                    validation = new
                    {
                        required = true,
                        min = 3,
                        max = 20
                    },
                    @default = 8
                });
            }

            var group = new
            {
                type = "group",
                name,
                label,
                children
            };

            _fields.Add(group);
        }

        private static void GenerateArchetypeSchema(List<Archetype> archetypes, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = "archetype";
            obj.label = "Archetype";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var archetype in archetypes)
            {
                obj.options.Add(
                    new
                    {
                        value = archetype.Name,
                        label = archetype.Name,
                        description = archetype.Description
                    }
                );
            }

            _fields.Add(obj);         
        }

        private static void GenerateClassSchema(List<Class> classes, List<Archetype> archetypes, string name, string label)
        {
            foreach (var archetype in archetypes)
            {
                dynamic obj = new ExpandoObject();

                obj.name = $"class.{archetype.Name.ToLower()}";
                obj.label = "Class";
                obj.type = "select";
                obj.className = "form-select";
                obj.options = new List<object>();

                foreach (var cl in classes.Where(c => c.Archetype == archetype.Name))
                {
                    if (cl.BonusCharacteristics != null) UpdateBonusCharacteristicValues(cl.BonusCharacteristics);

                    obj.options.Add(
                        new
                        {
                            value = cl.Name,
                            label = cl.Name,
                            image = cl.Image,
                            description = cl.Description,
                            bonusCharacteristics = JsonConvert.SerializeObject(cl.BonusCharacteristics, _jsonSettings),
                            userChoices = JsonConvert.SerializeObject(cl.UserChoices, _jsonSettings)
                        }
                    );

                    obj.dependsOn =
                        new
                        {
                            field = "archetype",
                            value = archetype.Name
                        };
                }

                _fields.Add(obj);
            }
        }

        private static void GenerateBackgroundSchema(List<Background> backgrounds, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = "background";
            obj.label = "Background";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var background in backgrounds.OrderBy(t => t.Name))
            {
                if (background.BonusCharacteristics != null) UpdateBonusCharacteristicValues(background.BonusCharacteristics);

                obj.options.Add(
                    new
                    {
                        value = background.Name,
                        label = background.Name,
                        description = background.Description,
                        bonusCharacteristics = JsonConvert.SerializeObject(background.BonusCharacteristics, _jsonSettings),
                        bonusAdjustments = JsonConvert.SerializeObject(background.BonusAdjustments, _jsonSettings),
                        userChoices = JsonConvert.SerializeObject(background.UserChoices, _jsonSettings)
                    }
                );
            }

            _fields.Add(obj);
        }        

        private static void GenerateProfessionSchema(List<Profession> professions, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = "profession";
            obj.label = "Profession";
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var profession in professions.OrderBy(t => t.Name))
            {
                if (profession.BonusCharacteristics != null) UpdateBonusCharacteristicValues(profession.BonusCharacteristics);

                obj.options.Add(
                    new
                    {
                        value = profession.Name,
                        label = profession.Name,
                        description = profession.Description,
                        bonusCharacteristics = JsonConvert.SerializeObject(profession.BonusCharacteristics, _jsonSettings),
                        bonusAdjustments = JsonConvert.SerializeObject(profession.BonusAdjustments, _jsonSettings),
                        userChoices = JsonConvert.SerializeObject(profession.UserChoices, _jsonSettings)
                    }
                );
            }

            _fields.Add(obj);
        }

        private static void GenerateSkillSchema(List<Skill> skills, string name, string label)
        {        
            _fields.Add(new
            {
                type = "divider"
            });

            _fields.Add(new
            {
                type = "textblock",
                label,
                text = label,
                name
            });

            foreach (var skill in skills)
            {
                var children = new List<object>
                {
                    new
                    {
                        name = $"skill.proficient.{skill.Name}",
                        id = $"skill.proficient.{skill.Name.ToLower()}",
                        label = "",
                        type = "switch"
                    },
                    new
                    {
                        name = $"skill.expertise.{skill.Name}",
                        id = $"skill.expertise.{skill.Name.ToLower()}",
                        label = "",
                        type = "switch"
                    },
                    new
                    {
                        name,
                        label,
                        text = skill.Name,
                        type = "textblock"
                    }
                };

                var group = new
                {
                    type = "group",
                    name = $"skills.{skill.Name}",
                    includeLabel = false,
                    label = "",
                    children
                };

                _fields.Add(group);
            } 

            _fields.Add(new
            {
                type = "divider"
            });           
        }

        private static void GenerateFeatSchema(List<Feat> feats, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var feat in feats.OrderBy(t => t.Name))
            {
                if (feat.BonusCharacteristics != null) UpdateBonusCharacteristicValues(feat.BonusCharacteristics);

                obj.options.Add(
                    new
                    {
                        value = feat.Name,
                        label = feat.Name,
                        description = feat.Description,
                        bonusCharacteristics = JsonConvert.SerializeObject(feat.BonusCharacteristics, _jsonSettings),
                        bonusAdjustments = JsonConvert.SerializeObject(feat.BonusAdjustments, _jsonSettings),
                        userChoices = JsonConvert.SerializeObject(feat.UserChoices, _jsonSettings),
                        prerequisites = JsonConvert.SerializeObject(feat.Prerequisites, _jsonSettings)
                    }
                );
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

        private static void GenerateTalentSchema(List<Talent> talents, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var talent in talents.OrderBy(t => t.Name))
            {
                obj.options.Add(
                    new
                    {
                        value = talent.Name,
                        label = talent.Name,
                        description = talent.Description
                    }
                );
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

        private static void GeneratePlanSchema(List<Plan> plans, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var plan in plans.OrderBy(t => t.Name))
            {
                obj.options.Add(
                    new
                    {
                        value = plan.Name,
                        label = plan.Name,
                        description = plan.Description
                    }
                );
            }

            obj.dependsOn =
            new
            {
                field = "archetype",
                value = "Smart"
            };

            dynamic array = new
            {
                name,
                label,
                type = "array",
                dependsOn = obj.dependsOn,
                component = obj
            };

            _fields.Add(array);
        }

        private static void GenerateTrickSchema(List<Trick> tricks, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var trick in tricks.OrderBy(t => t.Class))
            {
                obj.options.Add(
                    new
                    {
                        value = trick.Name,
                        label = trick.Name,
                        description = trick.Description
                    }
                );
            }

            obj.dependsOn =
            new
            {
                field = "archetype",
                value = "Charming"
            };

            dynamic array = new
            {
                name,
                label,
                type = "array",
                dependsOn = obj.dependsOn,
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

            foreach (var pack in packs.OrderBy(t => t.Name))
            {
                obj.options.Add(
                    new
                    {
                        value = pack.Name,
                        label = pack.Name,
                        description = pack.Description,
                        bonusCharacteristics = JsonConvert.SerializeObject(pack.BonusCharacteristics, _jsonSettings)
                    }
                );
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

            foreach (var item in items.OrderBy(t => t.Name))
            {
                obj.options.Add(
                    new
                    {
                        value = item.Name,
                        label = item.Name,
                        description = item.Description
                    }
                );
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

        private static void GenerateWeaponSchema(List<Weapon> weapons, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var weapon in weapons.OrderBy(t => t.Name))
            {
                obj.options.Add(
                    new
                    {
                        value = weapon.Name,
                        label = weapon.Name,
                        description = weapon.Description
                    }
                );
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

            foreach (var armor in armors.OrderBy(t => t.Name))
            {
                obj.options.Add(
                    new
                    {
                        value = armor.Name,
                        label = armor.Name,
                        description = armor.Description
                    }
                );
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

        private static void GenerateVehicleSchema(List<Vehicle> vehicles, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var vehicle in vehicles.OrderBy(t => t.Name))
            {
                obj.options.Add(
                    new
                    {
                        value = vehicle.Name,
                        label = vehicle.Name,
                        description = vehicle.Description
                    }
                );
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