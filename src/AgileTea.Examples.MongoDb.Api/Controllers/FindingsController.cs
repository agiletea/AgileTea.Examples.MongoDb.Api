using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AgileTea.Examples.MongoDb.Api.Adapters;
using AgileTea.Examples.MongoDb.Api.CsvMapping;
using AgileTea.Examples.MongoDb.Api.Entities;
using AgileTea.Examples.MongoDb.Api.Models;
using AgileTea.Examples.MongoDb.Api.Repositories;
using AgileTea.Persistence.Common.Persistence;
using AgileTea.Persistence.Common.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace AgileTea.Examples.MongoDb.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FindingsController : ControllerBase
    {
        private readonly IDataMappingService _dataMappingService;
        private readonly IFindingDocumentRepository _repository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly FindingDocumentAdapter _adapter;
        private readonly ILogger<FindingsController> _logger;

        public FindingsController(
            IDataMappingService dataMappingService,
            IFindingDocumentRepository repository,
            IUnitOfWorkFactory unitOfWorkFactory,
            FindingDocumentAdapter adapter,
            ILoggerFactory loggerFactory)
        {
            _dataMappingService = dataMappingService;
            _repository = repository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _adapter = adapter;
            _logger = loggerFactory.CreateLogger<FindingsController>();
        }

        [HttpGet]
        public async Task<IEnumerable<FindingModel>?> GetAsync()
        {
            var documents = await _repository.GetAllAsync().ConfigureAwait(false);

            return documents?.Select(_adapter.ToApiModel);
        }

        [HttpGet("anomaly/{id}")]
        public async Task<IEnumerable<FindingModel>?> GetByAnomalyAsync(Guid id)
        {
            var sw = new Stopwatch();
            sw.Start();
            var documents = await _repository.GetByAnomalyIdAsync(id).ConfigureAwait(false);
            
            _logger.LogInformation($"Time to find {documents.Count()} findings by anomaly id: {sw.ElapsedMilliseconds}ms");
            sw.Restart();

            var results = documents?.Select(_adapter.ToApiModel);

            _logger.LogInformation($"Time to adapt findings: {sw.ElapsedMilliseconds}ms");

            sw.Stop();

            return results;
        }

        [HttpGet("{id}")]
        public async Task<FindingModel> GetAsync(string id)
        {
            return _adapter.ToApiModel(await _repository.GetByIdAsync(new ObjectId(id)).ConfigureAwait(false));
        }

        [HttpPost("import")]
        public async Task PostAsync()
        {
            var formFile = HttpContext.Request.Form.Files.FirstOrDefault();

            using var stream = formFile.OpenReadStream();

            var reader = new FindingsCsvReader(stream);
            
            var dataMap = reader.ReadMappings();

            var collection = _dataMappingService.CreateMappedObjects<FindingCsvModel>(dataMap);

            using var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork(_repository);

            foreach (var findingModel in collection)
            {

                var document = _adapter.ToDocument(findingModel);

                _repository.Add(document);

            }

            await unitOfWork.CommitAsync().ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<FindingModel> PostAsync([FromBody] FindingModel model)
        {
            using var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork(_repository);

            var document = _adapter.ToDocument(model);
            
            _repository.Add(document);
            await unitOfWork.CommitAsync().ConfigureAwait(false);
            
            _logger.LogInformation($"Added new document with an id of {model.Id} and timestamp of {model.Timestamp}");

            return _adapter.ToApiModel(document);
        }

        //[HttpPut("updateTs/{index}/{pageCount}")]
        //public async Task PutTimestampAsync(int index, int pageCount)
        //{
        //    var sw = new Stopwatch();
            
        //    sw.Start();
            
        //    _logger.LogInformation($"Starting at {sw.ElapsedMilliseconds}");
        //    var documents = await _repository.GetAllAsync().ConfigureAwait(false);

        //    _logger.LogInformation($"All documents recevied at {sw.ElapsedMilliseconds}");
            
        //    var collection = documents.Skip(index).Take(pageCount);
            
        //    using var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork(_repository);

        //    foreach (var findingDocument in collection)
        //    {
        //        findingDocument.Timestamp = new BsonTimestamp(findingDocument.TimestampAsString);
        //        _repository.Update(findingDocument);
        //    }

        //    _logger.LogInformation($"Updated documents at {sw.ElapsedMilliseconds}");

        //    await unitOfWork.CommitAsync().ConfigureAwait(false);

        //    sw.Stop();
            
        //    _logger.LogInformation($"Completed update after {sw.ElapsedMilliseconds}ms");
        //}

        [HttpPut]
        public async Task<FindingModel> PutAsync([FromBody] FindingModel model)
        {
            using var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork(_repository);
            var document = _adapter.ToDocument(model);

            _repository.Update(document);
            await unitOfWork.CommitAsync().ConfigureAwait(false);

            _logger.LogInformation($"Updated document with an id of {document.Id} and timestamp of {document.Timestamp}");

            return _adapter.ToApiModel(document);
        }

        [HttpPut("anomaly/{findingId}")]
        public async Task<FindingModel> PutAsync(string findingId, [FromBody] AnomalyModel model)
        {
            using var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork(_repository);
            var document = _adapter.ToDocument(model);

            await _repository.UpdateAnomalyAsync(findingId, document);
            await unitOfWork.CommitAsync().ConfigureAwait(false);

            return _adapter.ToApiModel(await _repository.GetByIdAsync(new ObjectId(findingId)).ConfigureAwait(false));
        }



        [HttpDelete("{id}")]
        public async Task<string> DeleteAsync(string id)
        {
            using var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork(_repository);
            _repository.Remove(new ObjectId(id));
            await unitOfWork.CommitAsync().ConfigureAwait(false);

            _logger.LogInformation($"Removed document with an id of {id}");

            return id;
        }
    }
}
