using System;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas
{
    public abstract class Algorithm
    {
        public int MaxEFOs;
        public int EFOs;
        public Solution BestSolution;

        public abstract void Ejecutar(Knapsack theProblem, Random myRandom);
    }
}
