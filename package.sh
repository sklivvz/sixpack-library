#!/bin/bash

rm -rf ./bin

cp -r ./src/SixPack.Web.Services/bin .
cp -r ./src/SixPack.Net.Mail/bin .
cp -r ./src/SixPack/bin .

cp ./src/LICENSE ./bin


rm -rf ./packages
mkdir ./packages

zip -r -9 ./packages/SixPack-bin-1.0.zip bin/

rm -r ./src/SixPack.Web.Services/bin
rm -r ./src/SixPack.Net.Mail/bin
rm -r ./src/SixPack/bin
rm -r ./src/SixPack.Web.Services/obj
rm -r ./src/SixPack.Net.Mail/obj
rm -r ./src/SixPack/obj

zip -r -9 ./packages/SixPack-src-1.0.zip src/

