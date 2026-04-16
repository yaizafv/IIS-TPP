using System;
using System.Threading.Tasks;

namespace TPP.Concurrency.Delegates
{
    // BeginInvoke is not supported in .Net Core
    // https://devblogs.microsoft.com/dotnet/migrating-delegate-begininvoke-calls-for-net-core/

    /// <summary>
    /// Example use of delegates in to pass messages asynchronously 
    /// </summary>
    class Program {

        public static void Main(string[] args)
        {
            //GetImagesAsynchronously
            WebPage uniovi = new WebPage("http://www.uniovi.es");
            WebPage school = new WebPage("http://www.ingenieriainformatica.uniovi.es");

            DateTime before = DateTime.Now;
            // * Asynchronous execution (a secondary thread is created)
            var asynchronousResult = uniovi.GetNumberOfImagesAsyncTask();

            // * Synchronous execution in the main thread 
            var numberOfImgsInSchool = school.GetNumberOfImages();

            //int numberOfImgsInUniovi = await asynchronousResult; //if await is used here, it may lead to other kinds of problems
            var numberOfImgsInUniovi = asynchronousResult.Result;
            DateTime after = DateTime.Now;

            Console.WriteLine("The University Web has {0:N0} images, and {1:N0} the School Web.",
                numberOfImgsInUniovi, numberOfImgsInSchool);
            Console.WriteLine("Elapsed millisenconds to compute both results: {0:N0}.",
                (after - before).Ticks / TimeSpan.TicksPerMillisecond);
        }
    }
}
