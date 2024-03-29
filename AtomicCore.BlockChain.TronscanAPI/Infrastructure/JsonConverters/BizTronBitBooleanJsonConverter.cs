﻿using Newtonsoft.Json;
using System;

namespace AtomicCore.BlockChain.TronscanAPI
{
    /// <summary>
    /// Tron Boolean Json Converter
    /// </summary>
    public sealed class BizTronBitBooleanJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;

            return reader.Value.ToString() == "1";
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((bool)value ? "1" : "0");
        }
    }
}
