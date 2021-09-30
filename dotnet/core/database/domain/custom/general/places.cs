// <copyright file="Places.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using Allors.Database.Meta;

    public partial class Places
    {
        public Extent<Place> ExtentByPostalCode()
        {
            var places = this.Transaction.Extent<Place>();
            places.AddSort(M.Place.PostalCode);

            return places;
        }
    }
}
