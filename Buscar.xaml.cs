using SQLite;
using static ProgresoExamen3.Paises;

namespace ProgresoExamen3;

public partial class Buscar : ContentPage
{
    private SQLiteAsyncConnection basededatos;

    public Buscar()
    {
        InitializeComponent();
        iniciobase();
    }

    private async void iniciobase()
    {
      
        string rutaBD = Path.Combine(FileSystem.AppDataDirectory, "inicio.db3");
        basededatos = new SQLiteAsyncConnection(rutaBD);
        await basededatos.CreateTableAsync<Pais>();

        
        await CargarPaises();
    }

    private async Task CargarPaises()
    {
        
        var paises = await basededatos.Table<Pais>().ToListAsync();

        
        if (paises.Count == 0)
        {
            await DisplayAlert("Información", "No se encontraron países en la base de datos.", "OK");
            return;
        }

        
        var paisesFormateados = paises.Select(p => new
        {
            DisplayText = $"Nombre País: {p.NombreOficial}, Región: {p.Region}, Link: {p.Google}, NombreBD: Scordova"
        }).ToList();

        
        listViewPaises.ItemsSource = paisesFormateados;
    }
}
