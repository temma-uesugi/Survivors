#!/bin/bash

echo Start MessagePackGenerate

# スクリプトファイルのディレクトリに移動する
cd "$(dirname "$0")"
dotnet mmgen -i ../Assets/App/Scripts/Master/Tables -o ../Assets/App/Scripts/Generated/MasterMemory -n MasterData
dotnet mpc -i ../Assets/App/Scripts/Master/Tables -o ../Assets/App/Scripts/Generated/MessagePack

echo Finish MessagePackGenerate
