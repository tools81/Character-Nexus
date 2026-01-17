using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utility;

namespace BladeRunner
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = $"{new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent}/Ruleset.BladeRunner/Json/";
        private static readonly Regex sWhitespace = new Regex(@"\s+");

        public static void InitializeSchema()
        {
            try
            {
                string jsonOriginsData = File.ReadAllText(_jsonFilesPath + "Origins.json");
                List<Origin>? origins = JsonTo.List<Origin>(jsonOriginsData);
                string jsonArchetypesData = File.ReadAllText(_jsonFilesPath + "Archetypes.json");
                List<Archetype>? archetypes = JsonTo.List<Archetype>(jsonArchetypesData);
                string jsonAttributesData = File.ReadAllText(_jsonFilesPath + "Attributes.json");
                List<Attribute>? attributes = JsonTo.List<Attribute>(jsonAttributesData);
                string jsonTenuresData = File.ReadAllText(_jsonFilesPath + "Tenures.json");
                List<Tenure>? tenures = JsonTo.List<Tenure>(jsonTenuresData);
                string jsonSkillsData = File.ReadAllText(_jsonFilesPath + "Skills.json");
                List<Skill>? skills = JsonTo.List<Skill>(jsonSkillsData);
                string jsonSpecialtiesData = File.ReadAllText(_jsonFilesPath + "Specialties.json");
                List<Specialty>? specialties = JsonTo.List<Specialty>(jsonSpecialtiesData);
                string jsonGearsData = File.ReadAllText(_jsonFilesPath + "Gears.json");
                List<Gear>? gears = JsonTo.List<Gear>(jsonGearsData);
                string jsonArmorsData = File.ReadAllText(_jsonFilesPath + "Armors.json");
                List<Armor>? armors = JsonTo.List<Armor>(jsonArmorsData);
                string jsonVehiclesData = File.ReadAllText(_jsonFilesPath + "Vehicles.json");
                List<Vehicle>? vehicles = JsonTo.List<Vehicle>(jsonVehiclesData);
                string jsonWeaponsData = File.ReadAllText(_jsonFilesPath + "Weapons.json");
                List<Weapon>? weapons = JsonTo.List<Weapon>(jsonWeaponsData);

                string jsonTableMemoryWhenData = File.ReadAllText(_jsonFilesPath + "Table/Memory/When.json");
                List<TableItem>? tableMemoryWhenItems = JsonTo.List<TableItem>(jsonTableMemoryWhenData);
                string jsonTableMemoryWhoData = File.ReadAllText(_jsonFilesPath + "Table/Memory/Who.json");
                List<TableItem>? tableMemoryWhoItems = JsonTo.List<TableItem>(jsonTableMemoryWhoData);
                string jsonTableMemoryWhereData = File.ReadAllText(_jsonFilesPath + "Table/Memory/Where.json");
                List<TableItem>? tableMemoryWhereItems = JsonTo.List<TableItem>(jsonTableMemoryWhereData);
                string jsonTableMemoryWhatData = File.ReadAllText(_jsonFilesPath + "Table/Memory/What.json");
                List<TableItem>? tableMemoryWhatItems = JsonTo.List<TableItem>(jsonTableMemoryWhatData);
                string jsonTableMemoryHowData = File.ReadAllText(_jsonFilesPath + "Table/Memory/How.json");
                List<TableItem>? tableMemoryHowItems = JsonTo.List<TableItem>(jsonTableMemoryHowData);

                string jsonTableRelationshipWhoData = File.ReadAllText(_jsonFilesPath + "Table/Relationship/Who.json");
                List<TableItem>? tableRelationshipWhoItems = JsonTo.List<TableItem>(jsonTableRelationshipWhoData);
                string jsonTableRelationshipWhatData = File.ReadAllText(_jsonFilesPath + "Table/Relationship/What.json");
                List<TableItem>? tableRelationshipWhatItems = JsonTo.List<TableItem>(jsonTableRelationshipWhatData);
                string jsonTableRelationshipStatusData = File.ReadAllText(_jsonFilesPath + "Table/Relationship/Status.json");
                List<TableItem>? tableRelationshipStatusItems = JsonTo.List<TableItem>(jsonTableRelationshipStatusData);

                GenerateDescriptionSchema();

                GenerateOriginSchema(origins, "origin", "Origin");
                GenerateArchetypeSchema(archetypes, "archetype", "Archetype");
                GenerateTenureSchema(tenures, "tenure", "Tenure");
                GenerateAttributeSchema(attributes, "attributes", "Attributes");
                GenerateKeyMemorySchema(tableMemoryWhenItems, tableMemoryWhoItems, tableMemoryWhereItems,
                    tableMemoryWhatItems, tableMemoryHowItems, "memory", "Key Memory");
                GenerateKeyRelationshipSchema(tableRelationshipWhoItems, tableRelationshipWhatItems, tableRelationshipStatusItems,
                    "relationship", "Key Relationship");
                GenerateSkillSchema(skills, "skill", "Skill");
                GenerateSpecialtySchema(specialties, "specialty", "Specialties");
                GenerateWeaponSchema(weapons, "weapon", "Weapons");
                GenerateArmorSchema(armors, "armor", "Armor");
                GenerateGearSchema(gears, "gear", "Gear");
                GenerateVehicleSchema(vehicles, "vehicle", "Vehicle");

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
                    className = "form-control"
                }
            );
            _fields.Add(
                new
                {
                    name = "resolve",
                    id = "resolve",
                    label = "Resolve",
                    type = "hidden",
                    className = "form-control"
                }
            );
            _fields.Add(
                new
                {
                    name = "chinyen",
                    id = "chinyen",
                    label = "Chinyen",
                    type = "hidden",
                    className = "form-control"
                }
            );
            _fields.Add(
                new
                {
                    name = "promotionpoints",
                    id = "promotionpoints",
                    label = "Promotion Points",
                    type = "hidden",
                    className = "form-control"
                }
            );
            _fields.Add(
                new
                {
                    name = "humanitypoints",
                    id = "humanitypoints",
                    label = "Humanity Points",
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
                }
            );
            _fields.Add(
                new
                {
                    name = "image",
                    id = "image",
                    label = "Image",
                    type = "image",
                    className = "form-control"
                }
            );
            _fields.Add(
                new
                {
                    name = "appearance",
                    id = "appearance",
                    label = "Appearance",
                    type = "textarea",
                    className = "form-control"
                }
            );
            _fields.Add(
                new
                {
                    name = "notes",
                    id = "notes",
                    label = "Notes",
                    type = "textarea",
                    className = "form-control"
                }
            );
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
                        label = origin.Name,
                        bonusAdjustments = JsonConvert.SerializeObject(origin.BonusAdjustments),
                        userChoices = JsonConvert.SerializeObject(origin.UserChoices)
                    }
                );
            }

            _fields.Add(obj);

            //Add a div below dropdown after selecting a value, containing details of Origin
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

        private static void GenerateArchetypeSchema(List<Archetype> archetypes, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
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
                        bonusAdjustments = JsonConvert.SerializeObject(archetype.BonusAdjustments)
                    }
                );
            }

            _fields.Add(obj);

            //Add a div below dropdown after selecting a value, containing details of Origin
            foreach (var archetype in archetypes)
            {
                var children = new List<object>
                {
                    new
                    {
                        name = $"info-{name}-{archetype.Name}",
                        label = "Information",
                        type = "textblock",
                        className = "text-block",
                        text = archetype.Description
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
                            value = archetype.Name
                        }
                };

                _fields.Add(div);
            }
        }

        private static void GenerateTenureSchema(List<Tenure> tenures, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var tenure in tenures)
            {
                obj.options.Add(
                    new
                    {
                        value = tenure.Name,
                        label = tenure.Name,
                        bonusAdjustments = JsonConvert.SerializeObject(tenure.BonusAdjustments),
                        userChoices = JsonConvert.SerializeObject(tenure.UserChoices)
                    }
                );
            }

            _fields.Add(obj);
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

        private static void GenerateKeyMemorySchema(List<TableItem> tableMemoryWhenItems, List<TableItem> tableMemoryWhoItems, 
            List<TableItem> tableMemoryWhereItems, List<TableItem> tableMemoryWhatItems, List<TableItem> tableMemoryHowItems, 
            string name, string label)
        {
            var children = new List<object>();

            dynamic whenObj = new ExpandoObject();
            whenObj.name = name;
            whenObj.label = label;
            whenObj.type = "select";
            whenObj.className = "form-select";
            whenObj.options = new List<object>();
            foreach (var whenItem in tableMemoryWhenItems)
            {
                whenObj.options.Add(
                    new
                    {
                        value = whenItem.Description,
                        label = "When"
                    }
                );
            }
            children.Add(whenObj);

            dynamic whoObj = new ExpandoObject();
            whoObj.name = name;
            whoObj.label = label;
            whoObj.type = "select";
            whoObj.className = "form-select";
            whoObj.options = new List<object>();
            foreach (var whoItem in tableMemoryWhoItems)
            {
                whoObj.options.Add(
                    new
                    {
                        value = whoItem.Description,
                        label = "When"
                    }
                );
            }
            children.Add(whoObj);

            dynamic whereObj = new ExpandoObject();
            whereObj.name = name;
            whereObj.label = label;
            whereObj.type = "select";
            whereObj.className = "form-select";
            whereObj.options = new List<object>();
            foreach (var whereItem in tableMemoryWhereItems)
            {
                whereObj.options.Add(
                    new
                    {
                        value = whereItem.Description,
                        label = "Where"
                    }
                );
            }
            children.Add(whereObj);

            dynamic whatObj = new ExpandoObject();
            whatObj.name = name;
            whatObj.label = label;
            whatObj.type = "select";
            whatObj.className = "form-select";
            whatObj.options = new List<object>();
            foreach (var whatItem in tableMemoryWhatItems)
            {
                whatObj.options.Add(
                    new
                    {
                        value = whatItem.Description,
                        label = "What"
                    }
                );
            }
            children.Add(whatObj);

            dynamic howObj = new ExpandoObject();
            howObj.name = name;
            howObj.label = label;
            howObj.type = "select";
            howObj.className = "form-select";
            howObj.options = new List<object>();
            foreach (var howItem in tableMemoryHowItems)
            {
                howObj.options.Add(
                    new
                    {
                        value = howItem.Description,
                        label = "How"
                    }
                );
            }
            children.Add(howObj);

            var group = new
            {
                type = "group",
                name,
                label,
                children
            };

            _fields.Add(group);
        }

        private static void GenerateKeyRelationshipSchema(List<TableItem> tableRelationshipWhoItems, 
            List<TableItem> tableRelationshipWhatItems, List<TableItem> tableRelationshipStatusItems, 
            string name, string label)
        {
            var children = new List<object>();

            dynamic whoObj = new ExpandoObject();
            whoObj.name = name;
            whoObj.label = label;
            whoObj.type = "select";
            whoObj.className = "form-select";
            whoObj.options = new List<object>();
            foreach (var whoItem in tableRelationshipWhoItems)
            {
                whoObj.options.Add(
                    new
                    {
                        value = whoItem.Description,
                        label = "Who"
                    }
                );
            }
            children.Add(whoObj);

            dynamic whatObj = new ExpandoObject();
            whatObj.name = name;
            whatObj.label = label;
            whatObj.type = "select";
            whatObj.className = "form-select";
            whatObj.options = new List<object>();
            foreach (var whatItem in tableRelationshipWhatItems)
            {
                whatObj.options.Add(
                    new
                    {
                        value = whatItem.Description,
                        label = "What"
                    }
                );
            }
            children.Add(whatObj);

            dynamic statusObj = new ExpandoObject();
            statusObj.name = name;
            statusObj.label = label;
            statusObj.type = "select";
            statusObj.className = "form-select";
            statusObj.options = new List<object>();
            foreach (var statusItem in tableRelationshipStatusItems)
            {
                whatObj.options.Add(
                    new
                    {
                        value = statusItem.Description,
                        label = "Status"
                    }
                );
            }
            children.Add(statusObj);

            var group = new
            {
                type = "group",
                name,
                label,
                children
            };

            _fields.Add(group);
        }

        private static void GenerateSkillSchema(List<Skill> skills, string name, string label)
        {
            var children = new List<object>();

            foreach (var skill in skills)
            {
                children.Add(new
                {
                    name = $"skills.{skill.Name}",
                    id = $"skills.{skill.Name.ToLower()}",
                    label = skill.Name,
                    type = "number",
                    className = "form-control",
                    validation = new
                    {
                        required = true,
                        //TODO: Review these quantities
                        min = 1,
                        max = 4
                    },
                    @default = 1
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

        private static void GenerateSpecialtySchema(List<Specialty> specialties, string name, string label)
        {
            dynamic obj = new ExpandoObject();

            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var specialty in specialties)
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

            foreach (var gear in gears)
            {
                obj.options.Add(
                    new
                    {
                        value = gear.Name,
                        label = gear.Name
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

            foreach (var vehicle in vehicles)
            {
                obj.options.Add(
                    new
                    {
                        value = vehicle.Name,
                        label = vehicle.Name
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
