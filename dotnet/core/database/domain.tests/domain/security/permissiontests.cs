// <copyright file="PermissionTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the PermissionTests type.</summary>

namespace Tests
{
    using Allors.Database;
    using System;
    using System.Linq;
    using Allors.Database.Domain;
    using Allors.Database.Meta;
    using Xunit;
    using Allors.Database.Domain.Tests;

    public class PermissionTests : DomainTest, IClassFixture<Fixture>
    {
        public PermissionTests(Fixture fixture) : base(fixture) { }

        public override Config Config => new Config { SetupSecurity = true };

        // [Fact]
        // public void SyncMethod()
        // {
        //    var domain = (Domain)this.DatabaseSession.Population.MetaPopulation.FindById(new Guid("AB41FD0C-C887-4A1D-BEDA-CED69527E69A"));

        // var methodType = new MethodTypeBuilder(domain, Guid.NewGuid()).Build();
        //    methodType.ObjectType = M.Organisation.ObjectType;
        //    methodType.Name = "Method";

        // var count = new Permissions(this.DatabaseSession).Extent().Count;

        // new Permissions(this.DatabaseSession).Sync();

        // Assert.Equal(count + 1, new Permissions(this.DatabaseSession).Extent().Count);

        // var methodPermission = new Permissions(this.DatabaseSession).FindBy(M.Permission.OperandTypePointer, methodType.Id);
        //    Assert.NotNull(methodPermission);
        //    Assert.Equal(Operation.Execute, methodPermission.Operation);
        // }

        // [Fact]
        // public void SyncRelation()
        // {
        //    var domain = (Domain)this.DatabaseSession.Population.MetaPopulation.FindById(new Guid("AB41FD0C-C887-4A1D-BEDA-CED69527E69A"));

        // var count = new Permissions(this.DatabaseSession).Extent().Count;

        // var relationType = new RelationTypeBuilder(domain, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()).Build();

        // relationType.AssociationType.ObjectType = M.Organisation.ObjectType;
        //    relationType.RoleType.ObjectType = Persons.Meta.ObjectType;
        //    relationType.RoleType.AssignedSingularName = "Relation";
        //    relationType.RoleType.AssignedPluralName = "Relations";

        // new Permissions(this.DatabaseSession).Sync();

        // this.DatabaseSession.Derive(true);

        // Assert.Equal(count + 3, new Permissions(this.DatabaseSession).Extent().Count);

        // var roleTypePermissions = new Permissions(this.DatabaseSession).Extent();
        //    roleTypePermissions.Filter.AddEquals(M.Permission.OperandTypePointer, relationType.RoleType.Id);
        //    Assert.Equal(2, roleTypePermissions.Count);

        // var associationTypePermissions = new Permissions(this.DatabaseSession).Extent();
        //    associationTypePermissions.Filter.AddEquals(M.Permission.OperandTypePointer, relationType.AssociationType.Id);
        //    Assert.Equal(1, associationTypePermissions.Count);
        // }

        [Fact]
        public void NoPermissionsForAssociationsWhenUnitType()
        {
            Permissions.Sync(this.Transaction.Database);

            this.Transaction.Rollback();

            var permissions = new Permissions(this.Transaction).Extent().ToArray();

            Assert.Empty(permissions.Where(v => v.OperandType is AssociationType associationType && associationType.RoleType.ObjectType.IsUnit));
        }

        [Fact]
        public void WhenSyncingPermissionsThenObsolotePermissionsAreDeleted()
        {
            var domain = (Domain)this.Transaction.Database.MetaPopulation.FindById(new Guid("AB41FD0C-C887-4A1D-BEDA-CED69527E69A"));

            var count = new Permissions(this.Transaction).Extent().Count;

            var permission = new ExecutePermissionBuilder(this.Transaction).WithClassPointer(new Guid()).WithMethodTypePointer(new Guid()).Build();

            Permissions.Sync(this.Transaction.Database);

            this.Transaction.Rollback();

            Assert.Equal(count, new Permissions(this.Transaction).Extent().Count);
        }

        [Fact]
        public void WhenSyncingPermissionsThenDanglingPermissionsAreDeleted()
        {
            var permission = new ReadPermissionBuilder(this.Transaction).Build();

            Permissions.Sync(this.Transaction.Database);

            this.Transaction.Rollback();

            Assert.True(permission.Strategy.IsDeleted);
        }

        [Fact]
        public void GivenSyncedPermissionsWhenRemovingAnOperationThenThatPermissionIsInvalid()
        {
            // TODO: Permission members should be write once
            // var permission = (Permission)this.DatabaseSession.Extent<Permission>().First;
            // permission.RemoveOperationEnum();

            // var validation = this.DatabaseSession.Derive(false);

            // Assert.True(validation.HasErrors);
            // Assert.Equal(1, validation.Errors.Length);

            // var derivationError = validation.Errors[0];

            // Assert.Equal(1, derivationError.Relations.Length);
            // Assert.Equal(typeof(DerivationErrorRequired), derivationError.GetType());
            // Assert.Equal((RoleType)M.Permission.OperationEnum, derivationError.Relations[0].RoleType);
        }

        [Fact]
        public void GivenSyncedPermissionsWhenRemovingAnAccessControlledMemberThenThatPermissionIsInvalid()
        {
            // TODO: Permission members should be write once
            // var permission = this.DatabaseSession.Extent<Permission>().First;
            // permission.RemoveOperandTypePointer();

            // var validation = this.DatabaseSession.Derive(false);

            // Assert.True(validation.HasErrors);
            // Assert.Equal(1, validation.Errors.Length);

            // var derivationError = validation.Errors[0];

            // Assert.Equal(1, derivationError.Relations.Length);
            // Assert.Equal(typeof(DerivationErrorRequired), derivationError.GetType());
            // Assert.Equal((RoleType)M.Permission.OperandTypePointer, derivationError.Relations[0].RoleType);
        }
    }
}
