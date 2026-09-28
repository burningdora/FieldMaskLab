# Field Mask Lab

Структура:

```
FieldMaskLab/
├── FieldMaskLab/                 # основной проект (консольное приложение)
│   ├── Domain/
│   │   ├── DepartmentType.cs     # enum
│   │   └── Employee.cs           # Domain Model (int, string, float, enum, bool)
│   ├── Masks/
│   │   ├── FieldMask.cs          # маска на bool-ах (базовое задание)
│   │   ├── EmployeeField.cs      # [Flags] enum — по одному биту на поле
│   │   └── FieldMaskBit.cs       # маска на битах + Intersect/Union/Invert
│   ├── Database/
│   │   ├── IEmployeeDatabase.cs  # абстракция БД
│   │   └── EmployeeDatabase.cs   # реализация поверх List<Employee>: FindByField, Merge, CopyFields
│   ├── Printing/
│   │   └── EmployeePrinter.cs    # статическая печать по маске
│   └── Program.cs                # демонстрация всех пунктов задания
└── FieldMaskLab.Tests/            # xUnit-тесты
    ├── EmployeeDatabaseTests.cs
    └── FieldMaskBitTests.cs
```






