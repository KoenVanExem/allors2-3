// <copyright file="AccessControlListTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System;
    using System.Linq;
    using Allors;
    using Allors.Database;
    using Allors.Database.Domain;
    using Allors.Database.Domain.Tests;
    using Allors.Database.Meta;
    using Xunit;
    using Permission = Allors.Database.Domain.Permission;

    public class WorkspaceAccessControlListsTests : DomainTest, IClassFixture<Fixture>
    {
        public WorkspaceAccessControlListsTests(Fixture fixture) : base(fixture) { }

        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void GivenAWorkspaceAccessControlListsThenADatabaseDeniedPermissionsIsNotPresent()
        {
            var administrator = new PersonBuilder(this.Transaction).WithUserName("administrator").Build();
            var administrators = new UserGroups(this.Transaction).Administrators;
            administrators.AddMember(administrator);

            var databaseOnlyPermissions = new Permissions(this.Transaction).Extent().Where(v => v.OperandType.Equals(M.Person.DatabaseOnlyField));
            var databaseOnlyReadPermission = databaseOnlyPermissions.First(v => v.Operation == Operations.Read);

            var revocation = new RevocationBuilder(this.Transaction).WithUniqueId(Guid.NewGuid())
                .WithDeniedPermission(databaseOnlyReadPermission).Build();

            administrator.AddRevocation(revocation);

            this.Transaction.Derive();
            this.Transaction.Commit();

            var workspaceAccessControlLists = new WorkspaceAccessControl("Default", administrator);
            var acl = workspaceAccessControlLists[administrator];

            Assert.DoesNotContain(revocation, acl.Revocations);
        }

        [Fact]
        public void GivenAWorkspaceAccessControlListsThenAWorkspaceDeniedPermissionsIsNotPresent()
        {
            var administrator = new PersonBuilder(this.Transaction).WithUserName("administrator").Build();
            var administrators = new UserGroups(this.Transaction).Administrators;
            administrators.AddMember(administrator);

            var workspacePermissions = new Permissions(this.Transaction).Extent().Where(v => v.OperandType.Equals(M.Person.WorkspaceField));
            var workspaceReadPermission = workspacePermissions.First(v => v.Operation == Operations.Read);

            var revocation = new RevocationBuilder(this.Transaction)
                .WithUniqueId(Guid.NewGuid())
                .WithDeniedPermission(workspaceReadPermission)
                .Build();

            this.Transaction.Derive();
            this.Transaction.Commit();

            administrator.AddRevocation(revocation);

            var workspaceAccessControlLists = new WorkspaceAccessControl("Default", administrator);
            var acl = workspaceAccessControlLists[administrator];

            Assert.Contains(revocation, acl.Revocations);
        }

        private Permission FindPermission(IRoleType roleType, Operations operation)
        {
            var objectType = (Class)roleType.AssociationType.ObjectType;
            return new Permissions(this.Transaction).Get(objectType, roleType, operation);
        }
    }
}
