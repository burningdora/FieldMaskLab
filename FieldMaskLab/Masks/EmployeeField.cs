using System;

namespace FieldMaskLab.Masks
{
    // Каждому полю Employee соответствует один бит.
    // [Flags] позволяет комбинировать значения через | (OR) и читать их как список флагов.
    [Flags]
    public enum EmployeeField
    {
        None       = 0,
        Id         = 1 << 0, // 00001
        Name       = 1 << 1, // 00010
        Salary     = 1 << 2, // 00100
        Department = 1 << 3, // 01000
        IsActive   = 1 << 4, // 10000
        All = Id | Name | Salary | Department | IsActive
    }
}
