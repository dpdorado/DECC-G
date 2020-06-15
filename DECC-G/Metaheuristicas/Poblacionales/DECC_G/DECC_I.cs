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
            var wPopulation = new List<Solution>();
            definir_peso_poblacion();//Falta
            evaluar_poblacion();//Falta
            var index = randperm(D);
            for (var i = 1; i < this.cycles; i++)
            {
                for (var j = 1; j < this.D/this.s; j++)
                {
                    var l = (j - 1) * s + 1;
                    var u = j * s;
                    var subpob = obtener_subpoblacion(l,u);//falta->index(l,u)
                    subpob = SaNSDE(BestSolution,subpob,this.FEs);//Falta
                    definir_peso_poblacion();//Arreglar -> :,j
                    add_poblacion(l,u,subpob);//Agrega la subpoblación o una parte de ella a la Población general
                    evaluar_poblacion();//Falta
                }    
                findbest(Population);//Falta
                findrand(Population);//Falta
                findworst(Population);//Falta
                DE(BestSolution,wPopulation,wFEs);//Falta
                //DE(randSolution,wPopulation,wFEs);//Falta
                //DE(worstSolution,wPopulation,wFEs);//Falta
                evaluar_poblacion();
            }                        
        }
        //Algoritmo de evoluación diferencial
        public void DE(Solution BestSolution, List<Solution> wPopulation, int wFEs)
        {            
            //TODO:   

        }

        //Encuentra la peor solucón  en la población->worst,worst_indexs        
        public void findworst(List<Solution> Population)
        {
            //TODO:
        }
        //Encuentra una solucón aleatoria en la población->rand,rand_indexs        
        public void findrand(List<Solution> Population)
        {
            //TODO:
        }

        //Encuentra la mejor solución en la población->best,bets_indexs
        public void findbest(List<Solution> Population)
        {
            //TODO:
        }
        //Agrega la subpoblación o una parte de ella a la Población general
        public void add_poblacion(int l, int u, List<Solution>subpob)
        {
            //TODO:
            return;
        }
        //???????
        public int index(int l, int u)
        {
            //TODO; 
            return 0;
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