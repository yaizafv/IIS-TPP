using System;
using System.Threading.Tasks;

namespace TPP.Concurrency.TaskExplicit
{
    class Program
    {
        static void Main(string[] args)
        {
            String text = TextProcessing.ReadTextFile(@"..\..\..\..\clarin.txt");
            string[] words = TextProcessing.DivideIntoWords(text);

            // Processing
            DateTime before = DateTime.Now;
            var punctuationMarks = Task.Run(() => TextProcessing.NumberOfPunctuationMarks(text));
            var longestWords = Task.Run(() => TextProcessing.LongestWords(words));
            var shortestWords = Task.Run(() => TextProcessing.ShortestWords(words));

            Task<Tuple<string[], int>> wordsAppearMoreTimes = Task.Run(() =>
                {
                    int local_greatestOccurrence;
                    var moreTimes = TextProcessing.WordsAppearMoreTimes(words, out local_greatestOccurrence);
                    return new Tuple<string[], int>(moreTimes, local_greatestOccurrence);
                });
            Task<Tuple<string[], int>> wordsAppearFewerTimes = Task.Run(() =>
            {
                int local_lowestOccurrence;
                var fewerTimes = TextProcessing.WordsAppearFewerTimes(words, out local_lowestOccurrence);
                return new Tuple<string[], int>(fewerTimes, local_lowestOccurrence);
            });
            DateTime after = DateTime.Now;

            //Show Results method would be optimized following a-more-asynchronous approach (receving Task parameters): some results
            //will be partially shown while others still processing
            TextProcessing.ShowResults(punctuationMarks.Result,
                                       shortestWords.Result,
                                       longestWords.Result,
                                       wordsAppearFewerTimes.Result.Item1,
                                       wordsAppearFewerTimes.Result.Item2,
                                       wordsAppearMoreTimes.Result.Item1,
                                       wordsAppearMoreTimes.Result.Item2);

            Console.WriteLine("\nElapsed time: {0:N} milliseconds.", (after - before).Ticks / TimeSpan.TicksPerMillisecond);
        }
    }
}
