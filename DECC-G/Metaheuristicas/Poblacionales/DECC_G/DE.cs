using System;
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
            EFOs = 0;                  
            
            var Population = new List<Solution>();
            var Q =new List<Solution>();
            

            Population =  inicializar_poblacion(theProblem,myRandom);
            
            var Best = Population[0];//Entra como parametro
                        
            while (EFOs <= MaxEFOs && Population[0].IsOptimalKnown()){
                
                for(var i = 0; i < PopulationSize; i++)
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

                Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));
            }
            
                
            BestSolution = Population[0];
        } 

        public void Ejecutar(Random myRandom, Solution BestSolution, List<Solution> subpob, int index_i,int index_s)
        {
            Console.WriteLine("DE--");
            EFOs = 0;                  

            var Population = subpob;
            var Q = new List<Solution>();            
                       
            var Best = new Solution(BestSolution);

            var co =0;   
            while (EFOs <= MaxEFOs ){
                /*Console.WriteLine("while");
                Console.WriteLine(co);
                Console.WriteLine("EFOS :"+EFOs);
                Console.WriteLine("MaxEFOS :"+MaxEFOs);
                co++;*/
                for(var i = index_i; i <= index_s; i++)
                {
                    //Console.WriteLine("FOr_1");
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
                
                for (var j = index_i; j <= index_s; j++)
                {
                    //Console.WriteLine("For_2");
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
                    //EFOs++;

                    if (EFOs >= MaxEFOs) break;                    
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
        
        /*
        private List<Solution> obtenerAleatorios_index(List<Solution> Population, int index_Qi, Random aleatorio,int index_i,int index_s)
        {
            //List<Solution> varResultado = new List<Solution>();
            List<Solution> varResultado = null;
            do
            {                
                var a = aleatorio.Next(index_i, index_s);
                var b = aleatorio.Next(index_i, index_s);
                var c = aleatorio.Next(index_i, index_s);

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
        }*/
        private List<Solution> obtenerAleatorios(List<Solution> Population, int index_Qi, Random aleatorio)
        {
            List<Solution> varResultado = null;
            
            while (varResultado == null){                
                var a = aleatorio.Next(0, Population.Count-1);
                var b = aleatorio.Next(0, Population.Count-1);
                var c = aleatorio.Next(0, Population.Count-1);

                if (a == b || b == c || c == a)
                {
                    continue;
                }
                if (a == index_Qi || b == index_Qi || c ==index_Qi)
                {
                    continue;
                }                              
                varResultado = new List<Solution>();                  
                varResultado.Add(new Solution(Population[a]));
                varResultado.Add(new Solution(Population[b]));
                varResultado.Add(new Solution(Population[c]));                
            }

            return varResultado;
        }
            
    }
}