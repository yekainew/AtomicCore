﻿using Newtonsoft.Json;
using System;

namespace AtomicCore.BlockChain.TronscanAPI
{
    /// <summary>
    /// Tron ULong Type Json Converter
    /// </summary>
    public sealed class BizTronULongJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ulong);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;

            return ulong.Parse(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
