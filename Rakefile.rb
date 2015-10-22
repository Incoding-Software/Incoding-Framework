require 'albacore'
require 'fileutils'

def CreateDirIfNotExists(path)
  if !Dir.exist?(path)
    FileUtils.mkdir(path)
  end
end

def DeleteDirIfExists(path)
  if Dir.exist?(path)
    FileUtils.remove_dir(path)
  end
end

def DeleteFileIfExists(path)
  if File.exist?(path)
    File.delete(path)
  end
end

  def minify(files)
    files.each do |file|
      cmd = "java -jar tools/yuicompressor-2.4.7.jar #{file} -o #{file}"
      puts cmd
      ret = system(cmd)
      raise "Minification failed for #{file}" if !ret
    end
  end





Environment =  { :solution_name => 'IncFramework'}

Folder =
    {
        :src =>'src',
        :deploy => 'deploy',
	:config => File.join('config' , 'ci'),
        :live =>  File.join('deploy' , 'Live'),
        :dev =>  File.join( 'deploy' , 'Dev'),        
        :clientFramework =>  'src/Incoding/MvcContrib/Incoding Meta Language/Client Framework',                
        :lib => 'src/packages',
	:mspecResult =>  File.join('deploy','MspecReport')
    }

Files  =
    {
    :packageLive =>   Environment[:solution_name] + '.zip',
    :sln =>  Environment[:solution_name] +  '.sln',
    :mspecRunner =>  'tools/m-spec/mspec-clr4.exe', #path to mspec console runner
    :integratedTestDll =>     Folder[:dev] + '/Incoding.UnitTest.dll',  #path to test dll    
    :dbConfig => Folder[:config] + '/'  + 'db.config'
}

desc    'Folders create or delete'
task :estblish do
  DeleteDirIfExists(Folder[:deploy])
  CreateDirIfNotExists(Folder[:deploy])
  CreateDirIfNotExists(Folder[:live]) 
  FileUtils.cp_r(File.expand_path(Files[:dbConfig]),Folder[:src] + '/Incoding.UnitTest',:verbose => true)  
  FileUtils.cp_r(File.expand_path(Files[:dbConfig]),Folder[:src] + '/Incoding.SiteTest',:verbose => true)  

end

assemblyinfo :assemblyinfo do |asm|
  asm.version = ENV['build_number']
  asm.file_version = ENV['build_number']
  asm.output_file = "src/Incoding/properties/AssemblyInfo.cs"
end

assemblyinfo :assemblyinfoToMspec do |asm|
  asm.version = ENV['build_number']
  asm.file_version = ENV['build_number']
  asm.output_file = "src/Incoding.MSpecContrib/properties/AssemblyInfo.cs"
end

desc    'Clean and Build solution'
msbuild :build =>[:assemblyinfo,:assemblyinfoToMspec] do |msb|
  msb.properties :configuration => :Release, :OutputPath => :"../../#{Folder[:dev]}"
  msb.targets :Clean,:Build
  msb.solution = File.join(Folder[:src],Files[:sln])
end

desc 'Execute integrated test'
mspec do |mspec|
  CreateDirIfNotExists(Folder[:mspecResult])
  mspec.command = Files[:mspecRunner]
  mspec.assemblies Files[:integratedTestDll]
  mspec.html_output = Folder[:mspecResult]
end

task :combine do
  File.open(Folder[:live] + '/incoding.framework.js', 'w') {   |file| 
       file.write(File.read(File.expand_path(Folder[:clientFramework] + '/incoding.meta.helper.js')))
       file.write(File.read(File.expand_path(Folder[:clientFramework] + '/incoding.url.js')))
       file.write(File.read(File.expand_path(Folder[:clientFramework] + '/incoding.core.js')))       
       file.write(File.read(File.expand_path(Folder[:clientFramework] + '/incoding.meta.engine.js')))      	  
       file.write(File.read(File.expand_path(Folder[:clientFramework] + '/incoding.meta.executable.js')))
       file.write(File.read(File.expand_path(Folder[:clientFramework] + '/incoding.meta.conditional.js')))
   }     
end


  desc "minify javascript"
  task :minify =>[:combine] do
    FileUtils.cp_r(File.expand_path(Folder[:live] + '/incoding.framework.js'),Folder[:live] + '/incoding.framework.min.js',:verbose => true)  
    minify(FileList[Folder[:live] + '/incoding.framework.min.js'])
  end


task :publish =>[:build,:combine] do    
  FileUtils.cp_r(File.expand_path(Folder[:dev] + '/incoding.dll'),Folder[:live],:verbose => true)  
  FileUtils.cp_r(File.expand_path(Folder[:dev] + '/incoding.pdb'),Folder[:live],:verbose => true)
  FileUtils.cp_r(File.expand_path(Folder[:clientFramework] + '/incoding.meta.trace.js'),Folder[:live],:verbose => true)  
  FileUtils.cp_r(File.expand_path(Folder[:dev] + '/Incoding.MSpecContrib.dll'),Folder[:live],:verbose => true)
  FileUtils.cp_r(File.expand_path(Folder[:dev] + '/Incoding.MSpecContrib.pdb'),Folder[:live],:verbose => true)  

end



zip:packageArtifacts do |zip|
  zip.directories_to_zip Folder[:live]
  zip.output_file = "../#{Files[:packageLive]}"
end

task :default => [:estblish,:publish,:packageArtifacts] do
end





