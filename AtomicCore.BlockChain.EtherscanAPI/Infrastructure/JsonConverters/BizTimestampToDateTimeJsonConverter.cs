﻿using Newtonsoft.Json;
using System;

namespace AtomicCore.BlockChain.EtherscanAPI
{
    /// <summary>
    /// TimeStamp与DateTime互转JSON解析器
    /// </summary>
    public sealed class BizTimestampToDateTimeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;

            var t = long.Parse(reader.Value.ToString());
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(t);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((long)Math.Round(((DateTime)value - new DateTime(1970, 1, 1)).TotalSeconds));
        }
    }
}
