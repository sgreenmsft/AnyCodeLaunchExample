using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Workspace;
using Microsoft.VisualStudio.Workspace.Build;
using Microsoft.VisualStudio.Workspace.Extensions.VS;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AnyCodeLaunchExample
{
    /// <summary>
    /// This class actually performs the task.
    /// </summary>
    internal class ExampleTaskAction : IFileContextAction, IVsCommandItem
    {
        private static readonly Guid CommandGroupGuid = new Guid("36AD90C9-6D77-44E2-843E-ABA668B54E48");
        private readonly FileContext fileContext;
        private readonly ExampleTaskContext exampleTaskContext;

        public ExampleTaskAction(FileContext fileContext, ExampleTaskContext exampleTaskContext)
        {
            Requires.NotNull(fileContext, nameof(fileContext));
            Requires.NotNull(exampleTaskContext, nameof(exampleTaskContext));

            this.fileContext = fileContext;
            this.exampleTaskContext = exampleTaskContext;
        }

        public FileContext Source => this.fileContext;

        public string DisplayName => this.fileContext.DisplayName;

        public Guid CommandGroup => OpenFolderConstants.GuidWorkspaceExplorerBuildActionCmdSet;

        public uint CommandId
        {
            get
            {
                if (this.exampleTaskContext.BuildContextType == BuildContextTypes.BuildContextTypeGuid)
                {
                    return OpenFolderConstants.BuildActionContextId;
                }
                if (this.exampleTaskContext.BuildContextType == BuildContextTypes.RebuildContextTypeGuid)
                {
                    return OpenFolderConstants.RebuildActionContextId;
                }
                if (this.exampleTaskContext.BuildContextType == BuildContextTypes.CleanContextTypeGuid)
                {
                    return OpenFolderConstants.CleanActionContextId;
                }
                if (this.exampleTaskContext.BuildContextType == BuildContextTypes.CustomBuildContextTypeGuid)
                {
                    return OpenFolderConstants.CustomActionContextId;
                }

                return 0;
            }
        }

        public async Task<IFileContextActionResult> ExecuteAsync(IProgress<IFileContextActionProgressUpdate> progress, CancellationToken cancellationToken)
        {
            await TaskScheduler.Default;

            // For this example our action will simply invoke msbuild.
            var startInfo = new ProcessStartInfo();
            string anyCodeFolder = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Microsoft.VisualStudio.Workspace.IWorkspace)).Location);
            startInfo.FileName = Path.Combine(anyCodeFolder, @"..\..\..\..\..\MSBuild\Current\Bin\MSBuild.exe");
            startInfo.Arguments = exampleTaskContext.MSBuildTask + " \"" + exampleTaskContext.FilePath + "\"";

            Process msbuildProcess = Process.Start(startInfo);
            int exitCode = await msbuildProcess.WaitForExitAsync();

            return new FileContextActionResult(success: exitCode == 0);
        }
    }
}
