﻿using Newtonsoft.Json;
using System;

namespace AtomicCore.BlockChain.TronNet
{
    /// <summary>
    /// Tron Transfer Contract Value Json
    /// TRX Transfer,
    /// </summary>
    public class TronNetTransferContractVauleJson : TronNetContractBaseValueJson
    {
        #region Propertys

        /// <summary>
        /// toAddress
        /// </summary>
        [JsonProperty("to_address"), JsonConverter(typeof(TronNetHexAddressJsonConverter))]
        public virtual string ToAddress { get; set; }

        /// <summary>
        /// amount,unit is sun
        /// </summary>
        [JsonProperty("amount"), JsonConverter(typeof(TronNetULongJsonConverter))]
        public virtual ulong Amount { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Amount of decimal
        /// </summary>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public decimal GetAmount(int decimals = 6)
        {
            return Amount / (decimal)Math.Pow(10, decimals);
        }

        #endregion
    }
}