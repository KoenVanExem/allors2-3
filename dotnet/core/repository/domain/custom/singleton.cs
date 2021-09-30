// <copyright file="Singleton.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Attributes;
    using static Workspaces;

    public partial class Singleton
    {
        #region Allors
        [Id("94D078CB-BA58-42A4-8005-8D756DE7A463")]
        
        
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace(Default)]
        public Person AutocompleteDefault { get; set; }

        #region Allors
        [Id("7BA0F99D-4E79-4D55-89BE-F94B3B14E98F")]
        
        
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Workspace(Default)]
        public Person SelectDefault { get; set; }
    }
}
