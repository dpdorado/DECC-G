using System;
using System.Collections.Generic;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas.Poblacionales.DECC_G
{
    public class DE : Algorithm
    {
        public int PopulationSize = 100;
        public Double k = 2; //Paso 0 y 2

        public override void Ejecutar(Knapsack theProblem, Random myRandom)
        {      
            EFOs = 0;                  
            
            var Population = new List<Solution>();
            var Q =new List<Solution>();
            

            Population =  inicializar_poblacion(theProblem,myRandom);
            Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));
            var Best = Population[0];
                        
            do{
                
                if(Q.Count != 0)
                {
                    for(var i = 0; i < PopulationSize; i++)
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
                //Console.WriteLine("Best.Fitness:"+Best.Fitness);

                Q = Population;
                
                for (var j = 0; j < PopulationSize; j++)
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
                
            } while (EFOs <= MaxEFOs && Population[0].IsOptimalKnown()); 
            Population[Population.Count-1]=Best;                   
            Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));
            BestSolution = Population[0];
        } 

        public void Ejecutar(Random myRandom, Solution Best_Solution, List<Solution> Population, int index_i,int index_s)
        {            
            EFOs = 0;                  
           
            //var Population = subpob;
            var Q = new List<Solution>();            
                       
            var Best = new Solution(Best_Solution);
              
            
            do{  
                if(Q.Count != 0)
                {
                    for(var i = 0; i < Population.Count; i++)
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
               
                for (var j = 0; j < Population.Count; j++)
                {                    
                    List<Solution> solucionesAleatorias = obtenerAleatorios(Population, j, myRandom);                    

                    var a = solucionesAleatorias[0];
                    var b = solucionesAleatorias[1];
                    var c = solucionesAleatorias[2]; 

                    a.DEmutation(myRandom,b,c,k,index_i,index_s);                                                        

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

                    a.CruceIntercalado(padre, madre, index_i,index_s);

                    Population[j] = a;
                    Population[j].Repare(myRandom,index_i,index_s);
                    Population[j].Evaluate();
                    EFOs++;                    

                    if (EFOs >= MaxEFOs) break;                    
                }                

               
            }while (EFOs <= MaxEFOs );
            Population[Population.Count-1]=Best; 
            Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));
            BestSolution = new Solution(Population[0]);
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
        
        
        private List<Solution> obtenerAleatorios(List<Solution> Population, int index_Qi, Random ale)
        {
            List<Solution> varResultado = null;
            var count = 1;

            while (varResultado == null){   
                var seed = Environment.TickCount;
                var aleatorio = new Random(seed);             
                var a = aleatorio.Next(Population.Count);
                var b = aleatorio.Next(Population.Count);
                var c = aleatorio.Next(Population.Count);
                
                if (count>=5)
                {
                    varResultado = init_list_rand(Population,0,1,2);
                }
               
                count ++;
                if (a == b || b == c || c == a)
                {
                    continue;
                }
                if (a == index_Qi || b == index_Qi || c ==index_Qi)
                {
                    continue;
                }  
                varResultado = init_list_rand(Population,a,b,c);
                               
            }

            return varResultado;
        }
        public List<Solution> init_list_rand(List<Solution> Population, int a,int b, int c)
        {
                var varResultado = new List<Solution>();                  
                varResultado.Add(new Solution(Population[a]));
                varResultado.Add(new Solution(Population[b]));
                varResultado.Add(new Solution(Population[c]));
                return varResultado;
        }
            
    }
}