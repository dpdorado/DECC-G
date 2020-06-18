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
            DE de = new DE(){MaxEFOs = FEs, k = 2};//variar
            s = theProblem.TotalItems / 3;

                
            var Population = inicializar_poblacion(theProblem,myRandom);
            Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));
            var best_s = new Solution(Population[0]);
          
                                                                                 
            for (var i = 1; i <= this.cycles; i++)
            {      
                de.MaxEFOs = FEs;        
                for (var j = 1; j <= theProblem.TotalItems/this.s; j++)
                {    
                    int[] _index = obtener_indices(theProblem.TotalItems,j);

                    if(_index[0] == _index[1]){break;}                
                                        
                    de.Ejecutar(myRandom, best_s, Population, _index[0],_index[1]);
                } 
                //Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));                

                //Se optiene la peor solución, la mejor y una solución aleatoria            
                best_s = new Solution(Population[0]);
                
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
                de.MaxEFOs=2;              
                de.Ejecutar(myRandom, best_s, new_population,0,theProblem.TotalItems-1);                   
                best_s = de.BestSolution;                        
                
                /*
                var Copy_P = Population;
                
                //Console.WriteLine("Iteration-cicle"+i);
                de.Ejecutar(myRandom, best_s, Copy_P, 0,theProblem.TotalItems-1);                                  
                Population[0] = new Solution(de.BestSolution);                                

                de.Ejecutar(myRandom, rand_s, Copy_P = Population, 0,theProblem.TotalItems-1);  
                Population[index_rand] = new Solution(de.BestSolution);                                     

                de.Ejecutar(myRandom, worst_s, Copy_P = Population, 0,theProblem.TotalItems-1);
                Population[PopulationSize-1] = new Solution(de.BestSolution);                                  
                
                Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));                                
                best_s = Population[0];*/
            }    
            Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));        
            BestSolution = best_s; 
        }

        public int[] obtener_indices(int TotalItems,int j)
        {
            int[] indexs = new int[2];
            indexs[0] = (j - 1) * s ;
            indexs[1] = (j * s) - 1;        
            
            //Mejorar indices      

            if((TotalItems-1) - indexs[1] < s)
            {
                indexs[1] = TotalItems-1;
            }

            if( s >= TotalItems)
            {
                indexs[0] = 0;
                indexs[1] = TotalItems -1;
            }
            return indexs;

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