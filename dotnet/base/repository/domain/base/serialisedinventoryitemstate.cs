// <copyright file="SerialisedInventoryItemState.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes; using static Workspaces;

    #region Allors
    [Id("d042eeae-5c17-4936-861b-aaa9dfaed254")]
    #endregion
    public partial class SerialisedInventoryItemState : ObjectState
    {
        #region inherited properties
        public Restriction[] Restrictions { get; set; }

        public SecurityToken[] SecurityTokens { get; set; }

        public Restriction ObjectRestriction { get; set; }

        public string Name { get; set; }

        public Guid UniqueId { get; set; }

        #endregion

        #region inherited methods

        public void OnBuild() { }

        public void OnPostBuild() { }

        public void OnInit()
        {
        }

        public void OnPreDerive() { }

        public void OnDerive() { }

        public void OnPostDerive() { }

        #endregion

        #region Allors
        [Id("A38DDADC-22A1-4606-9059-2D803F3F4EF1")]
        
        
        #endregion
        [Indexed]
        [Workspace(Default)]
        public bool IsActive { get; set; }
    }
}
