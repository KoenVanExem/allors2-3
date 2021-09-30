// <copyright file="AccessControlTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AccessControlTests type.</summary>

namespace Tests
{
    using System.Collections;
    using Allors.Database.Configuration.Derivations.Default;
    using Allors.Database.Domain;
    using Allors.Database.Domain.Tests;
    using Allors.Database.Meta;
    using Xunit;

    public class AccessControlTests : DomainTest, IClassFixture<Fixture>
    {
        public AccessControlTests(Fixture fixture) : base(fixture) { }

        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void GivenNoAccessControlWhenCreatingAnAccessControlWithoutARoleThenAccessControlIsInvalid()
        {
            var userGroup = new UserGroupBuilder(this.Transaction).WithName("UserGroup").Build();
            var securityToken = new SecurityTokenBuilder(this.Transaction).Build();

            securityToken.AddGrant(new GrantBuilder(this.Transaction)
                .WithSubjectGroup(userGroup)
                .Build());

            var validation = this.Transaction.Derive(false);

            Assert.True(validation.HasErrors);
            Assert.Single(validation.Errors);

            var derivationError = validation.Errors[0];

            Assert.Single(derivationError.Relations);
            Assert.Equal(typeof(DerivationErrorRequired), derivationError.GetType());
            Assert.Equal(M.Grant.Role, derivationError.Relations[0].RelationType.RoleType);
        }

        [Fact]
        public void GivenNoAccessControlWhenCreatingAAccessControlWithoutAUserOrUserGroupThenAccessControlIsInvalid()
        {
            var securityToken = new SecurityTokenBuilder(this.Transaction).Build();
            var role = new RoleBuilder(this.Transaction).WithName("Role").Build();

            securityToken.AddGrant(
            new GrantBuilder(this.Transaction)
                .WithRole(role)
                .Build());

            var validation = this.Transaction.Derive(false);

            Assert.True(validation.HasErrors);
            Assert.Single(validation.Errors);

            var derivationError = validation.Errors[0];

            Assert.Equal(2, derivationError.Relations.Length);
            Assert.Equal(typeof(DerivationErrorAtLeastOne), derivationError.GetType());
            Assert.True(new ArrayList(derivationError.RoleTypes).Contains((RoleType)M.Grant.Subjects));
            Assert.True(new ArrayList(derivationError.RoleTypes).Contains((RoleType)M.Grant.SubjectGroups));
        }
    }
}
