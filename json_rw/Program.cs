using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;


public class CityData
{
    public string CityName { get; set; }
    public int Latitude { get; set; }
    public int Longitude { get; set; }
    // ... other properties
}
public class Properties
{
    [JsonPropertyName("COUNTRY")]
    public string Country { get; set; }
}

public class Geometry
{
    [JsonPropertyName("type")]
    public string Type { get; set; } // e.g., "Point", "LineString", "Polygon"

    [JsonPropertyName("coordinates")]
    // This is the problematic part that needs a custom converter or a dynamic approach 
    // because the List nesting depth changes based on the 'Type' property.
    public object Coordinates { get; set; }
}
public class Feature
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "Feature";

    [JsonPropertyName("properties")]
    public Properties Properties { get; set; }

    [JsonPropertyName("geometry")]
    public Geometry Geometry { get; set; }
}

public class RootObject // Represents a FeatureCollection
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "FeatureCollection";

    [JsonPropertyName("features")]
    public List<Feature> Features { get; set; }
}
class Program
{
    static void array_edit()
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
    static void geojson_parse()
    {
        // string geoJsonString = File.ReadAllText("countries_mexico.geojson");
        string geoJsonString = File.ReadAllText("countries.geojson");
        RootObject myDeserializedClass = JsonSerializer.Deserialize<RootObject>(geoJsonString);

        // Console.WriteLine($"{"myDeserializedClass.Features[0].Properties.Country : "}{myDeserializedClass.Features[0].Properties.Country})");
        foreach (var feature in myDeserializedClass.Features)
        {
            Console.WriteLine($"{"Features[n].Properties.Country : "}{feature.Properties.Country}");
        }
    }
    static void Main(string[] args)
    {
        // array_edit();
        geojson_parse();
    }
}