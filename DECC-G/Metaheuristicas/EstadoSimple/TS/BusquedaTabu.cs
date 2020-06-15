using System;
using OptimizacionBinaria.Funciones;
using System.Collections;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.TS
{
    public class BusquedaTabu : Algorithm
    {
        public int MaxLongituLitaTabu;
        public int AtrNumeroTweaks;
        private readonly Queue _atrListaTabu = new Queue();

        public override void Ejecutar(Knapsack parProblema, Random myRandom)
        {
            //Revisar bien
            RetoqueParametros(parProblema);
            EFOs = 0;
            // Solución inicial
            var s = new SolutionTS(parProblema, this);
            s.RandomInitialization(myRandom); 
            // Mejor solución           
            BestSolution = new SolutionTS(s);
            // Se agrega la MejorSolucion a la lista Tabú
            _atrListaTabu.Enqueue(BestSolution);

            while (EFOs < MaxEFOs && !BestSolution.IsOptimalKnown())
            {
                if (_atrListaTabu.Count >= MaxLongituLitaTabu)
                    _atrListaTabu.Dequeue();

                var r = new SolutionTS(s);
                r.Tweak(myRandom);
                for (var i = 0; i < AtrNumeroTweaks - 1; i++)
                {
                    var w = new SolutionTS(s);
                    w.Tweak(myRandom);
                    if (!PerteneceListaTabu(w) && (w.Fitness > r.Fitness || PerteneceListaTabu(r)))
                        r = w;  
                    if (EFOs >= MaxEFOs || r.IsOptimalKnown()) break;
                }
                if (!PerteneceListaTabu(r) && r.Fitness > s.Fitness){
                    s = r;
                    _atrListaTabu.Enqueue(r);
                }
                if (s.Fitness > BestSolution.Fitness)
                    BestSolution = new SolutionTS(s);
            }
        }

        public void RetoqueParametros(Knapsack parProblema)
        {            
            MaxLongituLitaTabu = parProblema.TotalItems;            
        }

        private bool PerteneceListaTabu(Solution parSolucion)
        {
            var varRespuesta = false;
            foreach (Solution varSolucion in _atrListaTabu)
                if (varSolucion.Equals(parSolucion))
                    varRespuesta = true;
            return varRespuesta;
        }
    }
}
