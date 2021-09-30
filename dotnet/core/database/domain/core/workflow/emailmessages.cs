// <copyright file="EmailMessages.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;

    public partial class EmailMessages
    {
        public void Send()
        {
            var session = this.Transaction;

            var mailer = session.Database.Services.Get<IMailer>();
            var emailMessages = this.Extent();
            emailMessages.Filter.AddNot().AddExists(this.Meta.DateSending);
            emailMessages.Filter.AddNot().AddExists(this.Meta.DateSent);

            foreach (EmailMessage emailMessage in emailMessages)
            {
                try
                {
                    emailMessage.DateSending = session.Now();

                    session.Derive();
                    session.Commit();

                    mailer.Send(emailMessage);
                    emailMessage.DateSent = session.Now();

                    session.Derive();
                    session.Commit();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    session.Rollback();
                    break;
                }
            }
        }
    }
}
