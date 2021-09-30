// <copyright file="Upgrade.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.IO;
    using System.Linq;
    using Database.Derivations;

    public class Upgrade
    {
        private readonly ITransaction session;

        private readonly DirectoryInfo dataPath;

        public Upgrade(ITransaction session, DirectoryInfo dataPath)
        {
            this.session = session;
            this.dataPath = dataPath;
        }

        public void Execute()
        {
        }

        private void Derive(Extent extent)
        {
            var derivation = this.session.Database.Services.Get<IDerivationService>().CreateDerivation(this.session) as ILegacyDerivation;
            derivation.Mark(extent.Cast<Domain.Object>().ToArray());
            var validation = derivation.Derive();
            if (validation.HasErrors)
            {
                foreach (var error in validation.Errors)
                {
                    Console.WriteLine(error.Message);
                }

                throw new Exception("Derivation Error");
            }
        }
    }
}
