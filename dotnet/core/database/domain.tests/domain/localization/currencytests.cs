// <copyright file="CurrencyTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the CurrencyTests type.</summary>

namespace Tests
{
    using Allors.Database;
    using Allors.Database.Domain;
    using Allors.Database.Domain.Tests;
    using Allors.Database.Meta;

    using Xunit;

    public class CurrencyTests : DomainTest, IClassFixture<Fixture>
    {
        public CurrencyTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void GivenCurrencyWhenValidatingThenRequiredRelationsMustExist()
        {
            var builder = new CurrencyBuilder(this.Transaction);
            builder.Build();

            Assert.True(this.Transaction.Derive(false).HasErrors);

            builder.WithIsoCode("BND").Build();

            Assert.True(this.Transaction.Derive(false).HasErrors);

            var locales = new Locales(this.Transaction).Extent().ToArray();
            var locale = new Locales(this.Transaction).FindBy(M.Locale.Name, Locales.EnglishGreatBritainName);

            builder
                .WithLocalisedName(
                    new LocalisedTextBuilder(this.Transaction)
                    .WithText("Brunei Dollar")
                    .WithLocale(locale)
                .Build());

            Assert.False(this.Transaction.Derive(false).HasErrors);
        }
    }
}
