// <copyright file="Person.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta
{
    using System.Linq;
    using Database.Data;

    public partial class Person
    {
        public Node[] AngularHome { get; private set; }

        internal void CustomExtend()
        {
            var person = this;
            this.AngularHome = new[]
                {
                    new Node(person.Photo),
                };
        }
    }
}
