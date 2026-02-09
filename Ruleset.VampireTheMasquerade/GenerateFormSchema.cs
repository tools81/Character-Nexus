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
                // string jsonAdvantagesData = File.ReadAllText(_jsonFilesPath + "Advantages.json");
                // List<Advantage>? advantages = JsonTo.List<Advantage>(jsonAdvantagesData);
                // string jsonBackgroundsData = File.ReadAllText(_jsonFilesPath + "Backgrounds.json");
                // List<Background>? backgrounds = JsonTo.List<Background>(jsonBackgroundsData);

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

                // string jsonCoteriesData = File.ReadAllText(_jsonFilesPath + "Coteries.json");
                // List<Coterie>? coteries = JsonTo.List<Coterie>(jsonCoteriesData);
                // string jsonDisciplinesData = File.ReadAllText(_jsonFilesPath + "Disciplines.json");
                // List<Discipline>? disciplines = JsonTo.List<Discipline>(jsonDisciplinesData);
                // string jsonFlawsData = File.ReadAllText(_jsonFilesPath + "Flaws.json");
                // List<Flaw>? flaws = JsonTo.List<Flaw>(jsonFlawsData);
                // string jsonMeritsData = File.ReadAllText(_jsonFilesPath + "Merits.json");
                // List<Merit>? merits = JsonTo.List<Merit>(jsonMeritsData);
                // string jsonPowersData = File.ReadAllText(_jsonFilesPath + "Powers.json");
                // List<Power>? powers = JsonTo.List<Power>(jsonPowersData);
                // string jsonRitualsData = File.ReadAllText(_jsonFilesPath + "Rituals.json");
                // List<Ritual>? rituals = JsonTo.List<Ritual>(jsonRitualsData);
                // string jsonWeaponsData = File.ReadAllText(_jsonFilesPath + "Weapons.json");
                // List<Weapon>? weapons = JsonTo.List<Weapon>(jsonWeaponsData);
                // string jsonArmorsData = File.ReadAllText(_jsonFilesPath + "Armors.json");
                // List<Armor>? armors = JsonTo.List<Armor>(jsonArmorsData);

                GenerateDescriptionSchema();

                GenerateClanSchema(clans, "clan", "Clan");
                GeneratePredatorSchema(predators, "predator", "Predator");
                GenerateGenerationSchema(generations, "generation", "Generation");
                GenerateAttributeSchema(attributes, "attributes", "Attributes");
                GenerateSkillSchema(skills, specialties, "skill", "Skills");
                // GenerateCoterieSchema(coteries, "coterie", "Coterie");                
                // GenerateDisciplineSchema(disciplines, powers, "disciplines", "Disciplines");
                // GenerateAdvantageSchema(advantages, backgrounds, merits, flaws, "advantages", "Advantages");
                // GenerateWeaponSchema(weapons, "weapons", "Weapons");
                // GenerateArmorSchema(armors, "armors", "Armors");

                // GenerateTemporaryValuesSchema();

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
                    name = "health",
                    id = "health",
                    label = "Health",
                    type = "hidden",
                    className = "form-control"
                }
            );
            _fields.Add(
                new
                {
                    name = "willpower",
                    id = "willpower",
                    label = "Willpower",
                    type = "hidden",
                    className = "form-control"
                }
            );
            _fields.Add(
                new
                {
                    name = "bloodPotency",
                    id = "bloodPotency",
                    label = "Blood Potency",
                    type = "hidden",
                    className = "form-control"
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
        }

        private static void GenerateTemporaryValuesSchema()
        {
            _fields.Add(
                new
                {
                    name = "resonance",
                    id = "resonance",
                    label = "Resonance",
                    type = "text",
                    className = "form-control",
                    @default = "Unknown"
                });
            _fields.Add(
                new
                {
                    name = "hunger",
                    id = "hunger",
                    label = "Hunger",
                    type = "number",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 0,
                        max = 5
                    },
                    @default = 0
                });
            _fields.Add(
                new
                {
                    name = "humanity",
                    id = "humanity",
                    label = "Humanity",
                    type = "number",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 0,
                        max = 10
                    },
                    @default = 0
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
                        label = coterie.Name
                    }
                );
            }

            _fields.Add(obj);

            foreach (var coterie in coteries)
            {
                var children = new List<object>
                {
                    new
                    {
                        name = $"info-{name}-{coterie.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = coterie.Description
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
                            value = coterie.Name
                        }
                };

                _fields.Add(div);
            }
        }

        private static void GenerateAttributeSchema(List<Attribute> attributes, string name, string label)
        {  
            _fields.Add( new
            {
                type = "divider"    
            });

            _fields.Add( new
            {
                name = $"{name}.label",
                text = label,
                type = "textblock"    
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
                    @default = 0
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
            var children = new List<object>();

            foreach (var skill in skills)
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

                skillSpecialtyList.Add(obj);

                var groupSkillSpecialty = new
                {
                    type = "group",
                    name = "",
                    label = skill.Name,
                    children = skillSpecialtyList
                };

                children.Add(groupSkillSpecialty);
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
                        bonusAdjustments = JsonConvert.SerializeObject(predator.BonusAdjustments),
                        userChoices = JsonConvert.SerializeObject(predator.UserChoices)
                    }
                );
            }

            _fields.Add(obj);
        }

        private static void GenerateDisciplineSchema(List<Discipline> disciplines, List<Power> powers, string name, string label)
        {
            //TODO: Include Powers
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
                accordionItem.component = new
                {
                    type = "listgroup",
                    items = new List<object>()
                };

                var children = new List<object>();

                for (int i = 1; i < 5; i++)
                {
                    children.Add( 
                        new
                        {
                            name = discipline.Name.ToLower(),
                            id = $"{discipline.Name}{i}",
                            label = "",
                            type = "radio",
                            value = i
                        }
                    );                    
                }

                var group = new
                {
                    type = "group",
                    name = discipline.Name,
                    label = discipline.Name,
                    children
                };

                var text = new
                {
                    name = $"info-discipline-{discipline.Name}",
                    label = "Information",
                    type = "textblock",
                    className = "text-block",
                    text = discipline.Description
                };

                accordionItem.component.items.Add(
                    new
                    {
                        group,
                        text
                    }
                );

                accordion.items.Add(accordionItem);
            }

            _fields.Add(accordion);
        }

        private static void GenerateAdvantageSchema(List<Advantage> advantages, List<Background> backgrounds, List<Merit> merits, 
            List<Flaw> flaws, string name, string label)
        {
            var accordion = new
            {
                id = name,
                label,
                type = "accordion",
                items = new List<object>()
            };

            foreach (var advantage in advantages)
            {
                dynamic accordionItem = new ExpandoObject();
                accordionItem.header = advantage.Name;
                accordionItem.name = advantage.Name.ReplaceWhitespace("");
                accordionItem.component = new
                {
                    type = "listgroup",
                    items = new List<object>()
                };

                var filteredBackgrounds = backgrounds.Where(b => b.Advantage == advantage.Name);

                if (filteredBackgrounds.Any())
                {
                    var header = new
                    {
                        name = $"header-background-{advantage.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = "Background"
                    };

                    accordionItem.component.items.Add(
                        new
                        {
                            header
                        }
                    );

                    foreach (var background in filteredBackgrounds)
                    {
                        var component = new
                        {
                            name = background.Name.ToLower(),
                            id = $"{advantage.Name}-{background.Name}",
                            label = background.Name,
                            type = "switch"
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

                var filteredMerits = merits.Where(m => m.Advantage == advantage.Name);

                if (filteredMerits.Any())
                {
                    var header = new
                    {
                        name = $"header-merit-{advantage.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = "Merits"
                    };

                    accordionItem.component.items.Add(
                        new
                        {
                            header
                        }
                    );

                    foreach (var merit in filteredMerits)
                    {
                        var component = new
                        {
                            name = merit.Name.ToLower(),
                            id = $"{advantage.Name}-{merit.Name}",
                            label = merit.Name,
                            type = "switch"
                        };

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
                    var header = new
                    {
                        name = $"header-flaw-{advantage.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = "Flaws"
                    };

                    accordionItem.component.items.Add(
                        new
                        {
                            header
                        }
                    );

                    foreach (var flaw in filteredFlaws)
                    {
                        var component = new
                        {
                            name = flaw.Name.ToLower(),
                            id = $"{advantage.Name}-{flaw.Name}",
                            label = flaw.Name,
                            type = "switch"
                        };

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

                accordion.items.Add(accordionItem);
            }

            _fields.Add(accordion);
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
                obj.options.Add(
                    new
                    {
                        value = armor.Name,
                        label = armor.Name
                    }
                );
            }

            _fields.Add(obj);            
        }
    }
}
