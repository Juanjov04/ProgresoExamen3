using SQLite;
using System.Net.Http.Json;

namespace ProgresoExamen3;

public partial class Paises : ContentPage
{
    private SQLiteAsyncConnection basededatos;

    public Paises()
    {
        InitializeComponent();
        iniciobase(); 
    }

    public class Pais
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NombreOficial { get; set; }
        public string Region { get; set; }
        public string Google { get; set; }
    }

    private async void iniciobase()
    {
        string rutaBD = Path.Combine(FileSystem.AppDataDirectory, "inicio.db3");
        basededatos = new SQLiteAsyncConnection(rutaBD);
        await basededatos.CreateTableAsync<Pais>();
    }

    private async void OnBuscarClicked(object sender, EventArgs e)
    {
        string nombrePais = IngresePais.Text?.Trim();
        if (string.IsNullOrEmpty(nombrePais))
        {
            await DisplayAlert("Error", "Por favor, ingrese un nombre de país.", "OK");
            return;
        }

        try
        {
            HttpClient client = new HttpClient();
            var url = $"https://restcountries.com/v3.1/name/{nombrePais}";
            var respuesta = await client.GetFromJsonAsync<List<CountryResponse>>(url);

            if (respuesta == null || respuesta.Count == 0)
            {
                await DisplayAlert("Error", "No se encontró información para el país ingresado.", "OK");
                return;
            }

           
            var pais = respuesta[0];
            string nombreOficial = pais.Name.Official;
            string region = pais.Region;
            string googleMaps = pais.Maps.GoogleMaps;

           
            await DisplayAlert("Información del País",
                $"Nombre Oficial: {nombreOficial}\nRegión: {region}\nGoogle Maps: {googleMaps}", "OK");

           
            var nuevoPais = new Pais
            {
                NombreOficial = nombreOficial,
                Region = region,
                Google = googleMaps
            };
            await basededatos.InsertAsync(nuevoPais);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error al buscar el país: {ex.Message}", "OK");
        }
    }

    private void OnLimpiarClicked(object sender, EventArgs e)
    {
        IngresePais.Text = string.Empty;
    }

    
    public class CountryResponse
    {
        public Name Name { get; set; }
        public string Region { get; set; }
        public Maps Maps { get; set; }
    }

    public class Name
    {
        public string Official { get; set; }
    }

    public class Maps
    {
        public string GoogleMaps { get; set; }
    }
}
