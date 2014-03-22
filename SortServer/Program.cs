using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SortServer
{
    class Program
    {
        static async void SolveAsync(HttpListenerContext context)
        {
            Console.WriteLine("New great task!");

            try
            {
                var arr = Converter.ToList(context.Request.InputStream);

//                arr.ForEach(x => Console.Write("{0} ", x));
//                Console.WriteLine();

                arr.Sort();
//                arr.ForEach(x => Console.Write("{0} ", x));
//                Console.WriteLine();

                var bytes = Converter.ToBytes(arr); 
                await context.Response.OutputStream.WriteAsync(bytes, 0, bytes.Length);
                context.Response.Close();

                Console.WriteLine("Sended");
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }

        static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.Error.WriteLine("Usage: name port");

                return;
            }

            int port;
            if(!int.TryParse(args[0], out port))
            {
                Console.Error.WriteLine("Invalid port");
                return;
            }

            var listener = new HttpListener();
            listener.Prefixes.Add(string.Format("http://+:{0}/sort/", port));
            listener.Start();

            while(true)
            {
                SolveAsync(listener.GetContext());
            }
        }
    }
}
