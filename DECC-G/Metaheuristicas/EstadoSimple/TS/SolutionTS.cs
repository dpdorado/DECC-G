using System;
using System.Collections;
using System.Collections.Generic;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.TS
{
    public class SolutionTS:Solution
    {
        public SolutionTS(Knapsack theProblem, Algorithm theAlgorithm) : base(theProblem, theAlgorithm)
        {
        }

        public SolutionTS(Solution original) : base(original)
        {
        }

        public int[] GetDimensiones()
        {
            return Objects;
        }

        public void Tweak(Random aleatorio, ArrayList parListaTabu)
        {
            // Paso 1 = Intercambiar un objeto seleccionado por uno no seleccionado
            var seleccionados = new List<KeyValuePair<int, double>>();
            for (var i = 0; i < MyProblem.TotalItems; i++)
            {
                if (Objects[i] == 1)
                    seleccionados.Add(new KeyValuePair<int, double>(i, MyProblem.Weight(i)));
            }

            int p;
            var noSeleccionados = new List<KeyValuePair<int, double>>();
            var pruebas = 0;
            do
            {
                p = aleatorio.Next(seleccionados.Count);
                var pesoDisponible = MyProblem.Capacity - (Weight - seleccionados[p].Value);

                for (var i = 0; i < MyProblem.TotalItems; i++)
                {
                    if (Objects[i] == 0 && MyProblem.Weight(i) <= pesoDisponible && !estaEnlalistaTabu(i, parListaTabu))
                        noSeleccionados.Add(new KeyValuePair<int, double>(i, MyProblem.Weight(i)));
                }

                pruebas++;
                if (pruebas >= 3) return; // No trato de hacer TweFak
            } while (noSeleccionados.Count == 0);

            Objects[seleccionados[p].Key] = 0;
            Weight -= seleccionados[p].Value;

            var q = aleatorio.Next(noSeleccionados.Count);
            Objects[noSeleccionados[q].Key] = 1;
            Weight += noSeleccionados[q].Value;

            Complete(aleatorio, new List<int>());

            Evaluate();
        }

        private bool estaEnlalistaTabu(int parDimension, ArrayList listaTabu)
        {
            var varResultado = false;
            foreach (Caracteristica obj in listaTabu)
                if (obj.AtrCaracteristica == parDimension)
                    varResultado = true;
            return varResultado;
        }
    }
}
