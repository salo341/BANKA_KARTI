;#DropDB BANKA_KARTIEasyTest

#Application BANKA_KARTIWin
#Application BANKA_KARTIBlazor

*FillForm
 User Name = Admin
*Action Log In

#IfDef BANKA_KARTIWin

*Action About...
 
*Action OK

#EndIf

#IfDef BANKA_KARTIBlazor
*Action Navigation(My Details)
#EndIf
