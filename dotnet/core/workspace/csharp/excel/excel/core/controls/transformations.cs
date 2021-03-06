namespace Application
{
    using System;
    using Allors.Workspace;
    using Allors.Workspace.Meta;

    /// <summary>
    /// Translates true -> YES and vice versa.
    /// </summary>
    public class BooleanTransformation : ITransformation
    {
        public object ToExcel(IObject @object, IRoleType roleType)
        {
            var bl = (bool?)@object.Strategy.GetRole(roleType);

            return bl == true ? "YES" : "NO";
        }

        public object ToDomain(dynamic value)
        {
            return string.Equals(value, "YES", StringComparison.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// Multiplies values by 1000 (ToDomain), and divides by 1000 (ToExcel)
    /// </summary>
    public class Factor1000Transformation : ITransformation
    {
        public object ToExcel(IObject @object, IRoleType roleType)
        {
            var dec = (decimal?)@object.Strategy.GetRole(roleType);

            return dec / 1000;
        }

        public object ToDomain(dynamic value)
        {
            return value * 1000;
        }
    }

    public class TreatZeroAsNullTransformation : ITransformation
    {
        public object ToExcel(IObject @object, IRoleType roleType)
        {
            return @object.Strategy.GetRole(roleType);
        }

        public object ToDomain(dynamic value)
        {
            if (value == 0)
            {
                return null;
            }

            return value;
        }
    }

    public class TreatSameValueAsNullTransformation<T> : ITransformation
    {
        private readonly T compareToValue;

        public TreatSameValueAsNullTransformation(T compareToValue)
        {
            this.compareToValue = compareToValue;
        }

        public object ToExcel(IObject @object, IRoleType roleType)
        {
            return @object.Strategy.GetRole(roleType);
        }

        public object ToDomain(dynamic value)
        {
            if (Equals(this.compareToValue, (T)value))
            {
                return null;
            }

            return value;
        }
    }
}
