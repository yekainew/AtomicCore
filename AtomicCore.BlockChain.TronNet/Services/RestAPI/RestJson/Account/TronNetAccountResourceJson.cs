﻿using Newtonsoft.Json;

namespace AtomicCore.BlockChain.TronNet
{
    /// <summary>
    /// TronNet Account Resource Json
    /// </summary>
    public class TronNetAccountResourceJson
    {
        /// <summary>
        /// frozen_balance_for_energy
        /// </summary>
        [JsonProperty("frozen_balance_for_energy")]
        public TronNetFrozenEnergyBalanceJson FrozenEnergyBalance { get; set; }

        /// <summary>
        /// latest_consume_time_for_energy
        /// </summary>
        [JsonProperty("latest_consume_time_for_energy")]
        public ulong LatestEnergyConsumeTime { get; set; }
    }
}
