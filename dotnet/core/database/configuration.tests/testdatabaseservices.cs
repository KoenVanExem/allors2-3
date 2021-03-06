// <copyright file="DefaultDatabaseScope.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the DomainTest type.</summary>

namespace Allors.Database.Configuration
{
    using Database.Derivations;
    using Derivations.Default;
    using Domain;
    using Microsoft.AspNetCore.Http;

    public class TestDatabaseServices : DatabaseServices
    {
        public TestDatabaseServices(IHttpContextAccessor httpContextAccessor = null) : base(httpContextAccessor) { }

        protected override IPasswordHasher CreatePasswordHasher() => new TestPasswordHasher();

        protected override IDerivationService CreateDerivationFactory() => new LegacyDerivationService();

        private class TestPasswordHasher : IPasswordHasher
        {
            public string HashPassword(string user, string password) => password;

            public bool VerifyHashedPassword(string user, string hashedPassword, string providedPassword) => hashedPassword == providedPassword;
        }
    }
}
