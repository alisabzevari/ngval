ngval
=====

ASP.NET MVC validation for AngularJS.
## Getting Started
1.Add data anotations to your entities

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
2.Reference ngval.js to your page

```html
<script src="~/Scripts/ngval.js"></script>
```

3.Add ngval module to your app module dependencies

```javascript
var App = angular.module('App', ['ngval']);
```

4.Use NgValFor Html helper method to insert angularjs directives for validation. It will also add native angularjs directives. And you can use ngval object for error messages and more.

```html
<form name="testForm" novalidate ng-submit="submit()">
    <input type="text" name="username" ng-model="user.name" @Html.NgValFor(u => u.RequiredProperty) />
    {{testForm.username.ngval.hasError}}
    <div ng-repeat="err in testForm.username.ngval.errors">
        <span>{{err.message}}</span>
        <br />
    </div>
    <input type="submit" />
</form>
```
