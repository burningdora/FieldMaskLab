using System;
using FieldMaskLab.Domain;
using FieldMaskLab.Masks;

namespace FieldMaskLab.Printing
{
    // === Пункт 5: статическая функция печати по маске ===
    public static class EmployeePrinter
    {
        // Версия для битовой маски.
        public static void Print(Employee e, FieldMaskBit mask)
        {
            Console.Write("Employee {");
            bool first = true;

            void PrintField(string label, object value)
            {
                if (!first) Console.Write(", ");
                Console.Write($"{label}={value}");
                first = false;
            }

            if (mask.Has(EmployeeField.Id)) PrintField("Id", e.Id);
            if (mask.Has(EmployeeField.Name)) PrintField("Name", e.Name);
            if (mask.Has(EmployeeField.Salary)) PrintField("Salary", e.Salary);
            if (mask.Has(EmployeeField.Department)) PrintField("Department", e.Department);
            if (mask.Has(EmployeeField.IsActive)) PrintField("IsActive", e.IsActive);

            Console.WriteLine("}");
        }

        // Версия для bool-маски (основное задание).
        public static void Print(Employee e, FieldMask mask)
        {
            Console.Write("Employee {");
            bool first = true;

            void PrintField(string label, object value)
            {
                if (!first) Console.Write(", ");
                Console.Write($"{label}={value}");
                first = false;
            }

            if (mask.Id) PrintField("Id", e.Id);
            if (mask.Name) PrintField("Name", e.Name);
            if (mask.Salary) PrintField("Salary", e.Salary);
            if (mask.Department) PrintField("Department", e.Department);
            if (mask.IsActive) PrintField("IsActive", e.IsActive);

            Console.WriteLine("}");
        }
    }
}
