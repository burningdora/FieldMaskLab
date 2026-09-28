using System;
using System.Collections.Generic;
using System.Linq;
using FieldMaskLab.Domain;
using FieldMaskLab.Masks;

namespace FieldMaskLab.Database
{
    // Простейшая реализация "БД": хранилище — обычный List<Employee> в памяти.
    // Имплементация нарочно наивная (задание прямо разрешает) — суть в контракте IEmployeeDatabase.
    public class EmployeeDatabase : IEmployeeDatabase
    {
        private readonly List<Employee> _storage = new List<Employee>();

        public void Add(Employee employee) => _storage.Add(employee);

        public IReadOnlyList<Employee> GetAll() => _storage.AsReadOnly();

        public List<Employee> FindByName(string name)
            => FindByField(EmployeeField.Name, name);

        // Пункт 4: универсальный поиск по одному полю (без reflection — явный switch,
        // чтобы было понятно и легко объяснить на защите).
        public List<Employee> FindByField(EmployeeField field, object value)
        {
            return _storage.Where(e => Equals(GetFieldValue(e, field), value)).ToList();
        }

        private static object GetFieldValue(Employee e, EmployeeField field)
        {
            switch (field)
            {
                case EmployeeField.Id: return e.Id;
                case EmployeeField.Name: return e.Name;
                case EmployeeField.Salary: return e.Salary;
                case EmployeeField.Department: return e.Department;
                case EmployeeField.IsActive: return e.IsActive;
                default:
                    throw new ArgumentException(
                        $"FindByField принимает ровно одно поле за раз, получено: {field}");
            }
        }

        // Сравнивает двух сотрудников только по полям, включённым в маску.
        // Это и есть смысл Field Mask: "равенство" не абсолютное, а "равенство по подмножеству полей".
        private static bool EqualsByMask(Employee a, Employee b, FieldMaskBit mask)
        {
            if (mask.Has(EmployeeField.Id) && a.Id != b.Id) return false;
            if (mask.Has(EmployeeField.Name) && a.Name != b.Name) return false;
            if (mask.Has(EmployeeField.Salary) && Math.Abs(a.Salary - b.Salary) > 0.0001f) return false;
            if (mask.Has(EmployeeField.Department) && a.Department != b.Department) return false;
            if (mask.Has(EmployeeField.IsActive) && a.IsActive != b.IsActive) return false;
            return true;
        }

        private static void CopyField(Employee from, Employee to, EmployeeField field)
        {
            switch (field)
            {
                case EmployeeField.Id: to.Id = from.Id; break;
                case EmployeeField.Name: to.Name = from.Name; break;
                case EmployeeField.Salary: to.Salary = from.Salary; break;
                case EmployeeField.Department: to.Department = from.Department; break;
                case EmployeeField.IsActive: to.IsActive = from.IsActive; break;
            }
        }

        // === Доп. задание: Merge ===
        // Схлопывает все объекты, которые "равны" друг другу по compareMask,
        // оставляя по одному представителю на группу.
        public void Merge(FieldMaskBit compareMask)
        {
            var result = new List<Employee>();
            foreach (var employee in _storage)
            {
                var existing = result.FirstOrDefault(r => EqualsByMask(r, employee, compareMask));
                if (existing == null)
                {
                    result.Add(employee); // новая группа — сотрудник становится её представителем
                }
                // иначе employee — дубликат по маске, "схлопываем", просто не добавляя его в result
            }
            _storage.Clear();
            _storage.AddRange(result);
        }

        // === Доп. задание: CopyFields ===
        // Для всех объектов в БД, которые "равны" source по compareMask,
        // копирует из source только поля, перечисленные в copyMask.
        public void CopyFields(Employee source, FieldMaskBit compareMask, FieldMaskBit copyMask)
        {
            foreach (var employee in _storage)
            {
                if (ReferenceEquals(employee, source)) continue;
                if (!EqualsByMask(employee, source, compareMask)) continue;

                foreach (EmployeeField field in Enum.GetValues(typeof(EmployeeField)))
                {
                    if (field == EmployeeField.None || field == EmployeeField.All) continue;
                    if (copyMask.Has(field))
                    {
                        CopyField(source, employee, field);
                    }
                }
            }
        }
    }
}
