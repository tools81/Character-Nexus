using System.Dynamic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Utility;

namespace Marvel
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = "C:/Users/toole/OneDrive/Source/Character Nexus/Ruleset.Marvel/Json/";
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
                string jsonOriginsData = File.ReadAllText(_jsonFilesPath + "Origins.json");
                string jsonOccupationsData = File.ReadAllText(_jsonFilesPath + "Occupations.json");
                string jsonTagsData = File.ReadAllText(_jsonFilesPath + "Tags.json");
                string jsonTraitsData = File.ReadAllText(_jsonFilesPath + "Traits.json");
                string jsonWeaponsData = File.ReadAllText(_jsonFilesPath + "Weapons.json");
                string jsonAttributesData = File.ReadAllText(_jsonFilesPath + "Attributes.json");
                string jsonPowersetsData = File.ReadAllText(_jsonFilesPath + "Powersets.json");
                string jsonPowersData = File.ReadAllText(_jsonFilesPath + "Powers.json");


                List<Origin>? origins = JsonConvert.DeserializeObject<List<Origin>?>(jsonOriginsData);

                if (origins == null)
                {
                    Console.WriteLine($"Unable to read origins json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Occupation>? occupations = JsonConvert.DeserializeObject<List<Occupation>?>(jsonOccupationsData);

                if (occupations == null)
                {
                    Console.WriteLine($"Unable to read occupations json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Tag>? tags = JsonConvert.DeserializeObject<List<Tag>?>(jsonTagsData);

                if (tags == null)
                {
                    Console.WriteLine($"Unable to read tags json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Trait>? traits = JsonConvert.DeserializeObject<List<Trait>?>(jsonTraitsData);

                if (traits == null)
                {
                    Console.WriteLine($"Unable to read traits json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Weapon>? weapons = JsonConvert.DeserializeObject<List<Weapon>?>(jsonWeaponsData);

                if (weapons == null)
                {
                    Console.WriteLine($"Unable to read weapons json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Attribute>? attributes = JsonConvert.DeserializeObject<List<Attribute>?>(jsonAttributesData);

                if (attributes == null)
                {
                    Console.WriteLine($"Unable to read attributes json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Powerset>? powersets = JsonConvert.DeserializeObject<List<Powerset>?>(jsonPowersetsData);

                if (powersets == null)
                {
                    Console.WriteLine($"Unable to read powersets json file. Aborting...");
                    Console.Read();
                    return;
                }

                List<Power>? powers = JsonConvert.DeserializeObject<List<Power>?>(jsonPowersData);

                if (powers == null)
                {
                    Console.WriteLine($"Unable to read powers json file. Aborting...");
                    Console.Read();
                    return;
                }

                GenerateDescriptionSchema();

                _fields.Add(new
                {
                    type = "divider"
                });

                GenerateOriginSchema(origins, "origin", "Origin");
                GenerateOccupationSchema(occupations, "occupation", "Occupation");
                GenerateAttributeSchema(attributes, "attributes", "Ability Scores");
                GenerateTraitSchema(traits, "trait", "Traits");
                GenerateTagSchema(tags, "tag", "Tags");
                GenerateWeaponSchema(weapons, "weapon", "Weapons");
                GeneratePowersSchema(powersets, powers, "power", "Powers");                

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
            _fields.Add(
                new
                {
                    name = "id",
                    id = "id",
                    label = "Id",
                    type = "hidden",
                    className = "form-control"
                }
            );
            _fields.Add(
                new
                {
                    name = "health",
                    id = "health",
                    label = "Health",
                    type = "hidden",
                    className = "form-control",
                    calculation = "[attributes.Resilience] * 30 === 0 ? 15 : [attributes.Resilience] * 30",
                    @default = 15
                }
            );
            _fields.Add(
                new
                {
                    name = "focus",
                    id = "focus",
                    label = "Focus",
                    type = "hidden",
                    className = "form-control",
                    calculation = "[attributes.Vigilance] * 30 === 0 ? 15 : [attributes.Vigilance] * 30",
                    @default = 15
                }
            );
            _fields.Add(
                new
                {
                    name = "healthDamageReduction",
                    id = "healthDamageReduction",
                    label = "Health Damage Reduction",
                    type = "hidden",
                    className = "form-control"
                }
            );
            _fields.Add(
                new
                {
                    name = "focusDamageReduction",
                    id = "focusDamageReduction",
                    label = "Focus Damage Reduction",
                    type = "hidden",
                    className = "form-control"
                }
            );
            _fields.Add(
                new
                {
                    name = "run",
                    id = "run",
                    label = "Run",
                    type = "hidden",
                    className = "form-control",
                    calculation = "5 + ([attributes.Agility] % 5 == 0 ? [attributes.Agility] / 5 : 0)"
                }
            );
            _fields.Add(
                new
                {
                    name = "climb",
                    id = "climb",
                    label = "Climb",
                    type = "hidden",
                    className = "form-control",
                    calculation = "Math.ceil([run] / 2)"
                }
            );
            _fields.Add(
                new
                {
                    name = "swim",
                    id = "swim",
                    label = "Swim",
                    type = "hidden",
                    className = "form-control",
                    calculation = "Math.ceil([run] / 2)"
                }
            );
            _fields.Add(
                new
                {
                    name = "karma",
                    id = "karma",
                    label = "Karma",
                    type = "hidden",
                    className = "form-control",
                    calculation = "[rank]"
                }
            );
            _fields.Add(
                new
                {
                    name = "initiativeModifier",
                    id = "initiativeModifier",
                    label = "Initiative Modifier",
                    type = "hidden",
                    className = "form-control",
                    calculation = "[attributes.Vigilance]"
                }
            );
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
                    name = "realName",
                    id = "realName",
                    label = "Real Name",
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
                    name = "rank",
                    id = "rank",
                    label = "Rank",
                    type = "number",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 1,
                        max = 6
                    },
                    @default = 1
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
                    name = "gender",
                    id = "gender",
                    label = "Gender",
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
                    name = "size",
                    id = "size",
                    label = "Size",
                    type = "text",
                    className = "form-control",
                    @default = "Average"
                });
            _fields.Add(
                new
                {
                    name = "distinguishingFeatures",
                    id = "distinguishingFeatures",
                    label = "Distinguishing Features",
                    type = "textarea",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "teams",
                    id = "teams",
                    label = "Teams",
                    type = "text",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "base",
                    id = "base",
                    label = "Base",
                    type = "text",
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
                    name = "history",
                    id = "history",
                    label = "History",
                    type = "textarea",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "personality",
                    id = "personality",
                    label = "Personality",
                    type = "textarea",
                    className = "form-control"
                });
        }

        public static void GenerateOriginSchema(List<Origin> elements, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var element in elements)
            {
                obj.options.Add(
                    new
                    {
                        value = element.Name,
                        label = element.Name,
                        bonusCharacteristics = JsonConvert.SerializeObject(element.BonusCharacteristics, _jsonSettings)
                    }
                );
            }

            _fields.Add(obj);

            //Add a div below dropdown after selecting a value, containing details of Origin
            foreach (var element in elements)
            {
                var children = new List<object>
                {
                    new
                    {
                        name = $"info-{name}-{element.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = element.Description
                    }
                };

                var div = new
                {
                    type = "div",
                    className = "alert alert-secondary",
                    children,
                    dependsOn =
                        new
                        {
                            field = name,
                            value = element.Name
                        }
                };

                _fields.Add(div);
            }
        }

        public static void GenerateOccupationSchema(List<Occupation> elements, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var element in elements)
            {
                obj.options.Add(
                    new
                    {
                        value = element.Name,
                        label = element.Name,
                        bonusCharacteristics = JsonConvert.SerializeObject(element.BonusCharacteristics, _jsonSettings)
                    }
                );
            }

            _fields.Add(obj);

            foreach (var element in elements)
            {
                var children = new List<object>
                {
                    new
                    {
                        name = $"info-{name}-{element.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = element.Description
                    }
                };

                var div = new
                {
                    type = "div",
                    className = "alert alert-secondary",
                    children,
                    dependsOn =
                        new
                        {
                            field = name,
                            value = element.Name
                        }
                };

                _fields.Add(div);
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
                    validation = new
                    {
                        required = true,
                        min = -3,
                        max = 9
                    },
                    @default = 0
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

        public static void GenerateTraitSchema(List<Trait> elements, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var element in elements)
            {
                obj.options.Add(
                    new
                    {
                        value = element.Name,
                        label = element.Name
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

        private static void GeneratePowersSchema(List<Powerset> powersets, List<Power> powers, string name, string label)
        {

            var accordion = new
            {
                id = name,
                label,
                type = "accordion",
                items = new List<object>()
            };

            foreach (var powerset in powersets)
            {
                dynamic accordionItem = new ExpandoObject();
                accordionItem.header = powerset.Name;
                accordionItem.name = powerset.Name.ReplaceWhitespace("");
                accordionItem.image = powerset.imageUrl;
                accordionItem.component = new
                {
                    type = "listgroup",
                    items = new List<object>()
                };

                foreach (var power in powers.Where(p => p.Powersets.Contains(powerset.Name)))
                {
                    var component = new
                    {
                        name = $"powers.{power.Name.ToLower()}",
                        id = $"{powerset.Name}-{power.Name}",
                        label = power.Name,
                        type = "switch",
                        bonusAdjustments = JsonConvert.SerializeObject(power.BonusAdjustments, _jsonSettings),
                        prerequisites = JsonConvert.SerializeObject(power.Prerequisites, _jsonSettings)
                    };

                    var text = new
                    {
                        name = $"info-power-{powerset.Name}-{power.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = power.Description
                    };

                    accordionItem.component.items.Add(
                        new
                        {
                            component,
                            text
                        }
                    );
                }
                accordion.items.Add(accordionItem);
            }

            _fields.Add(accordion);
        }

        private static void GenerateTagSchema(List<Tag> tags, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var tag in tags)
            {
                obj.options.Add(
                    new
                    {
                        value = tag.Name,
                        label = tag.Name
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

            foreach (var weapon in weapons)
            {
                obj.options.Add(
                    new
                    {
                        value = weapon.Name,
                        label = weapon.Name
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
    }
}