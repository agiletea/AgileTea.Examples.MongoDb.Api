using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using AgileTea.Examples.MongoDb.Api.Entities;
using AgileTea.Persistence.Common.Repository;
using AgileTea.Persistence.Mongo.Context;
using AgileTea.Persistence.Mongo.Repository;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AgileTea.Examples.MongoDb.Api.Repositories
{
    public class FindingDocumentRepository : ObjectIdDocumentRepositoryBase<FindingDocument>, IFindingDocumentRepository
    {
        private readonly IMongoContext _context;
        private readonly ILogger _logger;

        public FindingDocumentRepository(IMongoContext context, ILoggerFactory loggerFactory)
            : base(context, loggerFactory.CreateLogger<FindingDocumentRepository>())
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<FindingDocumentRepository>();
            CollectionName = "Findings";
        }

        public async Task<IEnumerable<FindingDocument>> GetByAnomalyIdAsync(Guid id)
        {
            //var documents = await GetAllAsync().ConfigureAwait(false);

            //var firstDocuments = documents.Take(10);

            //return documents.Where((doc => doc.Anomaly != null && doc.Anomaly.Id == id));

            //var guidFirstPart = id.ToString().Split('-')[0];


            var result = await ExecuteDbSetFuncAsync(collection =>
                    collection.FindAsync(doc => doc.Anomaly.Id == id))
                .ConfigureAwait(false);


            //var result = await ExecuteDbSetFuncAsync(collection =>
            //        collection.FindAsync(Builders<FindingDocument>.Filter.Eq("anomaly._id", id.ToString())))

            //    .ConfigureAwait(false);

            return result.ToList();
        }

        public async Task UpdateAnomalyAsync(string findingId, Anomaly anomaly)
        {
            var filter = new ObjectId(findingId);

            var update = Builders<FindingDocument>.Update.Set(x => x.Anomaly, anomaly);

            var updateOptions = new UpdateOptions
            {
                IsUpsert = true
            };

            await ExecuteDbSetFuncAsync(collection => collection.UpdateOneAsync(
                x => x.Id == filter,
                update,
                updateOptions)).ConfigureAwait(false);
        }
    }

    public interface IFindingDocumentRepository : IRepository<FindingDocument, ObjectId>
    {
        Task<IEnumerable<FindingDocument>> GetByAnomalyIdAsync(Guid id);
        Task UpdateAnomalyAsync(string findingId, Anomaly anomaly);
    }
}
