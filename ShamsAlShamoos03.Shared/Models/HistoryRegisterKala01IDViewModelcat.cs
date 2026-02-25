using System;
using System.Collections.Generic;

namespace ShamsAlShamoos03.Shared.Models
{
    public class ConfirmationStatus
    {
        public int? Status { get; set; }
    }
    public class HistoryRegisterKala01ViewModelcat
    {
        // آرایه متن‌ها
        public string?[] Vartexts { get; set; } = new string?[20];

        // لیست تاییدیه‌ها
        public List<int?> StatusConfirmations { get; set; } = new List<int?> { null, null, null, null, null, null };

        // Propertyهای کمکی برای Vartext01 تا Vartext20
        public string? Vartext01 { get => Vartexts[0]; set => Vartexts[0] = value; }
        public string? Vartext02 { get => Vartexts[1]; set => Vartexts[1] = value; }
        public string? Vartext03 { get => Vartexts[2]; set => Vartexts[2] = value; }
        public string? Vartext04 { get => Vartexts[3]; set => Vartexts[3] = value; }
        public string? Vartext05 { get => Vartexts[4]; set => Vartexts[4] = value; }
        public string? Vartext06 { get => Vartexts[5]; set => Vartexts[5] = value; }
        public string? Vartext07 { get => Vartexts[6]; set => Vartexts[6] = value; }
        public string? Vartext08 { get => Vartexts[7]; set => Vartexts[7] = value; }
        public string? Vartext09 { get => Vartexts[8]; set => Vartexts[8] = value; }
        public string? Vartext10 { get => Vartexts[9]; set => Vartexts[9] = value; }
        public string? Vartext11 { get => Vartexts[10]; set => Vartexts[10] = value; }
        public string? Vartext12 { get => Vartexts[11]; set => Vartexts[11] = value; }
        public string? Vartext13 { get => Vartexts[12]; set => Vartexts[12] = value; }
        public string? Vartext14 { get => Vartexts[13]; set => Vartexts[13] = value; }
        public string? Vartext15 { get => Vartexts[14]; set => Vartexts[14] = value; }
        public string? Vartext16 { get => Vartexts[15]; set => Vartexts[15] = value; }
        public string? Vartext17 { get => Vartexts[16]; set => Vartexts[16] = value; }
        public string? Vartext18 { get => Vartexts[17]; set => Vartexts[17] = value; }
        public string? Vartext19 { get => Vartexts[18]; set => Vartexts[18] = value; }
        public string? Vartext20 { get => Vartexts[19]; set => Vartexts[19] = value; }

        // Propertyهای کمکی برای StatusConfirmation01 تا StatusConfirmation06
        public int? StatusConfirmation01 { get => StatusConfirmations[0]; set => StatusConfirmations[0] = value; }
        public int? StatusConfirmation02 { get => StatusConfirmations[1]; set => StatusConfirmations[1] = value; }
        public int? StatusConfirmation03 { get => StatusConfirmations[2]; set => StatusConfirmations[2] = value; }
        public int? StatusConfirmation04 { get => StatusConfirmations[3]; set => StatusConfirmations[3] = value; }
        public int? StatusConfirmation05 { get => StatusConfirmations[4]; set => StatusConfirmations[4] = value; }
        public int? StatusConfirmation06 { get => StatusConfirmations[5]; set => StatusConfirmations[5] = value; }

        // سایر فیلدها
        public string? HistoryRegisterKala01ID { get; set; }
        public string? DocumentNO01 { get; set; }
        public string? SomeOtherField { get; set; } // فیلدهای دیگر
    }
    public class HistoryRegisterKala01ViewModel_Update
    {
        // آرایه متن‌ها
        public string?[] Vartexts { get; set; } = new string?[20];

        // لیست تاییدیه‌ها
        public List<ConfirmationStatus> Confirmations { get; set; } = new List<ConfirmationStatus>
        {
            new ConfirmationStatus(),
            new ConfirmationStatus(),
            new ConfirmationStatus(),
            new ConfirmationStatus(),
            new ConfirmationStatus(),
            new ConfirmationStatus()
        };

        // Propertyهای کمکی برای Vartext01 تا Vartext20
        public string? Vartext01 { get => Vartexts[0]; set => Vartexts[0] = value; }
        public string? Vartext02 { get => Vartexts[1]; set => Vartexts[1] = value; }
        public string? Vartext03 { get => Vartexts[2]; set => Vartexts[2] = value; }
        public string? Vartext04 { get => Vartexts[3]; set => Vartexts[3] = value; }
        public string? Vartext05 { get => Vartexts[4]; set => Vartexts[4] = value; }
        public string? Vartext06 { get => Vartexts[5]; set => Vartexts[5] = value; }
        public string? Vartext07 { get => Vartexts[6]; set => Vartexts[6] = value; }
        public string? Vartext08 { get => Vartexts[7]; set => Vartexts[7] = value; }
        public string? Vartext09 { get => Vartexts[8]; set => Vartexts[8] = value; }
        public string? Vartext10 { get => Vartexts[9]; set => Vartexts[9] = value; }
        public string? Vartext11 { get => Vartexts[10]; set => Vartexts[10] = value; }
        public string? Vartext12 { get => Vartexts[11]; set => Vartexts[11] = value; }
        public string? Vartext13 { get => Vartexts[12]; set => Vartexts[12] = value; }
        public string? Vartext14 { get => Vartexts[13]; set => Vartexts[13] = value; }
        public string? Vartext15 { get => Vartexts[14]; set => Vartexts[14] = value; }
        public string? Vartext16 { get => Vartexts[15]; set => Vartexts[15] = value; }
        public string? Vartext17 { get => Vartexts[16]; set => Vartexts[16] = value; }
        public string? Vartext18 { get => Vartexts[17]; set => Vartexts[17] = value; }
        public string? Vartext19 { get => Vartexts[18]; set => Vartexts[18] = value; }
        public string? Vartext20 { get => Vartexts[19]; set => Vartexts[19] = value; }

        // Propertyهای کمکی برای StatusConfirmation01 تا StatusConfirmation06
        public int? StatusConfirmation01 { get => Confirmations[0].Status; set => Confirmations[0].Status = value; }
        public int? StatusConfirmation02 { get => Confirmations[1].Status; set => Confirmations[1].Status = value; }
        public int? StatusConfirmation03 { get => Confirmations[2].Status; set => Confirmations[2].Status = value; }
        public int? StatusConfirmation04 { get => Confirmations[3].Status; set => Confirmations[3].Status = value; }
        public int? StatusConfirmation05 { get => Confirmations[4].Status; set => Confirmations[4].Status = value; }
        public int? StatusConfirmation06 { get => Confirmations[5].Status; set => Confirmations[5].Status = value; }

        // سایر فیلدهای مورد نیاز
        public string? HistoryRegisterKala01ID { get; set; }
        public string? DocumentNO01 { get; set; }
        public string? SomeOtherField { get; set; } // فیلدهای دیگر
    }
    public class LoadDataRequest
    {
        public string UserId { get; set; } = string.Empty;

        public int Skip { get; set; }
        public int Take { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }
    }
}
