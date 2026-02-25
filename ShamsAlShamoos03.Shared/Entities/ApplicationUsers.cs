using Microsoft.AspNetCore.Identity;

namespace ShamsAlShamoos03.Shared.Entities
{
    public class ApplicationUsers : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobsLevel { get; set; }
        public byte Gender { get; set; }
        public override string PhoneNumber { get; set; }
        public string ImagePath { get; set; }
        public DateTime BirthDayDate { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }
        public byte IsActive { get; set; }
        public string Melicode { get; set; } = string.Empty;
        public string PersonalCode { get; set; }
        public string TextRequest { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Rate { get; set; }
        public string Personelcodes { get; set; }
    }
}