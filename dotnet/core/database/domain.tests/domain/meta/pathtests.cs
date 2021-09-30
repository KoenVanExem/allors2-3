// <copyright file="PathTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Tests
{
    using System.Collections.Generic;
    using Allors.Database.Data;
    using Allors.Database.Domain;
    using Allors.Database.Domain.Tests;
    using Allors.Database.Meta;
    using Moq;
    using Xunit;
    using C1 = Allors.Database.Domain.C1;

    public class PathTests : DomainTest, IClassFixture<Fixture>
    {
        public PathTests(Fixture fixture) : base(fixture) { }

        [Fact]
        public void One2ManyWithPropertyTypes()
        {
            var c2A = new C2Builder(this.Transaction).WithC2AllorsString("c2A").Build();
            var c2B = new C2Builder(this.Transaction).WithC2AllorsString("c2B").Build();
            var c2C = new C2Builder(this.Transaction).WithC2AllorsString("c2C").Build();

            var c1a = new C1Builder(this.Transaction)
                .WithC1AllorsString("c1A")
                .WithC1C2One2Many(c2A)
                .Build();

            var c1b = new C1Builder(this.Transaction)
                .WithC1AllorsString("c1B")
                .WithC1C2One2Many(c2B)
                .WithC1C2One2Many(c2C)
                .Build();

            this.Transaction.Derive();

            var path = new Select(M.C1.C1C2One2Manies, M.C2.C2AllorsString);

            var result = (ISet<object>)path.Get(c1a, this.AclsMock.Object);
            Assert.Equal(1, result.Count);
            Assert.True(result.Contains("c2A"));

            result = (ISet<object>)path.Get(c1b, this.AclsMock.Object);
            Assert.Equal(2, result.Count);
            Assert.True(result.Contains("c2B"));
            Assert.True(result.Contains("c2C"));
        }

        [Fact]
        public void One2ManyWithPropertyTypeIds()
        {
            var c2A = new C2Builder(this.Transaction).WithC2AllorsString("c2A").Build();
            var c2B = new C2Builder(this.Transaction).WithC2AllorsString("c2B").Build();
            var c2C = new C2Builder(this.Transaction).WithC2AllorsString("c2C").Build();

            var c1a = new C1Builder(this.Transaction)
                .WithC1AllorsString("c1A")
                .WithC1C2One2Many(c2A)
                .Build();

            var c1b = new C1Builder(this.Transaction)
                .WithC1AllorsString("c1B")
                .WithC1C2One2Many(c2B)
                .WithC1C2One2Many(c2C)
                .Build();

            this.Transaction.Derive();

            var path = new Select(this.M.C1.C1C2One2Manies, this.M.C2.C2AllorsString);

            var result = (ISet<object>)path.Get(c1a, this.AclsMock.Object);
            Assert.Equal(1, result.Count);
            Assert.True(result.Contains("c2A"));

            result = (ISet<object>)path.Get(c1b, this.AclsMock.Object);
            Assert.Equal(2, result.Count);
            Assert.True(result.Contains("c2B"));
            Assert.True(result.Contains("c2C"));
        }

        [Fact]
        public void One2ManyWithPropertyNames()
        {
            var c2A = new C2Builder(this.Transaction).WithC2AllorsString("c2A").Build();
            var c2B = new C2Builder(this.Transaction).WithC2AllorsString("c2B").Build();
            var c2C = new C2Builder(this.Transaction).WithC2AllorsString("c2C").Build();

            var c1A = new C1Builder(this.Transaction)
                .WithC1AllorsString("c1A")
                .WithC1C2One2Many(c2A)
                .Build();

            var c1B = new C1Builder(this.Transaction)
                .WithC1AllorsString("c1B")
                .WithC1C2One2Many(c2B)
                .WithC1C2One2Many(c2C)
                .Build();

            this.Transaction.Derive();

            Select.TryParse(M.C2, "C1WhereC1C2One2Many", out var fetch);

            var result = (C1)fetch.Get(c2A, this.AclsMock.Object);
            Assert.Equal(result, c1A);

            result = (C1)fetch.Get(c2B, this.AclsMock.Object);
            Assert.Equal(result, c1B);
        }
    }
}
