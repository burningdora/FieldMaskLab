using System;
using FieldMaskLab.Domain;
using FieldMaskLab.Masks;
using FieldMaskLab.Database;
using FieldMaskLab.Printing;

namespace FieldMaskLab
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var db = new EmployeeDatabase();
            db.Add(new Employee(1, "Alice", 3200.5f, DepartmentType.Engineering, true));
            db.Add(new Employee(2, "Bob", 2800f, DepartmentType.Sales, true));
            db.Add(new Employee(3, "Alice", 3200.5f, DepartmentType.Engineering, true)); // дубликат Alice(1) кроме Id
            db.Add(new Employee(4, "Carol", 4100f, DepartmentType.Finance, false));

            Console.WriteLine("=== 1-2. Domain model + абстракция БД ===");
            foreach (var e in db.GetAll())
                Console.WriteLine(e);

            Console.WriteLine("\n=== 3-5. Bool-маска + печать только выбранных полей ===");
            var boolMask = FieldMask.Of("Name", "Salary");
            foreach (var e in db.GetAll())
                EmployeePrinter.Print(e, boolMask);

            Console.WriteLine("\n=== 4. Поиск по полю: FindByName(\"Alice\") ===");
            var found = db.FindByName("Alice");
            Console.WriteLine($"Найдено {found.Count} сотрудник(ов):");
            foreach (var e in found) Console.WriteLine("  " + e);

            Console.WriteLine("\n=== Доп: битовая маска ===");
            var bitMask = new FieldMaskBit(EmployeeField.Name | EmployeeField.Department);
            foreach (var e in db.GetAll())
                EmployeePrinter.Print(e, bitMask);

            Console.WriteLine("\n=== Доп: комбинирование масок (Intersect/Union/Invert) ===");
            var maskA = new FieldMaskBit(EmployeeField.Name | EmployeeField.Salary);
            var maskB = new FieldMaskBit(EmployeeField.Salary | EmployeeField.Department);
            Console.WriteLine("A              = " + maskA);
            Console.WriteLine("B              = " + maskB);
            Console.WriteLine("Intersect(A,B) = " + FieldMaskBit.Intersect(maskA, maskB));
            Console.WriteLine("Union(A,B)     = " + FieldMaskBit.Union(maskA, maskB));
            Console.WriteLine("Invert(A)      = " + FieldMaskBit.Invert(maskA));

            Console.WriteLine("\n=== Доп: Merge дубликатов (равенство по Name+Salary+Department+IsActive) ===");
            var compareMask = new FieldMaskBit(
                EmployeeField.Name | EmployeeField.Salary | EmployeeField.Department | EmployeeField.IsActive);
            db.Merge(compareMask);
            Console.WriteLine($"После Merge осталось {db.GetAll().Count} записей:");
            foreach (var e in db.GetAll()) Console.WriteLine("  " + e);

            Console.WriteLine("\n=== Доп: CopyFields ===");
            var db2 = new EmployeeDatabase();
            var template = new Employee(100, "Dave", 5000f, DepartmentType.Engineering, true);
            var target1 = new Employee(101, "Dave", 3000f, DepartmentType.Engineering, false); // совпадает по Name+Department
            var target2 = new Employee(102, "Eve", 3000f, DepartmentType.Marketing, false);     // не совпадает
            db2.Add(template);
            db2.Add(target1);
            db2.Add(target2);

            var compareByNameDept = new FieldMaskBit(EmployeeField.Name | EmployeeField.Department);
            var copySalaryActive = new FieldMaskBit(EmployeeField.Salary | EmployeeField.IsActive);
            db2.CopyFields(template, compareByNameDept, copySalaryActive);

            Console.WriteLine("После CopyFields (у target1 должны обновиться Salary и IsActive, у target2 — ничего):");
            foreach (var e in db2.GetAll()) Console.WriteLine("  " + e);
        }
    }
}
