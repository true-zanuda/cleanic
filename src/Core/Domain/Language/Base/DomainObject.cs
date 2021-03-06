﻿namespace Cleanic.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// The object having meaning in the domain.
    /// </summary>
    public abstract class DomainObject : IEquatable<DomainObject>
    {
        /// <summary>
        /// When overriden in a derived class, returns all components which constitute identity.
        /// </summary>
        /// <returns>An ordered list of identity components.</returns>
        protected virtual IEnumerable<Object> GetIdentityComponents()
        {
            return GetType().GetRuntimeProperties()
                .Where(x => x.GetMethod?.IsStatic == false)
                .Select(x => x.GetValue(this));
        }

        public Boolean Equals(DomainObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            var components = GetIdentityComponents().ToArray();
            var otherComponents = other.GetIdentityComponents().ToArray();
            if (components.Length != otherComponents.Length) return false;
            for (var i = 0; i < components.Length; i++)
            {
                if (components[i] is String str1 && otherComponents[i] is String str2)
                {
                    if (!String.Equals(str1, str2, StringComparison.Ordinal)) return false;
                }
                else if (components[i] is DateTime dt1 && otherComponents[i] is DateTime dt2)
                {
                    if (dt1.Year != dt2.Year) return false;
                    if (dt1.Month != dt2.Month) return false;
                    if (dt1.Day != dt2.Day) return false;
                    if (dt1.Hour != dt2.Hour) return false;
                    if (dt1.Minute != dt2.Minute) return false;
                    if (dt1.Second != dt2.Second) return false;
                    if (dt1.Millisecond != dt2.Millisecond) return false;
                }
                else
                {
                    if (!Equals(components[i], otherComponents[i])) return false;
                }
            }
            return true;
        }

        public override Boolean Equals(Object other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;
            return Equals((DomainObject)other);
        }

        public override Int32 GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                foreach (var component in GetIdentityComponents())
                {
                    hash = hash * 23 + component?.GetHashCode() ?? 0;
                }
                return hash;
            }
        }
    }
}