// <copyright file="ObjectOnPreDerive.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Generic;
    using Database.Derivations;
    

    public abstract partial class ObjectOnPreDerive
    {
        public ILegacyIteration Iteration { get; set; }

        public ObjectOnPreDerive WithIteration(ILegacyIteration iteration)
        {
            this.Iteration = iteration;
            return this;
        }

        public void Deconstruct(out ILegacyIteration iteration, out IAccumulatedChangeSet changeSet, out ISet<Object> derivedObjects)
        {
            changeSet = this.Iteration.ChangeSet;
            iteration = this.Iteration;
            derivedObjects = this.Iteration.LegacyCycle.Derivation.DerivedObjects;
        }
    }
}
