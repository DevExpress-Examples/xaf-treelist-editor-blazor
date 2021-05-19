using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using TreeListSample.Module.BusinessObjects;
using XpoSerialization;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string connectionString = "Integrated Security=SSPI;Pooling=false;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=TreeListSample";
            XpoDefault.DataLayer = XpoDefault.GetDataLayer(connectionString, AutoCreateOption.DatabaseAndSchema);

            // Read data:
            Console.WriteLine($"The 'Category' table now contains the following records:");
            using(UnitOfWork uow = new UnitOfWork()) {
                var query = uow.Query<Category>().AsEnumerable();
                foreach(var line in query) {
                    Console.WriteLine(line);
                }
                var jsonOptions = new JsonSerializerOptions() {
                    //ReferenceHandler = ReferenceHandler.Preserve
                    
                };
                jsonOptions.Converters.Add(new PersistentBaseConverterFactory());
                var data = JsonSerializer.Serialize(query, jsonOptions);
            }
            
        }
    }
}
