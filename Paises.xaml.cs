using SQLite;

namespace ProgresoExamen3;

public partial class Paises : ContentPage
{
    public Paises()
    {
        InitializeComponent();

    }
    public class Country
    {
        [PrimaryKey, AutoIncrement]
       
        public int Id { get; set; }
        public string NombreOficial { get; set; }
        public string Region { get; set; }
        public string Google { get; set; }
    }
}