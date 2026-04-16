using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace TPP.Concurrency.PLINQ {


    class PLinq {

        static short[] CreateRandomVector(int numberOfElements, short lowest, short greatest) {
            short[] vector = new short[numberOfElements];
            Random random = new Random();
            for (int i = 0; i < numberOfElements; i++)
                vector[i] = (short)random.Next(lowest, greatest + 1);
            return vector;
        }

        public static void Warmup(Action a)
        {
            //invokes same method 3 times. This will trigger any exisiting hot-spot optimization
            a();
            a();
            a();
        }


        static double VectorModulusLinq(IEnumerable<short> vector) {
            return Math.Sqrt(vector.Select(vi => (long)vi * vi)
                    .Aggregate((vi, vj) => vi + vj)
                );
        }

        static double VectorModulusPLinq_v1(IEnumerable<short> vector) {
            return Math.Sqrt(
                vector.AsParallel() // with just one aggregate
                    .Aggregate<short,long>(0, (acc, item) => acc + item * item)

                );
        }

        static double VectorModulusPLinq_v2(IEnumerable<short> vector)
        {
            return Math.Sqrt(
                vector.AsParallel()
                .Select(vi => (long)vi * vi)
                .Aggregate((acc, vi) => acc + vi)
                );
        }

        static double VectorModulusPLinqLocal_v1(IEnumerable<short> vector)
        {
            return Math.Sqrt(
                vector.AsParallel()
                    .Select(vi => (long)vi * vi)
                    .Aggregate<long, long, long>(() => 0,  //local init, also called seedFactory
                               (acc, vi) =>  acc + vi, //body: same as before
                               (acu1, acu2) => acu1 + acu2, //combine local acumulators
                               finalResult => finalResult) //final state


                );
        }
        static double VectorModulusPLinqLocal_v2(IEnumerable<short> vector)
        {
            return Math.Sqrt(
                vector.AsParallel()
                    // with just one aggregate with local acumulators
                    .Aggregate<short, long, long>(() => 0,  //local init, also called seedFactory
                               (acc, item) => acc + item * item, //body: same as before
                               (acu1, acu2) => acu1 + acu2, //combine local acumulators
                               finalResult => finalResult) //final state


                );
        }

        static void Main() {
            short[] vector = CreateRandomVector(100000000, -10, 10);

            //var r = vector.Select(elemento => Math.Sqrt(elemento));
            //var rr = vector.AsParallel().Select(elemento => Math.Sqrt(elemento));

            //method warm-up?
            Warmup(() => VectorModulusLinq(CreateRandomVector(100000000, -10, 10)));
            Stopwatch chrono = new Stopwatch();
            chrono.Start();
            double result = VectorModulusLinq(vector);
            chrono.Stop();
            long millisSequential = chrono.ElapsedMilliseconds;
            Console.WriteLine("Elapsed time in sequential Linq {0}, result {1}.", millisSequential, result);

            //method warm-up?
            Warmup(() => VectorModulusPLinq_v1(CreateRandomVector(100000000, -10, 10)));
            chrono.Restart();
            result = VectorModulusPLinq_v1(vector);
            chrono.Stop();
            long millisParallel_v1 = chrono.ElapsedMilliseconds;
            Console.WriteLine("Elapsed time in parallel Linq (v1) {0}, result {1}.", millisParallel_v1, result);

            //method warm-up?
            Warmup(() => VectorModulusPLinq_v2(CreateRandomVector(100000000, -10, 10)));
            chrono.Restart();
            result = VectorModulusPLinq_v2(vector);
            chrono.Stop();
            long millisParallel_v2 = chrono.ElapsedMilliseconds;
            Console.WriteLine("Elapsed time in parallel Linq (v2) {0}, result {1}.", millisParallel_v2, result);

            //method warm-up?
            Warmup(() => VectorModulusPLinqLocal_v1(CreateRandomVector(100000000, -10, 10)));
            chrono.Restart();
            result = VectorModulusPLinqLocal_v1(vector);
            chrono.Stop();
            long millisParallelLocal_v1 = chrono.ElapsedMilliseconds;
            Console.WriteLine("Elapsed time in parallel Linq with local variables (v1) in aggregate {0}, result {1}.", millisParallelLocal_v1, result);


            //method warm-up?
            Warmup(() => VectorModulusPLinqLocal_v2(CreateRandomVector(100000000, -10, 10)));
            chrono.Restart();
            result = VectorModulusPLinqLocal_v2(vector);
            chrono.Stop();
            long millisParallelLocal_v2 = chrono.ElapsedMilliseconds;
            Console.WriteLine("Elapsed time in parallel Linq with local variables (v2) in aggregate {0}, result {1}.", millisParallelLocal_v2, result);

            Console.WriteLine("Benefit in computing the modulus (Linq vs Plinq_v1) {0:N}%.", (millisSequential / (double)millisParallel_v1 - 1) * 100);
            Console.WriteLine("Benefit in computing the modulus (Linq vs Plinq_v2) {0:N}%.", (millisSequential / (double)millisParallel_v2 - 1) * 100);
            Console.WriteLine("Benefit in computing the modulus (Linq vs Plinq_local_v1) {0:N}%.", (millisSequential / (double)millisParallelLocal_v1 - 1) * 100);
            Console.WriteLine("Benefit in computing the modulus (Linq vs Plinq_local_v2) {0:N}%.", (millisSequential / (double)millisParallelLocal_v2 - 1) * 100);
            Console.WriteLine("Benefit in computing the modulus (Plinq_v1 vs Plinq_local_v2) {0:N}%.", (millisParallel_v1 / (double)millisParallelLocal_v2 - 1) * 100);


            Console.WriteLine("\n");

            vector = CreateRandomVector(50000000, -10, 10);
            chrono.Restart();
            result = HighComputationLinq(vector);
            chrono.Stop();
            millisSequential = chrono.ElapsedMilliseconds;
            Console.WriteLine("Elapsed time of high computation with sequential Linq {0}, result {1}.",  millisSequential, result);

            chrono.Restart();
            result = HighComputationPLinq(vector);
            chrono.Stop();
            millisParallel_v1 = chrono.ElapsedMilliseconds;
            Console.WriteLine("Elapsed time of high computation with parallel Linq {0}, result {1}.", millisParallel_v1, result);
            Console.WriteLine("Benefit in high computation {0:N}%.", ((double)millisSequential / millisParallel_v1 - 1) * 100);

        }

        static double HighComputationLinq(IEnumerable<short> vector) {
            return Math.Sqrt(
                vector
                .Select(vi => vi == 0 ? 1.0 : Math.PI / Math.Pow(Math.E, Math.Abs(vi)))
                    .Aggregate((vi, vj) => Math.Sqrt(Math.Abs(vi))/Math.Sqrt(Math.Abs(vi)))
                );
        }

        static double HighComputationPLinq(IEnumerable<short> vector) {
            return Math.Sqrt(
                vector.AsParallel()
                .Select(vi => vi == 0 ? 1.0 : Math.PI / Math.Pow(Math.E, Math.Abs(vi)))
                    .Aggregate((vi, vj) => Math.Sqrt(Math.Abs(vi)) / Math.Sqrt(Math.Abs(vi)))
                );
        }


    }

}