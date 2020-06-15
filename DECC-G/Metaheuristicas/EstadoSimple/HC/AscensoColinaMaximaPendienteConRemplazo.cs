using System;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.HC
{
    public class AscensoColinaMaximaPendienteConRemplazo : Algorithm
    {
        public int vecinos = 5;

        public override void Ejecutar(Knapsack theProblem, Random myRandom)
        {
            EFOs = 0;
            // Inicializacion aleatoria
            var s = new Solution(theProblem, this);
            s.RandomInitialization(myRandom);
            BestSolution = new Solution(s);

            // ciclo de evolución
            while (EFOs < MaxEFOs)
            {
                // r es Tweak de la copia de s
                var r = new Solution(s);
                r.Tweak(myRandom);

                for (var v = 0; v < vecinos - 1; v++)
                {
                    var w = new Solution(s);
                    w.Tweak(myRandom);
                    
                    if (w.Fitness > r.Fitness)  // se esta maximizando
                        r = w;

                    if (EFOs >= MaxEFOs) break;
                }

                s = r;

                if (s.Fitness > BestSolution.Fitness) // se esta maximizando
                {
                    BestSolution = new Solution(s);
                }
            }

            BestSolution = s;
        }

        public override string ToString()
        {
            return "Ascenso a la Colina por la Maxima Pendiente con Remplazo";
        }
    }
}
