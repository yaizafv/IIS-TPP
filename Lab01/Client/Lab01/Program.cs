namespace Lab01;

using Library;

class Program
{
    static void Main(string[] args)
    {
        Client c1 = new Client{ Name = "Yaiza", FechaNacimiento = new DateTime(2004, 04, 05)};
        Console.WriteLine($"Hola, {c1.Name}. Tienes {c1.Age} años!");
    }
}
