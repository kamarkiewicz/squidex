﻿// ==========================================================================
//  MongoPositionStorage.cs
//  PinkParrot Headless CMS
// ==========================================================================
//  Copyright (c) PinkParrot Group
//  All rights reserved.
// ==========================================================================

using MongoDB.Bson;
using MongoDB.Driver;
using PinkParrot.Infrastructure.CQRS.EventStore;
using PinkParrot.Store.MongoDb.Utils;

// ReSharper disable InvertIf

namespace PinkParrot.Store.MongoDb.Infrastructure
{
    public sealed class MongoStreamPositionStorage : MongoRepositoryBase<MongoStreamPositionEntity>, IStreamPositionStorage
    {
        private static readonly ObjectId Id = new ObjectId("507f1f77bcf86cd799439011");

        public MongoStreamPositionStorage(IMongoDatabase database)
            : base(database)
        {
        }

        protected override string CollectionName()
        {
            return "StreamPositions";
        }

        public int? ReadPosition()
        {
            var document = Collection.Find(t => t.Id == Id).FirstOrDefault();

            if (document == null)
            {
                document = new MongoStreamPositionEntity { Id = Id };

                Collection.InsertOne(document);
            }

            return document.Position;
        }

        public void WritePosition(int position)
        {
            Collection.UpdateOne(t => t.Id == Id, Update.Set(t => t.Position, position));
        }
    }
}