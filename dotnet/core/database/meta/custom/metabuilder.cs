// <copyright file="Organisation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;

    public partial class MetaBuilder
    {
        #region Workspace
        private static void AddWorkspace(string workspaceName, params MethodType[] methodTypes)
        {
            foreach (var methodType in methodTypes)
            {
                methodType.AssignedWorkspaceNames = (methodType.AssignedWorkspaceNames ?? Array.Empty<string>())
                    .Append(workspaceName).Distinct().ToArray();
            }
        }

        private static void AddWorkspace(string workspaceName, params RelationType[] relationTypes)
        {
            foreach (var relationType in relationTypes)
            {
                relationType.AssignedWorkspaceNames = (relationType.AssignedWorkspaceNames ?? Array.Empty<string>())
                    .Append(workspaceName).Distinct().ToArray();
            }
        }

        #endregion

        private void BuildCustom(MetaPopulation meta, Domains domains, RelationTypes relationTypes, MethodTypes methodTypes)
        {
            AddWorkspace("Default", methodTypes.DeletableDelete);
            AddWorkspace("Default", relationTypes.LocaleName);
            AddWorkspace("Default", relationTypes.PersonFirstName, relationTypes.PersonLastName, relationTypes.PersonMiddleName);
            AddWorkspace("Default", relationTypes.UserUserName);
            AddWorkspace("Default", relationTypes.UserGroupMembers);

            relationTypes.OrganisationName.RoleType.IsRequired = true;

            foreach (var composite in meta.Composites.Cast<Composite>())
            {
                composite.CustomExtend(meta);
            }
        }
    }
}
