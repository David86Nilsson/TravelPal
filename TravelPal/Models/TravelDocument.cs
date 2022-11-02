namespace TravelPal.Models
{
    public class TravelDocument : PackingListItem
    {
        public string Name { get; set; }
        public bool Required { get; set; }

        public TravelDocument(string name, bool required)
        {
            Name = name;
            Required = required;
        }

        //Returns info about the document
        public string GetInfo()
        {
            if (Required) return $"{Name} is required";

            return $"{Name} is not required";
        }
    }
}