using System;
using System.Linq;
using CodeBoss.MultiTenant;

namespace ChurchManager.Application.Tests.Common
{
    public class LocalTenantProvider : ITenantProvider
    {
        public bool Enabled => true;
        public ITenant[] Tenants()
        {
            return new[]
            {
                new Tenant
                {
                    Name = "Tenant1",
                    ConnectionString =
                        "Server=localhost;Port=5432;Database=churchmanager_db;User Id=admin;password=P455word1;"
                }
            };
        }

        public ITenant Get(string name) => Tenants().First();

        public ITenant CurrentTenant
        {
            get => Get("Tenant1");
            set => throw new NotImplementedException();
        }
    }
}