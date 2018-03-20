var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Release");
var proj = $"./src/PayEx.Client/PayEx.Client.csproj";

var version = "1.0.0-preview001"; 
var outputDir = "./output";

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
            OutputDirectory = outputDir,
        };
        coresettings.MSBuildSettings = new DotNetCoreMSBuildSettings()
                                        .WithProperty("Version", new[] { version });

        
        DotNetCorePack(proj, coresettings);
});

Task("PublishToNugetOrg")
    .IsDependentOn("Pack")
    .Does(() => {        
        var settings = new DotNetCoreNuGetPushSettings
        {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = EnvironmentVariable("NUGET_API_KEY")
        };

        DotNetCoreNuGetPush($"{outputDir}/PayEx.Client.{version}.nupkg", settings);        
});

RunTarget(target);