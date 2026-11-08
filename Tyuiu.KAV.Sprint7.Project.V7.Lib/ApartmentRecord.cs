using System;
using System.Globalization;

namespace Tyuiu.KAV.Sprint7.Project.V7.Lib
{
    public class ApartmentRecord
    {
        public int EntranceNumber { get; set; }           // Номер подъезда
        public int ApartmentNumber { get; set; }          // Номер квартиры
        public double TotalArea { get; set; }             // Общая площадь
        public double LivingArea { get; set; }            // Полезная (жилая) площадь
        public int RoomsCount { get; set; }               // Количество комнат
        public string OwnerLastName { get; set; }         // Фамилия квартиросъемщика
        public DateTime RegistrationDate { get; set; }    // Дата прописки
        public int FamilyMembersCount { get; set; }       // Количество членов семьи
        public int ChildrenCount { get; set; }            // Количество детей
        public bool HasDebt { get; set; }                 // Задолженность по квартплате
        public string Note { get; set; }                  // Примечание

        public string ToCsvLine()
        {
            return string.Join(";",
                EntranceNumber,
                ApartmentNumber,
                TotalArea.ToString(CultureInfo.InvariantCulture),
                LivingArea.ToString(CultureInfo.InvariantCulture),
                RoomsCount,
                OwnerLastName ?? string.Empty,
                RegistrationDate.ToString("yyyy-MM-dd"),
                FamilyMembersCount,
                ChildrenCount,
                HasDebt ? "1" : "0",
                Note ?? string.Empty
            );
        }

        public static ApartmentRecord FromCsvLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return null;
            var parts = line.Split(';');
            if (parts.Length < 11) return null;

            return new ApartmentRecord
            {
                EntranceNumber = int.Parse(parts[0]),
                ApartmentNumber = int.Parse(parts[1]),
                TotalArea = double.Parse(parts[2], CultureInfo.InvariantCulture),
                LivingArea = double.Parse(parts[3], CultureInfo.InvariantCulture),
                RoomsCount = int.Parse(parts[4]),
                OwnerLastName = parts[5],
                RegistrationDate = DateTime.Parse(parts[6]),
                FamilyMembersCount = int.Parse(parts[7]),
                ChildrenCount = int.Parse(parts[8]),
                HasDebt = parts[9] == "1" || parts[9].Equals("true", StringComparison.OrdinalIgnoreCase),
                Note = parts[10]
            };
        }
    }
}
