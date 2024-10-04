﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Utility;

namespace VampireTheMasquerade
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = "C:/Users/toole/OneDrive/Source/Character Nexus/Ruleset.VampireTheMasquerade/Json/";
        private static readonly Regex sWhitespace = new Regex(@"\s+");

        public static void InitializeSchema()
        {
            try
            {
                string jsonAdvantagesData = File.ReadAllText(_jsonFilesPath + "Advantages.json");
                List<Advantage>? advantages = JsonTo.List<Advantage>(jsonAdvantagesData);
                string jsonAttributesData = File.ReadAllText(_jsonFilesPath + "Attributes.json");
                List<Attribute>? attributes = JsonTo.List<Attribute>(jsonAttributesData);
                string jsonBackgroundsData = File.ReadAllText(_jsonFilesPath + "Backgrounds.json");
                List<Background>? backgrounds = JsonTo.List<Background>(jsonBackgroundsData);
                string jsonClansData = File.ReadAllText(_jsonFilesPath + "Clans.json");
                List<Clan>? clans = JsonTo.List<Clan>(jsonClansData);
                string jsonCoteriesData = File.ReadAllText(_jsonFilesPath + "Coteries.json");
                List<Coterie>? coteries = JsonTo.List<Coterie>(jsonCoteriesData);
                string jsonDisciplinesData = File.ReadAllText(_jsonFilesPath + "Disciplines.json");
                List<Discipline>? disciplines = JsonTo.List<Discipline>(jsonDisciplinesData);
                string jsonFlawsData = File.ReadAllText(_jsonFilesPath + "Flaws.json");
                List<Flaw>? flaws = JsonTo.List<Flaw>(jsonFlawsData);
                string jsonMeritsData = File.ReadAllText(_jsonFilesPath + "Merits.json");
                List<Merit>? merits = JsonTo.List<Merit>(jsonMeritsData);
                string jsonOriginsData = File.ReadAllText(_jsonFilesPath + "Origins.json");
                List<Origin>? origins = JsonTo.List<Origin>(jsonOriginsData);
                string jsonPowersData = File.ReadAllText(_jsonFilesPath + "Powers.json");
                List<Power>? powers = JsonTo.List<Power>(jsonPowersData);
                string jsonPredatorsData = File.ReadAllText(_jsonFilesPath + "Predators.json");
                List<Predator>? predators = JsonTo.List<Predator>(jsonPredatorsData);
                string jsonRitualsData = File.ReadAllText(_jsonFilesPath + "Rituals.json");
                List<Ritual>? rituals = JsonTo.List<Ritual>(jsonRitualsData);
                string jsonSkillsData = File.ReadAllText(_jsonFilesPath + "Skills.json");
                List<Skill>? skills = JsonTo.List<Skill>(jsonSkillsData);
                string jsonSpecialtiesData = File.ReadAllText(_jsonFilesPath + "Specialties.json");
                List<Specialty>? specialties = JsonTo.List<Specialty>(jsonSpecialtiesData);

                GenerateDescriptionSchema();

                GenerateOriginSchema(origins, "origin", "Origin");
                GenerateClanSchema(clans, "clan", "Clan");
                GenerateCoterieSchema(coteries, "coterie", "Coterie");
                GenerateAttributeSchema(attributes, "attributes", "Attributes");
                GenerateSkillSchema(skills, specialties, "skill", "Skill");
                GeneratePredatorSchema(predators, "predator", "Predator");
                GenerateDisciplineSchema(disciplines, powers, "disciplines", "Disciplines");
                GenerateAdvantageSchema(advantages, "advantages", "Advantages");

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
                    name = "generation",
                    id = "generation",
                    label = "Generation",
                    type = "number",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        min = 1,
                        max = 17
                    },
                    @default = 12
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
                obj.options.Add(
                    new
                    {
                        value = origin.Name,
                        label = origin.Name
                    }
                );
            }

            _fields.Add(obj);

            foreach (var origin in origins)
            {
                var children = new List<object>
                {
                    new
                    {
                        name = $"info-{name}-{origin.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = origin.Description
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
                            value = origin.Name
                        }
                };

                _fields.Add(div);
            }
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
                        label = clan.Name
                    }
                );
            }

            _fields.Add(obj);

            foreach (var clan in clans)
            {
                var children = new List<object>
                {
                    new
                    {
                        name = $"info-{name}-{clan.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = clan.Description
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
                            value = clan.Name
                        }
                };

                _fields.Add(div);
            }
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
            var children = new List<object>();

            //TODO: Implement a radio list type rather than number to match look of WOD sheets
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
                        min = 0,
                        max = 5
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

        private static void GenerateSkillSchema(List<Skill> skills, List<Specialty> specialties, string name, string label)
        {
            var children = new List<object>();

            foreach (var skill in skills)
            {
                var skillSpecialtyList = new List<object>();

                skillSpecialtyList.Add(new
                {
                    name = $"skills.{skill.Name}",
                    id = $"skills.{skill.Name.ToLower()}",
                    label = skill.Name,
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
                    label = "",
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
                        bonusAdjustments = JsonConvert.SerializeObject(predator.BonusAdjustments),
                        userChoices = JsonConvert.SerializeObject(predator.UserChoices)
                    }
                );
            }

            _fields.Add(obj);

            foreach (var predator in predators)
            {
                var children = new List<object>
                {
                    new
                    {
                        name = $"info-{name}-{predator.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = predator.Description
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
                            value = predator.Name
                        }
                };

                _fields.Add(div);
            }
        }

        private static void GenerateDisciplineSchema(List<Discipline> disciplines, List<Power> powers, string name, string label)
        {
            var children = new List<object>();
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var discipline in disciplines)
            {
                obj.options.Add(
                    new
                    {
                        value = discipline.Name,
                        label = discipline.Name
                    }
                );
            }

            children.Add(obj);

            //TODO: Not sure this will work with naming. Also, add powers details in a div after discipline selection
            children.Add(new
            {
                name = $"disciplines.{name}",
                id = $"disciplines.{name.ToLower()}",
                label = "",
                type = "number",
                className = "form-control",
                validation = new
                {
                    required = true,
                    min = 1,
                    max = 5
                },
                @default = 1
            });

            var group = new
            {
                type = "group",
                name,
                label,
                children
            };

            dynamic array = new
            {
                name,
                label,
                type = "array",
                component = group
            };                        

            _fields.Add(array);
        }

        private static void GenerateAdvantageSchema(List<Advantage> advantages, string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
