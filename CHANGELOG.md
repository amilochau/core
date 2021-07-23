[//]: # (Format this CHANGELOG.md with these titles:)
[//]: # (Breaking changes)
[//]: # (New features)
[//]: # (Bug fixes)
[//]: # (Minor changes)

## Breaking changes

- Key Vault can no more be configured with TenantId, ClientId and ClientSecret; you should now use Managed Identity
- `CoreHostOptions` has been moved, the old namespace `Milochau.Core.Models` is converted to `Milochau.Core.Abstractions` - and now part of the Milochau.Core.Abstractions library
