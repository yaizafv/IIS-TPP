namespace Library;

public enum Status
{
    VIP,
    Business,
    Standard
}

public class Client
{


    private Status status;
    private string id;
    private string name;
    private string surname;

    public DateTime FechaNacimiento { get; private set; }

    public Status Status
    {
        get { return status; }
        set
        {
            status = value;

        }
    }

    public Client(string name)
    {
        this.name = name;
    }

    public string Id
    {
        get { return id; }

        set
        {
            if (value != null)
                id = value;

        }
    }

    public string Name
    {
        get { return name; }

        set
        {
            if (value != null)
                name = value;

        }
    }

    public string Surname
    {
        get { return surname; }

        set
        {
            if (value != null)
                surname = value;        //NO PONER Surname, eso es la propiedad

        }
    }

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            int age = today.Year - FechaNacimiento.Year;

            // Si aún no ha cumplido este año, restamos 1
            if (FechaNacimiento.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }

}
//propiedad derivada edad, fecha de nacimiento. edad seria solo propiedad y no atributo
