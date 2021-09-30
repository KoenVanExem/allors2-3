// <copyright file="AccessControl.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Linq;
    using Database.Security;

    public partial class Grant : IGrant
    {
        public void CoreOnDerive(ObjectOnDerive method)
        {
            this.EffectiveUsers = this.SubjectGroups.SelectMany(v => v.Members).Union(this.Subjects).ToArray();
            this.EffectivePermissions = this.Role?.Permissions.ToArray();

            // Invalidate cache
            this.CacheId = Guid.NewGuid();
        }

        public void CoreOnPostDerive(ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;
            derivation.Validation.AssertAtLeastOne(this, this.Meta.Subjects, this.Meta.SubjectGroups);
        }

        IPermission[] IGrant.Permissions => this.EffectivePermissions.ToArray();
    }
}
