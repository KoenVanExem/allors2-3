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
    using MailKit;
    using Microsoft.AspNetCore.Http;
    using Xunit;
    using Grant = Allors.Database.Domain.Grant;
    using Object = System.Object;
    using Permission = Allors.Database.Domain.Permission;
    using Role = Allors.Database.Domain.Role;

    public class DatabaseAccessControlListTests : DomainTest, IClassFixture<Fixture>
    {
        public DatabaseAccessControlListTests(Fixture fixture) : base(fixture) { }

        public override Config Config => new Config { SetupSecurity = true };

        [Fact]
        public void GivenAnAuthenticationPopulatonWhenCreatingAnAccessListForGuestThenPermissionIsDenied()
        {
            this.Transaction.Derive();
            this.Transaction.Commit();

            var sessions = new ITransaction[] { this.Transaction };
            foreach (var session in sessions)
            {
                session.Commit();

                var guest = new AutomatedAgents(this.Transaction).Guest;
                var acls = new DatabaseAccessControl(guest);
                foreach (var aco in (IObject[])session.Extent(M.Organisation))
                {
                    // When
                    var accessList = acls[aco];

                    // Then
                    Assert.False(accessList.CanExecute(M.Organisation.JustDoIt));
                }

                session.Rollback();
            }
        }

        [Fact]
        public void GivenAUserAndAnAccessControlledObjectWhenGettingTheAccessListThenUserHasAccessToThePermissionsInTheRole()
        {
            var permission = this.FindPermission(M.Organisation.Name, Operations.Read);
            var role = new RoleBuilder(this.Transaction).WithName("Role").WithPermission(permission).Build();
            var person = new PersonBuilder(this.Transaction).WithFirstName("John").WithLastName("Doe").Build();
            new GrantBuilder(this.Transaction).WithSubject(person).WithRole(role).Build();

            this.Transaction.Derive();
            this.Transaction.Commit();

            var sessions = new ITransaction[] { this.Transaction };
            foreach (var session in sessions)
            {
                session.Commit();

                var organisation = new OrganisationBuilder(session).WithName("Organisation").Build();

                var token = new SecurityTokenBuilder(session).Build();
                organisation.AddSecurityToken(token);

                var accessControl = (Grant)session.Instantiate(role.GrantsWhereRole.First());
                token.AddGrant(accessControl);

                this.Transaction.Derive();

                Assert.False(this.Transaction.Derive(false).HasErrors);

                var acl = new DatabaseAccessControl(person)[organisation];

                Assert.True(acl.CanRead(M.Organisation.Name));

                session.Rollback();
            }
        }

        [Fact]
        public void GivenAUserGroupAndAnAccessControlledObjectWhenGettingTheAccessListThenUserHasAccessToThePermissionsInTheRole()
        {
            var permission = this.FindPermission(M.Organisation.Name, Operations.Read);
            var role = new RoleBuilder(this.Transaction).WithName("Role").WithPermission(permission).Build();
            var person = new PersonBuilder(this.Transaction).WithFirstName("John").WithLastName("Doe").Build();
            new UserGroupBuilder(this.Transaction).WithName("Group").WithMember(person).Build();

            new GrantBuilder(this.Transaction).WithSubject(person).WithRole(role).Build();

            this.Transaction.Derive();
            this.Transaction.Commit();

            var sessions = new ITransaction[] { this.Transaction };
            foreach (var session in sessions)
            {
                session.Commit();

                var organisation = new OrganisationBuilder(session).WithName("Organisation").Build();

                var token = new SecurityTokenBuilder(session).Build();
                organisation.AddSecurityToken(token);

                var accessControl = (Grant)session.Instantiate(role.GrantsWhereRole.First());
                token.AddGrant(accessControl);

                Assert.False(this.Transaction.Derive(false).HasErrors);

                var acl = new DatabaseAccessControl(person)[organisation];

                Assert.True(acl.CanRead(M.Organisation.Name));

                session.Rollback();
            }
        }

        [Fact]
        public void GivenAnotherUserAndAnAccessControlledObjectWhenGettingTheAccessListThenUserHasAccessToThePermissionsInTheRole()
        {
            var readOrganisationName = this.FindPermission(M.Organisation.Name, Operations.Read);
            var databaseRole = new RoleBuilder(this.Transaction).WithName("Role").WithPermission(readOrganisationName).Build();

            Assert.False(this.Transaction.Derive(false).HasErrors);

            var person = new PersonBuilder(this.Transaction).WithFirstName("John").WithLastName("Doe").Build();
            var anotherPerson = new PersonBuilder(this.Transaction).WithFirstName("Jane").WithLastName("Doe").Build();

            this.Transaction.Derive();
            this.Transaction.Commit();

            new GrantBuilder(this.Transaction).WithSubject(anotherPerson).WithRole(databaseRole).Build();
            this.Transaction.Commit();

            var sessions = new ITransaction[] { this.Transaction };
            foreach (var session in sessions)
            {
                session.Commit();

                var organisation = new OrganisationBuilder(session).WithName("Organisation").Build();

                var token = new SecurityTokenBuilder(session).Build();
                organisation.AddSecurityToken(token);

                var role = (Role)session.Instantiate(new Roles(this.Transaction).FindBy(M.Role.Name, "Role"));
                var accessControl = (Grant)session.Instantiate(role.GrantsWhereRole.First());
                token.AddGrant(accessControl);

                Assert.False(this.Transaction.Derive(false).HasErrors);

                var acl = new DatabaseAccessControl(person)[organisation];

                Assert.False(acl.CanRead(M.Organisation.Name));

                session.Rollback();
            }
        }

        [Fact]
        public void GivenAnotherUserGroupAndAnAccessControlledObjectWhenGettingTheAccessListThenUserHasAccessToThePermissionsInTheRole()
        {
            var readOrganisationName = this.FindPermission(M.Organisation.Name, Operations.Read);
            var databaseRole = new RoleBuilder(this.Transaction).WithName("Role").WithPermission(readOrganisationName).Build();

            var person = new PersonBuilder(this.Transaction).WithFirstName("John").WithLastName("Doe").Build();
            new UserGroupBuilder(this.Transaction).WithName("Group").WithMember(person).Build();
            var anotherUserGroup = new UserGroupBuilder(this.Transaction).WithName("AnotherGroup").Build();

            this.Transaction.Derive();
            this.Transaction.Commit();

            new GrantBuilder(this.Transaction).WithSubjectGroup(anotherUserGroup).WithRole(databaseRole).Build();

            this.Transaction.Commit();

            var sessions = new ITransaction[] { this.Transaction };
            foreach (var session in sessions)
            {
                session.Commit();

                var organisation = new OrganisationBuilder(session).WithName("Organisation").Build();

                var token = new SecurityTokenBuilder(session).Build();
                organisation.AddSecurityToken(token);

                var role = (Role)session.Instantiate(new Roles(this.Transaction).FindBy(M.Role.Name, "Role"));
                var accessControl = (Grant)session.Instantiate(role.GrantsWhereRole.First());
                token.AddGrant(accessControl);

                Assert.False(this.Transaction.Derive(false).HasErrors);

                var acl = new DatabaseAccessControl(person)[organisation];

                Assert.False(acl.CanRead(M.Organisation.Name));

                session.Rollback();
            }
        }

        [Fact]
        public void GivenAnAccessListWhenRemovingUserFromACLThenUserHasNoAccessToThePermissionsInTheRole()
        {
            var permission = this.FindPermission(M.Organisation.Name, Operations.Read);
            var role = new RoleBuilder(this.Transaction).WithName("Role").WithPermission(permission).Build();
            var person = new PersonBuilder(this.Transaction).WithFirstName("John").WithLastName("Doe").Build();
            var person2 = new PersonBuilder(this.Transaction).WithFirstName("Jane").WithLastName("Doe").Build();
            new GrantBuilder(this.Transaction).WithSubject(person).WithRole(role).Build();

            this.Transaction.Derive();
            this.Transaction.Commit();

            var sessions = new ITransaction[] { this.Transaction };
            foreach (var session in sessions)
            {
                session.Commit();

                var organisation = new OrganisationBuilder(session).WithName("Organisation").Build();

                var token = new SecurityTokenBuilder(session).Build();
                organisation.AddSecurityToken(token);

                var accessControl = (Grant)session.Instantiate(role.GrantsWhereRole.First());
                token.AddGrant(accessControl);

                this.Transaction.Derive();

                var acl = new DatabaseAccessControl(person)[organisation];

                accessControl.RemoveSubject(person);
                accessControl.AddSubject(person2);

                this.Transaction.Derive();

                acl = new DatabaseAccessControl(person)[organisation];

                Assert.False(acl.CanRead(M.Organisation.Name));

                session.Rollback();
            }
        }

        [Fact]
        public void Revocations()
        {
            var readOrganisationName = this.FindPermission(M.Organisation.Name, Operations.Read);
            var databaseRole = new RoleBuilder(this.Transaction).WithName("Role").WithPermission(readOrganisationName).Build();
            var person = new PersonBuilder(this.Transaction).WithFirstName("John").WithLastName("Doe").Build();
            new GrantBuilder(this.Transaction).WithRole(databaseRole).WithSubject(person).Build();

            this.Transaction.Derive();
            this.Transaction.Commit();

            var sessions = new ITransaction[] { this.Transaction };
            foreach (var session in sessions)
            {
                session.Commit();

                var organisation = new OrganisationBuilder(session).WithName("Organisation").Build();

                var token = new SecurityTokenBuilder(session).Build();
                organisation.AddSecurityToken(token);

                var role = (Role)session.Instantiate(new Roles(this.Transaction).FindBy(M.Role.Name, "Role"));
                var grant = (Grant)session.Instantiate(role.GrantsWhereRole.First());
                token.AddGrant(grant);

                Assert.False(this.Transaction.Derive(false).HasErrors);

                var acl = new DatabaseAccessControl(person)[organisation];

                Assert.True(acl.CanRead(this.M.Organisation.Name));

                var revocation = new RevocationBuilder(this.Transaction).WithUniqueId(Guid.NewGuid())
                    .WithDeniedPermission(readOrganisationName).Build();

                organisation.AddRevocation(revocation);

                acl = new DatabaseAccessControl(person)[organisation];

                Assert.False(acl.CanRead(this.M.Organisation.Name));

                session.Rollback();
            }
        }

        private Permission FindPermission(IRoleType roleType, Operations operation)
        {
            var objectType = (Class)roleType.AssociationType.ObjectType;
            return new Permissions(this.Transaction).Get(objectType, roleType, operation);
        }
    }
}
