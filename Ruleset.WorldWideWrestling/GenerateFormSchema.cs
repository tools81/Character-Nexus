using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Utility;

namespace WorldWideWrestling
{
    public static class GenerateFormSchema
    {
        private static List<object> _fields = new List<object>();
        private static string _jsonFilesPath = $"{new DirectoryInfo(AppContext.BaseDirectory).Parent.Parent.Parent.Parent}/Ruleset.WorldWideWrestling/Json/";
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            ContractResolver = JsonContractResolver.Get(),
            Formatting = Formatting.None
        };

        public static void InitializeSchema()
        {
            try
            {
                string jsonRolesData = File.ReadAllText(_jsonFilesPath + "Roles.json");
                var roles = JsonConvert.DeserializeObject<List<Role>>(jsonRolesData);
                if (roles == null) { Console.WriteLine("Unable to read Roles.json. Aborting..."); Console.Read(); return; }

                string jsonGimmicksData = File.ReadAllText(_jsonFilesPath + "Gimmicks.json");
                var gimmicks = JsonConvert.DeserializeObject<List<Gimmick>>(jsonGimmicksData);
                if (gimmicks == null) { Console.WriteLine("Unable to read Gimmicks.json. Aborting..."); Console.Read(); return; }

                string jsonHailingsData = File.ReadAllText(_jsonFilesPath + "Hailings.json");
                var hailings = JsonConvert.DeserializeObject<List<Hailing>>(jsonHailingsData);
                if (hailings == null) { Console.WriteLine("Unable to read Hailings.json. Aborting..."); Console.Read(); return; }

                string jsonEntrancesData = File.ReadAllText(_jsonFilesPath + "Entrances.json");
                var entrances = JsonConvert.DeserializeObject<List<Entrance>>(jsonEntrancesData);
                if (entrances == null) { Console.WriteLine("Unable to read Entrances.json. Aborting..."); Console.Read(); return; }

                string jsonStatsData = File.ReadAllText(_jsonFilesPath + "Stats.json");
                var stats = JsonConvert.DeserializeObject<List<Stat>>(jsonStatsData);
                if (stats == null) { Console.WriteLine("Unable to read Stats.json. Aborting..."); Console.Read(); return; }

                string jsonWantsData = File.ReadAllText(_jsonFilesPath + "Wants.json");
                var wants = JsonConvert.DeserializeObject<List<Want>>(jsonWantsData);
                if (wants == null) { Console.WriteLine("Unable to read Wants.json. Aborting..."); Console.Read(); return; }

                string jsonQuestionsData = File.ReadAllText(_jsonFilesPath + "Questions.json");
                var questions = JsonConvert.DeserializeObject<List<Question>>(jsonQuestionsData);
                if (questions == null) { Console.WriteLine("Unable to read Questions.json. Aborting..."); Console.Read(); return; }

                string jsonMovesData = File.ReadAllText(_jsonFilesPath + "Moves.json");
                var moves = JsonConvert.DeserializeObject<List<Move>>(jsonMovesData);
                if (moves == null) { Console.WriteLine("Unable to read Moves.json. Aborting..."); Console.Read(); return; }

                GenerateDescriptionSchema();
                GenerateRoleSchema(roles, "role", "Role");
                GenerateGimmickSchema(gimmicks, "gimmick", "Gimmick");
                GenerateHailingSchema(hailings, gimmicks, "hailing", "Hailing from");
                GenerateEntranceSchema(entrances, gimmicks, "entrance", "Entrance");
                GenerateQuestionsSchema(questions, gimmicks, "questions", "Heat");
                GenerateStatsSchema(stats, "stats", "Stats");                
                GenerateWantsSchema(wants, "wants", "Wants");
                GenerateMovesSchema(moves, "moves", "Moves");

                var schema = new
                {
                    title = "Wrestler Editor",
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
                className = "form-control"
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

        private static void GenerateRoleSchema(List<Role> roles, string name, string label)
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
                    advanced = role.Advanced
                });
            }

            _fields.Add(obj);
        }

        private static void GenerateGimmickSchema(List<Gimmick> gimmicks, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var gimmick in gimmicks)
            {
                obj.options.Add(new
                {
                    value = gimmick.Name,
                    label = gimmick.Name,
                    description = gimmick.Description,
                    image = gimmick.Image,
                    bonusAdjustments = JsonConvert.SerializeObject(gimmick.BonusAdjustments, _jsonSettings),
                    bonusCharacteristics = JsonConvert.SerializeObject(gimmick.BonusCharacteristics, _jsonSettings),
                    userChoices = JsonConvert.SerializeObject(gimmick.UserChoices, _jsonSettings)
                });
            }

            _fields.Add(obj);
        }

        private static void GenerateHailingSchema(List<Hailing> hailings, List<Gimmick> gimmicks, string name, string label)
        {
            foreach (var gimmick in gimmicks)
            {
                dynamic obj = new ExpandoObject();
                obj.name = $"{name}.{gimmick.Name}";
                obj.label = label;
                obj.type = "select";
                obj.className = "form-select";
                obj.options = new List<object>();

                foreach (var hailing in hailings.Where(h => h.Gimmick == gimmick.Name || string.IsNullOrEmpty(h.Gimmick)))
                {
                    obj.options.Add(new
                    {
                        value = hailing.Name,
                        label = hailing.Name,
                        description = hailing.Description
                    });

                    obj.dependsOn =
                        new
                        {
                            field = "gimmick",
                            value = gimmick.Name
                        };
                }

                _fields.Add(obj);
            }
        }

        private static void GenerateEntranceSchema(List<Entrance> entrances, List<Gimmick> gimmicks, string name, string label)
        {            
            foreach (var gimmick in gimmicks)
            {
                dynamic obj = new ExpandoObject();
                obj.name = $"{name}.{gimmick.Name}";
                obj.label = label;
                obj.type = "select";
                obj.className = "form-select";
                obj.options = new List<object>();
                
                foreach (var entrance in entrances.Where(e => e.Gimmick == gimmick.Name || string.IsNullOrEmpty(e.Gimmick)))
                {
                    obj.options.Add(new
                    {
                        value = entrance.Name,
                        label = entrance.Name,
                        description = entrance.Description
                    });

                    obj.dependsOn =
                        new
                        {
                            field = "gimmick",
                            value = gimmick.Name
                        };
                }

                _fields.Add(obj);
            }
        }

        private static void GenerateQuestionsSchema(List<Question> questions, List<Gimmick> gimmicks, string name, string label)
        {
            foreach (var gimmick in gimmicks)
            {
                foreach (var question in questions.Where(q => q.Gimmick == gimmick.Name))
                {
                    _fields.Add(new
                    {
                        name = $"{name}.{gimmick.Name}.{question.Name}",
                        id = $"{name}.{gimmick.Name}.{question.Name}",
                        label = question.Name,
                        type = "text",
                        className = "form-control",
                        @default = "Unknown",
                        dependsOn =
                        new
                        {
                            field = "gimmick",
                            value = gimmick.Name
                        }
                    });
                }
            }
        }

        private static void GenerateStatsSchema(List<Stat> stats, string name, string label)
        {
            var children = new List<object>();

            foreach (var stat in stats)
            {
                children.Add(new
                {
                    name = $"stats.{stat.Name}",
                    id = $"stats.{stat.Name.ToLower()}",
                    label = stat.Name,
                    description = stat.Description,
                    type = "number",
                    className = "form-control",
                    validation = new { required = true, min = stat.Min, max = stat.Max },
                    @default = stat.Value
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

        private static void GenerateWantsSchema(List<Want> wants, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var want in wants)
            {
                obj.options.Add(new
                {
                    value = want.Name,
                    label = want.Name,
                    description = want.Description,
                    gimmick = want.Gimmick
                });
            }

            _fields.Add(new
            {
                name,
                label,
                type = "array",
                component = obj
            });
        }

        private static void GenerateMovesSchema(List<Move> moves, string name, string label)
        {
            dynamic obj = new ExpandoObject();
            obj.name = name;
            obj.label = label;
            obj.type = "select";
            obj.className = "form-select";
            obj.options = new List<object>();

            foreach (var move in moves)
            {
                obj.options.Add(new
                {
                    value = move.Name,
                    label = move.Name,
                    description = move.Description,
                    classification = move.Classification
                });
            }

            _fields.Add(new
            {
                name,
                label,
                type = "array",
                component = obj
            });
        }
    }
}
