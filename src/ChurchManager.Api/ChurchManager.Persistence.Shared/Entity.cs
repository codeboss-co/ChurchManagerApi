using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Codeboss.Types;

namespace ChurchManager.Persistence.Shared
{
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        [Key]
        public TPrimaryKey Id { get; set; }
        public string RecordStatus { get; set; } = "Active";
        public DateTime? InactiveDateTime { get; set; }

        #region Public Methods

        /// <summary>
        /// Creates a dictionary containing the majority of the entity object's properties. The only properties that are excluded
        /// are the Id, Guid and Order.  
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey,TValue}"/> that represents the current entity object. Each <see cref="KeyValuePair{String, Object}"/> includes the property
        /// name as the key and the property value as the value.</returns>
        public virtual Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            var virtualPropsWhiteList = new HashSet<string>
            {
                "CreatedBy",
                "CreatedDate",
                "ModifiedDate",
                "ModifiedBy"
            };

            foreach(var propInfo in this.GetType().GetProperties())
            {
                if((propInfo.GetGetMethod() != null && !propInfo.GetGetMethod().IsVirtual) || virtualPropsWhiteList.Contains(propInfo.Name))
                {
                    dictionary.Add(propInfo.Name, propInfo.GetValue(this, null));
                }
            }

            return dictionary;
        }

        #endregion
    }
}
