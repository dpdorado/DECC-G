using System.Collections.Generic;
using System.IO;

namespace OptimizacionBinaria.Funciones
{
    public class Knapsack
    {
        private const string RootDirectory = "D:\\metaheuristicas\\DECC-G\\DECC-G\\knapsack-files\\";
        public int TotalItems;
        public double Capacity;
        public double OptimalKnown;
        private readonly List<Variable> _variables = new List<Variable>(); //Always should be sort by position
        public string FileName;

        public Knapsack(string fileName)
        {
            FileName = fileName;
            ReadFile(RootDirectory + fileName);
        }

        public double Weight(int index)
        {
            return _variables[index].Weight;
        }

        public void ReadFile(string fullFileName)
        {
            //read the problem
            var lines = File.ReadAllLines(fullFileName);
            var firstline = lines[0].Split(' ');
            TotalItems = int.Parse(firstline[0]);
            Capacity = double.Parse(firstline[1]);

            var positionLine = 1;
            for (var i = 0; i < TotalItems; i++)
            {
                var line = lines[positionLine++].Split(' ');
                var value = double.Parse(line[0]);
                var weight = double.Parse(line[1]);
                var newVariable = new Variable(i, value, weight);
                _variables.Add(newVariable);
            }

            OptimalKnown = double.Parse(lines[positionLine]);
        }

        public double Evaluate(int[] dim)
        {
            var summ = 0.0;
            for (var i = 0; i < TotalItems; i++)
                summ += dim[i] * _variables[i].Value;

            return summ;
        }

        public List<Variable> GetVariables()
        {
            return new List<Variable>(_variables);
        }

        public override string ToString()
        {
            var result = "Capacity:" + Capacity.ToString("##0") + "\n" +
                   "OptimalKnown:" + OptimalKnown.ToString("##0.00") + "\n";

            for (var i = 0; i < TotalItems; i++)
                result += _variables[i] + "\n";
            return result;
        }
    }
}