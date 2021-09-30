// <copyright file="ParametrizedTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the ApplicationTests type.
// </summary>

namespace Tests
{
    using System.Collections.Generic;

    using Allors.Database.Data;
    using Allors.Database.Domain;
    using Allors.Database.Domain.Tests;
    using Allors.Database.Meta;
    using Xunit;

    public class ParametrizedTests : DomainTest, IClassFixture<Fixture>
    {
        public ParametrizedTests(Fixture fixture) : base(fixture) { }

        // TODO: Koen
        //[Fact]
        //public void EqualsWithParameters()
        //{
        //    var filter = new Extent(M.Person)
        //    {
        //        Predicate = new Equals { PropertyType = M.Person.FirstName, Parameter = "firstName" },
        //    };

        //    var arguments = new Dictionary<string, string> { { "firstName", "John" } };
        //    var queryExtent = filter.Build(this.Transaction, arguments);

        //    var extent = this.Transaction.Extent(M.Person);
        //    extent.Filter.AddEquals(M.Person.FirstName, "John");

        //    Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        //}

        //[Fact]
        //public void EqualsWithDependencies()
        //{
        //    var filter = new Extent(M.Person)
        //    {
        //        Predicate = new Equals { Dependencies = new[] { "useFirstname" }, PropertyType = M.Person.FirstName, Value = "John"},
        //    };

        //    var arguments = new Dictionary<string, string> { };
        //    var queryExtent = filter.Build(this.Transaction, arguments);

        //    var extent = this.Transaction.Extent(M.Person);

        //    Assert.Equal(extent.ToArray(), queryExtent.ToArray());

        //    arguments.Add("useFirstname", "x");
        //    queryExtent = filter.Build(this.Transaction, arguments);

        //    extent = this.Transaction.Extent(M.Person);
        //    extent.Filter.AddEquals(M.Person.FirstName, "John");

        //    Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        //}

        //[Fact]
        //public void EqualsWithoutParameters()
        //{
        //    var filter = new Extent(M.Person)
        //    {
        //        Predicate = new Equals { PropertyType = M.Person.FirstName, Parameter = "firstName" },
        //    };

        //    var queryExtent = filter.Build(this.Transaction);

        //    var extent = this.Transaction.Extent(M.Person);

        //    Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        //}

        //[Fact]
        //public void AndWithParameters()
        //{
        //    // select from Person where FirstName='John' and LastName='Doe'
        //    var filter = new Extent(M.Person)
        //    {
        //        Predicate = new And
        //        {
        //            Operands = new IPredicate[]
        //                        {
        //                            new Equals
        //                                {
        //                                    PropertyType = M.Person.FirstName,
        //                                    Parameter = "firstName",
        //                                },
        //                            new Equals
        //                                {
        //                                    PropertyType = M.Person.LastName,
        //                                    Parameter = "lastName"
        //                                },
        //                        },
        //        },
        //    };

        //    var arguments = new Dictionary<string, string>
        //                        {
        //                            { "firstName", "John" },
        //                            { "lastName", "Doe" },
        //                        };
        //    var queryExtent = filter.Build(this.Transaction, arguments);

        //    var extent = this.Transaction.Extent(M.Person);
        //    var and = extent.Filter.AddAnd();
        //    and.AddEquals(M.Person.FirstName, "John");
        //    and.AddEquals(M.Person.LastName, "Doe");

        //    Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        //}

        //[Fact]
        //public void AndWithoutParameters()
        //{
        //    // select from Person where FirstName='John' and LastName='Doe'
        //    var filter = new Extent(M.Person)
        //    {
        //        Predicate = new And
        //        {
        //            Operands = new IPredicate[]
        //                {
        //                    new Equals
        //                        {
        //                            PropertyType = M.Person.FirstName,
        //                            Parameter = "firstName",
        //                        },
        //                    new Equals
        //                        {
        //                            PropertyType = M.Person.LastName,
        //                            Parameter = "lastName"
        //                        },
        //                },
        //        },
        //    };
        //    {
        //        var arguments = new Dictionary<string, string>
        //                            {
        //                                { "firstName", "John" },
        //                            };
        //        var queryExtent = filter.Build(this.Transaction, arguments);

        //        var extent = this.Transaction.Extent(M.Person);
        //        extent.Filter.AddEquals(M.Person.FirstName, "John");

        //        Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        //    }

        //    {
        //        var queryExtent = filter.Build(this.Transaction);

        //        var extent = this.Transaction.Extent(M.Person);

        //        Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        //    }
        //}

        //[Fact]
        //public void NestedWithParameters()
        //{
        //    var filter = new Extent(M.Organisation)
        //    {
        //        Predicate = new ContainedIn
        //        {
        //            PropertyType = M.Organisation.Employees,
        //            Extent = new Extent(M.Person)
        //            {
        //                Predicate = new Equals
        //                {
        //                    PropertyType = M.Person.Gender,
        //                    Parameter = "gender",
        //                },
        //            },
        //        },
        //    };

        //    var male = new Genders(this.Transaction).Male;

        //    var arguments = new Dictionary<string, string> { { "gender", male.Id.ToString() } };
        //    var queryExtent = filter.Build(this.Transaction, arguments);

        //    var employees = this.Transaction.Extent(M.Person);
        //    employees.Filter.AddEquals(M.Person.Gender, male);
        //    var extent = this.Transaction.Extent(M.Organisation);
        //    extent.Filter.AddContainedIn(M.Organisation.Employees, employees);

        //    Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        //}

        //[Fact]
        //public void NestedWithoutParameters()
        //{
        //    var filter = new Extent(M.Organisation)
        //    {
        //        Predicate = new ContainedIn
        //        {
        //            PropertyType = M.Organisation.Employees,
        //            Extent = new Extent(M.Person)
        //            {
        //                Predicate = new Equals
        //                {
        //                    PropertyType = M.Person.Gender,
        //                    Parameter = "gender",
        //                },
        //            },
        //        },
        //    };

        //    var arguments = new Dictionary<string, string>();
        //    var queryExtent = filter.Build(this.Transaction, arguments);

        //    var extent = this.Transaction.Extent(M.Organisation);

        //    Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        //}

        //[Fact]
        //public void AndNestedContainedInWithoutParameters()
        //{
        //    var filter = new Extent(M.Organisation)
        //    {
        //        Predicate = new And
        //        {
        //            Operands = new IPredicate[]
        //            {
        //                new ContainedIn
        //                {
        //                    PropertyType = M.Organisation.Employees,
        //                    Extent = new Extent(M.Person)
        //                    {
        //                        Predicate = new ContainedIn
        //                        {
        //                            PropertyType = M.Person.Gender,
        //                            Parameter = "gender",
        //                        },
        //                    },
        //                },
        //            },
        //        },
        //    };

        //    var parameters = new Dictionary<string, string>();
        //    var queryExtent = filter.Build(this.Transaction, parameters);

        //    var extent = this.Transaction.Extent(M.Organisation);

        //    Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        //}

        //[Fact]
        //public void AndNestedContainsWithoutParameters()
        //{
        //    var filter = new Extent(M.Organisation)
        //    {
        //        Predicate = new And
        //        {
        //            Operands = new IPredicate[]
        //            {
        //                new ContainedIn
        //                    {
        //                        PropertyType = M.Organisation.Employees,
        //                        Extent = new Extent(M.Person)
        //                                     {
        //                                         Predicate = new Contains
        //                                                         {
        //                                                             PropertyType = M.Person.Gender,
        //                                                             Parameter = "gender",
        //                                                         },
        //                                     },
        //                    },
        //            },
        //        },
        //    };

        //    var arguments = new Dictionary<string, string>();
        //    var queryExtent = filter.Build(this.Transaction, arguments);

        //    var extent = this.Transaction.Extent(M.Organisation);

        //    Assert.Equal(extent.ToArray(), queryExtent.ToArray());
        //}
    }
}
