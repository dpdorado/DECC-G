using System;
using System.Collections.Generic;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.Poblacionales.Geneticos
{
    public class GBHS: Algorithm
    {
        public int HarmonyMemorySize = 20;
        public double Hmcr = 0.85;
        public double Par= 0.35;

        public override void Ejecutar(Knapsack theProblem, Random myRandom)
        {
            EFOs = 0;

            var harmonyMemory = new List<Solution>();
            for (var i = 0; i < HarmonyMemorySize; i++)
            {
                var s = new Solution(theProblem, this);
                s.RandomInitialization(myRandom);
                harmonyMemory.Add(s);
            }
            harmonyMemory.Sort((x, y) => -1 * x.Fitness.CompareTo(y.Fitness));

            while (EFOs < MaxEFOs)
            {
                var improvise = new Solution(theProblem, this);
                improvise.Improvisation(harmonyMemory, Hmcr, Par, myRandom);
                improvise.Repare(myRandom);
                improvise.Complete(myRandom, new List<int>());
                improvise.Evaluate();

                //replace the worst
                harmonyMemory.Add(improvise);
                harmonyMemory.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));
                harmonyMemory.RemoveRange(HarmonyMemorySize, 1);

                if (harmonyMemory[0].IsOptimalKnown()) break;
            }

            BestSolution = harmonyMemory[0];
        }

        public override string ToString()
        {
            return "GBHS " + "HMS = " + HarmonyMemorySize + 
                   " HMCR = " + Hmcr + " PAR = " + Par ;
        }
    }
}
