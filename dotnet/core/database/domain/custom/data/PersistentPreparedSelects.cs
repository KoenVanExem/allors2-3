// <copyright file="Organisations.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    using Allors.Database.Data;

    public partial class PersistentPreparedSelects
    {
        public static readonly Guid FetchPeople = new Guid("F24CC434-8CDE-4E64-8970-4F693A606B7D");

        protected override void CustomSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(FetchPeople, v =>
            {
                v.Description = "Fetch People";
                v.Select = new Select
                {
                    Include = new[]
                    {
                        new Node(M.Organisation.Owner),
                        new Node(M.Organisation.Employees),
                    },
                };
            });
        }
    }
}
