namespace FieldMaskLab.Masks
{
    // === Пункт 3 (базовая версия): маска как коллекция bool-ов ===
    // Одно bool-поле на каждое поле Employee. Просто, наглядно, но плохо масштабируется:
    // при 20 полях в Employee пришлось бы городить 20 bool-свойств здесь же.
    public class FieldMask
    {
        public bool Id { get; set; }
        public bool Name { get; set; }
        public bool Salary { get; set; }
        public bool Department { get; set; }
        public bool IsActive { get; set; }

        public static FieldMask None() => new FieldMask();

        public static FieldMask All() => new FieldMask
        {
            Id = true,
            Name = true,
            Salary = true,
            Department = true,
            IsActive = true
        };

        // Удобный фабричный метод: FieldMask.Of("Name", "Salary")
        public static FieldMask Of(params string[] fieldNames)
        {
            var mask = None();
            foreach (var f in fieldNames)
            {
                switch (f.ToLowerInvariant())
                {
                    case "id": mask.Id = true; break;
                    case "name": mask.Name = true; break;
                    case "salary": mask.Salary = true; break;
                    case "department": mask.Department = true; break;
                    case "isactive": mask.IsActive = true; break;
                }
            }
            return mask;
        }
    }
}
