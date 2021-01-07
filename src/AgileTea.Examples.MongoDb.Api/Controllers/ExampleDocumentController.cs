using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AgileTea.Examples.MongoDb.Api.Adapters;
using AgileTea.Examples.MongoDb.Api.Entities;
using AgileTea.Examples.MongoDb.Api.Models;
using AgileTea.Persistence.Common.Persistence;
using AgileTea.Persistence.Common.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AgileTea.Examples.MongoDb.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExampleDocumentController : ControllerBase
    {
        private readonly IRepository<ExampleDocument, Guid> _repository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ExampleDocumentAdapter _adapter;
        private readonly ILogger<ExampleDocumentController> _logger;

        public ExampleDocumentController(
            IRepository<ExampleDocument, Guid> repository,
            IUnitOfWorkFactory unitOfWorkFactory,
            ExampleDocumentAdapter adapter,
            ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _adapter = adapter;
            _logger = loggerFactory.CreateLogger<ExampleDocumentController>();
        }

        [HttpGet]
        public async Task<IEnumerable<ExampleDocumentModel>?> GetAsync()
        {
            var documents = await _repository.GetAllAsync().ConfigureAwait(false);

            return documents?.Select(_adapter.ToApiModel);
        }
        
        [HttpGet("{id}")]
        public async Task<ExampleDocumentModel> GetAsync(Guid id)
        {
            return _adapter.ToApiModel(await _repository.GetByIdAsync(id).ConfigureAwait(false));
        }

        [HttpPost]
        public async Task<ExampleDocumentModel> PostAsync([FromBody] ExampleDocumentModel model)
        {
            using var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork(_repository);

            var document = _adapter.ToDocument(model);
            
            _repository.Add(document);
            await unitOfWork.CommitAsync().ConfigureAwait(false);
            
            _logger.LogInformation($"Added new document with an id of {model.Id}");

            return _adapter.ToApiModel(document);
        }

        [HttpPut]
        public async Task<ExampleDocumentModel> PutAsync([FromBody] ExampleDocumentModel model)
        {
            using var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork(_repository);
            var document = _adapter.ToDocument(model);

            _repository.Update(document);
            await unitOfWork.CommitAsync().ConfigureAwait(false);

            _logger.LogInformation($"Updated document with an id of {document.Id}");

            return _adapter.ToApiModel(document);
        }



        [HttpDelete("{id}")]
        public async Task<Guid> DeleteAsync(Guid id)
        {
            using var unitOfWork = _unitOfWorkFactory.CreateUnitOfWork(_repository);
            _repository.Remove(id);
            await unitOfWork.CommitAsync().ConfigureAwait(false);

            _logger.LogInformation($"Removed document with an id of {id}");

            return id;
        }
    }
}
