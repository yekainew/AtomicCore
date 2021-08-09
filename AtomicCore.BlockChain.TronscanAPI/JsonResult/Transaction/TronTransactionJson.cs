﻿using Newtonsoft.Json;

namespace AtomicCore.BlockChain.TronscanAPI
{
    /// <summary>
    /// Tron Transaction Json
    /// </summary>
    public class TronTransactionJson
    {
        /// <summary>
        /// transaction Hash
        /// </summary>
        [JsonProperty("transactionHash")]
        public string TransactionHash { get; set; }

        /// <summary>
        /// transaction timestamp
        /// </summary>
        [JsonProperty("timestamp"), JsonConverter(typeof(BizTronULongJsonConverter))]
        public ulong Timestamp { get; set; }

        /// <summary>
        /// transfer form address
        /// </summary>
        [JsonProperty("transferFromAddress")]
        public string TransferFromAddress { get; set; }

        /// <summary>
        /// transfer to address
        /// </summary>
        [JsonProperty("transferToAddress")]
        public string TransferToAddress { get; set; }

        /// <summary>
        /// amount
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// token name
        /// </summary>
        [JsonProperty("tokenName")]
        public string TokenName { get; set; }

        /// <summary>
        /// final result
        /// </summary>
        [JsonProperty("finalResult")]
        public string FinalResult { get; set; }

        /// <summary>
        /// contract return state
        /// </summary>
        [JsonProperty("contractRet")]
        public string ContractRet { get; set; }

        /// <summary>
        /// confirmed
        /// </summary>
        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }

        /// <summary>
        /// token info
        /// </summary>
        [JsonProperty("tokenInfo")]
        public TronTokenBasicJson TokenInfo { get; set; }

        /// <summary>
        /// from address is contract
        /// </summary>
        [JsonProperty("fromAddressIsContract")]
        public bool FromAddressIsContract { get; set; }

        /// <summary>
        /// to address is contract
        /// </summary>
        [JsonProperty("toAddressIsContract")]
        public bool ToAddressIsContract { get; set; }

        /// <summary>
        /// 所属区块高度
        /// </summary>
        [JsonProperty("blockId"), JsonConverter(typeof(BizTronULongJsonConverter))]
        public ulong BlockHeight { get; set; }
    }
}