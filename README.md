# Character Nexus

___

[![tools81](https://img.shields.io/badge/Created_by_-tools81-blue)](https://github.com/tools81)

Character Nexus is an ASP.NET API capable of generating and storing characters created for configured TTRPGs. In addition, a React frontend is included in the solution.

## Adding Rulesets

Each TTRPG is identified as a "Ruleset". Each Ruleset will, at minimum, have a model for Character, a means of generating a schema for a React form, a means of mapping the json returned by React to the Character model, and a means of populating a pdf template for a character sheet. Additional features will include an export to Foundry VTT.

Each Ruleset has its rules defined in Json files. Interfaces for the general identification of rules can be found in the Utility project (Ability, Attribute, Class, Equipment, Feature, Race, and Skill).

The method of storage is injected, but the default inclusion in this project is Azure Blob Storage.

1. Copy the Ruleset.Template project to a new project with naming convention "Ruleset.MyRulesetName".
2. Within the Resources folder, add a formfillable pdf for your character sheet template.
3. Within the Json folder identify the aspects that make up a character in the ruleset and include them as json arrays.
_(ex. Attributes like Str, Dex, etc would be a file called "Attributes.json" containing an array of Attribute items with properties "Name", "Description", "Value".)_
	- In cases where making a selection at character creation would adjust the value of another characterstic, use the BonusAdjustment property defined in the Utility project.
	- In cases where making a selection at character creation would add another characteristic, use the BonusCharacteristic property defined in the Utility project.
	- In cases where there is a prerequisite before a characteristic is avaible to choose, use the Prerequisite property defined in the Utility project.
	- In cases where making a selection at character creation would then offer a choice of options to the user to add or adjust other characterstics, use the UserChoice property defined in the Utility project.
4. Create a class for each aspect of the character makeup and inherit from the interfaces that identifies the category.
5. The Character model must Include every aspect of the character to be saved including an image and a Guid ID.
6. The GenerateFormSchema class will handle generating a schema that React can use to display a form to the user.
7. The CharacterJsonConverter class will convert the output of the React form to the Character class.
8. The Ruleset class has methods intended to be called by controllers posting to the API. It also has static properites that will have to be updated to identify Ruleset name, and image sources, etc. The FormResource is the full name of the form schema file generated at build.
9. The new Ruleset namespace must be referenced in the SchemaGenerator project and added to the Progam class. Then call InitializeSchema as is done for other Rulesets. This will generate the Form schema in the Json/Character at build, which is for React Form.
10. To add the Ruleset to the React app, inject the new project as a singleton into the Startup class in the CharacterNexus application. Also, add it to the MappingsRuleset in the appsettings.json file. The key will be how to display it and the value matches the project name. 