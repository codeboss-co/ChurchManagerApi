﻿using System.Collections.Generic;
using People.Persistence.Models;

namespace People.Domain.Model
{
    public class PersonDomain : Dictionary<string, object>
    {
        public PersonDomain(Person entity) : base(capacity:10)
        {
            Add("personId", entity.Id);

            Add("title", entity.FullName.Title);
            Add("firstName", entity.FullName.FirstName);
            Add("middleName", entity.FullName.MiddleName);
            Add("lastName", entity.FullName.LastName);
            Add("suffix", entity.FullName.Suffix);

            Add("birthDay", entity.BirthDate.BirthDay);
            Add("birthMonth", entity.BirthDate.BirthMonth);
        }
    }
}
