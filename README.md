# UnionFind (Disjoint Set) in C#

A simple implementation of the UnionFind data structure and operations. 

# Usage

Reference the NuGet package in the `.csproj`:
```xml
  <ItemGroup>
    <PackageReference Include="Hilke.DataStructures.UnionFind" Version="0.1.1" />
  </ItemGroup
```

Create an instance of the `UnionFind<TElement>` data structure:
```csharp
var sets = new UnionFind<int>(new [] {0, 1, 2, 3, 4, 5});

for (var i = 0; i < 3; ++i)
{ 
    sets.Union(0, 2 * i); // merge all the even numbers with the {0} set
    sets.Union(1, 2 * i + 1); // merge all the odd numbers with the {1} set
}
```
