"C:Program Files\dotnet\dotnet.exe" restore
"C:Program Files\dotnet\dotnet.exe" publish --configuration release -o c:\CENProject\publish --runtime active
C:\Windows\SysNative\WindowsPowerShell\v1.0\powershell.exe -Command {
             Import-Module WebAdministration
             Set-ItemProperty 'IIS:sitesDefault Web Site' 
                 -Name physicalPath -Value c:\CENProject\publish
}
