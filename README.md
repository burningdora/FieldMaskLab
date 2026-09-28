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

## Как запустить (нужен установленный .NET 8 SDK)

Собрать solution и создать связи между проектами (один раз):

```bash
cd FieldMaskLab
dotnet new sln -n FieldMaskLab
dotnet sln add FieldMaskLab/FieldMaskLab.csproj
dotnet sln add FieldMaskLab.Tests/FieldMaskLab.Tests.csproj
```

Запустить демо-программу:

```bash
dotnet run --project FieldMaskLab
```

Запустить unit-тесты:

```bash
dotnet test FieldMaskLab.Tests
```

## Важное примечание

Этот код написан и вычитан вручную (в среде без установленного .NET SDK, компиляция
не проверялась автоматически). Синтаксис стандартный и простой (C# 8+/.NET 8), но
перед защитой обязательно прогоните `dotnet build` и `dotnet test` сами — и пробегитесь
по коду, чтобы уверенно его объяснять (см. подробности в чате).
