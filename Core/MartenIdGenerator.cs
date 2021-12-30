using System;
using Marten;
using Marten.Schema.Identity;

namespace Core
{
    public class MartenIdGenerator : IIdGenerator
    {
        private readonly IDocumentSession _documentSession;

        public MartenIdGenerator(IDocumentSession documentSession)
        {
            this._documentSession = documentSession ?? throw new ArgumentNullException(nameof(documentSession));
        }

        public Guid New() => CombGuidIdGeneration.NewGuid();
    }
}