// <copyright file="TaskExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System.Collections.Generic;
    using System.Linq;
    using Derivations;

    public static partial class TaskExtensions
    {
        public static void CoreOnBuild(this Task @this, ObjectOnBuild method)
        {
            if (!@this.ExistDateCreated)
            {
                @this.DateCreated = @this.Strategy.Transaction.Now();
            }
        }

        public static void CoreOnPreDerive(this Task @this, ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            // TODO: Review
            if (iteration.IsMarked(@this) || changeSet.HasChangedRoles(@this) || changeSet.IsCreated(@this))
            {
                foreach (var taskAssignment in @this.TaskAssignmentsWhereTask)
                {
                    iteration.AddDependency(taskAssignment, @this);
                    iteration.Mark(taskAssignment);
                }
            }
        }

        public static void CoreDelete(this Task @this, DeletableDelete method)
        {
            foreach (var taskAssignment in @this.TaskAssignmentsWhereTask)
            {
                taskAssignment.Delete();
            }
        }

        public static void AssignPerformer(this Task @this)
        {
            var currentUser = @this.Strategy.Transaction.Services.Get<IUserService>().User as Person;
            @this.Performer = currentUser;
        }

        public static void AssignParticipants(this Task @this, IEnumerable<User> participants)
        {
            var session = @this.Strategy.Transaction;

            var participantSet = new HashSet<User>(participants.Where(v => v != null).Distinct());

            @this.Participants = participantSet.ToArray();

            // Manage Security
            var defaultSecurityToken = new SecurityTokens(session).DefaultSecurityToken;
            var securityTokens = new HashSet<SecurityToken> { defaultSecurityToken };
            var ownerSecurityTokens = participantSet.Where(v => v.ExistOwnerSecurityToken).Select(v => v.OwnerSecurityToken);
            securityTokens.UnionWith(ownerSecurityTokens);
            @this.SecurityTokens = securityTokens.ToArray();

            // Manage TaskAssignments
            foreach (var currentTaskAssignement in @this.TaskAssignmentsWhereTask.ToArray())
            {
                var user = currentTaskAssignement.User;
                if (!participantSet.Contains(user))
                {
                    currentTaskAssignement.Delete();
                }
                else
                {
                    participantSet.Remove(user);
                }
            }

            foreach (var user in participantSet)
            {
                new TaskAssignmentBuilder(session)
                    .WithTask(@this)
                    .WithUser(user)
                    .Build();
            }
        }
    }
}
