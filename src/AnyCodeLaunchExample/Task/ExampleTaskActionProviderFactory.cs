using Microsoft.VisualStudio.Workspace;
using Microsoft.VisualStudio.Workspace.Build;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnyCodeLaunchExample
{
    [ExportFileContextActionProvider(
        ProviderType,
        ProviderPriority.Normal,
        BuildContextTypes.CustomBuildContextType)]
    internal class ExampleTaskActionProviderFactory : IWorkspaceProviderFactory<IFileContextActionProvider>
    {
        private const string ProviderType = "DDAD3341-B8A3-4A06-9F2A-06AF10B114E8";

        public IFileContextActionProvider CreateProvider(IWorkspace workspaceContext)
        {
            return new ExampleTaskActionProvider();
        }

        private class ExampleTaskActionProvider : IFileContextActionProvider
        {
            public Task<IReadOnlyList<IFileContextAction>> GetActionsAsync(string filePath, FileContext fileContext, CancellationToken cancellationToken)
            {
                List<IFileContextAction> actions = new List<IFileContextAction>();

                if (fileContext.Context is ExampleTaskContext exampleTaskContext)
                {
                    actions.Add(new ExampleTaskAction(fileContext, exampleTaskContext));
                }

                return Task.FromResult<IReadOnlyList<IFileContextAction>>(actions);
            }
        }
    }
}
