using Microsoft.VisualStudio.Workspace;
using Microsoft.VisualStudio.Workspace.Build;
using Microsoft.VisualStudio.Workspace.CustomContexts;
using Microsoft.VisualStudio.Workspace.Settings;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyCodeLaunchExample
{
    /// <summary>
    /// This class implements the necessary provider to provide an IFileContext
    /// for the 'taskExample' task type. An Action Provider will use the File Context
    /// to actually perform the task.
    /// </summary>
    [ExportFileContextProvider(
        ProviderType,
        ProviderPriority.Normal,
        new Type[] { typeof(IWorkspaceSettingsSource) },
        BuildContextTypes.CustomBuildContextType)]
    internal class ExampleTaskContextProviderFactory : IWorkspaceProviderFactory<IFileContextProvider>
    {
        private const string ProviderType = "7D9E57A3-3F89-4573-829B-EA9955598522";
        private static readonly Guid ProviderTypeGuid = new Guid(ProviderType);

        public IFileContextProvider CreateProvider(IWorkspace workspace)
        {
            return new ExampleTaskContextProvider();
        }

        private class ExampleTaskContextProvider : IFileContextProvider, IFileContextProvider<IWorkspaceSettingsSource>
        {
            public Task<IReadOnlyCollection<FileContext>> GetContextsForFileAsync(string filePath, CancellationToken cancellationToken)
            {
                return Task.FromResult(FileContext.EmptyFileContexts);
            }

            public Task<IReadOnlyCollection<FileContext>> GetContextsForFileAsync(
                string filePath, IWorkspaceSettingsSource settings, CancellationToken cancellationToken)
            {
                string type = settings.Property<string>(CustomConfigurationConstants.Type);

                if (!StringComparer.Ordinal.Equals(type,TaskExampleConstants.TaskExampleType))
                {
                    return Task.FromResult(FileContext.EmptyFileContexts);
                }

                string label = CustomSettingsHelper.GetTaskLabelName(settings);
                Guid contextType = CustomSettingsHelper.GetFileContextType(settings);

                string msbuildTask = string.Empty;
                if (contextType == BuildContextTypes.BuildContextTypeGuid)
                {
                    msbuildTask = "/t:build";
                    if (string.IsNullOrEmpty(label))
                    {
                        label = "Example Build Task";
                    }
                }
                else if (contextType == BuildContextTypes.CleanContextTypeGuid)
                {
                    msbuildTask = "/t:clean";
                    if (string.IsNullOrEmpty(label))
                    {
                        label = "Example Clean Task";
                    }
                }
                else if (contextType == BuildContextTypes.RebuildContextTypeGuid)
                {
                    msbuildTask = "/t:rebuild";
                    if (string.IsNullOrEmpty(label))
                    {
                        label = "Example Rebuild Task";
                    }
                }
                else if (contextType == BuildContextTypes.CustomBuildContextTypeGuid)
                {
                    msbuildTask = "/t:build";
                    if (string.IsNullOrEmpty(label))
                    {
                        label = "Example build Task";
                    }
                }
                else
                {
                    return Task.FromResult(FileContext.EmptyFileContexts);
                }

                var exampleTaskContext = new ExampleTaskContext()
                {
                    BuildContextType = contextType,
                    FilePath = filePath,
                    Label = label,
                    MSBuildTask = msbuildTask,
                };

                FileContext context = new FileContext(
                    ProviderTypeGuid,
                    BuildContextTypes.CustomBuildContextTypeGuid,
                    exampleTaskContext,
                    new[] { filePath },
                    label);

                return Task.FromResult(FileContext.CreateFileContexts(context));
            }
        }
    }
}
