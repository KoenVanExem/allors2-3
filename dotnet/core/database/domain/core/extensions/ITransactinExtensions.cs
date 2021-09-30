// <copyright file="ITransactionExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ITransactionExtension type.</summary>

namespace Allors.Database.Domain
{
    using System;
    using Database.Derivations;
    using Derivations;

    public static partial class ITransactionExtensions
    {
        public static Validation Derive(this ITransaction transaction, bool throwExceptionOnError = true)
        {
            var derivationService = transaction.Database.Services.Get<IDerivationService>();
            var derivation = derivationService.CreateDerivation(transaction);
            var validation = derivation.Derive();
            if (throwExceptionOnError && validation.HasErrors)
            {
                throw new DerivationException(validation);
            }

            return (Validation)validation;
        }

        public static DateTime Now(this ITransaction transaction)
        {
            var now = DateTime.UtcNow;

            var timeService = transaction.Database.Services.Get<ITime>();
            var timeShift = timeService.Shift;
            if (timeShift != null)
            {
                now = now.Add((TimeSpan)timeShift);
            }

            return now;
        }
    }
}
