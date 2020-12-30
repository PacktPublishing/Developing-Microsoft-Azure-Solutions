using Newtonsoft.Json;
using System;

namespace DeckOfCards
{
public class Profile
{
    [JsonProperty(PropertyName = "id")]
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Workout Favorite { get; set; }
    public Location Location { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}

public class Workout
{
    public Guid WorkoutId { get; set; }
    public string Name { get; set; }
}

public class Location
{
    public string State { get; set; }
    public string City { get; set; }
}
}
