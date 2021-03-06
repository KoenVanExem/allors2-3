// <copyright file="ObjectOnPostDerive.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Database.Derivations;
    using Derivations;
    

    public abstract partial class ObjectOnPostDerive
    {
        public ILegacyDerivation Derivation { get; set; }

        public ObjectOnPostDerive WithDerivation(ILegacyDerivation derivation)
        {
            this.Derivation = derivation;
            return this;
        }
    }
}
