# Design Philosophy & Architectural Guidelines

## Structure

- **ECS First:** Essentially everything should exist in a `Senjata::ECS::Scene`. Logic should be in only systems
- **Cache Locality:** Keep components as small as possible, only `struct` types (`Senjata::ECS::IComponent`), and avoid placing reference types (`class`, `string`) inside of components to keep linear memory layout and less overhead for the garbage collector
- **No alloc in gameloop:** Refrain from instantiating objects or allocating arrays within `Senjata::Program::Update()` or `Senjata::Program::Render()` calls

## ECS Standards

- **Archetype mutability:** Systems should access component arrays via `Span<T>`s or `ref`s to avoid mutating local copies of components
- **General Mutability:** If a system only reads data, the data it reads should be contained within a `ReadOnlySpan<T>` instead of a `Span<T>` or `ref` to prevent accidental throwaway writes

## Graphics Standards

- **UBO Layouts:** Every uniform buffer object must have a `[StructLayout(LayoutKind.Explicit)]` struct to ensure that they match the GLSL block alignments as specified in `std140`
