using System;
using AgileTea.Examples.MongoDb.Api.Entities;
using AgileTea.Persistence.Common.Repository;
using AgileTea.Persistence.Mongo.Context;
using AgileTea.Persistence.Mongo.Repository;
using Microsoft.Extensions.Logging;

namespace AgileTea.Examples.MongoDb.Api.Repositories
{
    internal class ExampleDocumentRepository : GuidDocumentRepositoryBase<ExampleDocument>, IRepository<ExampleDocument, Guid>
    {
        public ExampleDocumentRepository(IMongoContext context, ILoggerFactory loggerFactory)
            : base(context, loggerFactory.CreateLogger<ExampleDocumentRepository>())
        {
        }
    }
}
