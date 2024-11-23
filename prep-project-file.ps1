param ($project, $version)

write-host $project 

$project_file_name = $project +  ".csproj"
$directory = Join-Path -Path "." -ChildPath $project
$project_file = Join-Path -Path $directory  -ChildPath $project_file_name
$release_Notes_file = Join-Path -Path $directory  -ChildPath "RELEASE-NOTES.txt"
$message = "Writing version "  + $version + " to " + $project_file

write-host $message
((Get-Content -path $project_file -Raw) -replace "987.654.3210", $version) | Set-Content -Path $project_file

if ($release_Notes_file){
    $release_Notes = Get-Content -path $release_Notes_file -Raw
    write-host "Writing release notes"
    write-host $release_Notes 

    ((Get-Content -path $project_file -Raw) -replace "@RELEASE-NOTES.txt", $release_Notes) | Set-Content -Path $project_file
}
