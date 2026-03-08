#Requires -RunAsAdministrator
<#
.SYNOPSIS
    Installs all development tools for the Coding Basics Workshop.
.DESCRIPTION
    Tools: .NET SDK, EF Core CLI (dotnet-ef), Git, VS Code, Node.js LTS,
    SSMS, SQL Server Express, and VS Code extensions.
    Strategy: winget first, manual download fallback if unavailable.
.NOTES
    Requires PowerShell 5.1+ and Administrator privileges.
    Temp files saved to $env:TEMP\DevToolsInstall and cleaned up at the end.
#>

$ErrorActionPreference = "Stop"
$ProgressPreference    = "SilentlyContinue"

# -- Configuration --------------------------------------------------------
# Change $DotNetSdkMajor to target a different .NET SDK major version.
# Winget ID, version check, and display text are all derived from this.
$DotNetSdkMajor = 10
$TempFolder     = "$env:TEMP\DevToolsInstall"

# -- Progress Tracker -----------------------------------------------------
$global:StepResults = [ordered]@{
    ".NET SDK"                     = "pending"
    "EF Core Tools"                = "pending"
    "Git"                          = "pending"
    "Visual Studio Code"           = "pending"
    "Node.js LTS"                  = "pending"
    "SQL Server Management Studio" = "pending"
    "SQL Server Express"           = "pending"
    "VS Code Extensions"           = "pending"
}

function Show-Progress {
    Write-Host ""
    Write-Host "  -------- Progress --------" -ForegroundColor Magenta
    $n = 0
    foreach ($item in $global:StepResults.GetEnumerator()) {
        $n++
        $val = $item.Value; if ($val -is [array]) { $val = $val[-1] }
        switch ($val) {
            "installed" { $icon = "[OK]";   $color = "Green"    }
            "skipped"   { $icon = "[SKIP]"; $color = "Yellow"   }
            "failed"    { $icon = "[FAIL]"; $color = "Red"      }
            default     { $icon = "[ . ]";  $color = "DarkGray" }
        }
        Write-Host "  $icon $n. $($item.Key)" -ForegroundColor $color
    }
    Write-Host "  ----------------------------" -ForegroundColor Magenta
    Write-Host ""
}

# -- Logging Helpers ------------------------------------------------------
function Write-Info    { param($M) Write-Host "[INFO]  $M" -ForegroundColor Cyan    }
function Write-Success { param($M) Write-Host "[OK]    $M" -ForegroundColor Green   }
function Write-Warn    { param($M) Write-Host "[WARN]  $M" -ForegroundColor Yellow  }
function Write-Err     { param($M) Write-Host "[ERROR] $M" -ForegroundColor Red     }

# -- Refresh-Path ---------------------------------------------------------
# Reloads Machine + User PATH so newly installed tools are available
# immediately without restarting the shell.
function Refresh-Path {
    $env:Path = [Environment]::GetEnvironmentVariable("Path","Machine") + ";" +
                [Environment]::GetEnvironmentVariable("Path","User")
    Write-Info "PATH refreshed."
}

# -- Invoke-WithSpinner ---------------------------------------------------
# Runs an external process with a spinning animation (| / - \).
# Uses System.Diagnostics.Process to avoid Start-Process pipeline leaks.
# Returns the process exit code.
function Invoke-WithSpinner {
    param([string]$FilePath, [string]$ArgumentList, [string]$Label = "Working")

    $psi = New-Object System.Diagnostics.ProcessStartInfo
    $psi.FileName               = $FilePath
    $psi.Arguments              = $ArgumentList
    $psi.UseShellExecute        = $false
    $psi.CreateNoWindow         = $true
    $psi.RedirectStandardOutput = $true
    $psi.RedirectStandardError  = $true

    $proc = New-Object System.Diagnostics.Process
    $proc.StartInfo = $psi
    [void]$proc.Start()

    # Drain stdout/stderr asynchronously to prevent buffer deadlock
    $stdoutTask = $proc.StandardOutput.ReadToEndAsync()
    $stderrTask = $proc.StandardError.ReadToEndAsync()

    $frames = @('|','/','-','\')
    $i = 0
    while (-not $proc.HasExited) {
        Write-Host "`r  [$($frames[$i % 4])] $Label " -NoNewline -ForegroundColor DarkGray
        Start-Sleep -Milliseconds 250
        $i += 1
    }
    Write-Host "`r$(' ' * ($Label.Length + 10))`r" -NoNewline

    $proc.WaitForExit()
    $null = $stdoutTask.Result
    $null = $stderrTask.Result
    return $proc.ExitCode
}

# -- Test-Winget ----------------------------------------------------------
function Test-Winget {
    try { $null = Get-Command winget -ErrorAction Stop; return $true }
    catch { return $false }
}

# -- Install-WithWinget ---------------------------------------------------
# Returns "installed", "skipped", or "failed".
function Install-WithWinget {
    param([string]$PackageId, [string]$PackageName)

    Write-Info "[$PackageName] Checking via winget..."
    try {
        $listExit = Invoke-WithSpinner -FilePath "winget" `
            -ArgumentList "list --id $PackageId --source winget --accept-source-agreements --disable-interactivity" `
            -Label "Checking $PackageName"
        if ($listExit -eq 0) {
            Write-Warn "[$PackageName] Already installed -- skipping."
            return "skipped"
        }
    } catch {
        Write-Info "[$PackageName] Could not query -- will attempt install."
    }

    Write-Info "[$PackageName] Installing via winget..."
    try {
        $exit = Invoke-WithSpinner -FilePath "winget" `
            -ArgumentList "install --id $PackageId --source winget --accept-source-agreements --accept-package-agreements --silent --disable-interactivity" `
            -Label "Installing $PackageName"
        if ($exit -eq 0) {
            Write-Success "[$PackageName] Installed successfully."
            return "installed"
        }
        Write-Err "[$PackageName] winget exit code $exit."
        return "failed"
    } catch {
        Write-Err "[$PackageName] Exception: $_"
        return "failed"
    }
}

# -- Get-Installer --------------------------------------------------------
# Downloads a file via background job with spinner. Returns $true/$false.
function Get-Installer {
    param([string]$Url, [string]$OutputPath)

    Write-Info "Downloading: $Url"
    $job = $null
    try {
        $job = Start-Job -ScriptBlock {
            param($u, $o)
            $ProgressPreference = "SilentlyContinue"
            Invoke-WebRequest -Uri $u -OutFile $o -UseBasicParsing
        } -ArgumentList $Url, $OutputPath

        $frames = @('|','/','-','\'); $i = 0
        while ($job.State -eq 'Running') {
            Write-Host "`r  [$($frames[$i % 4])] Downloading... " -NoNewline -ForegroundColor DarkGray
            Start-Sleep -Milliseconds 250; $i += 1
        }
        Write-Host "`r                            `r" -NoNewline

        $null = Receive-Job -Job $job -ErrorAction Stop
        Remove-Job -Job $job -Force; $job = $null
        $sizeMB = [math]::Round((Get-Item $OutputPath).Length / 1MB, 2)
        Write-Success "Downloaded ($sizeMB MB)."
        return $true
    } catch {
        Write-Err "Download failed: $_"
        if ($job) { Remove-Job -Job $job -Force -ErrorAction SilentlyContinue }
        return $false
    }
}

# -- Install-Manual -------------------------------------------------------
# Downloads an EXE installer and runs it silently. Returns "installed" or
# "failed". Accepts exit codes 0 and 3010 (reboot pending) as success.
function Install-Manual {
    param(
        [string]$Url,
        [string]$FileName,
        [string]$Arguments,
        [string]$Label,
        [string]$ManualUrl = ""
    )
    $path = "$TempFolder\$FileName"
    if (Get-Installer -Url $Url -OutputPath $path) {
        $exitCode = Invoke-WithSpinner -FilePath $path -ArgumentList $Arguments -Label $Label
        if ($exitCode -eq 0 -or $exitCode -eq 3010) {
            Write-Success "$Label -- done."
            return "installed"
        }
        Write-Err "$Label exited with code $exitCode."
    } else {
        Write-Err "Failed to download installer for $Label."
    }
    if ($ManualUrl) { Write-Warn "Install manually: $ManualUrl" }
    return "failed"
}

# =========================================================================
# SETUP
# =========================================================================

if (-not (Test-Path $TempFolder)) {
    New-Item -ItemType Directory -Path $TempFolder -Force | Out-Null
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Magenta
Write-Host "  Development Tools Installer"            -ForegroundColor Magenta
Write-Host "========================================" -ForegroundColor Magenta
Write-Host ""

# Check winget once for the whole script
$WingetAvailable = Test-Winget
if ($WingetAvailable) { Write-Success "winget found." }
else                  { Write-Warn "winget not found -- will use manual downloads." }

# =========================================================================
# STEP 1/8 -- .NET SDK
# =========================================================================

Write-Host "`n--- [Step 1/8] .NET $DotNetSdkMajor SDK ---" -ForegroundColor Yellow

$dotnetResult = "failed"
$dotnetCmd = Get-Command dotnet -ErrorAction SilentlyContinue
if ($dotnetCmd) {
    try {
        $ver = dotnet --version
        if ([int]($ver -split '\.')[0] -ge $DotNetSdkMajor) {
            Write-Success ".NET SDK $ver (>= $DotNetSdkMajor). Skipping."
            $dotnetResult = "skipped"
        } else {
            Write-Warn ".NET SDK $ver found but < $DotNetSdkMajor."
        }
    } catch { Write-Warn "Could not parse .NET SDK version." }
}

if ($dotnetResult -ne "skipped") {
    if ($WingetAvailable) {
        $dotnetResult = Install-WithWinget -PackageId "Microsoft.DotNet.SDK.$DotNetSdkMajor" -PackageName ".NET $DotNetSdkMajor SDK"
    }
    if ($dotnetResult -eq "failed") {
        $dotnetResult = Install-Manual `
            -Url "https://download.visualstudio.microsoft.com/download/pr/dotnet-sdk-$DotNetSdkMajor.0.100-win-x64.exe" `
            -FileName "dotnet-sdk-$DotNetSdkMajor.0.100-win-x64.exe" `
            -Arguments "/install /quiet /norestart" `
            -Label "Installing .NET $DotNetSdkMajor SDK" `
            -ManualUrl "https://dotnet.microsoft.com/download/dotnet/$DotNetSdkMajor.0"
    }
    if ($dotnetResult -eq "installed") { Refresh-Path }
}

$global:StepResults[".NET SDK"] = $dotnetResult
Show-Progress

# =========================================================================
# STEP 2/8 -- EF Core CLI Tools (dotnet-ef)
# =========================================================================

Write-Host "`n--- [Step 2/8] EF Core CLI Tools (dotnet-ef) ---" -ForegroundColor Yellow

$efResult = "failed"
if (Get-Command dotnet -ErrorAction SilentlyContinue) {
    try {
        $efList = dotnet tool list --global 2>&1 | Out-String
        if ($efList -match "dotnet-ef") {
            $efVer = if ($efList -match "dotnet-ef\s+([\d\.]+)") { $Matches[1] } else { "?" }
            Write-Success "dotnet-ef $efVer already installed. Skipping."
            $efResult = "skipped"
        }
    } catch { Write-Info "Could not check existing dotnet-ef: $_" }

    if ($efResult -ne "skipped") {
        try {
            $exit = Invoke-WithSpinner -FilePath "dotnet" `
                -ArgumentList "tool install --global dotnet-ef" -Label "Installing dotnet-ef"
            if ($exit -eq 0) {
                Write-Success "dotnet-ef installed."
                $efResult = "installed"
                Refresh-Path
            } else {
                # May already exist at an older version -- try update
                $exit2 = Invoke-WithSpinner -FilePath "dotnet" `
                    -ArgumentList "tool update --global dotnet-ef" -Label "Updating dotnet-ef"
                if ($exit2 -eq 0) {
                    Write-Success "dotnet-ef updated."
                    $efResult = "installed"
                } else {
                    Write-Err "dotnet-ef install/update failed (codes $exit/$exit2)."
                    Write-Warn "Install manually: dotnet tool install --global dotnet-ef"
                }
            }
        } catch {
            Write-Err "dotnet-ef install failed: $_"
            Write-Warn "Install manually: dotnet tool install --global dotnet-ef"
        }
    }
} else {
    Write-Warn ".NET SDK not available -- cannot install dotnet-ef. Run Step 1 first."
}

$global:StepResults["EF Core Tools"] = $efResult
Show-Progress

# =========================================================================
# STEP 3/8 -- Git
# =========================================================================

Write-Host "`n--- [Step 3/8] Git ---" -ForegroundColor Yellow

$gitResult = "failed"
if (Get-Command git -ErrorAction SilentlyContinue) {
    Write-Warn "Git already installed: $(git --version 2>$null). Skipping."
    $gitResult = "skipped"
}

if ($gitResult -eq "failed") {
    if ($WingetAvailable) { $gitResult = Install-WithWinget -PackageId "Git.Git" -PackageName "Git" }
    if ($gitResult -eq "failed") {
        $gitResult = Install-Manual `
            -Url "https://github.com/git-scm/git/releases/download/v2.47.1.windows.2/Git-2.47.1.2-64-bit.exe" `
            -FileName "Git-Setup.exe" `
            -Arguments "/VERYSILENT /NORESTART /NOCANCEL /SP- /CLOSEAPPLICATIONS /RESTARTAPPLICATIONS /COMPONENTS=ext,ext\shellhere,ext\guihere,gitlfs,assoc,assoc_sh" `
            -Label "Installing Git" `
            -ManualUrl "https://git-scm.com/download/win"
    }
    if ($gitResult -eq "installed") { Refresh-Path }
}

$global:StepResults["Git"] = $gitResult
Show-Progress

# =========================================================================
# STEP 4/8 -- Visual Studio Code
# =========================================================================

Write-Host "`n--- [Step 4/8] Visual Studio Code ---" -ForegroundColor Yellow

$vscodeResult = "failed"
if ($WingetAvailable) {
    $vscodeResult = Install-WithWinget -PackageId "Microsoft.VisualStudioCode" -PackageName "Visual Studio Code"
}
if ($vscodeResult -eq "failed") {
    $vscodeResult = Install-Manual `
        -Url "https://code.visualstudio.com/sha/download?build=stable&os=win32-x64" `
        -FileName "VSCodeSetup.exe" `
        -Arguments "/VERYSILENT /NORESTART /MERGETASKS=!runcode,addcontextmenufiles,addcontextmenufolders,associatewithfiles,addtopath" `
        -Label "Installing Visual Studio Code"
}
Refresh-Path

$global:StepResults["Visual Studio Code"] = $vscodeResult
Show-Progress

# =========================================================================
# STEP 5/8 -- Node.js LTS
# =========================================================================

Write-Host "`n--- [Step 5/8] Node.js LTS ---" -ForegroundColor Yellow

$nodeResult = "failed"
if (Get-Command node -ErrorAction SilentlyContinue) {
    Write-Warn "Node.js already installed: $(node --version 2>$null). Skipping."
    $nodeResult = "skipped"
}

if ($nodeResult -eq "failed") {
    if ($WingetAvailable) { $nodeResult = Install-WithWinget -PackageId "OpenJS.NodeJS.LTS" -PackageName "Node.js LTS" }
    if ($nodeResult -eq "failed") {
        $nodeMsi = "$TempFolder\nodejs-lts.msi"
        if (Get-Installer -Url "https://nodejs.org/dist/v20.11.1/node-v20.11.1-x64.msi" -OutputPath $nodeMsi) {
            $exit = Invoke-WithSpinner -FilePath "msiexec.exe" `
                -ArgumentList "/i `"$nodeMsi`" /quiet /norestart" -Label "Installing Node.js LTS"
            if ($exit -eq 0 -or $exit -eq 3010) {
                Write-Success "Node.js LTS installed."
                $nodeResult = "installed"
            } else {
                Write-Err "Node.js MSI installer exited with code $exit."
                Write-Warn "Install manually: https://nodejs.org"
            }
        }
    }
}
Refresh-Path

$global:StepResults["Node.js LTS"] = $nodeResult
Show-Progress

# =========================================================================
# STEP 6/8 -- SQL Server Management Studio (SSMS)
# =========================================================================

Write-Host "`n--- [Step 6/8] SSMS ---" -ForegroundColor Yellow
Write-Warn "SSMS is ~600 MB. This may take 10-20 minutes."

$ssmsResult = "failed"
if ($WingetAvailable) {
    $ssmsResult = Install-WithWinget -PackageId "Microsoft.SQLServerManagementStudio" -PackageName "SSMS"
}
if ($ssmsResult -eq "failed") {
    $ssmsResult = Install-Manual `
        -Url "https://aka.ms/ssmsfullsetup" `
        -FileName "SSMS-Setup.exe" `
        -Arguments "/install /quiet /norestart" `
        -Label "Installing SSMS"
}

$global:StepResults["SQL Server Management Studio"] = $ssmsResult
Show-Progress

# =========================================================================
# STEP 7/8 -- SQL Server Express
# =========================================================================

Write-Host "`n--- [Step 7/8] SQL Server Express ---" -ForegroundColor Yellow

$sqlResult = "failed"

# Multi-method detection: registry -> services -> file system
try {
    $reg = Get-ItemProperty "HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL" -ErrorAction SilentlyContinue
    if ($reg) {
        $names = $reg.PSObject.Properties | Where-Object { $_.Name -notlike 'PS*' } | ForEach-Object { $_.Name }
        if ($names) {
            Write-Warn "SQL Server instances found: $($names -join ', '). Skipping."
            $sqlResult = "skipped"
        }
    }
} catch {}

if ($sqlResult -eq "failed") {
    if (Get-Service -Name "MSSQL*","SQLAgent*","SQL Server*" -ErrorAction SilentlyContinue) {
        Write-Warn "SQL Server service(s) detected. Skipping."
        $sqlResult = "skipped"
    }
}

if ($sqlResult -eq "failed") {
    foreach ($p in @("$env:ProgramFiles\Microsoft SQL Server\MSSQL*",
                     "${env:ProgramFiles(x86)}\Microsoft SQL Server\MSSQL*")) {
        if (Get-Item $p -ErrorAction SilentlyContinue) {
            Write-Warn "SQL Server directory found. Skipping."
            $sqlResult = "skipped"; break
        }
    }
}

if ($sqlResult -eq "failed") {
    if ($WingetAvailable) {
        $sqlResult = Install-WithWinget -PackageId "Microsoft.SQLServer.2022.Express" -PackageName "SQL Server 2022 Express"
    }
    if ($sqlResult -eq "failed") {
        $dlr = "$TempFolder\SQL2022-SSEI-Expr.exe"
        if (Get-Installer -Url "https://go.microsoft.com/fwlink/p/?linkid=2216019" -OutputPath $dlr) {
            $null = Invoke-WithSpinner -FilePath $dlr `
                -ArgumentList "/ACTION=Download /MEDIAPATH=$TempFolder /MEDIATYPE=Core /QUIET" `
                -Label "Downloading SQL Server Express engine"

            $sqlExe = Get-ChildItem $TempFolder -Filter "SQLEXPR*.exe" | Select-Object -First 1
            if ($sqlExe) {
                $sqlArgs = "/ACTION=Install /QUIET /IACCEPTSQLSERVERLICENSETERMS /FEATURES=SQLENGINE " +
                           "/INSTANCENAME=SQLEXPRESS /SECURITYMODE=SQL /SAPWD=`"YourPassword123!`" /TCPENABLED=1"
                $null = Invoke-WithSpinner -FilePath $sqlExe.FullName -ArgumentList $sqlArgs `
                    -Label "Installing SQL Server Express"
                Write-Success "SQL Server Express installed."
                Write-Warn "SECURITY: Default SA password is 'YourPassword123!' -- change it immediately!"
                $sqlResult = "installed"
            } else {
                Write-Err "Could not find SQLEXPR*.exe in $TempFolder."
                Write-Warn "Install manually: https://www.microsoft.com/sql-server/sql-server-downloads"
            }
        }
    }
}

$global:StepResults["SQL Server Express"] = $sqlResult
Show-Progress

# =========================================================================
# STEP 8/8 -- VS Code Extensions
# =========================================================================

Write-Host "`n--- [Step 8/8] VS Code Extensions ---" -ForegroundColor Yellow

$codeCmd = Get-Command code -ErrorAction SilentlyContinue
if (-not $codeCmd) {
    foreach ($p in @(
        "$env:LOCALAPPDATA\Programs\Microsoft VS Code\bin\code.cmd",
        "$env:ProgramFiles\Microsoft VS Code\bin\code.cmd",
        "${env:ProgramFiles(x86)}\Microsoft VS Code\bin\code.cmd"
    )) { if (Test-Path $p) { $codeCmd = $p; break } }
}

if ($codeCmd) {
    $extensions = @(
        @{Id="ms-dotnettools.csharp";                Name="C# (OmniSharp)"},
        @{Id="ms-dotnettools.csdevkit";              Name="C# Dev Kit"},
        @{Id="ms-dotnettools.vscode-dotnet-runtime"; Name=".NET Install Tool"},
        @{Id="Vue.volar";                            Name="Vue - Official (Volar)"},
        @{Id="dbaeumer.vscode-eslint";               Name="ESLint"},
        @{Id="esbenp.prettier-vscode";               Name="Prettier"},
        @{Id="ms-vscode.vscode-typescript-next";     Name="TypeScript Nightly"},
        @{Id="formulahendry.auto-rename-tag";        Name="Auto Rename Tag"},
        @{Id="christian-kohler.path-intellisense";   Name="Path Intellisense"},
        @{Id="PKief.material-icon-theme";            Name="Material Icon Theme"}
    )

    $ok = 0; $fail = 0
    foreach ($ext in $extensions) {
        try {
            if ($codeCmd -is [System.Management.Automation.ApplicationInfo]) {
                & code --install-extension $ext.Id --force 2>$null
            } else {
                & $codeCmd --install-extension $ext.Id --force 2>$null
            }
            Write-Success "  $($ext.Name)"; $ok++
        } catch { Write-Warn "  $($ext.Name) -- failed"; $fail++ }
    }
    Write-Info "Extensions: $ok installed, $fail failed."
    $global:StepResults["VS Code Extensions"] = if ($fail -eq 0) { "installed" } else { "failed" }
} else {
    Write-Warn "VS Code CLI not found. Install these extensions manually:"
    @("ms-dotnettools.csharp","ms-dotnettools.csdevkit","Vue.volar",
      "dbaeumer.vscode-eslint","esbenp.prettier-vscode") | ForEach-Object { Write-Host "  $_" }
    $global:StepResults["VS Code Extensions"] = "skipped"
}
Show-Progress

# =========================================================================
# CLEANUP & FINAL SUMMARY
# =========================================================================

try { Remove-Item $TempFolder -Recurse -Force -ErrorAction SilentlyContinue; Write-Success "Temp files removed." }
catch { Write-Warn "Could not remove $TempFolder -- delete manually." }

Write-Host "`n========================================" -ForegroundColor Magenta
Write-Host "  Installation Complete"                    -ForegroundColor Magenta
Write-Host "========================================`n" -ForegroundColor Magenta

foreach ($item in $global:StepResults.GetEnumerator()) {
    $val = $item.Value; if ($val -is [array]) { $val = $val[-1] }
    switch ($val) {
        "installed" { Write-Host "  [OK]   $($item.Key)" -ForegroundColor Green  }
        "skipped"   { Write-Host "  [SKIP] $($item.Key)" -ForegroundColor Yellow }
        "failed"    { Write-Host "  [FAIL] $($item.Key)" -ForegroundColor Red    }
        default     { Write-Host "  [??]   $($item.Key)" -ForegroundColor Gray   }
    }
}

Write-Host "`nImportant:" -ForegroundColor Yellow
Write-Host "  - A system restart may be needed for PATH changes." -ForegroundColor White
Write-Host "  - SQL Server instance: SQLEXPRESS  |  Connection: Server=localhost\SQLEXPRESS" -ForegroundColor White
Write-Host "  - Change the SA password if you used the manual SQL Server installer!" -ForegroundColor Red

Write-Host "`nDone! Press Enter to close..." -ForegroundColor Cyan
Read-Host
