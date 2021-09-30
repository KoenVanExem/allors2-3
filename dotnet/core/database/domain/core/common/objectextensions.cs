// <copyright file="ObjectExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Concurrent;
    using System.Linq;

    using Allors.Database.Meta;
    using Derivations;

    public static partial class ObjectExtensions
    {
        private static readonly ConcurrentDictionary<string, RoleType[]> RequiredRoleTypesByClassName = new ConcurrentDictionary<string, RoleType[]>();
        private static readonly ConcurrentDictionary<string, RoleType[]> UniqueRoleTypesByClassName = new ConcurrentDictionary<string, RoleType[]>();

        public static void CoreOnPreDerive(this Object @this, ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(@this))
            {
                iteration.Schedule(@this);
            }
            else
            {
                if (!iteration.LegacyCycle.Derivation.DerivedObjects.Contains(@this))
                {
                    if (changeSet.IsCreated(@this) || changeSet.HasChangedRoles(@this))
                    {
                        iteration.Schedule(@this);
                    }
                }
            }
        }

        public static void CoreOnPostDerive(this Object @this, ObjectOnPostDerive method)
        {
            var derivation = method.Derivation;
            var @class = (Class)@this.Strategy.Class;

            foreach (var roleType in @class.RequiredRoleTypes)
            {
                derivation.Validation.AssertExists(@this, roleType);
            }

            foreach (var roleType in @class.UniqueRoleTypes)
            {
                derivation.Validation.AssertIsUnique(derivation.ChangeSet, @this, roleType);
            }
        }
    }
}
