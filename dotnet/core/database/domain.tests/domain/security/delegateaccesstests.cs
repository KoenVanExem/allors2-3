// <copyright file="DelegateAccessTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System.Linq;
    using Allors.Database;
    using Allors.Database.Domain;
    using Allors.Database.Domain.Tests;
    using Xunit;

    public class DelegateAccessTests : DomainTest, IClassFixture<Fixture>
    {
        public DelegateAccessTests(Fixture fixture) : base(fixture) { }

        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void DelegateAccessReturnsTokens()
        {
            var administrator = new PersonBuilder(this.Transaction).WithUserName("administrator").Build();
            var administrators = new UserGroups(this.Transaction).Administrators;
            administrators.AddMember(administrator);
            var accessClass = new AccessClassBuilder(this.Transaction).Build();

            this.Transaction.Derive();
            this.Transaction.Commit();

            var defaultSecurityToken = new SecurityTokens(this.Transaction).DefaultSecurityToken;
            var dstAcs = defaultSecurityToken.Grants.Where(v => v.EffectiveUsers.Contains(administrator));
            var dstAcs2 = defaultSecurityToken.Grants.Where(v => v.SubjectGroups.Contains(administrators));

            var acs = new Grants(this.Transaction).Extent().Where(v => v.EffectiveUsers.Contains(administrator));
            var acs2 = new Grants(this.Transaction).Extent().Where(v => v.SubjectGroups.Contains(administrators));

            var acl = new DatabaseAccessControl(administrator)[accessClass];
            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));

            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));
        }

        [Fact]
        public void DelegateAccessReturnsNoTokens()
        {
            var administrator = new PersonBuilder(this.Transaction).WithUserName("administrator").Build();
            new UserGroups(this.Transaction).Administrators.AddMember(administrator);
            var accessClass = new AccessClassBuilder(this.Transaction).WithBlock(true).Build();

            accessClass.Block = true;

            this.Transaction.Derive();
            this.Transaction.Commit();

            // Use default security from Singleton
            var acl = new DatabaseAccessControl(administrator)[accessClass];
            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));

            Assert.True(acl.CanRead(M.AccessClass.Property));
            Assert.True(acl.CanWrite(M.AccessClass.Property));
        }
    }
}
