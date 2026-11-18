$csc = 'C:\Program Files (x86)\Microsoft Visual Studio\18\BuildTools\MSBuild\Current\Bin\Roslyn\csc.exe'
$editorFiles = (Get-ChildItem -Path Assets\Editor\AirplaneBuilder\*.cs).FullName
$runtimeFiles = if (Test-Path Assets\Scripts) { (Get-ChildItem -Path Assets\Scripts -Recurse -Filter *.cs).FullName } else { @() }
$files = ($editorFiles + $runtimeFiles) | ForEach-Object { "`"$_`"" }
$rsp = [System.IO.Path]::GetTempFileName()
$managed = 'C:\Program Files\Unity\Hub\Editor\6000.6.8f1\Editor\Data\Managed'
$bclRefs = (Get-ChildItem -Path 'C:\Program Files\Unity\Hub\Editor\6000.6.8f1\Editor\Data\DotNetSdk\packs\NETStandard.Library.Ref\2.1.0\ref\netstandard2.1\*.dll').FullName | ForEach-Object { "`"-r:$_`"" }
$inputSys = (Get-ChildItem -Path 'Library' -Filter 'Unity.InputSystem.dll' -Recurse -ErrorAction SilentlyContinue | Select-Object -First 1).FullName
$inputSysArg = if ($inputSys) { @("`"-r:$inputSys`"") } else { @() }
$unityRefs = @(
    '-target:library',
    '-warnaserror',
    '-nowarn:CS0169,CS0414',
    "`"-r:$managed\UnityEngine\UnityEngine.CoreModule.dll`"",
    "`"-r:$managed\UnityEngine\UnityEngine.PhysicsModule.dll`"",
    "`"-r:$managed\UnityEngine\UnityEngine.AudioModule.dll`"",
    "`"-r:$managed\UnityEngine\UnityEngine.ImageConversionModule.dll`"",
    "`"-r:$managed\UnityEngine\UnityEngine.InputLegacyModule.dll`"",
    "`"-r:$managed\UnityEngine\UnityEditor.CoreModule.dll`""
) + $inputSysArg
$lines = $unityRefs + $bclRefs + $files
[System.IO.File]::WriteAllLines($rsp, $lines)
$arg = "@" + $rsp
& $csc $arg
$exitCode = $LASTEXITCODE
Remove-Item $rsp
exit $exitCode
