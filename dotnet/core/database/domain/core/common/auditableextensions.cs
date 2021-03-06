// <copyright file="Auditable.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    public static class AuditableExtensions
    {
        public static void CoreOnDerive(this Auditable @this, ObjectOnDerive method)
        {
            var user = @this.Strategy.Transaction.Database.Services.Get<IUserService>().User;
            if (user != null)
            {
                var derivation = method.Derivation;
                var changeSet = derivation.ChangeSet;

                if (changeSet.Created.Contains(@this))
                {
                    @this.CreationDate = @this.Strategy.Transaction.Now();
                    @this.CreatedBy = user;
                }

                if (changeSet.Associations.Contains(@this))
                {
                    @this.LastModifiedDate = @this.Strategy.Transaction.Now();
                    @this.LastModifiedBy = user;
                }
            }
        }
    }
}
