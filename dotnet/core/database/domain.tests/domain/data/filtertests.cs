// <copyright file="FilterTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Tests
{
    using Allors.Database.Data;
    using Allors.Database.Domain.Tests;
    using Xunit;

    public class FilterTests : DomainTest, IClassFixture<Fixture>
    {
        public FilterTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void Type()
        {
            var query = new Extent(M.Person);
            var queryExtent = query.Build(this.Transaction);

            var extent = this.Transaction.Extent(M.Person);

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

        [Fact]
        public void RoleEquals()
        {
            var filter = new Extent(M.Person)
            {
                Predicate = new Equals
                {
                    PropertyType = M.Person.FirstName,
                    Value = "John",
                },
            };

            var queryExtent = filter.Build(this.Transaction);

            var extent = this.Transaction.Extent(M.Person);
            extent.Filter.AddEquals(M.Person.FirstName, "John");

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }

        [Fact]
        public void And()
        {
            // select from Person where FirstName='John' and LastName='Doe'
            var filter = new Extent(M.Person)
            {
                Predicate = new And
                {
                    Operands = new IPredicate[]
                    {
                        new Equals
                        {
                            PropertyType = M.Person.FirstName,
                            Value = "John",
                        },
                        new Equals
                        {
                            PropertyType = M.Person.LastName,
                            Value = "Doe"
                        },
                    },
                },
            };

            var queryExtent = filter.Build(this.Transaction);

            var extent = this.Transaction.Extent(M.Person);
            var and = extent.Filter.AddAnd();
            and.AddEquals(M.Person.FirstName, "John");
            and.AddEquals(M.Person.LastName, "Doe");

            Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        }
    }
}
