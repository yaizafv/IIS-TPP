using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLockSlim
{
    class TranslationTableUsage
    {
        static String[] english = { "broccoli", "cauliflower",
                                                          "carrot", "apple", "orange",
                                                          "pear", "brussel sprout",
                                                          "cabbage", "banana",
                                                          "spinach", "grape",
                                                          "mint", "corn",
                                                          "radish", "cucumber",
                                                          "raddichio", "pea" };
        static String[] spanish = { "brócoli", "coliflor",
                                                          "zanahoria", "manzana", "naranja",
                                                          "pera", "col de bruselas",
                                                          "repollo", "plátano",
                                                          "espinaca", "uva",
                                                          "menta", "maíz",
                                                          "rábano", "pepinillo",
                                                          "achicoria", "guisante" };
        public static void Main()
        {
            var foodTranslationEnglishSpanish = new TranslationTable();
            var tasks = new List<Task>();
            int translationsWritten = 0;

            // Execute a writer.
            tasks.Add(Task.Run(() =>
            {

                for (int i = 0; i < english.Length; i++)
                    foodTranslationEnglishSpanish.AddTranslation(english[i], spanish[i]);

                translationsWritten = foodTranslationEnglishSpanish.StringCount;
                Console.WriteLine("Task {0} wrote {1} translations\n",
                                  Task.CurrentId, translationsWritten);
            }));

            // Execute two readers, one to read from first to last and the second from last to first.
            for (int i = 0; i <= 1; i++)
            {
                bool desc = Convert.ToBoolean(i);
                tasks.Add(Task.Run(() =>
                {
                    int start, last, step;
                    int items;
                    do
                    {
                        String output = String.Empty;
                        items = foodTranslationEnglishSpanish.StringCount;
                        if (!desc)
                        {
                            start = 0;
                            step = 1;
                            last = items;
                        }
                        else
                        {
                            start = items - 1;
                            step = -1;
                            last = 1;
                        }

                        for (int index = start; desc ? index > last : index < last; index += step)
                            output += String.Format("[{0}] ", foodTranslationEnglishSpanish.Translate(english[index]));

                        Console.WriteLine("Task {0} read {1} items: {2}\n",
                                          Task.CurrentId, items, output);
                    } while (items < translationsWritten | translationsWritten == 0);
                }));
            }

            // Execute a red/update task.
            tasks.Add(Task.Run(() =>
            {
                Thread.Sleep(100);
                for (int i = 0; i < foodTranslationEnglishSpanish.StringCount; i++)
                {
                    String value = foodTranslationEnglishSpanish.Translate(english[i]);
                    if (value.Equals("pepinillo"))
                    {
                        foodTranslationEnglishSpanish.AddOrUpdateTranslation("cucumber", "pepino");
                        Console.WriteLine("Changed 'cucumber' to " + foodTranslationEnglishSpanish.Translate("cucumber"));
                    }
                }
            }));

            // Wait for all three tasks to complete.
            Task.WaitAll(tasks.ToArray());

            // Display the final contents of the cache.
            Console.WriteLine();
            Console.WriteLine("Values in translation table: ");
            for (int i = 0; i < foodTranslationEnglishSpanish.StringCount; i++)
                Console.WriteLine("   {0}: {1}", i, foodTranslationEnglishSpanish.Translate(english[i]));

        }
    }
}
