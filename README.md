Buildable stacks for the craftable bars in the game. Made at request by Jiu Jitsu Jones in OdinPlus discord.

`NOTE:` If the mod is installed on the server, the crafting requirements will be synced. The mod doesn't have version checking.

### Made at the request of `Jiu Jitsu Jones#3378` in the OdinPlus discord.


> ## Configuration Options
`[piece_stack_...]`

Each type/stack of metal has its own configuration options. The default cost of each stack is 20 of the metal bars for that stack. Defaulted to recoverable. Example below.
 -  Default value: BlackMetal:20:True
 - `NOTE:` All options here are synced with the server if the mod is installed on the server.

`[General]`

* Force Server Config [Synced with Server]
    * Force Server Config (if installed on the server, this will lock the server's configuration and enforce on the client.)
        * Default value: true

> ## Installation Instructions
***You must have BepInEx installed correctly! I can not stress this enough.***

#### Windows (Steam)
1. Locate your game folder manually or start Steam client and :
    * Right click the Valheim game in your steam library
    * "Go to Manage" -> "Browse local files"
    * Steam should open your game folder
2. Extract the contents of the archive into the game folder.
3. Locate azumatt.StackedBars.cfg under BepInEx\config and configure the mod to your needs

#### Server

`﻿Must be installed on both the client and the server for syncing to work properly.`
1. Locate your main folder manually and :
   a. Extract the contents of the archive into the main folder that contains BepInEx
   b. Launch your game at least once to generate the config file needed if you haven't already done so.
   c. Locate azumatt.StackedBars.cfg under BepInEx\config on your machine and configure the mod to your needs
2. Reboot your server. All clients will now sync to the server's config file even if theirs differs. Config Manager mod changes will only change the client config, not what the server is enforcing.


`Feel free to reach out to me on discord if you need manual download assistance.`


# Author Information

### Azumatt

`DISCORD:` Azumatt#2625

`STEAM:` https://steamcommunity.com/id/azumatt/﻿


For Questions or Comments, find me in the Odin Plus Team Discord or in mine:

[![https://i.imgur.com/XXP6HCU.png](https://i.imgur.com/XXP6HCU.png)](https://discord.gg/Pb6bVMnFb2)
<a href="https://discord.gg/pdHgy6Bsng"><img src="https://i.imgur.com/Xlcbmm9.png" href="https://discord.gg/pdHgy6Bsng" width="175" height="175"></a>

***
> # Update Information (Latest listed first)
> ### v1.0.8
> - Update ServerSync internally
> ### v1.0.7
> - Fix default recipe for Flametal to now use the new internal name of FlametalNew. 
> - Update ServerSync internally
> - Automate uploading to Thunderstore
> ### v1.0.6
> - Change all crafting station requirements to now just be the workbench for ease.
> ### v1.0.5
> - Bog Witch
> ### v1.0.4
> - Update for Ashlands. Fix some sizing issues, update Flametal to the new material. Update internal PieceManager
> ### v1.0.3
> - Update
> ### v1.0.2
> - Update ServerSync + PieceManager internally. Update for 0.217.46 as well.
> ### v1.0.1
> - Update ServerSync internally. Deprecate OdinPlus version of this, move under my name.
> ### v1.0.0
> - Initial Release