### CHANGELOG

## 1.4.0

* Reorganized config option names in .cfg file (delete and create the .cfg file again for your convenience)
* Fixed important issue that was STILL not cancelling the freezing status after exiting mountains before having killed Moder.
* Fixed deer drop when Eikthyr is killed, the extra drop was not matching the .cfg file value

## 1.3.3

* Fixed important issue that was not removing the freezing status after exiting mountain before having killed Moder. Sorry again!

## 1.3.2

* Fixed important issue that was not removing the wet status after exiting swamp before having killed Bonemass. Sorry!

## 1.3.1

* Added new feature to reduce the cooldown on powers by killing top fighters in each biome.
  * Decrease amount configurable in config file. Set to 0 to disable (default = 60).
  * List of monsters configurable. Most powerful monsters assigned of each biome by default.

## 1.3.0

* Now after defeating The Elder, also the damage that the new bears receive is increased
* Damage to trolls and bears in black forest after defeating The Elder increased to double damage by default in configuration
* Fixed wisp light distance when not in Mistlands after killing the Queen (the distance should not be extended)
* Fixed wet message after defeating Bonemass in the swamp, the game was spamming it on top left corner on and off even after exiting the biome
* Small fix in the version number (1.2.3 still showed as 1.2.2, but now it will show the right number, sorry!)
* Code refactor to unify bosses information

## 1.2.3

* Fixed demister range value in the player buff text
* Recompiled with latest game version libraries

## 1.2.2

* Added command "/update-old-keys" to refresh old keys in games played before version 1.2.1. Type in chat to recover your benefits.
* Small bug fix when reloading the game from title screen

## 1.2.1

* Added new benefit when defeated Fader: extra damage resistant to fire in percentage (configurable, default = 100)

## 1.2.0

* Added new benefit when defeating Eikthyr: deer in meadows drop extra deer meat (configurable, default by 1)
* Added new benefit when defeating the Elder: trolls receive more damage in black forests (configurable, default by x1.5)
* Updated player key names automatically to avoid conflict with other mods (player doesn't have to do anything about, it will apply automatically)

## 1.1.2

* ServerSync integration
* Use worldProgression = true in config file to affect all players in a server if interested

## 1.1.1

* Small fix to apply queen benefit on wisp light immediately after dying
* Now you need to defeat the boss in his biome in order to earn the benefit

## 1.1.0

* Added new benefit when Yagluth is defeated. After defeating it, you will no longer be attacked by deathsquitos.
* Added option to show benefits as buffs in the player buffs when you enter the corresponding biome.
* Added hot key to open compendium (F3 by default)

## 1.0.4

* Wisp buff text updated to show the distance that the demister can cover in meters, giving better understanding of the area with fog removed.

## 1.0.3

* Small fix for configuration file names and compendium texts to avoid confusion.

## 1.0.2

* Description added under "Active effects" in Valheim Compendium to track the new effects

## 1.0.1

* Wisp Light range updated immediately after defeating the queen or updating the config file range parameter without having to equip/remove
* Adding world progression option instead of player own battles

## 1.0.0

Initial version:

* After defeating Bonemass, you will no longer get wet with the swamp rain (you will get in other biomes though).
* After defeating Moder, you will no longer get frozen in mountains when no wearing freezing protection effects.
* After defeating The Queen, the wisp light belt will have the range increased long enough to see your surroundings.