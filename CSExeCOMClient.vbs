SET obj = CreateObject("OutlookPresenceProvider.PresenceProvider")

MsgBox "A OutlookPresenceProvider.PresenceProvider object is created"

Dim retValue

retValue = obj.GetAuthenticationInfo("15.0.0.0")

' call the HelloWorld method that returns a string
MsgBox "The HelloWorld method returns " & retValue

SET obj = Nothing