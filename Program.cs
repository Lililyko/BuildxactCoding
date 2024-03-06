using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

namespace SuppliesPriceLister
{
    class Supply{
        public string ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Your solution begins here
            //create list of Supply
            List<Supply> supplies = new List<Supply>();

            //get exchange rate
            string exchangeRateString = File.ReadAllText("appsettings.json");
            double exchangeRate = (double)JsonObject.Parse(exchangeRateString)["audUsdExchangeRate"];

            //get supplies from megacorp.json
            string megaString = File.ReadAllText("megacorp.json");
            JObject obj = JObject.Parse(megaString);
            foreach (var partner in obj["partners"]){
                foreach (var supply in partner["supplies"]){
                    string id = supply["id"].ToString();
                    string name = supply["description"].ToString();
                    double price = Math.Round((double)supply["priceInCents"] / exchangeRate /100, 2);
                    supplies.Add(new Supply { ID=id, Name=name, Price=price });
                }
            }

            //get supplier from humphries.csv
            string[] humpStringLine = File.ReadAllLines("humphries.csv");
            for (int i=1; i<humpStringLine.Length; i++){
                string[] supplyDetails = humpStringLine[i].Split(',');
                string id = supplyDetails[0];
                string name = supplyDetails[1];
                double price = Math.Round(double.Parse(supplyDetails[3]), 2);
                supplies.Add(new Supply { ID=id, Name=name, Price=price });
            }

            //sort supplies with price from highest to lowest
            supplies.Sort((x, y) => y.Price.CompareTo(x.Price));

            //console log the supplies details
            foreach (var supply in supplies){
                Console.WriteLine($"{supply.ID}, {supply.Name}, ${supply.Price}");
            }

        }
    }
}
