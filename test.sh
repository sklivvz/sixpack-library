#!/bin/bash
cd ./test/SixPack.UnitTests
xbuild SixPack.UnitTests.csproj

cd ../../support/mbunit
mono MbUnit.Cons.exe /rf:../reports /rt:Text  ../../test/SixPack.UnitTests/bin/Debug/SixPack.UnitTests.dll

cd ../..
