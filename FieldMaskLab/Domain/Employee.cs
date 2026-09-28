namespace FieldMaskLab.Domain
{
    // === Пункт 1: Domain Model ===
    // Инкапсуляция: поля закрыты за public-свойствами (get/set), а не публичными полями,
    // конструктор гарантирует, что объект всегда создаётся в согласованном состоянии.
    // Содержит все требуемые типы: int, string, float, enum (+ bool для полноты).
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Salary { get; set; }
        public DepartmentType Department { get; set; }
        public bool IsActive { get; set; }

        public Employee() { }

        public Employee(int id, string name, float salary, DepartmentType department, bool isActive)
        {
            Id = id;
            Name = name;
            Salary = salary;
            Department = department;
            IsActive = isActive;
        }

        public Employee Clone()
        {
            return new Employee(Id, Name, Salary, Department, IsActive);
        }

        public override string ToString()
        {
            return $"Employee(Id={Id}, Name={Name}, Salary={Salary}, Department={Department}, IsActive={IsActive})";
        }
    }
}
