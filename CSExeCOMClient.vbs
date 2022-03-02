SET obj = CreateObject("OutlookPresenceProvider.PresenceProvider")

MsgBox "A OutlookPresenceProvider.PresenceProvider object is created"

Dim retValue

' call the GetAuthenticationInfo method that returns a string
retValue = obj.GetAuthenticationInfo("15.0.0.0")

MsgBox "The GetAuthenticationInfo method returns " & retValue

SET obj = Nothing
