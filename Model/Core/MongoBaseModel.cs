using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Core
{
    public class MongoBaseModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public MongoBaseModel()
        {
        }

        public DateTime CreateDate { get; set; }
    }
}
