﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomicCore.BlockChain.TronscanAPI
{
    /// <summary>
    /// TRON SCAN INTERFACE
    /// </summary>
    public interface ITronScanClient
    {
        /// <summary>
        /// Block Overview
        /// </summary>
        /// <returns></returns>
        TronOverviewJsonResult BlockOverview();

        /// <summary>
        /// Get Last Block
        /// </summary>
        /// <returns></returns>
        TronBlockJsonResult GetLastBlock();

        //void GetAddressAssets(string address);

        //void GetNormalTransactions();
    }
}
