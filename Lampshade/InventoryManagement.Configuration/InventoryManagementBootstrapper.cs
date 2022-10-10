using InventoryManagement.Application.Contracts.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using InventoryManagement.Infrastructure.EFCore.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;
using InventoryManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using Inventory.Application;
using InventoryManagement.Configuration.Permissions;

namespace InventoryManagement.Configuration
{
    public class InventoryManagementBootstrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IInventoryApplication, InventoryApplication>();
            services.AddTransient<IInventoryRepository, InventoryRepository>();

            services.AddTransient<IPermissionExposer, InventoryPermissionExposer>();

            services.AddDbContext<InventoryContext>(x => x.UseSqlServer(connectionString));

        }
    }
}
