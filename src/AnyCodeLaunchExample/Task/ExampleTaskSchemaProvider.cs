using Microsoft.VisualStudio.Workspace;
using Microsoft.VisualStudio.Workspace.CustomContexts;
using System;

namespace AnyCodeLaunchExample
{
    /// <summary>
    /// Used to customize the schema of the tasks.vs.json that the user edits.
    /// This ensures that our custom task type is shown as an option to the user.
    /// </summary>
    [ExportCustomFileContextProvider2(
        CustomFileContextProviderOptions.None,
        ProviderType,
        null,
        new string[] { "tasks.vs.json" },
        Schema,
        ProviderPriority.Normal)]
    internal class ExampleTaskSchemaProvider : ICustomFileContextProvider
    {
        /// <summary>
        /// The ProviderType GUID is just used to uniquely this particular class. Each Provider
        /// class will have its own unique ProvierType GUID.
        /// </summary>
        private const string ProviderType = "C046283B-CB1D-4EE9-B492-0A7056EFD30A";

        /// <summary>
        /// This is the json schema describing our example task that will be inserted into the combined
        /// "tasks_schema.json" file used by tasks.vs.json.
        /// </summary>
        private const string Schema =
@"{
    ""definitions"": {
        ""taskExample"": {
            ""type"": ""object"",
            ""properties"": {
                ""type"": {
                    ""type"": ""string"",
                    ""enum"": [
                        ""taskExample""
                    ]
                },
                ""command"": {
                    ""type"": ""string""
                },
            }
        },
        ""taskExampleTask"": {
            ""allOf"": [
                { ""$ref"": ""#/definitions/default"" },
                { ""$ref"": ""#/definitions/taskExample"" }
            ]
        }
    },
    ""task"": ""#/definitions/taskExampleTask""
}";

        public void CustomizeFileContext(string filePath, IPropertySettings customSettings)
        {
            if (customSettings == null)
            {
                throw new ArgumentNullException(nameof(customSettings));
            }

            customSettings[CustomConfigurationConstants.Type] = TaskExampleConstants.TaskExampleType;
        }
    }
}
