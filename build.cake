var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Release");
var proj = $"./src/PayEx.Client.csproj";

var version = "0.1.0"; 

Task("Build")    
    .Does(() => {
        DotNetCoreBuild(proj, new DotNetCoreBuildSettings { Configuration = "Release" });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        Warning("Lacking tests."); 
        //var testproj = $"./src/PayEx.Client/PayEx.Client.csproj";       
        //DotNetCoreTest(testproj);     
});

Task("Pack")
    .IsDependentOn("Test")
    .Does(() => {
        var coresettings = new DotNetCorePackSettings
        {
            Configuration = "Release",
            OutputDirectory = "./output",
        };
        coresettings.MSBuildSettings = new DotNetCoreMSBuildSettings()
                                        .WithProperty("Version", new[] { version });

        
        DotNetCorePack(proj, coresettings);
});

Task("Default")
.Does(() => {
   Information("Hello Cake!");
});

RunTarget(target);