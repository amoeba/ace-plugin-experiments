# ace-plugin-experiments

Experiments in building a plugin system in .NET Core.
Modeled after https://docs.microsoft.com/en-us/dotnet/core/tutorials/creating-app-with-plugin-support#create-the-plugin-interfaces.

## Scope

What this could let you make:

- An offline or web-based method for viewing detailed character information such as inventories and stats
- A web-based map of all players on the server
- Add custom @ commands to the server

What you can't make with this:

- Hook into or override existing ACE server functions

## Constraints

- With no plugins loaded, there should be a negligible impact on the server's performance

## Open Questions

- Should plugins have write-level access to the database?
- Should plugins directly call ACE methods or should there be a messaging mechanism?
