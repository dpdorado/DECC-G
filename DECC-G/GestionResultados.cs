using System;
using System.IO;
using ClosedXML.Excel;


namespace OptimizacionBinaria
{
    class GestionResultados
    {

        private string Path = @"/home/dpdaniel/Pruebas/Parcial 2 - Metaheuristicas/DECC-G/DECC-G/Resultados/";
        private string nameTemplate = "Plantilla/plantilla_resultados.xlsx";

        public GestionResultados(string path, string nameTemplate)
        {
            this.Path = path;
            this.nameTemplate = nameTemplate;
        }

        public GestionResultados(){}

        public string copiarPlantilla()
        {
            string new_path = "";            
            string thisDay = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");                                        
            new_path = this.Path+thisDay+".xlsx";
            File.Copy(this.Path+nameTemplate, new_path); 
            return new_path;            
        }

        public void editarCelda(string celda, double valor, string path)
        {
            var workbook = new XLWorkbook(path); // load the existing Excel file
            var worksheet = workbook.Worksheets.Worksheet(1);
            worksheet.Cell(celda).SetValue(valor);
            workbook.Save();

        }

    }

}