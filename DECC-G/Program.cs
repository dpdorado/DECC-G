using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OptimizacionBinaria.Exactos;
using OptimizacionBinaria.Funciones;
using OptimizacionBinaria.Metaheuristicas;
using OptimizacionBinaria.Metaheuristicas.EstadoSimple;
using OptimizacionBinaria.Metaheuristicas.EstadoSimple.HC;
using OptimizacionBinaria.Metaheuristicas.EstadoSimple.TS;
using OptimizacionBinaria.Metaheuristicas.Poblacionales.Geneticos;
using OptimizacionBinaria.Metaheuristicas.Poblacionales.PSO;
using OptimizacionBinaria.Metaheuristicas.Poblacionales.DECC_G;

namespace OptimizacionBinaria
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hola");
            var myProblems = new List<Knapsack>
            {
                new Knapsack("f1.txt"),
                new Knapsack("f2.txt"),
                new Knapsack("f3.txt"),
                new Knapsack("f4.txt"),
                new Knapsack("f5.txt"),
                new Knapsack("f6.txt"),
                new Knapsack("f7.txt"),
                new Knapsack("f8.txt"),
                new Knapsack("f9.txt"),
                new Knapsack("f10.txt"),
                new Knapsack("Knapsack1.txt"),
                new Knapsack("Knapsack2.txt"),
                new Knapsack("Knapsack3.txt"),
                new Knapsack("Knapsack4.txt"),
                new Knapsack("Knapsack5.txt"),
                new Knapsack("Knapsack6.txt")
            };

            var maxEFOS = 5000;
            var myAlgorithms = new List<Algorithm>
            {
                //new PSOBinario() {MaxEFOs = maxEFOS, PopulationSize = 10}        
                //new Exhaustive(),
                //new SimulatedAnnealing() {MaxEFOs = mexEFOS},
                //new HillClimbing {MaxEFOs = maxEFOS},
                //new AscensoColinaMaximaPendiente {MutationProbability = 0.5, radio = 10, vecinos = 10, MaxEFOs = mexEFOS},
                //new AscensoColinaMaximaPendienteConRemplazo {MutationProbability = 0.5, radio = 10, vecinos = 10, MaxEFOs = mexEFOS},
                //new AscensoColinaConReinicios {MaxEFOs = maxEFOS},
                //new RandomSearch() {MaxEFOs = maxEFOS}
                //new BusquedaTabu{MaxEFOs = maxEFOS, AtrNumeroTweaks = 2},
                //new BusquedaTabuConCaracteristicas {MaxEFOs = maxEFOS, AtrNumeroTweaks = 2}

                //Algoritmos para analizar
                new RandomSearch() {MaxEFOs = maxEFOS},
                new Genetico1() {MaxEFOs = maxEFOS, PopulationSize = 10},
                new GBHS() {MaxEFOs = maxEFOS, HarmonyMemorySize = 10},
                new DE(){MaxEFOs = maxEFOS, k = 2},
                new DECC_I(){MaxEFOs = maxEFOS, cycles =100, FEs = 50, s = 4}

            };

            const int maxRep = 30;
            var gestorR = new GestionResultados();
            var pathResultados = gestorR.copiarPlantilla();
            var limite_= myAlgorithms.Count*3;
            var fi = 68;
            var ci = 6;
            var confila = 0;
            var conColumna = 0;
            //68,79 ----6,21            
            var count = 1;

            var hmsList = new List<int>(){5};//{5, 10, 15, 20}; //4
            var hmcrList = new List<double>{0.7};// {0.7, 0.75, 0.8, 0.85, 0.9, 0.95}; //6
            var parList = new List<double>(){0.25};//; { 0.2, 0.25, 0.3, 0.35, 0.4}; //5


            Console.WriteLine("             Búsqueda Aleatoria        Algoritmo Genético        GBHS                      DE                        DECC_I");
            foreach (var Problem in myProblems)
            {
                Console.Write("Problema " + count+ ":  ");                
                foreach (var Algorithm in myAlgorithms)
                {                    
                    var mediaF = 0.0;
                    var conExito = 0;
                    var tasaExito = 0.0;                    
                    var DE = 0.0;
                    ArrayList mejSoluciones = new ArrayList();
                    for (var rep = 0; rep < maxRep; rep++)
                    {
                        var seed = Environment.TickCount;
                        var aleatorio = new Random(seed);
                        //var aleatorio2 = new Random(rep + 2);

                        Algorithm.Ejecutar(Problem, aleatorio);
                        mediaF += Algorithm.BestSolution.Fitness;
                        mejSoluciones.Add(Algorithm.BestSolution.Fitness);
                        if (Algorithm.BestSolution.IsOptimalKnown())
                        {
                            conExito++;
                        }                        
                    }
                    // Media
                    mediaF = mediaF / maxRep;
                    //Tasa de éxito
                    tasaExito = (Double)conExito /(Double)maxRep * 100;
                    //Desviación estandar
                    DE = calcularDE(mejSoluciones, mediaF);
                    //agregar a un archivo los datos

                    if (confila == limite_){confila=0;}                                    

                    var celda = convertirCelda(fi,confila,ci,conColumna);                                     
                    gestorR.editarCelda(celda, mediaF, pathResultados);
                    confila++;                    
                    celda = convertirCelda(fi,confila,ci,conColumna);                                        
                    gestorR.editarCelda(celda, DE, pathResultados);
                    confila++;                    
                    celda = convertirCelda(fi,confila,ci,conColumna);                                        
                    gestorR.editarCelda(celda, tasaExito, pathResultados);
                    confila++; 
                                                        

                    //Console.Write($"{mediaF,-25:0.0000}" + " | "+$"{DE,-25:0.0000}" +" | "+$"{tasaExito,-25:0.0000}");
                    Console.Write($"{mediaF,-25:0.000000000000000}" + " ");                    
                }
                Console.WriteLine();
                count++;
                conColumna++;               
            }                             

            /*
            foreach (var hms in hmsList)
            {
                foreach (var hmcr in hmcrList)
                {
                    foreach (var par in parList)
                    {
                        foreach (var theAlgorithm in myAlgorithms)
                        {
                            ((GBHS) theAlgorithm).HarmonyMemorySize = hms;
                            ((GBHS) theAlgorithm).Hmcr = hmcr;
                            ((GBHS)theAlgorithm).Par = par;

                            Console.Write($"{theAlgorithm} ");
                            //Console.Write($"{"Problem", -15} {"Items", 6} {"Ideal", 10} ");
                            //Console.WriteLine($"{"Avg-Fitness",15} {"SD-Fitness",15} {"Avg-Efos",15} {"Success Rate",15} { "Time",15}");

                            var success = 0.0;

                            foreach (var theProblem in myProblems)
                            {
                                //Console.Write($"{theProblem.FileName,-15} {theProblem.TotalItems,6} {theProblem.OptimalKnown,10} ");

                                var efos = new List<int>();
                                var mediaF = new List<double>();
                                var times = new List<double>();
                                var succesRate = 0;
                                for (var rep = 0; rep < maxRep; rep++)
                                {
                                    var myRandom = new Random(rep);
                                    var timeBegin = DateTime.Now;
                                    theAlgorithm.Ejecutar(theProblem, myRandom);
                                    times.Add((DateTime.Now - timeBegin).TotalSeconds);
                                    mediaF.Add(theAlgorithm.BestSolution.Fitness);
                                    efos.Add(theAlgorithm.EFOs);
                                    if (Math.Abs(theAlgorithm.BestSolution.Fitness - theProblem.OptimalKnown) < 1e-10)
                                        succesRate++;
                                }

                                success += succesRate * 100.0 / maxRep;

                                //var avg = mediaF.Average();
                                //Console.Write($"{avg,15:0.000} ");
                                //var deviation = mediaF.Sum(d => (d - avg) * (d - avg));
                                //deviation = Math.Sqrt(deviation / maxRep);
                                //Console.Write($"{deviation,15:0.000} ");
                                //Console.Write($"{efos.Average(),15:0.000} ");
                                //Console.Write($"{succesRate * 100.0 / maxRep,15:0.00}% ");
                                //Console.WriteLine($"{times.Average(),15:0.000000} ");
                            }

                            Console.WriteLine($"Existo Promedio = {success / myProblems.Count}% ");
                        }
                    }
                }
            }*/

            Console.ReadKey();
        }

        public static string convertirCelda(int fi, int confila, int ci, int conColumna)
        {
            var columan = ((char)(fi+confila)).ToString();
            var fila = (ci+conColumna).ToString();
            return columan+fila;
        }        

        public static double calcularDE(ArrayList mSoluciones, double M)
        {
            var suma = 0.0;
            var N = mSoluciones.Count;

            foreach(object Xi in mSoluciones)
            {
                var aux = (double)Xi - M;
                suma += Math.Pow(aux, 2);
            }            
            return  Math.Sqrt(suma/N);
        }
    }
}