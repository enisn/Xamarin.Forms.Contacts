# Xamarin.Forms.Contacts
Read Contacts Data on iOS and Android

Easy usage in Portable Project:

```csharp
var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
```

DO NOT FORGET ADD THIS PERMISSIONS:

# ANDROID
```
READ_CONTACTS
```

# iOS
If you don't have mac connection you should Right Click the **Info.plist** and Open With XML editor.
Add this key into **<dict>**
```xml
	<key>NSContactsUsageDescription</key>
	<string>We need contact permission to do ...</string>
```

