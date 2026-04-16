using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace TPP.Concurrency.Delegates {

    public class WebPage {

        /// <summary>
        /// Web page URL
        /// </summary>
        private string url;
        public string Url
        {
            get { return this.url; }
        }

        public WebPage(string url) {
            this.url = url;
        }

        /// <summary>
        /// Access to the Web page and returns the number of img tags in the html code
        /// </summary>
        /// <returns>The number of img tags in the html code</returns>
        public int GetNumberOfImages() {
            WebClient client = new WebClient();
            string html = client.DownloadString(this.url);
            return Ocurrencias(html.ToLower(), "<img");
        }

        /// <summary>
        /// Returns the number of occurrences of a word in a string
        /// </summary>
        /// <param name="str">The string where a word is going to be searched in</param>
        /// <param name="word">The word to be searched</param>
        /// <returns>The number of occurrences of word in str</returns>
        private int Ocurrencias(string str, string word) {
            return (str.Length - str.Replace(word, "").Length) / word.Length;
        }

        /// <summary>
        ///     Access to the Web page and returns the number of img tags in the html code asynchronously thanks to
        ///     the new async and await C# 5 features.
        /// </summary>
        /// <returns>The number of img tags in the html code</returns>

        public async Task<int> GetNumberOfImagesAsyncTask()
        {
            var client = new WebClient();
            //Asyncronous call to an API synchronous method. Wait for this asynchronous task to finish. Yields control to the caller.
            var html = await Task.Run(() => client.DownloadString(url));
            return Ocurrencias(html.ToLower(), "<img");
        }

    }

}
