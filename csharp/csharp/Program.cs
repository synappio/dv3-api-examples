using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            string operation = args[0];
            string apiKey = args[1];

            Dv3Api apiClient = new Dv3Api(apiKey);
            switch (operation)
            {
                case "realtime":
                    RealtimeValidation(apiClient, args[2]);
                    break;
                case "list":
                    CheckList(apiClient, args[2], args[3]);
                    break;
                default:
                    throw new System.ArgumentException("Invalid operation: " + operation);
            }
        }

        private static void RealtimeValidation(Dv3Api apiClient, string email)
        {
            Console.WriteLine("{0} got a(n) {1}.", email, apiClient.Realtime(email));
        }

        private static void CheckList(Dv3Api apiClient, string listName, string fileName)
        {
            Console.WriteLine("Getting upload URL...");
            string uploadUrl = apiClient.GetUploadUrl(listName, 0, false);
            Console.WriteLine("Upload URL: {0}", uploadUrl);
            Console.WriteLine("Uploading file contents");
            string listId = apiClient.Upload(uploadUrl, fileName);
            Console.WriteLine("List created. List id: {0}", listId);

            while (true)
            {
                dynamic info = apiClient.ListInfo(listId);
                if (info.status_value == "VALIDATED")
                {
                    Console.WriteLine("List validated. Grade summary: {0}", info.grade_summary);
                    break;
                }
                else if (info.status_value == "FAILED")
                {
                    Console.WriteLine("List validationd failed!");
                    break;
                }

                Console.WriteLine("Status: {0} ({1} %)", info.status_value, info.status_percent_complete);
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}
