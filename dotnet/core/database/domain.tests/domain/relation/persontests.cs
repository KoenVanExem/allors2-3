// <copyright file="PersonTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

namespace Tests
{
    using Allors.Database;
    using Allors.Database.Domain;
    using Allors.Database.Domain.Tests;
    using Allors.Database.Meta;

    using Xunit;

    public class PersonTests : DomainTest, IClassFixture<Fixture>
    {
        public PersonTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void NoRequiredFields()
        {
            new PersonBuilder(this.Transaction).Build();
            var log = this.Transaction.Derive(false);
            Assert.Equal(0, log.Errors.Length);
        }

        [Fact]
        public void Fullname()
        {
            var john = new PersonBuilder(this.Transaction).WithFirstName("John").WithLastName("Doe").Build();
            this.Transaction.Derive();

            Assert.Equal("John Doe", john.FullName);
        }
    }
}
