// <copyright file="LocalizedTextTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using Allors.Database;
    using Allors.Database.Domain;
    using Allors.Database.Domain.Tests;
    using Xunit;

    public class LocalisedTextTests : DomainTest, IClassFixture<Fixture>
    {
        public LocalisedTextTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void GivenLocalisedTextWhenValidatingThenRequiredRelationsMustExist()
        {
            var builder = new LocalisedTextBuilder(this.Transaction);
            builder.Build();

            Assert.True(this.Transaction.Derive(false).HasErrors);

            this.Transaction.Rollback();

            builder.WithLocale(new Locales(this.Transaction).EnglishGreatBritain);
            builder.Build();

            Assert.False(this.Transaction.Derive(false).HasErrors);
        }
    }
}
