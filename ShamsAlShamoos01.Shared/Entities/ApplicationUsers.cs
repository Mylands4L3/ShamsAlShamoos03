

using Microsoft.AspNetCore.Identity;

namespace ShamsAlShamoos01.Shared.Entities
{
    public class ApplicationUsers:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobsLevel { get; set; }

        public byte gender { get; set; }
        public override string PhoneNumber { get; set; }
        public string ImagePath { get; set; }
        //public DateTime BirthDayDate { get; set; } = DateTime.Now;
        //public DateTime? LastPasswordChangeDate { get; set; } = DateTime.Now;
        public DateTime BirthDayDate { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }

        public byte IsActive { get; set; }
        //public bool IsActive { get; set; }

        public string Melicode { get; set; } = "";
        public string PersonalCode { get; set; }
        public string TextRequest { get; set; }
        //public virtual IList<UsersCategorySub> UsersCategorySubs { get; set; }
        //public virtual IList<UsersCategory> UsersCategorys { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Rate { get; set; }
        public string Personelcodes { get; set; }

        //public string? MelliCodeID { get; set; }
        //[ForeignKey("MelliCodeID")]
        //public virtual TblMaster tblMelicodeID { get; set; }




        //public string? Sortable013d { get; set; }  
        //[ForeignKey("Sortable013d")]

        //public virtual Sortable03 Sortable03 { get; set; }




        //public int? YeganID { get; set; }
        //[ForeignKey("YeganID")]
        //public virtual Category tblYeganID { get; set; }

        //public int? SematID { get; set; }
        //[ForeignKey("SematID")]
        //public virtual Category tblSematID { get; set; }

        //public int? ShobehID { get; set; }
        //[ForeignKey("ShobehID")]
        //public virtual Category tblShobehID { get; set; }

        //public int? RateID { get; set; }
        //[ForeignKey("RateID")]
        //public virtual CategoryGeneral tblRateID { get; set; }

        //public virtual IList<UjobPesronel01> UjobPesronel01s { get; set; }


    }
}
