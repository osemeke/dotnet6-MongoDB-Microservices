using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
//using OrderService.Data;
using ReportingService.Entities;
using ReportingService.Options;
using System.Text;
using System.Text.Json;

namespace ReportingService.Services
{
    public class ProductService
    {
        private readonly IDistributedCache _cache;
        private readonly IMongoCollection<Product> _collection;

        public ProductService(IOptions<AppSettings> options, IDistributedCache cache)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _collection = client.GetDatabase(options.Value.Db)
                .GetCollection<Product>(options.Value.Collection);

            _cache = cache;
        }

        public async Task<Product> GetById(string id) => await GetByIdCached(id);
            //await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetAll() =>
            await _collection.Find(_ => true).ToListAsync();

        private async Task<Product> GetByIdCached(string id)
        {
            Product result;

            string item = await _cache.GetStringAsync($"product_{id}");

            if(item is not null) 
                return JsonSerializer.Deserialize<Product>(item);
            
            result = await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

            var content = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result));

            await _cache.SetAsync($"product_{id}", content).ConfigureAwait(false);

            return result;
        }

    }
}
