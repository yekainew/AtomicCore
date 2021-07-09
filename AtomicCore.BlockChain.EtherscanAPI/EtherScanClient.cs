﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AtomicCore.BlockChain.EtherscanAPI
{
    /// <summary>
    /// IEtherScanClient interface implementation
    /// </summary>
    public class EtherScanClient : IEtherScanClient
    {
        #region Variables

        /// <summary>
        /// 基础URL模版
        /// </summary>
        private const string c_baseUrl = "https://api.etherscan.io";

        /// <summary>
        /// ApiKey Temp Append To End,eg => apikey={0}
        /// </summary>
        private const string c_apiKeyTemp = "&apikey={0}";

        /// <summary>
        /// 标记为最后一个TAG,查询余额等请求会用到该参数
        /// </summary>
        private const string c_latestTag = "&tag=latest";

        /// <summary>
        /// 追加地址
        /// </summary>
        private const string c_addressTemp = "&address={0}";

        /// <summary>
        /// 追加合约地址参数
        /// </summary>
        private const string c_contractAddressTemp = "&contractaddress={0}";

        /// <summary>
        /// API KEY
        /// </summary>
        private readonly string _apiKey;

        #endregion

        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="apiKey"></param>
        public EtherScanClient(string apiKey)
        {
            this._apiKey = apiKey;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 创建Rest Url
        /// </summary>
        /// <param name="module">模块名称</param>
        /// <param name="action">行为名称</param>
        /// <returns></returns>
        private string CreateRestUrl(string module, string action)
        {
            return string.Format(
                "{0}/api?module={1}&action={2}{3}",
                c_baseUrl,
                module,
                action,
                string.IsNullOrEmpty(this._apiKey) ?
                    string.Empty :
                    string.Format(c_apiKeyTemp, this._apiKey)
            );
        }

        /// <summary>
        /// Rest Get Request
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <returns></returns>
        private string RestGet(string url)
        {
            string resp;
            try
            {
                resp = HttpProtocol.HttpGet(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return resp;
        }

        /// <summary>
        /// JSON解析OBJECT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resp"></param>
        /// <returns></returns>
        private T ObjectParse<T>(string resp)
            where T : class, new()
        {
            T jsonResult;
            try
            {
                jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resp);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return jsonResult;
        }

        /// <summary>
        /// JSON解析结构体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resp"></param>
        /// <returns></returns>
        private EtherscanStructResult<T> StructParse<T>(string resp)
            where T : struct
        {
            EtherscanStructResult<T> jsonResult;
            try
            {
                jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<EtherscanStructResult<T>>(resp);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return jsonResult;
        }

        /// <summary>
        /// JSON解析单模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resp"></param>
        /// <returns></returns>
        private EtherscanSingleResult<T> SingleParse<T>(string resp)
            where T : class, new()
        {
            EtherscanSingleResult<T> jsonResult;
            try
            {
                jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<EtherscanSingleResult<T>>(resp);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return jsonResult;
        }

        /// <summary>
        /// JSON解析列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resp"></param>
        /// <returns></returns>
        private EtherscanListResult<T> ListParse<T>(string resp)
            where T : class, new()
        {
            EtherscanListResult<T> jsonResult;
            try
            {
                jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<EtherscanListResult<T>>(resp);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return jsonResult;
        }

        #endregion

        #region IEtherScanClient Methods

        /// <summary>
        /// 获取网络手续费（三档）
        /// https://api-cn.etherscan.com/api?module=gastracker&action=gasoracle
        /// </summary>
        /// <returns></returns>
        public EtherscanSingleResult<EthGasOracleJsonResult> GetGasOracle()
        {
            //拼接URL
            string url = this.CreateRestUrl("gastracker", "gasoracle");

            //请求API
            string resp = this.RestGet(url);

            //解析JSON
            EtherscanSingleResult<EthGasOracleJsonResult> jsonResult = SingleParse<EthGasOracleJsonResult>(resp);

            return jsonResult;
        }

        /// <summary>
        /// 获取地址余额
        /// </summary>
        /// <param name="address">钱包地址</param>
        /// <param name="contractAddress">合约地址,若为空则表示为查询主链行为</param>
        /// <param name="contractDecimals">合约代码小数位</param>
        /// <returns></returns>
        public EtherscanStructResult<decimal> GetBalance(string address, string contractAddress = null, int contractDecimals = 0)
        {
            //基础判断
            if (string.IsNullOrEmpty(address))
                throw new ArgumentNullException("address");

            //URL拼接
            StringBuilder urlBuilder;
            if (string.IsNullOrEmpty(contractAddress))
                urlBuilder = new StringBuilder(this.CreateRestUrl("account", "balance"));
            else
            {
                urlBuilder = new StringBuilder(this.CreateRestUrl("account", "tokenbalance"));
                urlBuilder.AppendFormat(c_contractAddressTemp, contractAddress);
            }
            urlBuilder.AppendFormat(c_addressTemp, address);
            urlBuilder.Append(c_latestTag);

            //请求API
            string resp = this.RestGet(urlBuilder.ToString());

            //解析JSON
            EtherscanStructResult<BigInteger> jsonResult = StructParse<BigInteger>(resp);
            if (jsonResult.Status != EtherscanJsonStatus.Success)
            {
                return new EtherscanStructResult<decimal>
                {
                    Status = jsonResult.Status,
                    Message = jsonResult.Message,
                    Result = decimal.Zero
                };
            }

            //定义返回值
            decimal balance;
            if (string.IsNullOrEmpty(contractAddress))
                balance = Nethereum.Util.UnitConversion.Convert.FromWei(jsonResult.Result);
            else
            {
                if (contractDecimals > 0)
                    balance = Nethereum.Util.UnitConversion.Convert.FromWei(jsonResult.Result, contractDecimals);
                else
                    balance = (decimal)jsonResult.Result;
            }

            return new EtherscanStructResult<decimal>
            {
                Status = jsonResult.Status,
                Message = jsonResult.Message,
                Result = balance
            };
        }

        /// <summary>
        /// 获取交易记录列表
        /// </summary>
        /// <param name="address">钱包地址</param>
        /// <param name="startBlock">起始区块高度</param>
        /// <param name="endBlock">截止区块高度</param>
        /// <param name="sort">排序规则</param>
        /// <param name="page">当前页码</param>
        /// <param name="limit">每页多少条数据</param>
        /// <returns></returns>
        public EtherscanListResult<EthTransactionJsonResult> GetTransactions(string address, ulong? startBlock = null, ulong? endBlock = null, EtherscanSort sort = EtherscanSort.Asc, int? page = 1, int? limit = 1000)
        {
            //拼接URL
            string url = this.CreateRestUrl("account", "txlist");

            //请求参数拼接
            StringBuilder urlBuilder = new StringBuilder(url);
            urlBuilder.AppendFormat("&address={0}", address);
            urlBuilder.AppendFormat("&sort={0}", sort.ToString());
            if (null != startBlock && startBlock > 0)
                urlBuilder.AppendFormat("&startblock={0}", startBlock);
            if (null != endBlock && endBlock > 0)
                urlBuilder.AppendFormat("&endblock={0}", endBlock);
            if (null != page && page > 0)
                urlBuilder.AppendFormat("&page={0}", page);
            if (null != limit && limit > 0)
                urlBuilder.AppendFormat("&offset={0}", limit);

            //请求API
            string resp = this.RestGet(urlBuilder.ToString());

            //解析JSON
            EtherscanListResult<EthTransactionJsonResult> jsonResult = ListParse<EthTransactionJsonResult>(resp);

            return jsonResult;
        }



        #endregion
    }
}