using CountriesProject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectWpf.Countries
{
    /// <summary>
    /// Interaction logic for Countries.xaml
    /// </summary>
    public partial class Countries : Window
    {
        public ObservableCollection<Country> CountriesWorld { get; set; } = new ObservableCollection<Country>();
        private ObservableCollection<Country> _allCountries = new ObservableCollection<Country>();

        public static HttpClient client = new HttpClient();
        public Countries()
        {
            InitializeComponent();
            LoadCountriesDataAsync();

        }

        private async Task LoadCountriesDataAsync()
        {
            string json = await client.GetStringAsync("https://restcountries.com/v3.1/all");

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            CountriesWorld = JsonSerializer.Deserialize<ObservableCollection<Country>>(json, options);

            foreach (Country c in CountriesWorld)
            {
                _allCountries.Add(c);
            }
            CountriesDataGrid.ItemsSource = CountriesWorld;
        }


        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text.ToLower();
            List<Country> filteredCountries = _allCountries
                .Where(c => c.Name.Common.ToLower().Contains(searchText))
                .ToList();

            UpdateCountriesCollection(filteredCountries);
        }

        private void RegionFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedRegion = (RegionFilterComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedRegion == "All Regions")
            {
                UpdateCountriesCollection(_allCountries.ToList());
            }
            else
            {
                List<Country> filteredCountries = _allCountries
                    .Where(c => c.Region.ToLower() == selectedRegion.ToLower())
                    .ToList();

                UpdateCountriesCollection(filteredCountries);
            }
        }



        private void UpdateCountriesCollection(List<Country> countries)
        {
            CountriesWorld.Clear();
            foreach (Country country in countries)
            {
                CountriesWorld.Add(country);
            }
        }
    }
}
