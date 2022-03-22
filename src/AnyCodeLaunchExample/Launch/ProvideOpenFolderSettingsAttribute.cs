//-----------------------------------------------------------------------
// <copyright file="ProvideOpenFolderSettingsAttribute.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace AnyCodeLaunchExample
{
    using System;
    using System.ComponentModel.Composition;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.ComponentModelHost;

    /// <summary>
    /// This class attribute class is used in order to create the following entry in the .pkgdef file:
    ///
    /// [$RootKey$\OpenFolder\Settings\VSWorkspaceSettings\{583B3792-4C1A-414A-A438-85D5F7D1260C}]
    ///  @="$PackageFolder$\OpenFolderSchema.json"
    ///  
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ProvideOpenFolderSettingsAttribute : RegistrationAttribute
    {
        public override void Register(RegistrationAttribute.RegistrationContext context)
        {
            Key packageKey = null;
            try
            {
                packageKey = context.CreateKey(GetKeyName(context));
                packageKey.SetValue(string.Empty, "$PackageFolder$\\OpenFolderSchema.json");
            }
            finally
            {
                if (packageKey != null)
                    packageKey.Close();
            }
        }

        public override void Unregister(RegistrationContext context)
        {
            context.RemoveKey(GetKeyName(context));
        }

        private string GetKeyName(RegistrationAttribute.RegistrationContext context)
        {
            return $"OpenFolder\\Settings\\VSWorkspaceSettings\\{{{context.ComponentType.GUID}}}";
        }
    }
}
