using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NgVal.Tests.Models
{
    public class TestEntity
    {
        [Required]
        public string RequiredProperty { get; set; }

        [StringLength(10)]
        public string Length10Property { get; set; }

        [Required]
        [StringLength(10)]
        public string MultipleValidationProperty { get; set; }
    }
}