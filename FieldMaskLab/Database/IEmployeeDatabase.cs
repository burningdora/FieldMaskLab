using System.Collections.Generic;
using FieldMaskLab.Domain;
using FieldMaskLab.Masks;

namespace FieldMaskLab.Database
{
    // === Пункт 2: абстракция базы данных ===
    // Клиентский код зависит от интерфейса, а не от конкретного хранилища
    // (список в памяти, файл, реальная БД — реализацию можно подменить не трогая клиента).
    public interface IEmployeeDatabase
    {
        void Add(Employee employee);
        IReadOnlyList<Employee> GetAll();

        // Пункт 4: поиск по одному полю.
        List<Employee> FindByName(string name);
        List<Employee> FindByField(EmployeeField field, object value);

        // Доп. задание: merge объектов, равных по маске.
        void Merge(FieldMaskBit compareMask);

        // Доп. задание: копирование полей (по copyMask) у объектов, равных source по compareMask.
        void CopyFields(Employee source, FieldMaskBit compareMask, FieldMaskBit copyMask);
    }
}
