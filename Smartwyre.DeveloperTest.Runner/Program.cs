using Microsoft.Extensions.DependencyInjection;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<IRebateService, RebateService>()
            .AddTransient<IProductDataStore, ProductDataStore>()
            .AddTransient<IRebateDataStore, RebateDataStore>()
            .BuildServiceProvider();

        var productDataStore = new ProductDataStore();
        var rebateDataStore = new RebateDataStore();
        var request = new CalculateRebateRequest();
        var service = new RebateService(productDataStore, rebateDataStore);

        var result = service.CalculateAsync(request);
    }
}

