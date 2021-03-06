// <copyright file="Setup.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Generic;
    using Meta;

    public partial class Setup
    {
        private readonly ITransaction transaction;

        private readonly Dictionary<IObjectType, IObjects> objectsByObjectType;
        private readonly ObjectsGraph objectsGraph;

        public Setup(IDatabase database, Config config)
        {
            this.Config = config;
            this.M = database.Services.Get<MetaPopulation>();

            Permissions.Sync(database);
            this.transaction = database.CreateTransaction();

            this.objectsByObjectType = new Dictionary<IObjectType, IObjects>();
            foreach (var objectType in this.transaction.Database.MetaPopulation.DatabaseComposites)
            {
                this.objectsByObjectType[objectType] = objectType.GetObjects(this.transaction);
            }

            this.objectsGraph = new ObjectsGraph();
        }

        public Config Config { get; }

        public MetaPopulation M { get; }

        public void Apply()
        {
            this.OnPrePrepare();

            foreach (var objects in this.objectsByObjectType.Values)
            {
                objects.Prepare(this);
            }

            this.OnPostPrepare();

            this.OnPreSetup();

            this.objectsGraph.Invoke(objects => objects.Setup(this));

            this.OnPostSetup();

            this.transaction.Derive();

            if (this.Config.SetupSecurity)
            {
                new Security(this.transaction).Apply();
            }

            this.transaction.Derive();
            this.transaction.Commit();
        }

        public void Add(IObjects objects) => this.objectsGraph.Add(objects);

        /// <summary>
        /// The dependee is set up before the dependent object;.
        /// </summary>
        /// <param name="dependent"></param>
        /// <param name="dependee"></param>
        public void AddDependency(ObjectType dependent, ObjectType dependee) => this.objectsGraph.AddDependency(this.objectsByObjectType[dependent], this.objectsByObjectType[dependee]);

        private void CoreOnPrePrepare()
        {
        }

        private void CoreOnPostSetup()
        {
        }

        private void CoreOnPostPrepare()
        {
        }

        private void CoreOnPreSetup()
        {
        }
    }
}
