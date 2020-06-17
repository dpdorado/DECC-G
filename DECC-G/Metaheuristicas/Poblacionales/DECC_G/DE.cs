using System;
using System.Collections;
using System.Collections.Generic;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.Poblacionales.DECC_G
{
    public class DE : Algorithm
    {
        public int PopulationSize = 100;
        public Double k = 0.01; //Paso 0 y 2

        public override void Ejecutar(Knapsack theProblem, Random myRandom)
        {                        
            
            var Population = new List<Solution>();
            var Q = new List<Solution>();
            
                
            Population =  inicializar_poblacion(theProblem,myRandom);
            
            var Best = Population[0];//Entra como parametro
                        
            while (EFOs <= MaxEFOs && Population[0].IsOptimalKnown()){
                
                for(var i = 0; i <= PopulationSize; i++)
                {
                    if(Q.Count != 0)
                    {
                        if (Q[i].Fitness > Population[i].Fitness)
                        {
                            Population[i] = new Solution(Q[i]);
                        }
                        if (Best == null || Q[i].Fitness > Best.Fitness)
                        {
                            Best = new Solution(Population[i]);
                        }
                    }
                   
                }

                Q = Population;
                
                for (var j = 0; j <= PopulationSize; j++)
                {
                    List<Solution> solucionesAleatorias = obtenerAleatorios(Population, j, myRandom);
                    var a = solucionesAleatorias[0];
                    var b = solucionesAleatorias[1];
                    var c = solucionesAleatorias[2];                    
                    a.DEmutation(myRandom,b,c,k);                                        
                    
                    Solution padre = null;
                    Solution madre = null; 

                    if (myRandom.Next(0,1) ==0)
                    {   
                        padre = new Solution(a);
                        madre = new Solution(Q[j]);
                    }else
                    {
                        padre = new Solution(Q[j]);
                        madre = new Solution(a);
                    }

                    a.CruceIntercalado(padre, madre);

                    Population[j] = a;
                    Population[j].Repare(myRandom);
                    Population[j].Evaluate();

                    if (EFOs >= MaxEFOs) break;
                    if (Population[j].IsOptimalKnown()) break;
                }                

                Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));
            }
            
                
            BestSolution = Population[0];
        } 

        public List<Solution> inicializar_poblacion(Knapsack theProblem,Random myRandom)
        {
            var Population = new List<Solution>();
            for (var i = 0; i < PopulationSize; i++)
            {
                var s = new Solution(theProblem, this);
                s.RandomInitialization(myRandom);
                Population.Add(s);
            }
            return Population;
        }
        
        private List<Solution> obtenerAleatorios(List<Solution> Population, int index_Qi, Random aleatorio)
        {
            List<Solution> varResultado = new List<Solution>();
            do
            {                
                var a = aleatorio.Next(0, PopulationSize);
                var b = aleatorio.Next(0, PopulationSize);
                var c = aleatorio.Next(0, PopulationSize);

                if (a == b || b == c || c == a)
                {
                    continue;
                }
                if (a == index_Qi || b == index_Qi || c ==index_Qi)
                {
                    continue;
                }                                
                varResultado.Add(new Solution(Population[a]));
                varResultado.Add(new Solution(Population[b]));
                varResultado.Add(new Solution(Population[c]));                
            } while (varResultado != null);

            return varResultado;
        }
            
    }
}