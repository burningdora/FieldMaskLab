using FieldMaskLab.Masks;
using Xunit;

namespace FieldMaskLab.Tests
{
    public class FieldMaskBitTests
    {
        [Fact]
        public void Has_ReturnsTrueOnlyForIncludedFields()
        {
            var mask = new FieldMaskBit(EmployeeField.Name | EmployeeField.Salary);

            Assert.True(mask.Has(EmployeeField.Name));
            Assert.True(mask.Has(EmployeeField.Salary));
            Assert.False(mask.Has(EmployeeField.Id));
            Assert.False(mask.Has(EmployeeField.Department));
        }

        [Fact]
        public void Intersect_ReturnsOnlyCommonFields()
        {
            var a = new FieldMaskBit(EmployeeField.Name | EmployeeField.Salary);
            var b = new FieldMaskBit(EmployeeField.Salary | EmployeeField.Department);

            var result = FieldMaskBit.Intersect(a, b);

            Assert.True(result.Has(EmployeeField.Salary));
            Assert.False(result.Has(EmployeeField.Name));
            Assert.False(result.Has(EmployeeField.Department));
        }

        [Fact]
        public void Union_ReturnsAllFieldsFromBoth()
        {
            var a = new FieldMaskBit(EmployeeField.Name);
            var b = new FieldMaskBit(EmployeeField.Salary);

            var result = FieldMaskBit.Union(a, b);

            Assert.True(result.Has(EmployeeField.Name));
            Assert.True(result.Has(EmployeeField.Salary));
        }

        [Fact]
        public void Invert_ReturnsComplementWithinAll()
        {
            var a = new FieldMaskBit(EmployeeField.Name | EmployeeField.Salary);

            var result = FieldMaskBit.Invert(a);

            Assert.False(result.Has(EmployeeField.Name));
            Assert.False(result.Has(EmployeeField.Salary));
            Assert.True(result.Has(EmployeeField.Id));
            Assert.True(result.Has(EmployeeField.Department));
            Assert.True(result.Has(EmployeeField.IsActive));
        }
    }
}
