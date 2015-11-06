# security proof-of-concept 
# by Dr. Tobias Weltner 
# launches arbitrary software remotely on behalf of 
# the currently logged on person 
# requires Windows Vista/Server 2008 or better 
# requires local admin rights on the target 
 
function Start-ProcessInteractive { 
  param( 
  $filepath = 'powershell.exe', 
  $arguments = '-noprofile -command Get-Date | Out-File $env:windir\testfile.txt', 
  [Parameter(Mandatory=$true)] 
  $computername 
  ) 
 
 
function Execute-Tool($path) { 
    $r = (Invoke-Expression $path) 2>&1 
    if ($LASTEXITCODE -ne 0) { Throw $r[0].Exception.Message } 
  } 
 
  $computername | ForEach-Object { 
    try { 
    $username = Get-WmiObject Win32_ComputerSystem -ComputerName $_ |  
    Select-Object -ExpandProperty UserName 
    } catch {} 
    $computer = $_ 
     
    if ($username -eq $null) { 
      Write-Warning "On $computername no user is currently physically logged on." 
      $username = Read-Host "Enter username of logged on user at the remote system" 
     } 
     if ($username -ne '') { 
 
       
$xml = @" 
<?xml version="1.0" encoding="UTF-16"?> 
<Task version="1.2" xmlns="http://schemas.microsoft.com/windows/2004/02/mit/task"> 
  <RegistrationInfo /> 
  <Triggers /> 
  <Settings> 
    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy> 
    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries> 
    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries> 
    <AllowHardTerminate>true</AllowHardTerminate> 
    <StartWhenAvailable>false</StartWhenAvailable> 
    <RunOnlyIfNetworkAvailable>false</RunOnlyIfNetworkAvailable> 
    <IdleSettings /> 
    <AllowStartOnDemand>true</AllowStartOnDemand> 
    <Enabled>true</Enabled> 
    <Hidden>false</Hidden> 
    <RunOnlyIfIdle>false</RunOnlyIfIdle> 
    <WakeToRun>false</WakeToRun> 
    <ExecutionTimeLimit>PT72H</ExecutionTimeLimit> 
    <Priority>7</Priority> 
  </Settings> 
  <Actions Context="Author"> 
    <Exec> 
      <Command>$filepath</Command> 
      <Arguments>$arguments</Arguments> 
    </Exec> 
  </Actions> 
  <Principals> 
    <Principal id="Author"> 
      <UserId>$username</UserId> 
      <LogonType>InteractiveToken</LogonType> 
      <RunLevel>HighestAvailable</RunLevel> 
    </Principal> 
  </Principals> 
</Task> 
"@ 
 
       
       
      $jobname = 'remotejob{0}' -f (Get-Random) 
       
      try { 
        $xml | Out-File "$env:temp\tj1.xml" 
        Execute-Tool "schtasks /CREATE /TN $jobname /XML $env:temp\tj1.xml /S $computer" 
        Start-Sleep -Seconds 1 
        Execute-Tool "schtasks /RUN /TN $jobname /S $computer" 
        Execute-Tool "schtasks /DELETE /TN $jobname /s $computer /F" 
      } 
      catch { 
        Write-Warning "$_ (trying to access user '$username' on system '$computer')" 
      } 
    } 
  } 
} 