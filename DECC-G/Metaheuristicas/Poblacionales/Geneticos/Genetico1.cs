using System;
using System.Collections.Generic;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.Poblacionales.Geneticos
{
    public class Genetico1: Algorithm
    {
        public int PopulationSize = 100;
        public int MaxGenerations = 1000;

        public override void Ejecutar(Knapsack theProblem, Random myRandom)
        {
            //var timeBegin = DateTime.Now;
            EFOs = 0;

            // Inicialización de la población P(t=0)
            var Population = new List<Solution>();
            for (var i = 0; i < PopulationSize; i++)
            {
                var s = new Solution(theProblem, this);
                s.RandomInitialization(myRandom);
                Population.Add(s);
            }
            

            for (var g = 1; g < MaxGenerations; g++)
            {               
                var offsprints = new List<Solution>();
                for (var p = 0; p < PopulationSize / 2; p++)
                {
                    //seleccion binario
                    var padre = TorneoBinario(Population, myRandom);
                    var madre = TorneoBinario(Population, myRandom);
                    while (padre == madre) madre = TorneoBinario(Population, myRandom);

                    //cruce
                    var h1 = new Solution(theProblem, this);
                    var h2 = new Solution(theProblem, this);
                    h1.CruceIntercalado(Population[padre], Population[madre]);
                    h2.CruceIntercalado(Population[madre], Population[padre]);

                    //mutation one-bit
                    h1.OneBitMutation(myRandom);
                    h2.OneBitMutation(myRandom);

                    //La mochila es factible? toca reparala?
                    h1.Repare(myRandom);
                    h1.Complete(myRandom, new List<int>());
                    h1.Evaluate();                    

                    h2.Repare(myRandom); 
                    h2.Complete(myRandom, new List<int>());
                    h2.Evaluate();

                    offsprints.Add(h1);
                    offsprints.Add(h2);                                  
                }

                //remplazo
                Population.AddRange(offsprints);
                Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));
                Population.RemoveRange(PopulationSize, PopulationSize);

                if (EFOs >= MaxEFOs) break;
                if (Population[0].IsOptimalKnown()) break;
            }

            BestSolution = Population[0];
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
