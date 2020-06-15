using System;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.HC
{
    public class AscensoColinaConReinicios : Algorithm
    {
        public int MaxLocalIter = 15;

        public override void Ejecutar(Knapsack theProblem, Random myRandom)
        {
            EFOs = 0;

            var s = new Solution(theProblem, this);
            s.RandomInitialization(myRandom);

            BestSolution = new Solution(s);

            while (EFOs < MaxEFOs)
            {
                var time = myRandom.Next(MaxLocalIter);

                for (var local = 0; local < time; local++) {
                    var r = new Solution(s);
                    r.Tweak(myRandom);

                    if (r.Fitness > s.Fitness)
                        s = r;
                    if (EFOs >= MaxEFOs) break;
                }

                if (s.Fitness > BestSolution.Fitness)
                    BestSolution = new Solution(s);

                if (s.IsOptimalKnown()) break;

                s.RandomInitialization(myRandom);
            }

            BestSolution = s;
        }

        public override string ToString()
        {
            return "Ascenso a la Colina con Reinicios";
        }
    }
}
