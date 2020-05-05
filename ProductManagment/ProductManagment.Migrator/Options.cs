using CommandLine;

namespace ProductManagment.Migrator
{
    public class Options
    {
        [Option('c', "connectionString", Required = true, HelpText = "The connection string to database that needs to be updated.")]
        public string ConnectionString { get; set; }
    }
}
