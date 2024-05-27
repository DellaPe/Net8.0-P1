using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.VisualBasic;

var currentDirectory = Directory.GetCurrentDirectory(); // Ruta comple en donde se hace el console
var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);  // Crea el directorio si no existe

var salesFiles = FindFiles("stores");
var salesTotal = CalculateSalesTotal(salesFiles);

// Si el archivo no existe, lo crea, s
File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

// Logica para encontrar los archivos
IEnumerable<string> FindFiles(string folderName)
{
  List<string> salesFiles = new List<string>();

  var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

  foreach (var file in foundFiles)
  {
    // The file name will contain the full path, so only check the end of it
    if (file.EndsWith("sales.json"))
    {
      salesFiles.Add(file);
    }
  }

  return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
  double salesTotal = 0;

  foreach (var file in salesFiles)
  {
    string salesJson = File.ReadAllText(file);
    SalesTotal? salesData = JsonConvert.DeserializeObject<SalesTotal>(salesJson);
    salesTotal += salesData?.Total ?? 0;
  }

  return salesTotal;
}
record SalesData(double Total);
class SalesTotal
{
  public double Total { get; set; }
}

/*
Elementos que se fueron eliminado
----------------

var SEPARATOR_CHAR = Path.DirectorySeparatorChar; // Windows: \, Linux: /
var storesDirectory = Path.Combine(currentDirectory, "stores");

----------------

var salesJson = File.ReadAllText($"stores{SEPARATOR_CHAR}201{SEPARATOR_CHAR}sales.json");
var salesData = JsonConvert.DeserializeObject<SalesTotal>(salesJson);

Console.WriteLine($"obj.Total: {salesData.Total}");
File.WriteAllText(Path.Combine(salesTotalDir, "totals.txt"), String.Empty);

File.AppendAllText($"salesTotalDir{Path.DirectorySeparatorChar}totals.txt", $"{Environment.NewLine}{salesData.Total}");

----------------

string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // Mis documentos
Console.WriteLine(docPath);
Console.WriteLine($"stores{Path.DirectorySeparatorChar}201"); // stores\201 (detenca el path)
Console.WriteLine(Path.Combine("stores", "201")); // outputs: stores\201
Console.WriteLine(Path.GetExtension("sales.json")); // outputs: .json
string fileName = Path.Combine(docPath, "stores", "201", "sales.json");
FileInfo info = new FileInfo(fileName);
Console.WriteLine($"Full Name: {info.FullName}{Environment.NewLine}Directory: {info.Directory}{Environment.NewLine}Extension: {info.Extension}{Environment.NewLine}Create Date: {info.CreationTime}"); // And many more
*/