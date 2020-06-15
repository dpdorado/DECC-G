using System;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.Poblacionales.PSO
{
    public class PSOSolution:Solution
    {
        protected int[] LocalBestObjects; // {0, 1}
        public double LocalBestFitness;
        protected double[] Velocity; // [-4, 4]

        public PSOSolution(Knapsack theProblem, Algorithm theAlgorithm) : base(theProblem, theAlgorithm)
        {
            LocalBestObjects = new int[MyProblem.TotalItems];
            LocalBestFitness = 0;
            Velocity = new double[MyProblem.TotalItems];
        }

        public PSOSolution(PSOSolution original) : base(original)
        {
            LocalBestObjects = new int[MyProblem.TotalItems];
            for (var i = 0; i < MyProblem.TotalItems; i++)
                LocalBestObjects[i] = original.LocalBestObjects[i];
            LocalBestFitness = original.LocalBestFitness;
            Velocity = new double[MyProblem.TotalItems];
            for (var i = 0; i < MyProblem.TotalItems; i++)
                Velocity[i] = original.Velocity[i];
        }

        public override void RandomInitialization(Random myRandom)
        {
            Weight = 0;
            var opciones = MyProblem.GetVariables();

            while (Weight <= MyProblem.Capacity)
            {
                var p = myRandom.Next(opciones.Count);
                SelectObject(opciones[p].Position);
                opciones.RemoveAt(p);

                var availableWeight = MyProblem.Capacity - Weight;
                opciones.RemoveAll(x => x.Weight > availableWeight);
                if (opciones.Count == 0) break;
            }

            Evaluate();

            for (var i = 0; i < MyProblem.TotalItems; i++)
                Velocity[i] = -4 + 8 * myRandom.NextDouble();
            
            for (var i = 0; i < MyProblem.TotalItems; i++)
                LocalBestObjects[i] = Objects[i];
            LocalBestFitness = Fitness;
        }

        public void UpdateVelocity(PSOSolution bestSolution, double w,
            double c1, double c2, Random myRandom)
        {
            for (var i = 0; i < MyProblem.TotalItems; i++)
            {
                Velocity[i] = w * Velocity[i] +
                              c1 * myRandom.NextDouble() * (LocalBestObjects[i] - Objects[i]) +
                              c2 * myRandom.NextDouble() * (bestSolution.Objects[i] - Objects[i]);
                if (Velocity[i] > 4) Velocity[i] = 4;
                if (Velocity[i] < -4) Velocity[i] = -4;
            }
        }

        public void UpdatePosition(Random myRandom)
        {
            Weight = 0;
            for (var i = 0; i < MyProblem.TotalItems; i++)
            {
                var sigma = 1 / (1 + Math.Exp(-Velocity[i]));
                if (myRandom.NextDouble() < sigma) SelectObject(i);
                else Objects[i] = 0;
            }
        }

        public void UpdateHistory()
        {
            if (LocalBestFitness < Fitness)
            {
                for (var i = 0; i < MyProblem.TotalItems; i++)
                    LocalBestObjects[i] = Objects[i];
                LocalBestFitness = Fitness;
            }
        }

        public void LocalOptimizer(int iterations, Random myRandom)
        {
            var cont = 0;
            do
            {
                var copy = new Solution(this);
                copy.Tweak(myRandom);
                if (copy.Fitness > this.Fitness)
                {
                    for (var i = 0; i < MyProblem.TotalItems; i++)
                        Objects[i] = copy.Objects[i];
                    Weight = copy.Weight;
                    Fitness = copy.Fitness;
                    UpdateHistory();
                }

                cont++;
            } while (cont < iterations);
        }
    }
}