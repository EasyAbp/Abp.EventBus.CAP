# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

# List of projects
$projects = (

    "src/EasyAbp.Abp.EventBus.CAP",
    "src/EasyAbp.Abp.EventBus.CAP.MySql",
    "src/EasyAbp.Abp.EventBus.CAP.PostgreSql",
    "src/EasyAbp.Abp.EventBus.CAP.SqlServer"
)