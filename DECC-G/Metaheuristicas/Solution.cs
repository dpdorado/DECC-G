using System;
using System.Collections.Generic;
using System.Linq;
using OptimizacionBinaria.Funciones;

namespace OptimizacionBinaria.Metaheuristicas
{
    public class Solution
    {
        public Algorithm MyAlgorithm;
        public Knapsack MyProblem;
        public int[] Objects; // {0, 1}
        public double Weight;
        public double Fitness;

        public Solution(Knapsack theProblem, Algorithm theAlgorithm)
        {
            MyProblem = theProblem;
            MyAlgorithm = theAlgorithm;
            Objects = new int[MyProblem.TotalItems];
            Weight = 0;
            Fitness = 0;
        }

        public Solution(Solution original)
        {
            MyAlgorithm = original.MyAlgorithm;
            MyProblem = original.MyProblem;
            Objects = new int[MyProblem.TotalItems];
            for (var i = 0; i < MyProblem.TotalItems; i++)
                Objects[i] = original.Objects[i];
            Weight = original.Weight;
            Fitness = original.Fitness;
        }

        public void Modify(int value)
        {
            var binary = Convert.ToString(value, 2);
            Weight = 0;
            var j = 0;
            for (var i = binary.Length - 1; i >= 0; i--)
            {
                Objects[j] = (int)char.GetNumericValue(binary[i]);
                if (Objects[j] == 1)
                    Weight += MyProblem.Weight(j);
                j++;
            }
            Evaluate();
        }

        protected void SelectObject(int index)
        {
            Objects[index] = 1;
            Weight += MyProblem.Weight(index);
        }

        protected void UnSelectObject(int index)
        {
            Objects[index] = 0;
            Weight -= MyProblem.Weight(index);
        }

        public virtual void RandomInitialization(Random myRandom)
        {
            Weight = 0;
            var opciones = MyProblem.GetVariables();

            while (Weight <= MyProblem.Capacity)
            {
                var p = myRandom.Next(opciones.Count);
                SelectObject(opciones[p].Position);
                opciones.RemoveAt(p);

                var availableWeight = MyProblem.Capacity - Weight;
                opciones.RemoveAll(x => x.Weight > availableWeight);
                if (opciones.Count == 0) break;
            }
            Evaluate();
        }

        public virtual void Tweak(Random myRandom)
        {
            var tweakProbabilities = new[] {0.7, 1}; //{0.65, 0.85, 0.95, 1};
            var probability = myRandom.NextDouble();
            for (var neighboorhood = 0; neighboorhood < tweakProbabilities.Length; neighboorhood++)
                if (probability < tweakProbabilities[neighboorhood])
                    Tweak2(myRandom, neighboorhood + 1);
        }
 
        /// This tweak operator turn off a numberOff randomly selected objects
        /// then complete the solution (try to randomly include other objects
        /// while available weight exits) - do not complete with elements previously
        /// extracted
 
        public void Tweak2(Random myRandom, int numberOff)
        {
            var selectedObjets = new List<KeyValuePair<int, double>>();
            for (var i = 0; i < MyProblem.TotalItems; i++)
                if (Objects[i] == 1)
                    selectedObjets.Add(new KeyValuePair<int, double>(i, MyProblem.Weight(i)));

            // It should remain at least one element
            if (selectedObjets.Count - numberOff <= 0)
                numberOff = selectedObjets.Count - 1;

            var exceptionList = new List<int>();

            for (var i = 0; i < numberOff; i++)
            {
                var p = myRandom.Next(selectedObjets.Count);
                UnSelectObject(selectedObjets[p].Key);
                exceptionList.Add(selectedObjets[p].Key);
                selectedObjets.RemoveAt(p);
            }

            Complete(myRandom, exceptionList);
            Evaluate();
        }

        public void Complete(Random aleatorio, List<int> exceptionList)
        {
            // create a list of unselected objects that fit into the knapsack 
            var availableWeight = MyProblem.Capacity - Weight;
            var unselectedItemsThatFit = new List<KeyValuePair<int, double>>();
            for (var i = 0; i < MyProblem.TotalItems; i++)
            {
                if (Objects[i] == 0 && MyProblem.Weight(i) <= availableWeight)
                    if (!exceptionList.Contains(i))
                        unselectedItemsThatFit.Add(new KeyValuePair<int, double>(i, MyProblem.Weight(i)));
            }

            // while exits objects in the list of unselected object that fit into de knapsack
            while (unselectedItemsThatFit.Count != 0)
            {
                // randomly select a position of the list, include the object
                // into the knapsack and update the weight
                var t = aleatorio.Next(unselectedItemsThatFit.Count);
                Objects[unselectedItemsThatFit[t].Key] = 1;
                Weight += unselectedItemsThatFit[t].Value;

                // remove the unselected objects with higher weight than the available
                // weight in the knapsack
                availableWeight = MyProblem.Capacity - Weight;
                unselectedItemsThatFit.RemoveAt(t);
                for (var i = unselectedItemsThatFit.Count - 1; i >= 0; i--)
                {
                    if (unselectedItemsThatFit[i].Value > availableWeight)
                        unselectedItemsThatFit.RemoveAt(i);
                }
            }
        }

        public void CruceIntercalado(Solution p1, Solution p2, int index_i, int index_s)
        {
            Weight = 0;
            for (var i = index_i; i < index_s; i++)
            {
                if (i % 2 == 0) Objects[i] = p1.Objects[i];
                else Objects[i] = p2.Objects[i];                
            }

            for (var i = 0;i < MyProblem.TotalItems;i++)
            {
                if (Objects[i] == 1)
                    Weight += MyProblem.Weight(i);
            }
        }
        public void CruceIntercalado(Solution p1, Solution p2)
        {
            Weight = 0;
            for (var i = 0; i < MyProblem.TotalItems; i++)
            {
                if (i % 2 == 0) Objects[i] = p1.Objects[i];
                else Objects[i] = p2.Objects[i];
                if (Objects[i] == 1)
                    Weight += MyProblem.Weight(i);
            }
        }
        
        public void DEmutation(Random myRandom, Solution b, Solution c, Double F,int index_i,int index_s)
        {
            Weight = 0;
            for (var i = index_i; i <= index_s; i++)
            {
                var f = Objects[i] +  F*(b.Objects[i]-c.Objects[i]);
                var sig_x = 1/(1 + Math.Exp(f));
                var rand = myRandom.NextDouble();

                if (sig_x >= rand)
                {
                    Objects[i] = 1;
                }else
                {
                    Objects[i] = 0;
                }                           
            }

            for (var i = 0;i < MyProblem.TotalItems;i++)
            {
                if (Objects[i] == 1)
                    Weight += MyProblem.Weight(i);
            }
            
        }

        public void DEmutation(Random myRandom, Solution b, Solution c, Double F)
        {
            Weight = 0;
            for (var i = 0; i < MyProblem.TotalItems; i++)
            {
                var f = Objects[i] +  F*(b.Objects[i]-c.Objects[i]);
                var sig_x = 1/(1 + Math.Exp(f));
                var rand = myRandom.NextDouble();

                if (sig_x >= rand)
                {
                    Objects[i] = 1;
                }else
                {
                    Objects[i] = 0;
                }
           
                if (Objects[i] == 1)
                    Weight += MyProblem.Weight(i);
            }
        }
        public void OneBitMutation(Random myRandom)
        {
            var p = myRandom.Next(MyProblem.TotalItems);
            if (Objects[p] == 1)
            {
                Objects[p] = 0;
                Weight -= MyProblem.Weight(p);
            }
            else {
                Objects[p] = 1;
                Weight += MyProblem.Weight(p);
            }
        }

        public void Repare(Random myRandom)
        {
            if (Weight <= MyProblem.Capacity) return;

            var inKnapsack = new List<int>();
            for (var i = 0; i < MyProblem.TotalItems; i++)
                if (Objects[i] == 1)
                    inKnapsack.Add(i);

            while (Weight > MyProblem.Capacity)
            {
                var p = myRandom.Next(inKnapsack.Count);
                var id = inKnapsack[p];
                Objects[id] = 0;
                Weight -= MyProblem.Weight(id);
                inKnapsack.RemoveAt(p);
            }
        }

        //Reparación del DECC_I
        public void Repare(Random myRandom, int index_i, int index_s)
        {                       
            var count = 0;
            var s = index_s - index_i;
            //D --> Total items
            if (Weight <= MyProblem.Capacity) return;

            var inKnapsack = new List<int>();
            for (var i = index_i; i <= index_s; i++)
                if (Objects[i] == 1)
                    inKnapsack.Add(i);


            while (Weight > MyProblem.Capacity && count < s)
            {
                var p = myRandom.Next(0, inKnapsack.Count);
                var id = inKnapsack[p];
                Objects[id] = 0;
                Weight -= MyProblem.Weight(id);
                inKnapsack.RemoveAt(p);
                count++;
            }

            if (count >= s)
            {
                Repare(myRandom);
            }
        }

        public void Improvisation(List<Solution> hm, double hmcr, double par, Random myRandom)
        {
            Weight = 0;
            for (var i = 0; i < MyProblem.TotalItems; i++)
            {
                if (myRandom.NextDouble() < hmcr)
                {
                    // Sale de la memoria
                    var p = myRandom.Next(hm.Count);
                    Objects[i] = hm[p].Objects[i];

                    if (myRandom.NextDouble() < par) // tomelo de la mejor = 0 (best)
                        Objects[i] = hm[0].Objects[i];
                }
                else
                {
                    // aleatorio
                    Objects[i] = myRandom.Next(2);
                }

                if (Objects[i] == 1) Weight += MyProblem.Weight(i);
            }
        }

        public void Evaluate()
        {
            Fitness = Weight <= MyProblem.Capacity ? MyProblem.Evaluate(Objects) : double.NegativeInfinity;
            MyAlgorithm.EFOs++;
        }

        public override string ToString()
        {
            var result = "";
            for (var i = 0; i < MyProblem.TotalItems; i++)
                result = result + (Objects[i] + " ");
            result = result + " f = " + Fitness +  " w = " + Weight;
            return result;
        }

        public bool IsOptimalKnown()
        {
            return Math.Abs(Fitness - MyProblem.OptimalKnown) < 1e-10;
        }

    }
}