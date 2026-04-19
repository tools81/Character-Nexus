using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class FormSchemaExtensions
    {
        public static List<object>? EnrichUserChoices(List<UserChoice>? userChoices, Dictionary<string, List<IBaseJson>> typedLookup)
        {
            if (userChoices == null) return null;

            return userChoices.Select(uc =>
            {
                typedLookup.TryGetValue(uc.Type, out var typeItems);
                typeItems ??= new List<IBaseJson>();

                var sourceItems = (uc.Choices == null || uc.Choices.Count == 0)
                    ? typeItems
                    : uc.Choices
                        .Select(c => typeItems.FirstOrDefault(x => x.Name == c))
                        .OfType<IBaseJson>()
                        .ToList();

                return (object)new
                {
                    type = uc.Type,
                    label = uc.Label,
                    choices = sourceItems.Select(x => x.Name).ToList(),
                    choiceDescriptions = sourceItems.Select(x => x.Description ?? "").ToList(),
                    count = uc.Count,
                    category = uc.Category,
                    value = uc.Value
                };
            }).ToList();
        }
    }
}