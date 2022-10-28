using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TravelPal.Enums
{
    public enum EuroCountries
    {
        Austria,
        Belgium,
        Bulgaria,
        Croatia,
        [Description("Republic of Cyprus")]
        Republic_of_Cyprus,
        [Description("Czech Republic")]
        Czech_Republic,
        Denmark,
        Estonia,
        Finland,
        France,
        Germany,
        Greece,
        Hungary,
        Ireland,
        Italy,
        Latvia,
        Lithuania,
        Luxembourg,
        Malta,
        Netherlands,
        Poland,
        Portugal,
        Romania,
        [Display(Name = "San Marino")]
        San_Marino,
        Slovakia,
        Slovenia,
        Spain,
        Sweden
    }
}
