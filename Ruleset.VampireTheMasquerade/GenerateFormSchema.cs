using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Utility;

namespace VampireTheMasquerade
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = $"{new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent}/Ruleset.VampireTheMasquerade/Json/";
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
                string jsonClansData = File.ReadAllText(_jsonFilesPath + "Clans.json");
                List<Clan> clans = JsonConvert.DeserializeObject<List<Clan>>(jsonClansData);

                if (clans == null)
                {
                    Console.WriteLine($"Unable to read clans json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonPredatorsData = File.ReadAllText(_jsonFilesPath + "Predators.json");
                List<Predator> predators = JsonConvert.DeserializeObject<List<Predator>>(jsonPredatorsData);

                if (predators == null)
                {
                    Console.WriteLine($"Unable to read predators json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonGenerationsData = File.ReadAllText(_jsonFilesPath + "Generations.json");
                List<Generation> generations = JsonConvert.DeserializeObject<List<Generation>>(jsonGenerationsData);

                if (generations == null)
                {
                    Console.WriteLine($"Unable to read generations json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonCoteriesData = File.ReadAllText(_jsonFilesPath + "Coteries.json");
                List<Coterie> coteries = JsonConvert.DeserializeObject<List<Coterie>>(jsonCoteriesData);

                if (coteries == null)
                {
                    Console.WriteLine($"Unable to read coteries json file. Aborting...");
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

                string jsonSpecialtiesData = File.ReadAllText(_jsonFilesPath + "Specialties.json");
                List<Specialty> specialties = JsonConvert.DeserializeObject<List<Specialty>>(jsonSpecialtiesData);

                if (specialties == null)
                {
                    Console.WriteLine($"Unable to read specialties json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonDisciplinesData = File.ReadAllText(_jsonFilesPath + "Disciplines.json");
                List<Discipline> disciplines = JsonConvert.DeserializeObject<List<Discipline>>(jsonDisciplinesData);

                if (disciplines == null)
                {
                    Console.WriteLine($"Unable to read disciplines json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonPowersData = File.ReadAllText(_jsonFilesPath + "Powers.json");
                List<Power> powers = JsonConvert.DeserializeObject<List<Power>>(jsonPowersData);

                if (powers == null)
                {
                    Console.WriteLine($"Unable to read powers json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonAdvantagesData = File.ReadAllText(_jsonFilesPath + "Advantages.json");
                List<Advantage> advantages = JsonConvert.DeserializeObject<List<Advantage>>(jsonAdvantagesData);

                if (advantages == null)
                {
                    Console.WriteLine($"Unable to read advantages json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonMeritsData = File.ReadAllText(_jsonFilesPath + "Merits.json");
                List<Merit> merits = JsonConvert.DeserializeObject<List<Merit>>(jsonMeritsData);

                if (powers == null)
                {
                    Console.WriteLine($"Unable to read powers json file. Aborting...");
                    Console.Read();
                    return;
                }

                string jsonFlawsData = File.ReadAllText(_jsonFilesPath + "Flaws.json");
                List<Flaw> flaws = JsonConvert.DeserializeObject<List<Flaw>>(jsonFlawsData);

                if (flaws == null)
                {
                    Console.WriteLine($"Unable to read flaws json file. Aborting...");
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

                string jsonRitualsData = File.ReadAllText(_jsonFilesPath + "Rituals.json");
                List<Ritual> rituals = JsonConvert.DeserializeObject<List<Ritual>>(jsonRitualsData);

                if (rituals == null)
                {
                    Console.WriteLine($"Unable to read rituals json file. Aborting...");
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

                string jsonGearsData = File.ReadAllText(_jsonFilesPath + "Gears.json");
                List<Gear> gears = JsonConvert.DeserializeObject<List<Gear>>(jsonGearsData);

                if (gears == null)
                {
                    Console.WriteLine($"Unable to read gears json file. Aborting...");
                    Console.Read();
                    return;
                }

                GenerateDescriptionSchema();

                GenerateClanSchema(clans, "clan", "Clan");
                GeneratePredatorSchema(predators, "predator", "Predator");
                GenerateGenerationSchema(generations, "generation", "Generation");
                GenerateCoterieSchema(coteries, "coterie", "Coterie"); 
                GenerateAttributeSchema(attributes, "attributes", "Attributes");
                GenerateSkillSchema(skills, specialties, "skill", "Skills");
                GenerateDisciplineSchema(disciplines, powers, "disciplines", "Disciplines");
                GenerateRitualSchema(rituals, "rituals", "Rituals");                                               
                GenerateAdvantageSchema(advantages, backgrounds, merits, flaws, "advantages", "Advantages");
                GenerateWeaponSchema(weapons, "weapons", "Weapons");
                GenerateArmorSchema(armors, "armors", "Armors");
                GenerateGearSchema(gears, "gears", "Gears");

                GenerateTemporaryValuesSchema();

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
            catch(Exception ex)
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
                    name = "appearance",
                    id = "appearance",
                    label = "Appearance",
                    type = "textarea",
                    className = "form-control",
                    @default = "Unknown"
                });
            _fields.Add(
                new
                {
                    name = "distinguishingFeatures",
                    id = "distinguishingFeatures",
                    label = "Distinguishing Features",
                    type = "textarea",
                    className = "form-control",
                    @default = "Unknown"
                });
            _fields.Add(
                new
                {
                    name = "concept",
                    id = "concept",
                    label = "Concept",
                    type = "text",
                    className = "form-control",
                    @default = "Unknown"
                });
            _fields.Add(
                new
                {
                    name = "age",
                    id = "age",
                    label = "Age",
                    type = "number",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "dateOfBirth",
                    id = "dateOfBirth",
                    label = "Date of Birth",
                    type = "date",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "dateOfDeath",
                    id = "dateOfDeath",
                    label = "Date of Death",
                    type = "date",
                    className = "form-control"
                });
            _fields.Add(
                new
                {
                    name = "sire",
                    id = "sire",
                    label = "Sire",
                    type = "text",
                    className = "form-control",
                    @default = "Unknown"
                });
            _fields.Add(
                new
                {
                    name = "chronicle",
                    id = "chronicle",
                    label = "Chronicle",
                    type = "text",
                    className = "form-control",
                    @default = "Unknown"
                });
            _fields.Add(
                new
                {
                    name = "ambition",
                    id = "ambition",
                    label = "Ambition",
                    type = "text",
                    className = "form-control",
                    @default = "Unknown"
                });
            _fields.Add(
                new
                {
                    name = "desire",
                    id = "desire",
                    label = "Desire",
                    type = "text",
                    className = "form-control",
                    @default = "Unknown"
                });
            _fields.Add(
                new
                {
                    name = "chronicleTenets",
                    id = "chronicleTenets",
                    label = "Chronicle Tenets",
                    type = "textarea",
                    className = "form-control",
                    @default = "Unknown"
                });
            _fields.Add(
                new
                {
                    name = "touchstonesConvictions",
                    id = "touchstonesConvictions",
                    label = "Touchstones and Convictions",
                    type = "textarea",
                    className = "form-control",
                    @default = "Unknown"
                }); 
            _fields.Add(
                new
                {
                    name = "notes",
                    id = "notes",
                    label = "Notes",
                    type = "textarea",
                    className = "form-control",
                    @default = "Unknown"
                });  
            _fields.Add(
                new
                {
                    name = "history",
                    id = "history",
                    label = "History",
                    type = "textarea",
                    className = "form-control",
                    @default = "Unknown"
                });         
        }

        private static void GenerateTemporaryValuesSchema()
        {     
            _fields.Add( new
            {
                name = "health.label",
                text = "Health",
                type = "textblock"    
            });
            _fields.Add(
                new
                {
                    name = "health",
                    id = "health",
                    includeLabel = false,
                    type = "radiogroup",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 0,
                        max = 15
                    },
                    count = 15,
                    @default = 3
                });

            _fields.Add( new
            {
                name = "willpower.label",
                text = "Willpower",
                type = "textblock"    
            });
            _fields.Add(
                new
                {
                    name = "willpower",
                    id = "willpower",
                    includeLabel = false,
                    type = "radiogroup",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 0,
                        max = 15
                    },
                    count = 15,
                    @default = 0
                });
            
            _fields.Add( new
            {
                name = "humanity.label",
                text = "Humanity",
                type = "textblock"    
            });
            _fields.Add(
                new
                {
                    name = "humanity",
                    id = "humanity",
                    includeLabel = false,
                    type = "radiogroup",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 0,
                        max = 10
                    },
                    count = 10,
                    @default = 7
                });

            _fields.Add( new
            {
                name = "bloodpotency.label",
                text = "Blood Potency",
                type = "textblock"    
            });
            _fields.Add(
                new
                {
                    name = "bloodpotency",
                    id = "bloodpotency",
                    includeLabel = false,
                    type = "radiogroup",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 0,
                        max = 10
                    },
                    count = 10,
                    @default = 1
                });

            _fields.Add( new
            {
                name = "hunger.label",
                text = "Hunger",
                type = "textblock"    
            });
            _fields.Add(
                new
                {
                    name = "hunger",
                    id = "hunger",
                    includeLabel = false,
                    type = "radiogroup",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 0,
                        max = 5
                    },
                    count = 5,
                    @default = 0
                });

            _fields.Add( new
            {
                type = "divider"    
            });
            
            _fields.Add(
                new
                {
                    name = "totalExperience",
                    id = "totalExperience",
                    label = "Total Experience",
                    type = "number",
                    className = "form-control",
                    @default = 0
                });
            _fields.Add(
                new
                {
                    name = "spentExperience",
                    id = "spentExperience",
                    label = "Spent Experience",
                    type = "number",
                    className = "form-control",
                    @default = 0
                });
        }

        private static void GenerateClanSchema(List<Clan> clans, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var clan in clans)
            {
                obj.options.Add(
                    new
                    {
                        value = clan.Name,
                        label = clan.Name,
                        image = clan.Image,
                        description = $"{clan.Description}<br /><b>Bane: </b>{clan.Bane}<br /><b>Compulsion: </b>{clan.Compulsion}",
                        bonusCharacteristics = JsonConvert.SerializeObject(clan.BonusCharacteristics, _jsonSettings)
                    }
                );
            }

            _fields.Add(obj);
        }

        private static void GenerateGenerationSchema(List<Generation> generations, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var generation in generations)
            {
                obj.options.Add(
                    new
                    {
                        value = generation.Name,
                        label = generation.Name,
                        description = generation.Description,
                        bonusAdjustments = JsonConvert.SerializeObject(generation.BonusAdjustments, _jsonSettings)
                    }
                );
            }

            _fields.Add(obj);
        }

        private static void GenerateCoterieSchema(List<Coterie> coteries, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var coterie in coteries)
            {
                obj.options.Add(
                    new
                    {
                        value = coterie.Name,
                        label = coterie.Name,
                        description = coterie.Description
                    }
                );
            }

            _fields.Add(obj);
        }

        private static void GenerateAttributeSchema(List<Attribute> attributes, string name, string label)
        {              
            _fields.Add( new
            {
                name = $"{name}.label",
                text = label,
                type = "textblock"    
            });

            _fields.Add( new
            {
                type = "divider"    
            });

            var physicalChildren = new List<object>();

            foreach (var attribute in attributes.Where(a => a.Aspect == "Physical"))
            {
                physicalChildren.Add(new
                {
                    name = $"attributes.{attribute.Name}",
                    id = $"attributes.{attribute.Name.ToLower()}",
                    label = attribute.Name,
                    type = "radiogroup",
                    className = "form-control",
                    count = 5,
                    validation = new
                    {
                        required = true,
                        min = 0,
                        max = 5
                    },
                    @default = 1
                });
            }

            var physicalGroup = new
            {
                type = "group",
                name = $"{name}.physical",
                label = "Physical",
                children = physicalChildren
            };

            _fields.Add(physicalGroup);

            var socialChildren = new List<object>();

            foreach (var attribute in attributes.Where(a => a.Aspect == "Social"))
            {
                socialChildren.Add(new
                {
                    name = $"attributes.{attribute.Name}",
                    id = $"attributes.{attribute.Name.ToLower()}",
                    label = attribute.Name,
                    type = "radiogroup",
                    className = "form-control",
                    count = 5,
                    validation = new
                    {
                        required = true,
                        min = 0,
                        max = 5
                    },
                    @default = 0
                });
            }

            var socialGroup = new
            {
                type = "group",
                name = $"{name}.social",
                label = "Social",
                children = socialChildren
            };

            _fields.Add(socialGroup);

            var mentalChildren = new List<object>();

            foreach (var attribute in attributes.Where(a => a.Aspect == "Mental"))
            {
                mentalChildren.Add(new
                {
                    name = $"attributes.{attribute.Name}",
                    id = $"attributes.{attribute.Name.ToLower()}",
                    label = attribute.Name,
                    type = "radiogroup",
                    className = "form-control",
                    count = 5,
                    validation = new
                    {
                        required = true,
                        min = 0,
                        max = 5
                    },
                    @default = 0
                });
            }

            var mentalGroup = new
            {
                type = "group",
                name = $"{name}.mental",
                label = "Mental",
                children = mentalChildren
            };

            _fields.Add(mentalGroup);

            _fields.Add( new
            {
                type = "divider"    
            });
        }

        private static void GenerateSkillSchema(List<Skill> skills, List<Specialty> specialties, string name, string label)
        {
            _fields.Add( new
            {
                name = $"{name}.label",
                text = label,
                type = "textblock"    
            });

            _fields.Add( new
            {
                type = "divider"    
            });

            var physicalChildren = new List<object>();

            foreach (var skill in skills.Where(s => s.Aspect == "Physical"))
            {
                var skillSpecialtyList = new List<object>
                {
                    new
                    {
                        name = $"skills.{skill.Name}",
                        id = $"skills.{skill.Name.ToLower()}",
                        label = skill.Name,
                        type = "radiogroup",
                        count = 5,
                        className = "form-control",
                        validation = new
                        {
                            required = true,
                            min = 0,
                            max = 5
                        },
                        @default = 0
                    }
                };

                dynamic obj = new ExpandoObject();

                obj.name = $"specialties.{skill.Name}";
                obj.label = "Specialty";
                obj.includeLabel = false;
                obj.type = "select";
                obj.className = "form-select";
                obj.options = new List<object>();

                foreach (var specialty in specialties.Where(s => s.Skill == skill.Name))
                {
                    obj.options.Add(
                        new
                        {
                            value = specialty.Name,
                            label = specialty.Name
                        }
                    );
                }

                dynamic array = new
                {
                    name = $"{skill.Name}.specialties",
                    includeLabel = false,
                    type = "array",
                    component = obj
                };

                skillSpecialtyList.Add(new { type = "linebreak" });
                skillSpecialtyList.Add(array);                

                var groupSkillSpecialty = new
                {
                    type = "group",
                    name = $"{skill.Name}.specialties",
                    label = skill.Name,
                    children = skillSpecialtyList
                };

                physicalChildren.Add(groupSkillSpecialty);
            }

            var physicalGroup = new
            {
                type = "group",
                name = $"{name}.physical",
                label = "Physical",
                children = physicalChildren
            };

            _fields.Add(physicalGroup);

            var socialChildren = new List<object>();

            foreach (var skill in skills.Where(s => s.Aspect == "Social"))
            {
                var skillSpecialtyList = new List<object>
                {
                    new
                    {
                        name = $"skills.{skill.Name}",
                        id = $"skills.{skill.Name.ToLower()}",
                        label = skill.Name,
                        type = "radiogroup",
                        count = 5,
                        className = "form-control",
                        validation = new
                        {
                            required = true,
                            min = 0,
                            max = 5
                        },
                        @default = 0
                    }
                };

                dynamic obj = new ExpandoObject();

                obj.name = $"specialties.{skill.Name}";
                obj.label = "Specialty";
                obj.includeLabel = false;
                obj.type = "select";
                obj.className = "form-select";
                obj.options = new List<object>();

                foreach (var specialty in specialties.Where(s => s.Skill == skill.Name))
                {
                    obj.options.Add(
                        new
                        {
                            value = specialty.Name,
                            label = specialty.Name
                        }
                    );
                }

                dynamic array = new
                {
                    name = $"{skill.Name}.specialties",
                    includeLabel = false,
                    type = "array",
                    component = obj
                };

                skillSpecialtyList.Add(new { type = "linebreak" });
                skillSpecialtyList.Add(array);

                var groupSkillSpecialty = new
                {
                    type = "group",
                    name = $"{skill.Name}.specialties",
                    label = skill.Name,
                    children = skillSpecialtyList
                };

                socialChildren.Add(groupSkillSpecialty);
            }

            var socialGroup = new
            {
                type = "group",
                name = $"{name}.social",
                label = "Social",
                children = socialChildren
            };

            _fields.Add(socialGroup);

            var mentalChildren = new List<object>();

            foreach (var skill in skills.Where(s => s.Aspect == "Mental"))
            {
                var skillSpecialtyList = new List<object>
                {
                    new
                    {
                        name = $"skills.{skill.Name}",
                        id = $"skills.{skill.Name.ToLower()}",
                        label = skill.Name,
                        type = "radiogroup",
                        count = 5,
                        className = "form-control",
                        validation = new
                        {
                            required = true,
                            min = 0,
                            max = 5
                        },
                        @default = 0
                    }
                };

                dynamic obj = new ExpandoObject();

                obj.name = $"specialties.{skill.Name}";
                obj.label = "Specialty";
                obj.includeLabel = false;
                obj.type = "select";
                obj.className = "form-select";
                obj.options = new List<object>();

                foreach (var specialty in specialties.Where(s => s.Skill == skill.Name))
                {
                    obj.options.Add(
                        new
                        {
                            value = specialty.Name,
                            label = specialty.Name
                        }
                    );
                }

                dynamic array = new
                {
                    name = $"{skill.Name}.specialties",
                    includeLabel = false,
                    type = "array",
                    component = obj
                };

                skillSpecialtyList.Add(new { type = "linebreak" });
                skillSpecialtyList.Add(array);

                var groupSkillSpecialty = new
                {
                    type = "group",
                    name = $"{skill.Name}.specialties",
                    label = skill.Name,
                    children = skillSpecialtyList
                };

                mentalChildren.Add(groupSkillSpecialty);
            }

            var mentalGroup = new
            {
                type = "group",
                name = $"{name}.mental",
                label = "Mental",
                children = mentalChildren
            };

            _fields.Add(mentalGroup);

            _fields.Add( new
            {
                type = "divider"    
            });
        }

        private static void GeneratePredatorSchema(List<Predator> predators, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var predator in predators)
            {
                obj.options.Add(
                    new
                    {
                        value = predator.Name,
                        label = predator.Name,
                        description = predator.Description,
                        bonusAdjustments = JsonConvert.SerializeObject(predator.BonusAdjustments, _jsonSettings),
                        userChoices = JsonConvert.SerializeObject(predator.UserChoices, _jsonSettings)
                    }
                );
            }

            _fields.Add(obj);
        }

        private static void GenerateDisciplineSchema(List<Discipline> disciplines, List<Power> powers, string name, string label)
        {
            var accordion = new
            {
                id = name,
                label,
                type = "accordion",
                items = new List<object>()
            };

            foreach (var discipline in disciplines)
            {
                dynamic accordionItem = new ExpandoObject();
                accordionItem.header = discipline.Name;
                accordionItem.name = discipline.Name.ReplaceWhitespace("");
                accordionItem.embedField = new
                {
                    name = $"disciplines.{discipline.Name}",
                    id = $"disciplines.{discipline.Name.ToLower()}",
                    label = "",
                    includeLabel = false,
                    type = "radiogroup",
                    count = 5,
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 0,
                        max = 5
                    },
                    @default = 0
                };

                accordionItem.component = new
                {
                    name = $"discipline.list.group",
                    type = "listgroup",
                    items = new List<object>()
                };

                foreach (var power in powers.Where(p => p.Discipline == discipline.Name).OrderBy(p => p.Level))
                {
                    var component = 
                        new
                        {
                            name = $"powers.{power.Name.ToLower()}",
                            id = $"{discipline.Name}-{power.Name}",
                            label = $"{power.Name} - Level: {power.Level}",
                            type = "switch"
                        };

                    var text = new
                        {
                            name = $"info-power-{discipline.Name}-{power.Name}",
                            label = "Information",
                            type = "textblock",
                            className = "text-block",
                            text = $"{power.Description}<br /><b>Cost:</b> {power.Cost}<br /><b>Duration:</b> {power.Duration}<br /><b>Dicepool:</b> {power.DicePool}<br /><b>System:</b> {power.System}"
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

        private static void GenerateRitualSchema(List<Ritual> rituals, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var ritual in rituals.OrderBy(r => r.Level))
            {
                obj.options.Add(
                    new
                    {
                        value = ritual.Name,
                        label = ritual.Name,
                        description = $"{ritual.Description}<br /><b>Level: </b>{ritual.Level}<br /><b>Type: </b>{ritual.Type}<br /><b>Ingredients: </b>{ritual.Ingredients}<br /><b>Process: </b>{ritual.Process}<br /><b>System: </b>{ritual.System}"
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

        private static void GenerateAdvantageSchema(List<Advantage> advantages, List<Background> backgrounds, 
            List<Merit> merits, List<Flaw> flaws, string name, string label)
        {
            var backgroundAccordion = new
            {
                id = name,
                label = "Backgrounds",
                type = "accordion",
                items = new List<object>()
            };

            foreach (var advantage in advantages.Where(a => a.Category == "Background").OrderBy(a => a.Name))
            {
                dynamic accordionItem = new ExpandoObject();
                accordionItem.header = advantage.Name;
                accordionItem.name = advantage.Name.ReplaceWhitespace("");
                accordionItem.component = new
                {
                    name ="background.list.group",
                    type = "listgroup",
                    items = new List<object>()
                };

                var filteredBackgrounds = backgrounds.Where(b => b.Advantage == advantage.Name);

                if (filteredBackgrounds.Any())
                {
                    foreach (var background in filteredBackgrounds)
                    {
                        var component = new
                        {
                            name = background.Name.ToLower(),
                            id = $"{advantage.Name}.{background.Name}",
                            label = $"BACKGROUND - {background.Name}",
                            includeLabel = true,
                            type = "radiogroup",
                            className = "form-control",
                            count = background.Range,
                            validation = new
                            {
                                required = false,
                                min = 0,
                                max = background.Range
                            },
                            @default = 0
                        };

                        var text = new
                        {
                            name = $"info-background-{advantage.Name}-{background.Name}",
                            label = "Information",
                            type = "textblock",
                            className = "text-block",
                            text = background.Description
                        };

                        accordionItem.component.items.Add(
                            new
                            {
                                component,
                                text
                            }
                        );
                    }
                }

                var filteredFlaws = flaws.Where(f => f.Advantage == advantage.Name);

                if (filteredFlaws.Any())
                {
                    foreach (var flaw in filteredFlaws)
                    {
                        var component = new object(); 

                        if (flaw.Range == 0)
                        {
                            component = new
                            {
                                name = flaw.Name.ToLower(),
                                id = $"{advantage.Name}.{flaw.Name}",
                                label = $"FLAW - {flaw.Name}",
                                type = "switch"
                            };
                        }
                        else
                        {
                            component = new
                            {
                                name = flaw.Name.ToLower(),
                                id = $"{advantage.Name}.{flaw.Name}",
                                label = $"FLAW - {flaw.Name}",
                                includeLabel= true,
                                type = "radiogroup",
                                className = "form-control",
                                count = flaw.Range,
                                validation = new
                                {
                                    required = false,
                                    min = flaw.Minimum,
                                    max = flaw.Range
                                },
                                @default = 0
                            };
                        }

                        var text = new
                        {
                            name = $"info-flaw-{advantage.Name}-{flaw.Name}",
                            label = "Information",
                            type = "textblock",
                            className = "text-block",
                            text = flaw.Description
                        };

                        accordionItem.component.items.Add(
                            new
                            {
                                component,
                                text
                            }
                        );
                    }
                }                

                backgroundAccordion.items.Add(accordionItem);
            }

            _fields.Add(backgroundAccordion);

            var meritAccordion = new
            {
                id = name,
                label = "Merits",
                type = "accordion",
                items = new List<object>()
            };

            foreach (var advantage in advantages.Where(a => a.Category == "Merit").OrderBy(a => a.Name))
            {
                dynamic accordionItem = new ExpandoObject();
                accordionItem.header = advantage.Name;
                accordionItem.name = advantage.Name.ReplaceWhitespace("");
                accordionItem.component = new
                {
                    name ="merits.list.group",
                    type = "listgroup",
                    items = new List<object>()
                };

                var filteredMerits = merits.Where(m => m.Advantage == advantage.Name);

                if (filteredMerits.Any())
                {
                    foreach (var merit in filteredMerits)
                    {
                        var component = new object(); 

                        if (merit.Range == 0)
                        {
                            component = new
                            {
                                name = merit.Name.ToLower(),
                                id = $"{advantage.Name}.{merit.Name}",
                                label = $"MERIT - {merit.Name}",
                                type = "switch"
                            };
                        }
                        else
                        {
                            component = new
                            {
                                name = merit.Name.ToLower(),
                                id = $"{advantage.Name}.{merit.Name}",
                                label = $"MERIT - {merit.Name}",
                                includeLabel = true,
                                type = "radiogroup",
                                className = "form-control",
                                count = merit.Range,
                                validation = new
                                {
                                    required = false,
                                    min = merit.Minimum,
                                    max = merit.Range
                                },
                                @default = 0
                            };
                        }

                        var text = new
                        {
                            name = $"info-merit-{advantage.Name}-{merit.Name}",
                            label = "Information",
                            type = "textblock",
                            className = "text-block",
                            text = merit.Description
                        };

                        accordionItem.component.items.Add(
                            new
                            {
                                component,
                                text
                            }
                        );
                    }
                }

                var filteredFlaws = flaws.Where(f => f.Advantage == advantage.Name);

                if (filteredFlaws.Any())
                {
                    foreach (var flaw in filteredFlaws)
                    {
                        var component = new object(); 

                        if (flaw.Range == 0)
                        {
                            component = new
                            {
                                name = flaw.Name.ToLower(),
                                id = $"{advantage.Name}.{flaw.Name}",
                                label = $"FLAW - {flaw.Name}",
                                type = "switch"
                            };
                        }
                        else
                        {
                            component = new
                            {
                                name = flaw.Name.ToLower(),
                                id = $"{advantage.Name}.{flaw.Name}",
                                label = $"FLAW - {flaw.Name}",
                                includeLabel= true,
                                type = "radiogroup",
                                className = "form-control",
                                count = flaw.Range,
                                validation = new
                                {
                                    required = false,
                                    min = flaw.Minimum,
                                    max = flaw.Range
                                },
                                @default = 0
                            };
                        }

                        var text = new
                        {
                            name = $"info-flaw-{advantage.Name}-{flaw.Name}",
                            label = "Information",
                            type = "textblock",
                            className = "text-block",
                            text = flaw.Description
                        };

                        accordionItem.component.items.Add(
                            new
                            {
                                component,
                                text
                            }
                        );
                    }
                }                

                meritAccordion.items.Add(accordionItem);
            }

            _fields.Add(meritAccordion);
        }

        private static void GenerateWeaponSchema(List<Weapon> weapons, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var weapon in weapons.OrderBy(w => w.Name))
            {
                obj.options.Add(
                    new
                    {
                        value = weapon.Name,
                        label = weapon.Name,
                        description = $"{weapon.Description}<br /><b>Type: </b>{weapon.Type}<br /><b>Damage: </b>{weapon.Damage}"
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

            foreach (var armor in armors.OrderBy(a => a.Name))
            {
                obj.options.Add(
                    new
                    {
                        value = armor.Name,
                        label = armor.Name,
                        description = $"{armor.Description}<br /><b>Ballistic: </b>{armor.Ballistic}<br /><b>Physical: </b>{armor.Physical}"
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

        private static void GenerateGearSchema(List<Gear> gears, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var gear in gears.OrderBy(g => g.Name))
            {
                obj.options.Add(
                    new
                    {
                        value = gear.Name,
                        label = gear.Name,
                        description = gear.Description
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
