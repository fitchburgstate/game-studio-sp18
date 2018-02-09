# Game Studio Spring 2018 Coding Style Guide

Modified from https://github.com/MakingSense/code-style-guides/blob/master/CSharp/README.md

## Table of Contents

1. [General](#general)
1. [Layout](#layout)
1. [Spacing](#spacing)
1. [Ordering](#ordering)
1. [Naming](#naming)
1. [Unity Specific](#unity-specific)

## General

### Only one public class per .cs file
Feel free to include as many internal classes to that public class as you want. However, there should only be one main public classs per file.

### All classes and enums should be in a namespace

Enums can be both in the scope of a class and a namespace. We prefer to put it in the scope of the namespace for ease of access.

```csharp
namespace GameStudio
{

    public enum GameState
    {
        Idle,
        Ready,
        Playing,
        Game Over
    }

    public class GameManager : Monobehaviour
    {
        //...
    }
}
```

### Use var instead of explicit types

Tool configuration:
[EditorConfig](https://docs.microsoft.com/en-us/visualstudio/ide/editorconfig-code-style-settings-reference#var)

Exceptions are allowed. This rule is mostly to cut down on redundancy when constructing new objects. IDEs (Visual Studio) also have features where you are able to mouse over a variable to learn its type, even if it is declared with var. Feel free to still use int, string, float or any built in language types if you want. 

```csharp
// Bad
GameObject nextEnemy = new GameObject();

// Good (We already see GameObject being decalred as the type on the right.)
var nextEnemy = new GameObject();

// Allowed for using an interface instead of a class
IUserService userService = new UserService();
```

```csharp
// Good
int i = 0;
string menuText = "Next?";

var j = 0;
var menuButtonText = "Cancel";
```

### Don't use explicit *this* reference

Tool configuration:
[EditorConfig](https://docs.microsoft.com/en-us/visualstudio/ide/editorconfig-code-style-settings-reference#this_and_me)

It is redundant and does not add for readability.

```csharp
// Bad
this.ValidateParameters();

// Good
ValidateParameters();
```

### Use language built-in alias instead of class names

Tool configuration:
[EditorConfig](https://docs.microsoft.com/en-us/visualstudio/ide/editorconfig-code-style-settings-reference#language_keywords)

```csharp
// Bad
String.IsNullOrEmpty(name);

// Good
string.IsNullOrEmpty(name);
```

### Use string interpolation over *string.Format*

This is a feature that was released in a version of C# that Unity was only recently upgraded to be able to use (.NET 4.6). You use string interpolation by starting adding a '$' character before the first quotation mark in a string. You are then free to add variables and logic throughout the string using curly brackets.

```csharp
// Bad
var message = string.Format("My name is {0} {1}.", person.FirstName, person.LastName);

// Good
var message = $"My name is {person.FirstName} {person.LastName}.";
```

### 

## Layout

### Use *four spaces* per indentation level

Tool configuration:
[EditorConfig](http://editorconfig.org/#file-format-details)

You can enable the "View White Space" option and use CTRL+R/CTRL+W keyboard shortcuts.

```csharp
// Bad
public void SomeMethod()
{
∙∙DoSomethingFirst();
∙∙DoSomethingLater();
}

// Good
public void SomeMethod()
{
∙∙∙∙DoSomethingFirst();
∙∙∙∙DoSomethingLater();
}
```

### Don't use more than one empty line in a row

Whitespace is good. Too much is bad.

```csharp
// Bad
public void SomeMethod()
{
    DoSomethingFirst();
    DoSomethingLater();


    DoSomethingAtTheEnd();
}

// Good
public void SomeMethod()
{
    DoSomethingFirst();
    DoSomethingLater();

    DoSomethingAtTheEnd();
}
```

### Remove unused or redundant *using* statements (or directives)

They are usually grayed out and suggested to be removed anyways.

### Use single line and lambda getters for simple methods and read-only properties

Tool configuration:
[EditorConfig](https://docs.microsoft.com/en-us/visualstudio/ide/editorconfig-code-style-settings-reference#expression_bodied_members_properties)

```csharp
// Good
public string Name { get; private set; }

// Good
public string UppercaseName => Name.toUpperCase(); 
```

### Curly braces for multi line statements must not share line

Curly brackets always get their own line.

```csharp
// Bad
public void SomeMethod() {
    // ...
}

// Good
public void SomeMethod()
{
    // ...
}
```

### Opening or ending curly braces must not be followed/preceded by a blank line

Again, a whitespace thing.

```csharp
// Bad
public void SomeMethod() 
{

    DoSomethingFirst();
    DoSomethingLater();

}

// Good
public void SomeMethod()
{
    DoSomethingFirst();
    DoSomethingLater();
}
```

### Always use brackets, even on blocks with only one line.

This makes your blocks consistent and easy to read. You can still use one line statements, just include the brackets anyways incase it needs to be added onto later.

```csharp
// Bad
if (payment == null)
    return "A payment is required.";

// Good
if (payment == null)
{
    return "A payment is required.";
}

// Also Good
if (payment == null) { return "A payment is required."; }
```

## Spacing

### Commas must be spaced correctly

A comma should be followed by a single space and never be preceded by any whitespace.

```csharp
// Bad
var result = Calculate(3 , 5);
var result = Calculate(3,5);

// Good
var result = Calculate(3, 5);
```

### Symbols must be spaced correctly

An operator symbol must be surrounded by a single space on either side.

```csharp
// Bad
var result = 5+3;
var result = 5 +3;
var result = 5+ 3;

// Good
var result = 5 + 3;
```

Except for unary operators:

```csharp
// Bad
var result = ! toggle;

// Good
var result = !toggle;
```

### Opening parenthesis should not be followed by a space

```csharp
// Bad
var result = Calculate( 3, 5);

// Good
var result = Calculate(3, 5);
```

### Closing parenthesis should not be preceded by a space

```csharp
// Bad
var result = Calculate(3, 5 );

// Good
var result = Calculate(3, 5);
```

### Opening square brackets should not be preceded or followed by a space

```csharp
// Bad
var element = myArray [index];
var element = myArray[ index];

// Good
var element = myArray[index];
```

### Closing square brackets should not be preceded or followed by a space

```csharp
// Bad
var element = myArray[index ];
var element = myArray[index] ;

// Good
var element = myArray[index];
```

### Opening curly brackets should be preceded and followed by a space

```csharp
// Bad
var names = new string[] {"Marie", "John", "Paul"};

// Good
var names = new string[] { "Marie", "John", "Paul" };
```

### Closing curly brackets should be preceded and followed by a space

Unless it's followed by a semicolon, in which case it's only preceded.

```csharp
// Bad
var names = new string[] { "Marie", "John", "Paul"};

// Good
var names = new string[] { "Marie", "John", "Paul" };
```

## Ordering

### Within a class, struct, or interface, elements should be ordered

1. Constants
1. Fields
1. Constructors
1. Delegates
1. Events
1. Properties
1. Methods
1. Finalizers (Destructors)

### Elements should be ordered by access


1. `public`
1. `internal`
1. `protected internal`
1. `protected`
1. `private`

### Follow certain order when using declaration keywords

1. Access modifiers
1. `static`
1. All other keywords

```csharp
// Bad
override public static int Area()
{
    // ...
}

// Good
public static override int Area()
{
    // ...
}
```

### Order *using* directives alphabetically


```csharp
// Bad
using System;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

// Good
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
```

## Naming

### Be descriptive with your naming, avoid abbreviations and single letter names

```csharp
// Bad
var go = new GameObject();

// Good
var projectile = new GameObject();
```

```csharp
// Bad
var e = "test@test.com";

// Good
var email = "test@test.com";
```

### Use *PascalCase* for namespaces, classes, enums, structs, constants, delegates, events, methods and properties


```csharp
// Bad
public class file_reader
{
    // ...
}

// Good
public class FileReader
{
    // ...
}
```

```csharp
// Bad
public const int TIME_IN_SECONDS = 5;

// Good
public const int TimeInSeconds = 5;
```

```csharp
// Bad
public string secondAddress { get; set; }

// Good
public string SecondAddress { get; set; }
```

### Use *camelCase* for variables

```csharp
// Bad
var FileReader = new FileReader();
var file_reader = new FileReader();
var _fileReader = new FileReader();

// Good
var fileReader = new FileReader();
```

### Use *_underscoreCase* for private instance/static fields


```csharp
// Bad
private IUserService UserService;
private IUserService userService;
private IUserService user_service;

// Good
private IUserService _userService;
```

### Interface names must begin with "I"


```csharp
// Bad
public interface LoggerInterface
{
    // ...
}

// Good
public interface ILogger
{
    // ...
}
```

### Include *Async* suffix on async methods

```csharp
// Bad
public async Task<int> GetLatestPosition()
{
    // ...
}

// Good
public async Task<int> GetLatestPositionAsync()
{
    // ...
}
```

## Unity Specific

### Use attributes for inspector fields where at all possible

```csharp
using UnityEngine;

public class GameManager : MonoBehaviour 
{

    [Header("Debug Options")]
    [Range(1, 500)]
    public int enemyCount = 50;
    public bool respawnOnDeath = false;
//...
}
```

### Don't leave blank Unity Messages in your code

They still add overhead even though they don't contain anything!

```csharp
// Bad
void Start()
{

}

void Update()
{

}
```

### Use the Generic overload of a method if it exists

Most notable for GetComponent.

```csharp
// Bad
 HingeJoint hinge = gameObject.GetComponent(typeof(HingeJoint)) as HingeJoint;

// Good
 HingeJoint hinge = gameObject.GetComponent<HingeJoint>();
```

### Use Lists instead of Arrays

It is easier to add/remove items in Lists than it is to add/remove items in Arrays.

```csharp
// Bad
GameObject[] enemies;

// Good
List<GameObject> enemies = new List<GameObject>();
```
