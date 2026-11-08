using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tyuiu.KAV.Sprint7.Project.V7.Lib
{
    public class DataService
    {
        public const string CsvHeader = "Entrance;Apartment;TotalArea;LivingArea;Rooms;Owner;RegDate;FamilyMembers;Children;HasDebt;Note";

        public List<ApartmentRecord> LoadFromFile(string filePath)
        {
            var list = new List<ApartmentRecord>();
            if (!File.Exists(filePath)) return list;

            var lines = File.ReadAllLines(filePath, Encoding.UTF8);
            for (int i = 0; i < lines.Length; i++)
            {
                if (i == 0 && lines[i].StartsWith("Entrance")) continue;
                var record = ApartmentRecord.FromCsvLine(lines[i]);
                if (record != null) list.Add(record);
            }
            return list;
        }

        public void SaveToFile(string filePath, IEnumerable<ApartmentRecord> records)
        {
            var sb = new StringBuilder();
            sb.AppendLine(CsvHeader);
            foreach (var r in records)
            {
                sb.AppendLine(r.ToCsvLine());
            }
            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        public int GetTotalApartments(IEnumerable<ApartmentRecord> list) => list?.Count() ?? 0;

        public double GetSumTotalArea(IEnumerable<ApartmentRecord> list) =>
            list == null || !list.Any() ? 0 : Math.Round(list.Sum(x => x.TotalArea), 2);

        public double GetAverageTotalArea(IEnumerable<ApartmentRecord> list) =>
            list == null || !list.Any() ? 0 : Math.Round(list.Average(x => x.TotalArea), 2);

        public double GetMinTotalArea(IEnumerable<ApartmentRecord> list) =>
            list == null || !list.Any() ? 0 : Math.Round(list.Min(x => x.TotalArea), 2);

        public double GetMaxTotalArea(IEnumerable<ApartmentRecord> list) =>
            list == null || !list.Any() ? 0 : Math.Round(list.Max(x => x.TotalArea), 2);

        public int GetTotalResidents(IEnumerable<ApartmentRecord> list) =>
            list == null || !list.Any() ? 0 : list.Sum(x => x.FamilyMembersCount);

        public int GetDebtorsCount(IEnumerable<ApartmentRecord> list) =>
            list == null || !list.Any() ? 0 : list.Count(x => x.HasDebt);
    }
}
