// <copyright file="Constraints.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Configuration.Derivations.Default
{
    public partial class LegacyDerivationConfig
    {
        public int MaxCycles { get; set; } = 0;

        public int MaxIterations { get; set; } = 0;

        public int MaxPreparations { get; set; } = 0;
    }
}
