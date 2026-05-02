# NexusCore Agent Guide

## Purpose
- Use this file as the default project guide for creating, updating, and organizing code in this repository.
- Prefer extending existing patterns over introducing parallel structures.

## Solution Layout
- `src/Services/*` contains shared cross-cutting libraries used by multiple services.
- `src/AuthService/*` contains the auth bounded context.
- `src/NexusCore` is the main application host project.
- `src/NexusCore.AppHost` contains Aspire orchestration and local infrastructure wiring.
- `src/NexusCore.ServiceDefaults` contains shared hosting, telemetry, resilience, and health-check defaults.

## Layering Rules
- `*.Domain` contains entities, enums, and domain-level models.
- `*.Application` contains abstractions, contracts, request/response models, and use-case-facing interfaces.
- `*.Infrastructure.Persistence` contains EF Core `DbContext`, configurations, write/read services, interceptors, and persistence-specific wiring.
- `*.Infrastructure.Identity` contains identity and token-related infrastructure.
- `Services/Messaging` contains shared messaging abstractions and transport implementations.
- `Services/Serialization` contains serializer implementations used by infrastructure such as messaging.

## Persistence Conventions
- Put EF Core entity configurations under `Configurations/`.
- Put save pipeline hooks, auditing, and cross-cutting write behavior under `Interceptors/`.
- Prefer central EF Core interception for cross-cutting write concerns instead of duplicating logic in each write service.
- Keep `DbContext` focused on sets, model configuration, and conventions.
- Register persistence services and interceptors through a service collection extension instead of wiring them ad hoc in app startup.

## Messaging Conventions
- Reuse `IMessageBus` for asynchronous integration events.
- Prefer stable event contracts with explicit names and serializable primitive payloads.
- Place shared or persistence-driven event contracts in a nearby `Messaging/` folder within the owning project when they are feature-specific.
- Use topic publishing for broadcast-style write events unless a point-to-point queue is explicitly needed.

## Code Organization Preferences
- Prefer small, focused service registration extensions for each infrastructure area.
- Keep feature write logic inside infrastructure services such as `RoleWriteService`; keep controllers/endpoints thin.
- Favor consistent naming: `*WriteService`, `*ReadService`, `*Interceptor`, `*Configuration`, `*Extensions`.
- When adding new bounded contexts, mirror the existing `Domain` / `Application` / `Infrastructure.*` split.

## Update Rules
- Before creating a new folder, check whether an existing project already owns that responsibility.
- Do not move or rename major folders unless the task explicitly requires a structural refactor.
- Preserve project references and shared abstractions unless there is a clear architectural reason to change them.

## Verification
- Prefer targeted builds for the affected project first, then broader solution validation when environment constraints allow it.
- If sandbox or network restrictions block restore/build, document that clearly in the final handoff.
