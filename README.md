ngval
=====

ASP.NET MVC validation for AngularJS.

## Getting Started
1. Add data anotations to your entities
```c#
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
```
