#!/usr/bin/env bash

dotnet build && \
cp ./bin/Debug/netstandard2.1/BetterInverseTeleporter.dll \
   $HOME/.local/share/Steam/steamapps/common/Lethal\ Company/BepInEx/plugins/
