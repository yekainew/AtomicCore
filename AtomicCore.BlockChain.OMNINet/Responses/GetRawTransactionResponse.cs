﻿// Copyright (c) 2014 George Kimionis
// Distributed under the GPLv3 software license, see the accompanying file LICENSE or http://opensource.org/licenses/GPL-3.0

namespace AtomicCore.BlockChain.OMNINet
{
    public class GetRawTransactionResponse : ITransactionResponse
    {
        public string Hex { get; set; }
        public string TxId { get; set; }
        public long Version { get; set; }
        public uint LockTime { get; set; }
        public Vin[] Vin { get; set; }
        public Vout[] Vout { get; set; }
        public string BlockHash { get; set; }
        public int Confirmations { get; set; }
        public uint Time { get; set; }
        public uint BlockTime { get; set; }
    }
}
