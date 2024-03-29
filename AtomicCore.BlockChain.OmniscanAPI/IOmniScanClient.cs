﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AtomicCore.BlockChain.OmniscanAPI
{
    /// <summary>
    /// Omni scan client interface
    /// </summary>
    public interface IOmniScanClient
    {
        /// <summary>
        /// Returns the balance for a given address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        Dictionary<string, OmniBtcBalanceJson> GetAddressBTC(string address);

        /// <summary>
        /// Returns the balance information for a given address. 
        /// For multiple addresses in a single query use the v2 endpoint
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        OmniAssetCollectionJson GetAddressV1(string address);

        /// <summary>
        /// Returns the balance information for multiple addresses
        /// </summary>
        /// <param name="addresses"></param>
        /// <returns></returns>
        Dictionary<string, OmniAssetCollectionJson> GetAddressV2(params string[] addresses);

        /// <summary>
        /// Returns the balance information and transaction history list for a given address
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        OmniAddressDetailsResponse GetAddressDetails(string address);

        /// <summary>
        /// Returns the Armory encoded version of an unsigned transaction for 
        /// use with Armory offline transactions. 
        /// Data: 
        ///     unsigned_hex : raw bitcoin hex formatted tx to be converted 
        ///     pubkey : pubkey of the sending address
        /// </summary>
        /// <param name="unsignedHex"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        [Obsolete("The remote server returned an error: (502) Bad Gateway.")]
        OmniArmoryUnsignedResponse GetUnsigned(string unsignedHex, string publicKey);

        /// <summary>
        /// Decodes and returns the raw hex and signed status from an armory transaction. 
        /// Data: 
        ///     armory_tx : armory transaction in text format
        /// </summary>
        /// <param name="armoryTx"></param>
        /// <returns></returns>
        [Obsolete("The remote server returned an error: (502) Bad Gateway.")]
        OmniRawTransactionResponse GetRawtransaction(string armoryTx);

        /// <summary>
        /// Decodes raw hex returning Omni and Bitcoin transaction information
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        [Obsolete("The remote server returned an error: (502) Bad Gateway.")]
        OmniDecodeResponse Decode(string hex);

        /// <summary>
        /// Return a list of currently active/available base currencies the omnidex 
        /// has open orders against. 
        /// Data: 
        ///     ecosystem : 
        ///         1 for main / production ecosystem 
        ///         or 
        ///         2 for test/development ecosystem
        /// </summary>
        /// <param name="ecosystem"></param>
        /// <returns></returns>
        OmniDesignatingCurrenciesResponse DesignatingCurrencies(int ecosystem);

        /// <summary>
        /// Returns list of transactions (up to 10 per page) relevant to queried Property ID. 
        /// Returned transaction types include: 
        /// Creation Tx, Change issuer txs, Grant Txs, Revoke Txs, Crowdsale Participation Txs, 
        /// Close Crowdsale earlier tx
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        OmniTxHistoryResponse GetHistory(int propertyId, int page = 1);

        /// <summary>
        /// Return list of properties created by a queried address.
        /// </summary>
        /// <param name="addresses"></param>
        /// <returns></returns>
        OmniListByOwnerResponse ListByOwner(params string[] addresses);

        /// <summary>
        /// Returns list of currently active crowdsales. 
        /// Data: 
        ///     ecosystem : 
        ///         1 for production/main ecosystem. 
        ///         2 for test/dev ecosystem
        /// </summary>
        /// <param name="ecosystem"></param>
        /// <returns></returns>
        OmniCrowdSalesResponse ListActiveCrowdSales(int ecosystem);

        /// <summary>
        /// returns list of created properties filtered by ecosystem. 
        /// Data: 
        ///     ecosystem : 
        ///         1 for production/main ecosystem. 
        ///         2 for test/dev ecosystem
        /// </summary>
        /// <param name="ecosystem"></param>
        /// <returns></returns>
        OmniListByEcosystemResponse ListByEcosystem(int ecosystem);

        /// <summary>
        /// Returns list of all created properties.
        /// </summary>
        /// <returns></returns>
        OmniCoinListResponse PropertyList();

        /// <summary>
        /// Search by transaction id, address or property id. 
        /// Data: 
        ///     query : 
        ///         text string of either Transaction ID, Address, or property id to search for
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        OmniSearchResponse Search(string query);

        /// <summary>
        /// Returns list of transactions for queried address. 
        /// Data: 
        ///     addr : 
        ///         address to query page : 
        ///             cycle through available response pages (10 txs per page)
        /// </summary>
        /// <param name="address"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        OmniTransactionListResponse GetTxList(string address, int page = 0);

        /// <summary>
        /// Broadcast a signed transaction to the network. 
        /// Data: 
        ///     signedTransaction : signed hex to broadcast
        /// </summary>
        /// <param name="signedTransaction"></param>
        /// <returns></returns>
        OmniPushTxResponse PushTx(string signedTransaction);

        /// <summary>
        /// Returns transaction details of a queried transaction hash.
        /// </summary>
        /// <param name="txHash"></param>
        /// <returns></returns>
        OmniTxInfoResponse GetTx(string txHash);
    }
}
