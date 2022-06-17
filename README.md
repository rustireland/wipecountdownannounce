# WipeCountdownAnnounce
**WipeCountdownAnnounce** is an [Oxide](https://umod.org/) plugin that provides regular public chat announcements to players on a Rust server about the time remaining until the next wipe. Wipe information is supplied by the [WipeCountdown](https://codefling.com/plugins/wipe-countdown) plugin.

Individually customizable messages are available for:
- Standard days (e.g. *"X days, Y minutes until the next wipe"*).
- The final day before the wipe (e.g. *"X hours, Y minutes until the next wipe"*).
- The final hour before the wipe (e.g. *"X minutes until the next wipe"*).

Additionally, a set list of timings can be used for the final hour before the wipe:
- 60 minutes
- 30 minutes
- 15 minutes
- 10 minutes
- 5 minutes
- 1 minute
### Table of Contents  
- [Requirements](#requirements)  
- [Installation](#installation)  
- [Permissions](#permissions)  
- [Commands](#commands)  
- [Configuration](#configuration)  
- [Localization](#localization)  
- [Credits](#credits)
## Requirements
| Depends On | Works With |
| --- | --- |
| [WipeCountdown](https://codefling.com/plugins/wipe-countdown) | |
## Installation
Download the plugin:
```bash
git clone https://github.com/rustireland/wipecountdownannounce.git
```
Copy it to the Oxide plugins directory:
```bash
cp wipecountdownannounce/WipeCountdownAnnounce.cs oxide/plugins
```
Oxide will compile and load the plugin automatically.
## Permissions
This plugin doesn't make use of the Oxide permissions system.
## Commands
This plugin doesn't provide any console or chat commands.
## Configuration
The settings and options can be configured in the `WipeCountdownAnnounce.json` file under the `oxide/config` directory. The use of an editor and validator is recommended to avoid formatting issues and syntax errors.

When run for the first time, the plugin will create a default configuration file with all wipe announcements *enabled*.
```json
{
  "Announcement Interval (in Minutes)": 20.0,
  "Countdown Final Hour (60, 30, 15, 10, 5, 1 minutes)": true,
  "Chat Icon (SteamID)": 0
}
```
## Localization
The default messages are in the `WipeCountdownAnnounce.json` file under the `oxide/lang/en` directory. To add support for another language, create a new language folder (e.g. **de** for German) if not already created, copy the default language file to the new folder and then customize the messages.
```json
{
  "StandardMessage": "Next wipe will occur in <color=#ffa500>{days} days</color>, <color=#ffa500>{hours} hours</color>.",
  "FinalDayMessage": "Next wipe will occur in <color=#ffa500>{hours} hours</color>, <color=#ffa500>{minutes} minutes</color>.",
  "FinalHourMessage": "Next wipe will occur in <color=#ffa500>{minutes} minutes</color>."
}
```
# Credits
- [Agamemnon](https://github.com/agamemnon23) - Code, testing.
- [Black_demon6](https://github.com/TheBlackdemon6) - Initial concept, testing, and French translations.
