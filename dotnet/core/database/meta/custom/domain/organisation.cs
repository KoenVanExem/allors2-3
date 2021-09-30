// <copyright file="Organisation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Meta
{
    using Database.Data;

    public partial class Organisation
    {
        public Node[] AngularEmployees { get; private set; }

        public Node[] AngularShareholders { get; private set; }

        internal void CustomExtend(MetaPopulation meta)
        {
            this.AngularEmployees = new[]
            {
                new Node(meta.Organisation.Employees),
            };

            this.AngularShareholders = new[] {
                new Node(meta.Organisation.Shareholders)
                    .Add(meta.Person.Photo),
            };
        }
    }
}
