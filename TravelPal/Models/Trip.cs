using TravelPal.Enums;

namespace TravelPal.Models
{
    public class Trip : Travel
    {
        public TripTypes Type { get; set; }
        public Trip(string destination, Countries country, int travellers, TripTypes type) : base(destination, country, travellers)
        {
            Type = type;
        }

        //returns the trip as a string
        public override string GetInfo()
        {
            return Type.ToString();
        }
    }
}
