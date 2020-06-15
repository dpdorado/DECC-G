using System;
using OptimizacionBinaria.Funciones;
using OptimizacionBinaria.Metaheuristicas;

namespace OptimizacionBinaria.Exactos
{
    class Exhaustive: Algorithm
    {
        public override void Ejecutar(Knapsack theProblem, Random myRandom)
        {
            EFOs = 0;
            var s = new Solution(theProblem, this);
            s.Evaluate();
            BestSolution = new Solution(s);
            for (var i = 1; i < Math.Pow(2, theProblem.TotalItems); i++)
            {
               s.Modify(i);
               if (s.Fitness > BestSolution.Fitness)
                   BestSolution = new Solution(s);
            }
        }

        public override string ToString()
        {
            return "Exhaustivo";
        }
    }
}
