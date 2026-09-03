using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Views;
using System.Globalization;

namespace MauiAppMinhasCompras
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db;

        public static SQLiteDatabaseHelper Db
        {
            get
            {
                if (_db == null)
                {
                    string caminho = Path.Combine(
                        FileSystem.AppDataDirectory,
                        "banco_sqlite_compras.db3");

                    _db = new SQLiteDatabaseHelper(caminho);
                }

                return _db;
            }
        }

        public App()
        {
            InitializeComponent();

            // Configura o formato brasileiro
            CultureInfo.DefaultThreadCurrentCulture =
                new CultureInfo("pt-BR");

            CultureInfo.DefaultThreadCurrentUICulture =
                new CultureInfo("pt-BR");

            MainPage = new NavigationPage(
                new ListaProduto());
        }
    }
}