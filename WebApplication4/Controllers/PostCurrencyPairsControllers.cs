using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using WebAPiCurrencies.Models;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace WebAPiCurrencies.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostCurrencyPairsControllers : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private const string DATA = @"{""object"":{""name"":""Name""}}";
        private static string[] pairsValueToSearch = new string[] { "USDILS", "USDEUR", "USDJPY", "USDGGP" };
        private static string url = $"http://apilayer.net/api/live?access_key=8b8b9482b59ef490e54175be2e4790dc&currencies=&source=USD&format=1";

        public PostCurrencyPairsControllers(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("SetCurrencies")]
        public static async Task<JsonResult> PostAsync()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = DATA.Length;
                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(DATA);
                }
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    foreach (string pairValueToSearch in pairsValueToSearch)
                    {
                      if (response.Contains(pairValueToSearch))
                      {
                            string substringOfKeyValue = response.Substring(response.LastIndexOf(pairValueToSearch));
                            float pairValue = RetunPairValue(substringOfKeyValue);
                            if (pairValue != 0)
                            {
                                IDictionary<string, object> pair = new Dictionary<string, object>();
                                pair.Add("@currency_pair", pairValueToSearch);
                                pair.Add("@currency_pair_value", pairValue);
                                pair.Add("@currency_pair_time", DateTime.Now);
                                if (!await UpdateInTableAsync("Proc_Set_Currency_Pairs_", pair))
                                    return new JsonResult($"Got an issue adding {pair} to the DB");
                            }
                      }
                    }                   
                }
                return new JsonResult("added successfully");
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }
            return new JsonResult("Bad request");
        }


        /// <summary>
        /// Updating the DB 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="commandParams"></param>
        public static Task<bool> UpdateInTableAsync(string query, IDictionary<string, object> commandParams)
        {
            using (var conn = new SqlConnection(ConfigSettings.connection))
            {
                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Connection = conn;
                    command.CommandType = CommandType.StoredProcedure;

                    foreach (var param in commandParams)
                    {
                        if (param.Value == null)
                            command.Parameters.AddWithValue(param.Key, DBNull.Value);
                        else
                            command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                    try
                    {
                        conn.Open();
                        int effecedRows=command.ExecuteNonQuery();
                        if(effecedRows>0)
                          return Task.FromResult(true);
                        return Task.FromResult(false);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        command.Parameters.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// get the substring after the pair name. The first value after that is the substring value.
        /// return the pair value 
        /// </summary>
        /// <param name="substringOfKeyValue"></param>
        /// <returns>Returns pair's value</returns>
        private static float RetunPairValue(string str )
        {
 
            try
            {
                // string arr of the float numbers in the substring 
                string[] values = Regex.Split(str, @"[^0-9\.]");
                // ignore empty string
                values = values.Where(s => s != "").ToArray();
                //finding the float number
                 try
                 {
                 //returns the first
                     return Convert.ToSingle(values[0]);  // the first value is the pair's value
                 }catch (FormatException)
                 {
                   Console.WriteLine("Unable to convert '{0}' to a Single.", values[0]);
                 }
                 catch (OverflowException)
                 {
                  Console.WriteLine("'{0}' is outside the range of a Single.", values[0]);
                 }
               
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }
            return 0;
        }
    }
}
