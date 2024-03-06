using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace SuppliesPriceLister
{
    class Supply{
        public string ID { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Your solution begins here
            //get exchange rate
            string exchangeRateString = File.ReadAllText("appsettings.json");
            double exchangeRate = (double)JsonObject.Parse(exchangeRateString)["audUsdExchangeRate"];
            Console.WriteLine(exchangeRate);

            //get supplies from megacorp.json
            string megaString = File.ReadAllText("megacorp.json");
            List<Supply> megaSupply = new List<Supply>();
            JObject obj = JObject.Parse(megaString);
            foreach (var partner in obj["partners"]){
                foreach (var supply in partner["supplies"]){
                    string id = supply["id"].ToString();
                    string name = supply["description"].ToString();
                    string price = supply["priceInCents"].ToString();
                    megaSupply.Add(new Supply { ID=id, Name=name, Price=price });
                }
            }
            foreach (var supply in megaSupply){
                Console.WriteLine($"{supply.ID}, {supply.Name}, {supply.Price}");
            }

            //get supplier from humphries.csv
            string[] humpStringLine = File.ReadAllLines("humphries.csv");
            List<Supply> humpSupply = new List<Supply>();
            for (int i=1; i<humpStringLine.Length; i++){
                string[] supplyDetails = humpStringLine[i].Split(',');
                string id = supplyDetails[0];
                string name = supplyDetails[1];
                string price = supplyDetails[3];
                humpSupply.Add(new Supply { ID=id, Name=name, Price=price });
            }
            foreach (var supply in humpSupply){
                Console.WriteLine($"{supply.ID}, {supply.Name}, {supply.Price}");
            }

        }
    }
}
