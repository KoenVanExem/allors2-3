// <copyright file="PreparedExtentTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System.Collections.Generic;
    using Allors.Database;
    using Allors.Database.Configuration;
    using Allors.Database.Domain;
    using Allors.Database.Domain.Tests;
    using Allors.Database.Services;
    using Microsoft.Extensions.DependencyInjection;

    using Xunit;

    [Collection("Api")]
    public class PreparedExtentTests : DomainTest, IClassFixture<Fixture>
    {
        public PreparedExtentTests(Fixture fixture) : base(fixture) { }

        // TODO: Koen
        //[Fact]
        //public async void WithParameter()
        //{
        //    var organisations = new Organisations(this.Transaction).Extent().ToArray();

        //    var extentService = this.Transaction.Database.Services.Get<IPreparedExtents>();
        //    var organizationByName = extentService.Get(PreparedExtents.ByName);

        //    var arguments = new Dictionary<string, string>
        //    {
        //        { "name", "Acme" },
        //    };

        //    Extent<Organisation> organizations = organizationByName.Build(this.Transaction, arguments).ToArray();

        //    Assert.Single(organizations);

        //    var organization = organizations[0];

        //    Assert.Equal("Acme", organization.Name);
        //}
    }
}
