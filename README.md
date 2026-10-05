# Library Management System (C# OOP Assignment)

A C# console application (.NET 10) that demonstrates: abstract classes, `IComparable<T>`, `ICloneable`,
static classes, sealed classes, partial classes, operator overloading, and the Singleton pattern.

## Project structure

| File | Purpose |
|------|---------|
| `LibraryItem.cs` | Part 1 - abstract base class |
| `Magazine.cs` | Part 1 - derived class |
| `Book.cs` | Parts 2, 3, 6, 7 - `Book` (partial, file 1): properties, constructor, `IComparable`, `ICloneable`, operators |
| `Book.Display.cs` | Part 6 - `Book` (partial, file 2): `DisplayInfo()`, `PrintBookDetails()` |
| `LibraryUtilities.cs` | Part 4 - static class |
| `LibraryCard.cs` | Part 5 - sealed class |
| `LibraryConfiguration.cs` | Part 8 - thread-safe Singleton |
| `Program.cs` | Part 9 - integration demo of everything |

Run with: `dotnet run`

---

## Part 1 - Abstract Class

**1. Why is `LibraryItem` abstract?**
It is a general concept, not a real item you can borrow. Only concrete items such as `Book` and `Magazine` exist. It holds the common data (Id, Title, Year) and forces every derived class to implement `DisplayInfo()`.

**2. Why can't we create `new LibraryItem();`?**
An abstract class is incomplete: it contains an abstract method with no body. If an object could be created, calling `DisplayInfo()` on it would have nothing to run, so the compiler forbids it.

**3. Abstract `DisplayInfo()` vs concrete `PrintBasicInfo()`?**
`DisplayInfo()` has no implementation in the base class; each derived class must `override` it with its own behavior (polymorphism). `PrintBasicInfo()` has a full implementation that all derived classes inherit and share as-is.

---

## Part 2 - IComparable<T>

`Book.CompareTo` compares by `Year`, and if the years are equal, by `Title` (ordinal comparison).
Sorted result: Design Patterns (1994), The Pragmatic Programmer (1999), C# in Depth (2008), Clean Code (2008).

**1. Negative value from `CompareTo()`?** The current object comes **before** the other one in sort order (it is "smaller").

**2. Zero?** Both objects are considered **equal** in sort order.

**3. Positive value?** The current object comes **after** the other one (it is "greater").

**4. Why does `List<Book>.Sort()` work without a separate comparison function?**
`Sort()` uses the default comparer, which looks for `IComparable<T>` on the element type. Because `Book` implements it, the list knows how to order two books by calling `CompareTo`.

---

## Part 3 - ICloneable

**1. Copying a reference vs cloning an object?**
Copying a reference creates a second variable pointing to the **same** object in memory. Cloning creates a **new, separate** object with the same values.

**2. What happens with `Book clone = original;`?**
No new object is created. Both variables refer to the same `Book`, so changing the title through one is visible through the other.

**3. Why does `ICloneable.Clone()` return `object`?**
`ICloneable` is a non-generic interface from the first version of .NET (before generics existed), so it can only return the common base type `object`. The caller must cast: `(Book)original.Clone()`.

**4. A limitation/criticism of `ICloneable`:**
It does not say whether `Clone()` is a **shallow** or **deep** copy, so callers cannot know if nested reference objects are shared or copied. Also, it returns `object` (no type safety, needs casting). Because of this, Microsoft recommends not using it in new public APIs.

---

## Part 4 - Static Class

**1. Why can't a static class be instantiated?**
It is meant to hold only static members that belong to the type itself, so creating an object would be meaningless. The compiler marks static classes as `abstract sealed` internally and has no instance constructor.

**2. When is a static class appropriate?**
For stateless helper/utility methods that do not need object data (math helpers, formatting, code generation, conversions).

**3. Static class vs normal class with static methods?**
A normal class with static methods can still be instantiated, inherited, and can have instance members. A static class guarantees at compile time that none of this is possible and that **every** member is static.

---

## Part 5 - Sealed Class

`public class PremiumLibraryCard : LibraryCard { }` is invalid because `LibraryCard` is `sealed`; the compiler reports error CS0509 (cannot derive from sealed type).

**1. What does `sealed` mean?** The class cannot be inherited.

**2. Why prevent inheritance on purpose?** To protect the class's behavior and invariants from being changed by subclasses, for security/design safety (e.g. a card's data should not be altered by derived types), and sometimes for slight performance benefits.

**3. Can a sealed class inherit from another class?** Yes. `sealed` only stops others from inheriting **from it**; it can still have its own base class.

**4. Can a sealed class implement an interface?** Yes.

---

## Part 6 - Partial Class

`Book` is split into `Book.cs` (properties, constructor, `IComparable`, plus `ICloneable` and operators) and `Book.Display.cs` (`DisplayInfo()`, `PrintBookDetails()`). Both declare `public partial class Book`.

**1. What does `partial` do?** It lets the definition of one class be split across multiple files; the compiler merges them at compile time.

**2. Does it create two different `Book` classes?** No. It is one single class.

**3. Effect on the runtime type?** None. `typeof(Book)` is a single type; partial only exists at compile time.

**4. Real-world use:** Auto-generated code (Windows Forms designer, Entity Framework models, source generators) lives in one file while the developer's own code lives in another, so regenerating never overwrites hand-written code. Also useful for letting several developers work on a large class.

---

## Part 7 - Operator Overloading

`>` and `<` compare `PageCount` (they must be overloaded together). `+` returns a new `Book`.

**Decision for the combined book (`book1 + book2`):**
- `PageCount` = sum of both (as required).
- `Title` = `"Title1 + Title2"` (shows what it was made from).
- `Year` = the later (max) year of the two books.
- `Author` = the same author if both are equal, otherwise `"Author1 & Author2"`.
- `Id` = `0`, because the combined book is a temporary object not yet stored in the library.

**1. Why can operators be overloaded in C#?** The language allows a type to define `public static operator` methods, so the compiler translates `a + b` into a call to that method.

**2. Why is it useful?** It lets custom types be used with natural, readable syntax (`book1 > book2`) instead of verbose method calls.

**3. When does it make code harder to understand?** When the operator's meaning is not obvious (e.g. `book1 + book2` could mean many things), or when it hides expensive work or has surprising side effects.

**4. Why should overloaded operators behave intuitively?** Readers assume `>` means "greater" and `+` means "combine". If the behavior differs from that expectation, the code becomes misleading and bug-prone.

---

## Part 8 - Singleton

Implementation: `sealed` class, `private` constructor, a `private static readonly Lazy<LibraryConfiguration>` created with `LazyThreadSafetyMode.ExecutionAndPublication`, exposed through the `static Instance` property.

**1. Why is the constructor private?** So no outside code can call `new LibraryConfiguration()`; only the class itself can create its instance.

**2. Why is the instance static?** A static member belongs to the class, not to an object, so it can be reached globally (`LibraryConfiguration.Instance`) without already having an object.

**3. Why is the class sealed?** To prevent a derived class from being created that could bypass or break the single-instance guarantee (e.g. by adding a public constructor).

**4. How is a single instance guaranteed?** The constructor is private, so only the static `Lazy<T>` field can create it, and that field is `static readonly`, so it exists once per application and its factory runs only once.

**5. Behavior with multiple threads?** `Lazy<T>` with `ExecutionAndPublication` makes the runtime lock internally so only one thread runs the factory; other threads wait and then receive the same object. `Program.cs` verifies this with `Parallel.For` (100 parallel requests all return the same instance).

---

## Part 9 - Integration

`Program.cs` demonstrates all eight concepts in order, with printed output for each section.
