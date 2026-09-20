# CIARELiveShareAPI

API for Live Share feature in CIARE text editor. 

CIARE project: https://github.com/0x78654C/CIARE/

# Requirements

.NET 8

Disconnects notify the remaining participants in the same session through
`UserDisconnected(connectionId)`, allowing CIARE to remove that user's name and caret.
Deploy the updated API alongside clients that handle this event.

Run the local disconnect regression checks with:

```sh
dotnet run --project Tests.LiveShare/Tests.LiveShare.csproj
```
