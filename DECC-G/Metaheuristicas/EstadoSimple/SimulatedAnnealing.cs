using System;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple
{
    public class SimulatedAnnealing : Algorithm
    {
        public override void Ejecutar(Knapsack theProblem, Random myRandom)
        {
            var timeBegin = DateTime.Now;
            EFOs = 0;
            var temperature = 100f;
            var decrease = 0.999f;

            var s = new Solution(theProblem, this);
            s.RandomInitialization(myRandom);
            BestSolution = new Solution(s);

            while (EFOs < MaxEFOs)
            {
                var r = new Solution(s);
                r.Tweak(myRandom);

                var pBad = Math.Exp((r.Fitness - s.Fitness) / temperature);
                if (r.Fitness > s.Fitness) // Maximization
                {
                    s = r;
                    BestSolution = new Solution(s);
                }
                else if (myRandom.NextDouble() < pBad)
                    s = r;

                temperature *= decrease;
                if (Math.Abs(BestSolution.Fitness - theProblem.OptimalKnown) < 1e-10) break;
                //if ((DateTime.Now - timeBegin).TotalMilliseconds >= 300) break;
            }
        }

        public override string ToString()
        {
            return "Simulated Annealing";
        }
    }
}
