using System;
using System.Collections.Generic;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.Poblacionales.PSO
{
    public class PSOBinario : Algorithm
    {
        public int PopulationSize = 100;
        public int MaxGenerations = 1000;
        public double W = 1;
        public double C1 = 2;
        public double C2 = 2;
        public int LocalIterations = 10;

        public override void Ejecutar(Knapsack theProblem, Random myRandom)
        {
            EFOs = 0;

            var population = new List<PSOSolution>();
            for (var i = 0; i < PopulationSize; i++)
            {
                var s = new PSOSolution(theProblem, this);
                s.RandomInitialization(myRandom);
                population.Add(s);
            }
            population.Sort((x, y) => -1 * x.Fitness.CompareTo(y.Fitness));
            BestSolution = new PSOSolution(population[0]);

            if (population[0].IsOptimalKnown()) return;

            for (var g = 1; g < MaxGenerations; g++)
            {
                for (var i = 0; i < PopulationSize; i++)
                {
                    population[i].UpdateVelocity((PSOSolution) BestSolution, W, C1, C2, myRandom);
                    population[i].UpdatePosition(myRandom);
                    population[i].Repare(myRandom);
                    population[i].Complete(myRandom, new List<int>());
                    population[i].Evaluate();
                    population[i].UpdateHistory();
                }

                population.Sort((x, y) => -1 * x.Fitness.CompareTo(y.Fitness));

                population[0].LocalOptimizer(LocalIterations, myRandom);


                if (BestSolution.Fitness < population[0].Fitness)
                    BestSolution = new PSOSolution(population[0]);

                if (EFOs >= MaxEFOs) break;
                if (population[0].IsOptimalKnown())
                    break;
            }
        }


        public int TorneoBinario(List<Solution> population, Random aleatorio)
        {
            var p1 = aleatorio.Next(PopulationSize);
            var p2 = aleatorio.Next(PopulationSize);
            while (p1 == p2) p2 = aleatorio.Next(PopulationSize);

            var padre = p1;
            if (population[p2].Fitness > population[p1].Fitness) padre = p2;
            return padre;
        }


        public override string ToString()
        {
            return "Genetico 1";
        }
    }
}
