# ColoredPassword
[![Version](https://img.shields.io/github/release/rookiestyle/coloredpassword)](https://github.com/rookiestyle/coloredpassword/releases/latest)
[![Releasedate](https://img.shields.io/github/release-date/rookiestyle/coloredpassword)](https://github.com/rookiestyle/coloredpassword/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/rookiestyle/coloredpassword/total?color=%2300cc00)](https://github.com/rookiestyle/coloredpassword/releases/latest/download/ColoredPassword.plgx)\
[![License: GPL v3](https://img.shields.io/github/license/rookiestyle/coloredpassword)](https://www.gnu.org/licenses/gpl-3.0)

ColoredPassword lets you use different colors for normal letters, digits and special characters when passwords are displayed.  
The plugin handles password fields and can also color passwords in the entry list.

It's main use case is for scenarios where auto-typing / copying won't work.

# Table of Contents
- [Configuration](#configuration)
- [Usage](#usage)
- [Translations](#translations)
- [Download & updates](#download--updates)
- [Requirements](#requirements)

# Configuration
ColoredPassword integrates into KeePass' options form.  
![Options](images/ColoredPassword%20-%20Options.png)

You can define both text- and background colors for
- letters
- digits
- special characters

# Usage
ColoredPassword will color passwords only if the password itself is shown.  

If passwords are hidden using asterisks, no coloring takes place.
If showing passwords is not allowed in KeePass' policies, no coloring takes place.

If you configured Keepass to show passwords in the entry list and decided to have them colored as well, you can optionally decide to **not** change the background color.  
This will ensure the entire line will have the same background color, but - depending on the colors you picked - can result in decreased readability.

## Example - Editing entries and entering the masterkey
![Options](images/ColoredPassword%20-%20Entry.png)  
![Options](images/ColoredPassword%20-%20Masterkey.png)

## Example - Entry list
![Options](images/ColoredPassword%20-%20Entry%20List.png)

# Translations
ColoredPassword is provided with English language built-in and allow usage of translation files.
These translation files need to be placed in a folder called *Translations* inside in your plugin folder.
If a text is missing in the translation file, it is backfilled with English text.
You're welcome to add additional translation files by creating a pull request as described in the [wiki](https://github.com/Rookiestyle/ColoredPassword/wiki/Create-or-update-translations).

Naming convention for translation files: `<plugin name>.<language identifier>.language.xml`\
Example: `ColoredPassword.de.language.xml`
  
The language identifier in the filename must match the language identifier inside the KeePass language that you can select using *View -> Change language...*\
This identifier is shown there as well, if you have [EarlyUpdateCheck](https://github.com/rookiestyle/earlyupdatecheck) installed.

# Download & updates
Please follow these links to download the plugin file itself.
- [Download newest release](https://github.com/rookiestyle/coloredpassword/releases/latest/download/ColoredPassword.plgx)
- [Download history](https://github.com/rookiestyle/coloredpassword/releases)

If you're interested in any of the available translations in addition, please download them from the [Translations](Translations) folder.

In addition to the manual way of downloading the plugin, you can use [EarlyUpdateCheck](https://github.com/rookiestyle/earlyupdatecheck/) to update both the plugin and its translations automatically.  
See the [one click plugin update wiki](https://github.com/Rookiestyle/EarlyUpdateCheck/wiki/One-click-plugin-update) for more details.
# Requirements
* KeePass: 2.46
