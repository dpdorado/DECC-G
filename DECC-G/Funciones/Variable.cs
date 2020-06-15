namespace OptimizacionBinaria.Funciones
{
    public class Variable
    {
        public int Position;
        public double Value;
        public double Weight;

        public double Density => Value / Weight;

        public Variable(int p, double v, double w)
        {
            Position = p;
            Value = v;
            Weight = w;
        }

        public override string ToString()
        {
            return "P: "+ $"{Position,3:##0}" + 
                   " V: " + $"{Value, 6:##0.0}" + 
                   " W: " + $"{Weight,6:##0.0}" + 
                   " D: " + $"{Density,6:##0.0}";
        }
    }
}