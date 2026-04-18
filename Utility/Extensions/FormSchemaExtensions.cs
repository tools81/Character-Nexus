using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class FormSchemaExtensions
    {
        public static List<object>? EnrichUserChoices(List<UserChoice>? userChoices, List<IBaseJson> choiceLookup)
        {
            if (userChoices == null) return null;

            return userChoices.Select(uc => (object)new
            {
                type = uc.Type,
                label = uc.Label,
                choices = uc.Choices,
                choiceDescriptions = uc.Choices.Select(c =>
                    choiceLookup.FirstOrDefault(x => x.Name == c)?.Description ?? ""
                ).ToList(),
                count = uc.Count,
                category = uc.Category,
                value = uc.Value
            }).ToList();
        }
    }
}