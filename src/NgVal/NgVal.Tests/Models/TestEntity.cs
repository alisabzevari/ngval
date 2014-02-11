using System.ComponentModel.DataAnnotations;

namespace NgVal.Tests.Models
{
    public class TestEntity
    {
        [Required]
        public string RequiredProperty { get; set; }

        [StringLength(10)]
        public string Length10Property { get; set; }

        [Required]
        [RegularExpression("\\d")]
        public string MultipleValidationProperty { get; set; }
    }
}