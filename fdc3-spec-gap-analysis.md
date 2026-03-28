# FDC3 .NET API Gap Analysis vs. `finos/fdc3` main branch

> Generated: 2026-03-28
> Source: https://github.com/finos/fdc3 (main branch, `packages/fdc3-standard/src/api/`)

---

## 1. NEW IN FINOS/FDC3 API AND CONTEXTS

These items represent additions or changes to the FDC3 specification that are not yet implemented in the .NET port.

### High Priority (API Contract Changes)

#### 1.1. `IChannel` — Two missing methods

| Spec method | .NET status |
|---|---|
| `clearContext(contextType?: string): Promise<void>` | Missing entirely |
| `addEventListener(type: string \| null, handler: EventHandler): Promise<Listener>` | Missing from `IChannel` (only exists on `IPrivateChannel`) |

`clearContext` notifies existing listeners via a `contextCleared` event when context is cleared from the channel.

**Original Implementation**:
- **`clearContext`**: [Context Clearing #1379](https://github.com/finos/FDC3/pull/1379) (merged 2025-07-29)
  - Related Issue: [Context clearing #1197](https://github.com/finos/FDC3/issues/1197)
- **`addEventListener`**: [Async listener and private channel events #1305](https://github.com/finos/FDC3/pull/1305) (merged 2024-08-01)
  - Related Issue: [Event listener support #1207](https://github.com/finos/FDC3/pull/1207)

#### 1.2. `IPrivateChannel` — `Disconnect` is synchronous instead of async

- **Spec**: `disconnect(): Promise<void>`
- **.NET**: `void Disconnect()` — should return `Task`

**Original Implementation**: [Async listener and private channel events #1305](https://github.com/finos/FDC3/pull/1305)
- Part of async listener support implementation

#### 1.3. Events (`Fdc3Event.cs`) — Missing `contextCleared` event

**Spec `FDC3EventTypes`**: `'userChannelChanged' | 'contextCleared'`
- .NET `Fdc3EventType` only has `UserChannelChanged`
- Missing: `contextCleared` constant, `FDC3ContextClearedEvent` class (with `details.contextType: string | null`)

**Original Implementation**: [Context Clearing #1379](https://github.com/finos/FDC3/pull/1379)
- Related Issue: [Context clearing #1197](https://github.com/finos/FDC3/issues/1197)

#### 1.4. Error Enum Updates

- `OpenError`: Add `ApiTimeout`, `InvalidArguments`
- `ResolveError`: Add `IntentListenerConflict`, `ApiTimeout`, `InvalidArguments`
- `ResultError`: Add `ApiTimeout`
- `ChannelError`: Add `ApiTimeout`, `InvalidArguments`

---

### Content (New Context Types)

#### 1.5. Context Types — Six missing types

| Spec schema | .NET status |
|---|---|
| `fdc3.interaction` | Likely part of general context type additions |
| `fdc3.order` | Missing |
| `fdc3.orderList` | Missing |
| `fdc3.product` | Missing |
| `fdc3.trade` | Missing |
| `fdc3.tradeList` | Missing |

**Original Implementation**:
- **`fdc3.order`, `fdc3.trade`, `fdc3.product`**: [Order and trade experimental contexts #1021](https://github.com/finos/FDC3/pull/1021) (merged 2023-07-28)
  - Related Issues: [Order and trade experimental contexts #644](https://github.com/finos/FDC3/issues/644), [Order and trade experimental contexts #655](https://github.com/finos/FDC3/issues/655)
  - Implemented with documentation in the schema
  - Includes examples for Order, Trade, and Product types
  - Note: `order` and `trade` have `product` nested under `details` field, while `product` is a standalone context type
- **`fdc3.orderList`, `fdc3.tradeList`**: Part of [Order and trade experimental contexts #1021](https://github.com/finos/FDC3/pull/1021)

---

## 2. CHANGES TO FINOS/FDC3 API/CONTEXTS

These items represent deviations from the FDC3 specification that need correction in the .NET implementation.

#### 2.1. `IIntentResolution` — Extra property not in spec

- `Version { get; }` exists in .NET but **is not part of the spec** — should be removed

**Note**: This is a .NET-specific addition not found in the FDC3 specification. The TypeScript implementation has never included a `Version` property.

---

#### 2.2. `ContextMetadata` — `Source` nullability difference

- **Spec**: `source: AppIdentifier` (non-optional — always present)
- **.NET** `IContextMetadata`: `IAppIdentifier? Source` (nullable) — should be non-nullable

**Note**: This is a spec requirement that needs to be corrected in the .NET implementation.

---

#### 2.3. `IImplementationMetadata.ProviderVersion` — Nullability difference

- **Spec**: `providerVersion` is optional (`string?`)
- **.NET**: `providerVersion` is required (`string`) — should be made nullable

**Note**: This is a spec requirement that needs to be corrected in the .NET implementation.

---

#### 2.4. `IIntentResolution.Intent` — Type difference

- **Spec**: `intent: Intent` (typed object)
- **.NET**: `Intent { get; }` (string)

**Note**: The .NET implementation uses a string for the intent name instead of the typed `Intent` object from the spec. This is a deviation from the spec.

---

## 3. UNIQUE TO FDC3-DOTNET

These items are in the .NET implementation but not present in the FDC3 specification. Some may be due to language-specific design choices.

#### 3.1. `IIntentResolution.Version` Property

- **Status**: **DEVIATION FROM SPEC** — This property exists in .NET but is not part of the FDC3 specification
- **Action**: Should be removed to maintain spec compliance

---

#### 3.2. `ContextMetadata.Source` Nullability

- **Status**: **DEVIATION FROM SPEC** — The spec requires `source` to be non-nullable, but .NET makes it nullable
- **Action**: Should be corrected to match the spec

---

#### 3.3. `IIntentResolution.Intent` Type

- **Status**: **DEVIATION FROM SPEC** — The spec uses a typed `Intent` object, but .NET uses a string
- **Action**: Should be changed to use the typed `Intent` object

---

#### 3.4. Deprecated Backward-Compat Overloads

The .NET implementation is missing these deprecated overloads (for FDC3 <2.0 compatibility):

| Deprecated method | Notes |
|---|---|
| `Open(string name, IContext? context)` | Targets by name string instead of `AppIdentifier` |
| `RaiseIntent(string, IContext, string name)` | Targets by app name string |
| `RaiseIntentForContext(IContext, string name)` | Targets by app name string |
| `GetSystemChannels()` | Alias for `GetUserChannels` |
| `JoinChannel(string channelId)` | Alias for `JoinUserChannel` |
| `AddContextListener(ContextHandler)` | No `contextType` parameter |

**Note**: These are FDC3 <2.0 compatibility methods - not found in recent PRs, likely part of initial spec implementation. They are not part of the current FDC3 specification.

---

## Priority Summary

| Priority | Items |
|---|---|
| **High** (API contract changes) | `IChannel.clearContext`, `IChannel.addEventListener`, `IPrivateChannel.disconnect` async, new error values across all enums, `contextCleared` event |
| **Medium** (spec deviations) | `IIntentResolution.Version` removal, `ContextMetadata.Source` nullability, `IImplementationMetadata.ProviderVersion` nullability, `IIntentResolution.Intent` type change, deprecated backward-compat overloads |
| **Content** | 6 missing context types (`Interaction`, `Order`, `OrderList`, `Product`, `Trade`, `TradeList`) |

---

## NOT APPLICABLE (Desktop Agent Bridging)

These items are **NOT APPLICABLE** because this .NET port does not support desktop agent bridging.

### 1. `IAppIdentifier` — Missing experimental field

**Spec adds** `desktopAgent?: string` (introduced FDC3 2.1, marks which bridge DA hosts the app)
- .NET has no equivalent property

**Original Implementation**: [FDC3 API Metadata #1706](https://github.com/finos/FDC3/pull/1706) (merged 2026-01-09)
- Related Issue: [Add property to support analytics across apps #1290](https://github.com/finos/FDC3/issues/1290)
- Part of broader metadata proposal for tracking workflow and data usage

---

### 2. `IAppMetadata` — Missing field

**Spec adds** `instanceMetadata?: { [key: string]: any }` - implementation-specific data to disambiguate instances (e.g. window title, screen position). Must only be set if `instanceId` is set.
- .NET has no equivalent property

**Original Implementation**: [FDC3 API Metadata #1706](https://github.com/finos/FDC3/pull/1706)
- Part of broader metadata proposal for tracking workflow and data usage

---

### 5. `IImplementationMetadata` — Two gaps

| Field | Spec | .NET |
|---|---|---|
| `providerVersion` | optional (`string?`) | required (`string`) — should be made nullable |
| `optionalFeatures.DesktopAgentBridging` | required `boolean` | missing from `OptionalDesktopAgentFeatures` |

**Original Implementation**: Likely part of [FDC3 API Metadata #1706](https://github.com/finos/FDC3/pull/1706) or bridging specification updates

---

### 8. Errors — Bridging-related missing values and enums

#### `OpenError` — missing:
- `DesktopAgentNotFound` (experimental, bridging) — **NOT APPLICABLE**
- `ApiTimeout` — Part of async/timeout improvements
- `InvalidArguments` — Part of error handling improvements

#### `ResolveError` — missing:
- `IntentListenerConflict` (raised when `addIntentListener` is called for an intent that already has a listener) — [Clarification re repeated addContextListener and addIntentListener calls #1394](https://github.com/finos/FDC3/pull/1394)
- `DesktopAgentNotFound` (experimental, bridging) — **NOT APPLICABLE**
- `ApiTimeout` — Part of async/timeout improvements
- `InvalidArguments` — Part of error handling improvements

#### `ResultError` — missing:
- `ApiTimeout` — Part of async/timeout improvements

#### `ChannelError` — missing:
- `ApiTimeout` — Part of async/timeout improvements
- `InvalidArguments` — Part of error handling improvements

#### `BridgingError` enum — entirely missing:
```
ResponseTimedOut, AgentDisconnected, NotConnectedToBridge, MalformedMessage
```
(all marked `@experimental`) — **NOT APPLICABLE**

#### `AgentError` enum — entirely missing:
```
AgentNotFound, AccessDenied, ErrorOnConnect, InvalidFailover, ApiTimeout
```
(used by `getAgent()` / connection flow) — **NOT APPLICABLE**

---

### 10. New type: `DesktopAgentIdentifier` — entirely missing

Used in bridging scenarios where a DA (not a specific app) is the source/target of a message.

```csharp
public interface IDesktopAgentIdentifier {
    string DesktopAgent { get; }
}
```

**Original Implementation**: Likely part of [Desktop Agent Bridging #968](https://github.com/finos/FDC3/pull/968) or bridging specification updates

## Key Implementation PRs

### Major Metadata Support
- **[FDC3 API Metadata #1706](https://github.com/finos/FDC3/pull/1706)** (merged 2026-01-09)
  - Adds metadata support for tracking workflow and data usage
  - Related Issue: [Add property to support analytics across apps #1290](https://github.com/finos/FDC3/issues/1290)
  - Affects: Items 1, 2
  - **Status**: NOT APPLICABLE — This .NET port does not support desktop agent bridging

### Context Clearing
- **[Context Clearing #1379](https://github.com/finos/FDC3/pull/1379)** (merged 2025-07-29)
  - Adds context clearing functionality
  - Related Issue: [Context clearing #1197](https://github.com/finos/FDC3/issues/1197)
  - Affects: Items 3, 9

### Async Listeners
- **[Async listener and private channel events #1305](https://github.com/finos/FDC3/pull/1305)** (merged 2024-08-01)
  - Adds async listener support including `addEventListener` and async `disconnect`
  - Related Issue: [Event listener support #1207](https://github.com/finos/FDC3/pull/1207)
  - Affects: Items 3b, 4

### Order/Trade/Product Context Types
- **[Order and trade experimental contexts #1021](https://github.com/finos/FDC3/pull/1021)** (merged 2023-07-28)
  - Adds experimental Order, Trade, and Product context types
  - Related Issues: [Order and trade experimental contexts #644](https://github.com/finos/FDC3/issues/644), [Order and trade experimental contexts #655](https://github.com/finos/FDC3/issues/655)
  - Affects: Item 12

### Bridging Support
- **[Desktop Agent Bridging #968](https://github.com/finos/FDC3/pull/968)**
  - Adds bridging protocol and error types
  - Affects: Items 8 (BridgingError, AgentError, DesktopAgentNotFound)
  - **Status**: NOT APPLICABLE — This .NET port does not support desktop agent bridging

## Implementation Notes

1. **Metadata PR (#1706)** is the most significant recent addition - provides hooks for telemetry tracking tools
   - **Note**: Items 1, 2, 5, 8, 10 are NOT APPLICABLE because this .NET port does not support desktop agent bridging

2. **Context Clearing PR (#1379)** adds the `clearContext` method and `contextCleared` event

3. **Async Listeners PR (#1305)** is essential for modern async/await patterns

4. **Order/Trade/Product PR (#1021)** provides experimental context types for trading workflows

5. **Bridging PR (#968)** is NOT APPLICABLE because this .NET port does not support desktop agent bridging
