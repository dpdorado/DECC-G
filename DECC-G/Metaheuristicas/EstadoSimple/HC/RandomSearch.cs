using System;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.HC
{
    public class RandomSearch:Algorithm
    {
        public override void Ejecutar(Knapsack theProblem, Random myRandom)
        {
            EFOs = 0;
            BestSolution = new Solution(theProblem, this);
            BestSolution.RandomInitialization(myRandom);

            while (EFOs < MaxEFOs)
            {
                var r = new Solution(theProblem, this);
                r.RandomInitialization(myRandom);

                if (r.Fitness > BestSolution.Fitness) //maximization
                {
                    BestSolution = r;
                }
                if (BestSolution.IsOptimalKnown()) break;
            }
        }

        public override string ToString()
        {
            return "Random Search";
        }
    }
}
