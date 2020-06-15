using System;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.HC
{
    public class HillClimbing: Algorithm
    {
        public override void Ejecutar(Knapsack theProblem, Random myRandom)
        {
            var timeBegin = DateTime.Now;
            EFOs = 0;

            var s = new Solution(theProblem, this);
            s.RandomInitialization(myRandom);

            while (EFOs < MaxEFOs)
            {
                var r = new Solution(s);
                r.Tweak(myRandom);

                if (r.Fitness > s.Fitness) // Maximization
                    s = r;

                if (s.IsOptimalKnown()) break;
                if ((DateTime.Now - timeBegin).TotalMilliseconds >= 700) break;
            }

            BestSolution = s;
        }

        public override string ToString()
        {
            return "Hill Climbing";
        }
    }
}
