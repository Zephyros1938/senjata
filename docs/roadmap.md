# Senjata Roadmap

## ECS & Engine

- [x] Archetype implementation
- [x] Windowing & OpenGL
- [x] UBO support
- [ ] Fix component mutability issues (use `ref` and `Span<>`)
- [ ] Add entity and component removal support to the ECS
- [ ] Add query caching or archetype indexing to improve query perf

## Graphics & Rendering

- [x] Perspective & Cameras
- [ ] Model loading system
- [ ] Texture & material management (Diffuse, Normal, Specular)
- [ ] Basic lighting system

## Input & Utils for game Engine

- [ ] Mouse input support
- [ ] Deltatime smoothing & fixed timestep for the update loop
- [ ] Resource Manager / Asset Cache (Shader, Texture, Mesh)
- [ ] Log system

## Tooling

- [ ] Implement ImGui
- [ ] Debug renderers
- [ ] Scene serialization (JSON/XML/Binary)
