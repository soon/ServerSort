using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SortServer;

namespace Client
{
    class Program
    {
        static Stopwatch sw = new Stopwatch();
        static void Main(string[] args)
        {
//            var addr = IPAddress.Parse(args[0]);
//            var port = int.Parse(args[1]);
//            var url = string.Format("http://{0}:{1}/sort/", addr, port);

            var tasks = new List<Task>();

            var rng = new Random(42);

            
            sw.Start();
            
            for(int i = 1; i < 20; ++i)
            {
                var arr = new List<ulong>();

                for(int j = 0; j < i*10000; ++j)
                {
                    arr.Add((ulong)rng.Next());
//                    Console.Write("{0} ", arr[arr.Count - 1]);
                }

//                Console.WriteLine();
                tasks.Add(Task.Run(() => DoRequest(arr)));
            }

            Task.WaitAll(tasks.ToArray());
        }

        private async static Task DoRequest(List<ulong> arr)
        {
            var client = CreateWebRequest("http://127.0.0.1:31337/sort/", arr);

            using(var response = await client.GetResponseAsync())
            {
                var ms = new MemoryStream();
                await response.GetResponseStream().CopyToAsync(ms);

//                var arr1 = Converter.ToList(ms);
//
//                foreach(var x in arr1)
//                {
//                    Console.Write("{0} ", x);
//                }
//
//                Console.WriteLine();
                Console.WriteLine(sw.Elapsed);
            }
        }


        private static HttpWebRequest CreateWebRequest(string url, List<ulong> arr)
        {
            var webRequest = WebRequest.CreateHttp(url);
            webRequest.Method = WebRequestMethods.Http.Post;
            webRequest.ServicePoint.Expect100Continue = false;
            webRequest.ServicePoint.UseNagleAlgorithm = false;
            webRequest.ServicePoint.ConnectionLimit = 1024;
            webRequest.Proxy = null;
            webRequest.KeepAlive = true;
            webRequest.Timeout = 30000;

            using(var stream = webRequest.GetRequestStream())
            {
                var bytes = Converter.ToBytes(arr);
                stream.Write(bytes, 0, bytes.Length);
            }

            return webRequest;
        }
    }
}
