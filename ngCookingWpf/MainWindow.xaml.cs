using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using apis.Controllers;
using apis.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ngCookingWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.LoadDatabase();
        }

        private void LoadDatabase()
        {
            // Initialisation de la bas de données
            var configurationBuilder = new ConfigurationBuilder();
            IConfigurationRoot configuration = configurationBuilder.Build();
            NgContext ngContext = new NgContext(configuration);

            // Récupération des utilisateurs
            CommunityController communityController = new CommunityController(new CommunityRepository(ngContext));
            communityController.Get();

            // Récupération des recettes
            IHostingEnvironment appEnvironment = null;
            RecettesController recettesController = new RecettesController(new RecetteRepository(ngContext), new IngredientRepository(ngContext), appEnvironment);
            recettesController.Get(null);
        }
    }
}
