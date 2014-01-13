# Runs every time a package is installed in a project

param($installPath, $toolsPath, $package, $project)

$project.Object.References.Add("Microsoft.CSharp");
$project.Object.References.Add("System.Configuration"); 
$project.Object.References.Add("System.Web.Mvc");



