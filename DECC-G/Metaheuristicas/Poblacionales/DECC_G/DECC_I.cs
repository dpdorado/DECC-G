using System;
using System.Collections;
using System.Collections.Generic;
using OptimizacionBinaria.Funciones;


namespace OptimizacionBinaria.Metaheuristicas.Poblacionales.DECC_G
{
    public class DECC_I: Algorithm
    {

        public int PopulationSize = 100;
        //public int MaxGenerations = 1000;

        public int cycles = 2;//cambiar valor por defecto
        //Número predefinido de evaluaciones de estado físico (FE)

        //Número de diviciones del vector
        public int s = 2;//cambiar valor por defecto
        //Número de ciclos
        
        public int FEs = 2;//cambiar valor por defecto
        //Peso del componente FEs                  

        public override void Ejecutar(Knapsack theProblem, Random myRandom)
        { 
            //var timeBegin = DateTime.Now;
            EFOs = 0;
            DE de = new DE(){MaxEFOs = FEs, k = 0.01};//variar
            s = theProblem.TotalItems / 3;

                
            var Population = inicializar_poblacion(theProblem,myRandom);
            Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));
            var best_s = new Solution(Population[0]);
          
                                                                                 
            for (var i = 1; i < this.cycles; i++)
            {              
                for (var j = 1; j < theProblem.TotalItems/this.s; j++)
                {                    
                    var l = ((j - 1) * s + 1) -1;//+1-1=0se puede eliminar
                    var u = (j * s) - 1;              
                                        
                    de.Ejecutar(myRandom, best_s, Population, l,u);                                                                             
                }    
                
                for (var j = 0; j <= PopulationSize; j++)
                {
                    Population[i].Evaluate();
                }                
                Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));

                //Se optiene la peor solución, la mejor y una solución aleatoria            
                best_s = Population[0];
                var index_rand =index(1,PopulationSize-1);
                Solution rand_s = Population[index_rand];
                Solution rand_s1 = Population[index(1,PopulationSize-1)];//--               
                Solution worst_s = Population[PopulationSize-1];
                               
                List<Solution> new_population = new List<Solution>
                {
                    rand_s,
                    rand_s1,
                    worst_s
                };                
                de.Ejecutar(myRandom, best_s, new_population,0,new_population.Count);                
                Population[PopulationSize-1] = de.BestSolution; 

                /*de.Ejecutar(myRandom, best_s, Population, 0,theProblem.TotalItems-1);  
                Population[0] = new Solution(de.BestSolution);
                de.Ejecutar(myRandom, rand_s, Population, 0,theProblem.TotalItems-1);  
                Population[index_rand] = new Solution(de.BestSolution);
                de.Ejecutar(myRandom, worst_s, Population, 0,theProblem.TotalItems-1);  
                Population[PopulationSize] = new Solution(de.BestSolution); 

                for (var j = 0; j <= PopulationSize; j++)
                {
                    Population[i].Evaluate();
                }

                Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));                                
                best_s = Population[0];*/
            }
            //Aqui el problema-----------------------------------------por ->
            //Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));//Error al ordenar
            BestSolution = Population[0];                        
        }

        //Reparar soluciones
        public void reparar_soluciones(Random myRandom, List<Solution> Population, int s, int group)
        {
            foreach(var solution in Population)
            {
                solution.Repare(myRandom,s,group);
            }            
        }

        //Genera un indice netre el limite inferior(li) y el limite superior(ls)
        public int index(int li,int ls)
        {
            var rand = new Random();            
            return rand.Next(li,ls);
        }                            
     

        //Inicializar la población
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
    }   
}