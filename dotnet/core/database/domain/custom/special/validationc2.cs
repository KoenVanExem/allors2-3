// <copyright file="ValidationC2.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Allors.Database.Meta;

    public partial class ValidationC2
    {
        public void CustomOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertIsUnique(derivation.ChangeSet, this, M.ValidationC1.UniqueId);
        }
    }
}
