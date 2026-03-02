using System;
using System.IO;
using System.Text.Json;


public class CityData
{
    public string CityName { get; set; }
    public int Latitude { get; set; }
    public int Longitude { get; set; }
    // ... other properties
}

class Program
{
    static void Main(string[] args)
    {

        // Read the JSON from a file into a string
        string jsonString = File.ReadAllText(@"taiwan_cities.json");

        List<CityData> List_cities = JsonSerializer.Deserialize<List<CityData>>(jsonString);

        Console.WriteLine($"{"List_cities.Count:"}{List_cities.Count()})");

        foreach (var city in List_cities)
        {
            Console.WriteLine($"Name: {city.CityName}, Latitude: {city.Latitude}, Longitude: {city.Longitude} minus!");
            city.Latitude -= 5;
            city.Longitude -= 5;
        }
        // Serialize the object into a formatted JSON string
        string newjsonString = JsonSerializer.Serialize(List_cities, new JsonSerializerOptions { WriteIndented = true });

        // Write the string to a file (this overwrites the file if it exists)
        File.WriteAllText(@"taiwan_cities_output.json", newjsonString);


    }
}