// <copyright file="Person.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Repository
{
    using System;

    using Allors.Repository.Attributes; using static Workspaces;

    [Workspace(Default)]
    public partial class Person : Addressable, Deletable
    {
        #region inherited properties
        public Address Address { get; set; }

        #endregion

        #region Allors
        [Id("e9e7c874-4d94-42ff-a4c9-414d05ff9533")]
        
        
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        #endregion
        public Address[] AllAddresses { get; set; }

        #region Allors
        [Id("2a25125f-3545-4209-afc6-523eb0d8851e")]
        
        
        #endregion
        public int Age { get; set; }

        #region Allors
        [Id("adf83a86-878d-4148-a9fc-152f56697136")]
        
        
        [Workspace(Default)]
        #endregion
        public DateTime BirthDate { get; set; }

        #region Allors
        [Id("688ebeb9-8a53-4e8d-b284-3faa0a01ef7c")]
        
        
        [Size(256)]
        #endregion
        [Workspace(Default)]
        [Derived]
        public string FullName { get; set; }

        #region Allors
        [Id("654f6c84-62f2-4c0a-9d68-532ed3f39447")]
        
        
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public Gender Gender { get; set; }

        #region Allors
        [Id("a8a3b4b8-c4f2-4054-ab2a-2eac6fd058e4")]
        
        
        #endregion
        public bool IsMarried { get; set; }

        #region Allors
        [Id("54f11f06-8d3f-4d58-bcdc-d40e6820fdad")]
        
        
        #endregion
        [Workspace(Default)]
        public bool IsStudent { get; set; }

        #region Allors
        [Id("6340de2a-c3b1-4893-a7f3-cb924b82fa0e")]
        
        
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public MailboxAddress MailboxAddress { get; set; }

        #region Allors
        [Id("0375a3d3-1a1b-4cbb-b735-1fe508bcc672")]
        
        
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        #endregion
        public Address MainAddress { get; set; }

        #region Allors
        [Id("b3ddd2df-8a5a-4747-bd4f-1f1eb37386b3")]
        
        
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        [Workspace(Default)]
        #endregion
        public Media Photo { get; set; }

        #region Allors
        [Id("2E878C18-9DF7-4DEF-8145-983F4A5CCB2D")]
        
        
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        [Workspace(Default)]
        #endregion
        public Media[] Pictures { get; set; }

        #region Allors
        [Id("6b626ba5-0c45-48c7-8b6b-5ea85e002d90")]
        
        
        #endregion
        public int ShirtSize { get; set; }

        #region Allors
        [Id("1b057406-3343-426b-ab5b-ceb93ba02446")]
        
        
        [Size(-1)]
        #endregion
        public string Text { get; set; }

        #region Allors
        [Id("15de4e58-c5ef-4ebb-9bf6-5ab06a02c5a4")]
        
        
        [Size(-1)]
        #endregion
        public string TinyMCEText { get; set; }

        #region Allors
        [Id("afc32e62-c310-421b-8c1d-6f2b0bb88b54")]
        
        
        [Precision(19)]
        [Scale(2)]
        [Workspace(Default)]
        #endregion
        public decimal Weight { get; set; }

        #region Allors
        [Id("5661A98D-A935-4325-9B28-9D86175B1BD6")]
        
        
        #endregion
        [Workspace(Default)]
        [Multiplicity(Multiplicity.ManyToOne)]
        [Indexed]
        public Organisation CycleOne { get; set; }

        #region Allors
        [Id("2EB2AF4F-2BF4-475F-BB41-D740197F168E")]
        
        
        #endregion
        [Workspace(Default)]
        [Multiplicity(Multiplicity.ManyToMany)]
        [Indexed]
        public Organisation[] CycleMany { get; set; }

        #region Allors
        [Id("DF313FD7-2DC5-48E8-87FD-B629896E3242")]
        
        
        #endregion
        [Workspace(Default)]
        [Indexed]
        public string WorkspaceField { get; set; }

        #region Allors
        [Id("06AD47A5-28F2-4AF2-96C7-4E748428B081")]
        
        
        #endregion
        [Indexed]
        public string DatabaseOnlyField { get; set; }
        
        #region inherited methods
        public void Delete() { }
        #endregion

        [Id("FAF120ED-09D1-4E42-86A6-F0D9FF75E03C")]
        public void Method() { }
    }
}
