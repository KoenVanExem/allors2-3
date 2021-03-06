// <copyright file="InternalAccountingTransaction.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using Allors.Repository.Attributes; using static Workspaces;

    #region Allors
    [Id("5a783d98-845a-4784-9c92-5c75a4af3fb8")]
    #endregion
    public partial interface InternalAccountingTransaction : AccountingTransaction
    {
        #region Allors
        [Id("EF969E4C-ADD5-4A3D-A718-857CC99BBACA")]
        
        
        [Indexed]
        #endregion
        [Multiplicity(Multiplicity.ManyToOne)]
        [Required]
        [Workspace(Default)]
        InternalOrganisation InternalOrganisation { get; set; }
    }
}
