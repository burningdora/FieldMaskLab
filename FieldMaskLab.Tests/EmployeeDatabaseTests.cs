using FieldMaskLab.Domain;
using FieldMaskLab.Masks;
using FieldMaskLab.Database;
using Xunit;

namespace FieldMaskLab.Tests
{
    public class EmployeeDatabaseTests
    {
        private static EmployeeDatabase BuildSampleDb()
        {
            var db = new EmployeeDatabase();
            db.Add(new Employee(1, "Alice", 3200.5f, DepartmentType.Engineering, true));
            db.Add(new Employee(2, "Bob", 2800f, DepartmentType.Sales, true));
            db.Add(new Employee(3, "Alice", 3200.5f, DepartmentType.Engineering, true));
            db.Add(new Employee(4, "Carol", 4100f, DepartmentType.Finance, false));
            return db;
        }

        [Fact]
        public void FindByName_ReturnsAllMatches()
        {
            var db = BuildSampleDb();
            var result = db.FindByName("Alice");
            Assert.Equal(2, result.Count);
            Assert.All(result, e => Assert.Equal("Alice", e.Name));
        }

        [Fact]
        public void FindByField_WorksForNonStringField()
        {
            var db = BuildSampleDb();
            var result = db.FindByField(EmployeeField.Department, DepartmentType.Engineering);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Merge_CollapsesDuplicatesByMask()
        {
            var db = BuildSampleDb();
            var mask = new FieldMaskBit(
                EmployeeField.Name | EmployeeField.Salary | EmployeeField.Department | EmployeeField.IsActive);

            db.Merge(mask);

            // Alice(1) и Alice(3) совпадают по Name+Salary+Department+IsActive (Id в маску не входит) -> схлопнутся в одну
            Assert.Equal(3, db.GetAll().Count);
        }

        [Fact]
        public void Merge_KeepsDistinctWhenMaskIncludesId()
        {
            var db = BuildSampleDb();
            var mask = FieldMaskBit.All; // Id уникален у всех -> ничего не схлопнётся

            db.Merge(mask);

            Assert.Equal(4, db.GetAll().Count);
        }

        [Fact]
        public void CopyFields_OnlyAffectsMatchingEmployeesAndMaskedFields()
        {
            var db = new EmployeeDatabase();
            var template = new Employee(100, "Dave", 5000f, DepartmentType.Engineering, true);
            var matching = new Employee(101, "Dave", 3000f, DepartmentType.Engineering, false);
            var nonMatching = new Employee(102, "Eve", 3000f, DepartmentType.Marketing, false);

            db.Add(template);
            db.Add(matching);
            db.Add(nonMatching);

            var compareMask = new FieldMaskBit(EmployeeField.Name | EmployeeField.Department);
            var copyMask = new FieldMaskBit(EmployeeField.Salary | EmployeeField.IsActive);

            db.CopyFields(template, compareMask, copyMask);

            Assert.Equal(5000f, matching.Salary);
            Assert.True(matching.IsActive);

            // Не подошедший по маске сравнения объект не должен измениться
            Assert.Equal(3000f, nonMatching.Salary);
            Assert.False(nonMatching.IsActive);

            // Поля, не входящие в copyMask, копироваться не должны
            Assert.Equal(101, matching.Id);
            Assert.Equal("Dave", matching.Name);
        }
    }
}
