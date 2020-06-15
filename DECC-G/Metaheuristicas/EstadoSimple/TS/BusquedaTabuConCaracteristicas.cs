using System;
using System.Collections;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.EstadoSimple.TS
{
    public class BusquedaTabuConCaracteristicas : Algorithm
    {
        public int MaxLongitudListaTabu;
        public int AtrNumeroTweaks;
        private readonly ArrayList _atrListaTabu = new ArrayList();
        public int AtrIteracionActual;

        public override void Ejecutar(Knapsack parProblema, Random myRandom)
        {
            RetoqueParametros(parProblema);
            EFOs = 0;
            // Solución inicial
            var s = new SolutionTS(parProblema, this);
            s.RandomInitialization(myRandom);
            BestSolution = new SolutionTS(s);
            // Agrego la Mejor solución a la lista Tabú            
            AddListaCarateristicas(s,AtrIteracionActual = 0);
            while (EFOs < MaxEFOs && !BestSolution.IsOptimalKnown())
            {
                AtrIteracionActual++;
                // Remover de la lista Tabú todas las tublas en la iteracion c - d > l
                DeleteListaCaracteristicas();
                var r = new SolutionTS(s);                               
                //R.Tweak(myRandom, pm, radio, this.atrListaTabu);
                r.Tweak(myRandom, _atrListaTabu);
                for (var i = 0; i < AtrNumeroTweaks - 1; i++)
                {
                    var w = new SolutionTS(s);
                    //W.Tweak(myRandom, pm, radio,atrListaTabu);
                    w.Tweak(myRandom, _atrListaTabu);
                    if (w.Fitness > r.Fitness)
                        r = w;
                    if (EFOs >= MaxEFOs || r.IsOptimalKnown()) break;               
                }
                s = r;
                AddListaCarateristicas(s, AtrIteracionActual);
                if (s.Fitness > BestSolution.Fitness)
                    BestSolution = new SolutionTS(s);            
            }

        }
        public void RetoqueParametros(Knapsack parProblema)
        {            
            MaxLongitudListaTabu = parProblema.TotalItems;            
        }

        private void AddListaCarateristicas(SolutionTS parSolucion, int parIteracion)
        {
            // 1. Obtener el vector solución
            int[] dimensiones = parSolucion.GetDimensiones();

            // 2. Obtener las características imersas o unos dentro de la solución sus dimensiones???
            for (var i = 0; i < dimensiones.Length; i++)
            {
                if (dimensiones[i] == 1 && !EstaCaracteristica(i))
                {
                    Caracteristica objCaracteristica = new Caracteristica(i, parIteracion);
                    //3. Guardar en la lista Tabú
                    _atrListaTabu.Add(objCaracteristica);
                }
            }
            
        }   

        private bool EstaCaracteristica(int i){
            var resultado = false;
            foreach(Caracteristica c in _atrListaTabu){
                if (c.GetCaracteristica() == i)
                {
                    resultado = true;
                    break;
                }
            }
            return resultado;
        }

        private void DeleteListaCaracteristicas()
        {
            for(var i = 0; i < _atrListaTabu.Count ; i++)
            {
                if (AtrIteracionActual - ((Caracteristica)_atrListaTabu[i]).AtrIteracion > MaxLongitudListaTabu)
                {
                    _atrListaTabu.RemoveAt(i);
                }
            }
        } 
    }

    public class Caracteristica
    {
        public int AtrCaracteristica; //valor de la dimension dentro del vector
        public int AtrIteracion;

        public Caracteristica(int parCarateristica, int parIteracion)
        {
            AtrCaracteristica = parCarateristica;
            AtrIteracion = parIteracion;
        }  

        public int GetCaracteristica()
        {
            return AtrCaracteristica;
        }      

    }
}