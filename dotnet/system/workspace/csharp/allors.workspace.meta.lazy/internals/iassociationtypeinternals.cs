// <copyright file="AssociationType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the AssociationType type.</summary>

namespace Allors.Workspace.Meta
{
    public interface IAssociationTypeInternals : IAssociationType
    {
        MetaPopulation MetaPopulation { get; set; }

        new ICompositeInternals ObjectType { get; set; }

        new IRelationTypeInternals RelationType { get; set; }
    }
}
