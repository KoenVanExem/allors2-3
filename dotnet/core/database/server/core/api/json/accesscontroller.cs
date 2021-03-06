// <copyright file="DatabaseController.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Protocol.Json
{
    using System;
    using Allors.Protocol.Json.Api.Security;
    using Allors.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("allors/access")]
    public class AccessController : ControllerBase
    {
        public AccessController(IDatabaseService databaseService, IWorkspaceService workspaceService, IPolicyService policyService, ILogger<AccessController> logger)
        {
            this.DatabaseService = databaseService;
            this.WorkspaceService = workspaceService;
            this.PolicyService = policyService;
            this.Logger = logger;
        }

        private IDatabaseService DatabaseService { get; }
        public IWorkspaceService WorkspaceService { get; }

        private IPolicyService PolicyService { get; }

        private ILogger<AccessController> Logger { get; }

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        public ActionResult<AccessResponse> Post([FromBody] AccessRequest accessRequest) =>
            this.PolicyService.SyncPolicy.Execute(
                () =>
                {
                    try
                    {
                        using var transaction = this.DatabaseService.Database.CreateTransaction();
                        var api = new Api(transaction, this.WorkspaceService.Name);
                        return api.Access(accessRequest);
                    }
                    catch (Exception e)
                    {
                        this.Logger.LogError(e, "AccessRequest {request}", accessRequest);
                        throw;
                    }
                });
    }
}
