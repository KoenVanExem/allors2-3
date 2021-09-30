// <copyright file="UserGroup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Derivations;

    public partial class UserGroup
    {
        public void CoreOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (changeSet.HasChangedRole(this, this.Meta.Members))
            {
                foreach (var grant in this.GrantsWhereSubjectGroup)
                {
                    iteration.AddDependency(grant, this);
                    iteration.Mark(grant);
                }
            }
        }
    }
}
