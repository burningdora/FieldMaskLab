namespace FieldMaskLab.Masks
{
    // === Доп. задание: маска на основе битов ===
    // "Более мощная абстракция на основе абстракции": вместо N bool-полей — одно целое число,
    // где каждый бит = одно поле Employee. Это даёт бесплатно быстрые операции
    // пересечения/объединения/инверсии через побитовые &, |, ~.
    public readonly struct FieldMaskBit
    {
        public EmployeeField Value { get; }

        public FieldMaskBit(EmployeeField value) => Value = value;

        public static FieldMaskBit None => new FieldMaskBit(EmployeeField.None);
        public static FieldMaskBit All => new FieldMaskBit(EmployeeField.All);

        // Проверка, что конкретное поле включено в маску.
        public bool Has(EmployeeField field) => (Value & field) == field;

        public FieldMaskBit With(EmployeeField field) => new FieldMaskBit(Value | field);
        public FieldMaskBit Without(EmployeeField field) => new FieldMaskBit(Value & ~field);

        // === Доп. задание: 3 метода, комбинирующие маски ===

        // 1. Пересечение — поля, присутствующие в ОБЕИХ масках одновременно.
        //    Полезно, например, чтобы узнать: "какие поля точно можно сравнивать/копировать
        //    и в контексте задачи A, и в контексте задачи B".
        public static FieldMaskBit Intersect(FieldMaskBit a, FieldMaskBit b)
            => new FieldMaskBit(a.Value & b.Value);

        // 2. Объединение — поля, присутствующие хотя бы в одной из масок.
        //    Полезно, чтобы объединить "что попросил клиент 1" и "что попросил клиент 2"
        //    в единый набор полей для одного запроса к БД.
        public static FieldMaskBit Union(FieldMaskBit a, FieldMaskBit b)
            => new FieldMaskBit(a.Value | b.Value);

        // 3. Инверсия — все поля, которых НЕТ в данной маске (дополнение до All).
        //    Полезно, например, для маски "все поля, кроме тех, что уже показали клиенту".
        public static FieldMaskBit Invert(FieldMaskBit a)
            => new FieldMaskBit(~a.Value & EmployeeField.All);

        public override string ToString() => Value.ToString();
    }
}
