﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IsatiWei.Api.Models.Team
{
    public class Team
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ImageId { get; set; }
        [BsonIgnore]
        public byte[] Image { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CaptainId { get; set; }
        [BsonIgnore]
        public string CaptainName { get; set; }
        public string Name { get; set; }
        public List<string> Members { get; set; }
        public int Score { get; set; }

        public Dictionary<string, int> FinishedCallenges;
    }
}
