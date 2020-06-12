using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("First Challenge:");
            ProcessBankTransactions();
            Console.WriteLine("----------------------------");
            Console.WriteLine("Second Challenge:");
            ReverseInPlace();

            Console.ReadKey();
        }

        public static void ReverseInPlace()
        {
            int[] a = { 1, 4, 6, 9 };

            for (int i = 0; i < a.Length / 2; i++)
            {
                int temp = a[i];
                a[i] = a[a.Length - i - 1];
                a[a.Length - i - 1] = temp;
            }

            for (int i = 0; i < a.Length; i++)
            {
                Console.Write(a[i] + " ");
            }

        }

        public static void ProcessBankTransactions()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "list.json");
            string json = File.ReadAllText(path);
            var list = JsonConvert.DeserializeObject<List<TranItem>>(json);

            var finalList = new List<TranItem>();

            list.GroupBy(x => (x.Sender, x.Receiver))
                .Select(item =>
                new TranItem
                {
                    Sender = item.Key.Sender,
                    Receiver = item.Key.Receiver,
                    Amount = item.Sum(x => Convert.ToInt32(x.Amount))
                }).ToList()
                  .ForEach(item => CompareAndAddToList(finalList, item));

            foreach (var item in finalList.ToList())
            {
                Console.WriteLine(item.Receiver + "," + item.Sender + "," + item.Amount);
            }
        }

        private static void CompareAndAddToList(List<TranItem> finalList, TranItem newRecord)
        {
            TranItem inListItem = finalList.FirstOrDefault(x => x.Receiver == newRecord.Sender && x.Sender == newRecord.Receiver);
            if (inListItem!=null)
            {
                if (newRecord.Amount >= inListItem.Amount)
                {
                    newRecord.Amount -= inListItem.Amount;
                    finalList.Remove(inListItem);
                }
                else
                {
                    inListItem.Amount -= newRecord.Amount;
                    return;
                }
            }

            if (newRecord.Amount != 0)
            {
                finalList.Add(newRecord);
            }

        }

        public class TranItem
        {
            [JsonProperty("sender")]
            public string Sender { get; set; }

            [JsonProperty("receiver")]
            public string Receiver { get; set; }

            [JsonProperty("amount")]
            public int Amount { get; set; }
            public TranItem()
            {

            }
            public TranItem(string sender, string receiver, int amount)
            {
                Sender = sender;
                Receiver = receiver;
                Amount = amount;
            }
        }
    }
}
