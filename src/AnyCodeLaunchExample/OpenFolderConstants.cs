using System;

namespace AnyCodeLaunchExample
{
    internal class OpenFolderConstants
    {
        public const string guidWorkspaceExplorerBuildActionCmdSet = "16537F6E-CB14-44DA-B087-D1387CE3BF57";
        public static readonly Guid GuidWorkspaceExplorerBuildActionCmdSet = new Guid(guidWorkspaceExplorerBuildActionCmdSet);

        public const int BuildActionContextId = 0x1000;
        public const int CleanActionContextId = 0x1020;
        public const int RebuildActionContextId = 0x1010;
        public const int CustomActionContextId = 0x0100;
    }
}
