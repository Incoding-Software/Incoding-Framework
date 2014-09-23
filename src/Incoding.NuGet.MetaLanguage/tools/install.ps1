# Runs every time a package is installed in a project

param($installPath, $toolsPath, $package, $project)
$file = $project.ProjectItems | ForEach-Object { $_.ProjectItems } | where { $_.Name -eq "Global.asax.cs" }
if($file) {
    $file.Open()
    $file.Document.Activate()
    $file.Document.Selection.StartOfDocument()
	$file.Document.ReplaceText("Bootstrapper.Start();","")
	$file.Document.ReplaceText("new DispatcherController(); // init routes","")    
	}