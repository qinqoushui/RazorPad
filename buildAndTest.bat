msbuild /m /p:Configuration=Release
mstest /testcontainer:RazorPad.Core.Tests\bin\Release\RazorPad.Core.Tests.dll /testcontainer:RazorPad.UI.Tests\bin\Release\RazorPad.UI.Tests.dll