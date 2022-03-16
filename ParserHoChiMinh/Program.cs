using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ParserHoChiMinh
{
    class Program
    {        
        private static Logger logger = LogManager.GetCurrentClassLogger();
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                logger.Info("НАЧАЛО РАБОТЫ ПАРСЕРА");
                List<string> Name = new List<string>();
                List<string> Code = new List<string>();
                List<string> ClosePrise = new List<string>();
                List<string> OpenPrise = new List<string>();
                List<string> CeilingPrice = new List<string>();
                List<string> FloorPrice = new List<string>();
                List<string> UpDown = new List<string>();
                List<string> PercUpDown = new List<string>();
                List<string> ReferencePrice = new List<string>();
                List<string> LowPrice = new List<string>();
                List<string> HighPrice = new List<string>();
                List<string> AvaragePrice = new List<string>();
                List<string> TradingVolume = new List<string>();
                List<string> TradingValue = new List<string>();
                string url = "https://www.hsx.vn/Modules/Rsde/Report/QuoteReport?pageFieldName1=Date&pageFieldValue1=" + DateTime.Today.ToString("dd.MM.yyyy") + "&pageFieldOperator1=eq&pageFieldName2=KeyWord&pageFieldOperator2=&pageFieldName3=IndexType&pageFieldValue3=0&pageFieldOperator3=&pageCriteriaLength=3&rows=2147483647&page=1";
                WebRequest webRequest;
                bool exit = true;
                while (exit)
                {
                    exit = false;
                    string json = "";
                    webRequest = (HttpWebRequest)WebRequest.Create(url);
                    webRequest.Timeout = 40000;
                    webRequest.Method = "GET";
                    webRequest.ContentType = "/application/text";
                    using (var response = webRequest.GetResponse())
                    {
                        using (var resopnseStream = response.GetResponseStream())
                        {
                            using (StreamReader streamReader = new StreamReader(resopnseStream))
                            {
                                json = streamReader.ReadToEnd();
                            }
                        }
                    }
                    dynamic dataJson = JsonConvert.DeserializeObject(json);
                    foreach (var item in dataJson.rows)
                    {
                        try
                        {
                            Code.Add(item.cell[3].ToString().Replace(',', '.') + ", ");
                            CeilingPrice.Add(item.cell[4].ToString().Replace(',', '.') + ", ");
                            FloorPrice.Add(item.cell[5].ToString().Replace(',', '.') + ", ");
                            ReferencePrice.Add(item.cell[6].ToString().Replace(',', '.') + ", ");
                            OpenPrise.Add(item.cell[7].ToString().Replace(',', '.') + ", ");
                            ClosePrise.Add(item.cell[8].ToString().Replace(',', '.') + ", ");
                            UpDown.Add(item.cell[9].ToString().Replace(',', '.') + ", ");
                            PercUpDown.Add(item.cell[10].ToString().Replace(',', '.') + ", ");
                            LowPrice.Add(item.cell[11].ToString().Replace(',', '.') + ", ");
                            HighPrice.Add(item.cell[12].ToString().Replace(',', '.') + ", ");
                            AvaragePrice.Add(item.cell[13].ToString().Replace(',', '.') + ", ");
                            TradingVolume.Add(item.cell[14].ToString().Replace(',', '.') + ", ");
                            TradingValue.Add(item.cell[15].ToString().Replace(',', '.'));
                            logger.Info("Загрузка stocks завершена");
                        }
                        catch (Exception ex)
                        {
                            logger.Debug(ex);
                        }
                        Name.Add(item.cell[0].ToString().Replace(',', '.') + "-" + item.cell[2].ToString().Replace(',', '.') + ", ");
                    }
                }
                try
                {
                    using (StreamWriter stream = new StreamWriter(args[0] + "\\" + DateTime.Today.ToString("dd" + "MM" + "yyyy") + ".csv"))
                    {
                        logger.Info("Загрузка данных в файл");
                        int k = 0;
                        stream.WriteLine("sep=,");
                        stream.WriteLine("Code, Name, Close price, Open price, Ceiling price, Floor price, Reference price, Up/Down, Perc Up/Down, Low price, High price, Average price, Trading volume, Trading value");
                        foreach (var item in Name)
                        {
                            try
                            {
                                stream.WriteLine(Code[k] + item + ClosePrise[k] + OpenPrise[k] + CeilingPrice[k] + FloorPrice[k] + ReferencePrice[k] + UpDown[k] + PercUpDown[k] + LowPrice[k] + HighPrice[k] + AvaragePrice[k] + TradingVolume[k] + TradingValue[k]);
                                k++;
                            }
                            catch (Exception ex)
                            {
                                logger.Debug(ex);
                            }                          
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Debug(ex);
                }                              
            }
            catch (Exception ex)
            {
                logger.Debug(ex);
            }
            logger.Info("ОКОНЧАНИЕ РАБОТЫ ПАРСЕРА");
        }
    }
}
