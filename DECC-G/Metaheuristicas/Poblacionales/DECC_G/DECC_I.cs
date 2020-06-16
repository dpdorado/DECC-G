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

        //Atributos
        //Número de diviciones del vector
        public int s = 2;//cambiar valor por defecto
        //Número de ciclos
        public int cycles = 2;//cambiar valor por defecto
        //Número predefinido de evaluaciones de estado físico (FE)
        public int FEs = 2;//cambiar valor por defecto
        //Peso del componente FEs
        public int wFEs = 0;
        //Dimensiones
        public int D = 0;

        

        public override void Ejecutar(Knapsack theProblem, Random myRandom)
        { 
            //var timeBegin = DateTime.Now;
            EFOs = 0;
            
            //Agregar conodiciones de salida -> EFOs, y si se encuentra soucion

            //Population = pop, wpop ->otra lista de soluciones

            // Inicialización de la población P(t=0)
            var Population = inicializar_poblacion(theProblem,myRandom);//->CMBIAR POP(VALOR1,VALOR2)
            var wPopulation = new List<Double>();
            
            definir_peso_poblacion();//Falta            
            
            ///var index = randperm(D);//index(0,D);

            for (var i = 1; i < this.cycles; i++)
            {
                for (var j = 1; j < this.D/this.s; j++)
                {
                    var l = (j - 1) * s + 1;
                    var u = j * s;

                    var ind = index(l,u);

                    var subpob = obtener_subpoblacion(0, ind);

                    subpob = SaNSDE(BestSolution,subpob,this.FEs);//Falta//FEs->número de evaluaciones

                    definir_peso_poblacion();//Arreglar -> :,j

                    add_poblacion(0,ind,subpob);//remplazar la subpoblación mejorada

                    evaluar_poblacion();//Falta
                }    

                Population.Sort((x,y) => -1 * x.Fitness.CompareTo(y.Fitness));

                //Se optiene la peor solución, la mejor y una solución aleatoria            
                Solution best_s = Population[this.PopulationSize];
                Solution rand_s =Population[this.index(1,this.PopulationSize-1)];
                Solution worst_s = Population[0];
                                
                DE(best_s,wPopulation,wFEs);//Falta
                DE(rand_s,wPopulation,wFEs);//Falta
                DE(worst_s,wPopulation,wFEs);//Falta

                evaluar_poblacion();
            }                        
        }

        //Genera un indice netre el limite inferior(li) y el limite superior(ls)
        public int index(int li,int ls)
        {
            var rand = new Random();            
            return rand.Next(li,ls);
        }
        //Algoritmo de evoluación diferencial
        public void DE(Solution BestSolution, List<Double> wPopulation, int wFEs)
        {            
            //TODO:   

        }
       
        //Agrega la subpoblación o una parte de ella a la Población general
        public void add_poblacion(int l, int u, List<Solution>subpob)
        {
            //TODO:
            return;
        }        
        //SaNSDE....
        public List<Solution> SaNSDE(Solution BestSolution,List<Solution> subpob,int FEs)
        {
            //TODO:
            return null;
        }

        //Obtener subpoblación
        public List<Solution> obtener_subpoblacion(int l,int u)
        {
            //TODO:
            return null;//lista
            
        }
        //Generar número aleatorio entre 1 y número de diviciones del vector (D)
        public int randperm(int D)
        {
            var rand = new Random();            
            return rand.Next(1,D);
        }


        //Evaluar la población -> best, best_val
        public void evaluar_poblacion()
        {
            //TODO:
        }

        //Definir peso de la población: linea 2
        public void definir_peso_poblacion()
        {
            //TODO:
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