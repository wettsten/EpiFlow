$serviceName = "EpiFlow.MessageProcessor"
$displayName = "EpiFlow Message Processor"
$exePath = "M:\Projects\EpiFlow\src\EpiFlow.FileWatcher\bin\Debug\EpiFlow.FileWatcher.exe"

$existingService = Get-WmiObject -Class Win32_Service -Filter "Name='$serviceName'"

if ($existingService) 
{
  "'$serviceName' exists already. Stopping."
  Stop-Service $serviceName
  "Waiting 3 seconds to allow existing service to stop."
  Start-Sleep -s 3

  $existingService.Delete()
  "Waiting 5 seconds to allow service to be uninstalled."
  Start-Sleep -s 5  
}

"Installing the service."
New-Service -BinaryPathName $exePath -Name $serviceName -DisplayName $displayName -StartupType Automatic -DependsOn RavenDB
"Installed the service."
"Starting the service."
Start-Service $serviceName
"Completed."