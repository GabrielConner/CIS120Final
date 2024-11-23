# CIS120Final

## How to run
**To run the program all you need to do make sure there is a `.KEYMAP` in the `maps` folder**\
**Then run the `.exe` file**

## In the zip file
**In the zip file there will be 3 folders and multiple files**\
**Within the `maps` folder there is an example map called `map.KEYMAP`**\
**There is a larger map called 'largeMap.KEYMAP' as well**\
**To reset progress, delete the `saves` folder**

## What it is
**I made a basic console game which uses text to show a map**\
**The player is an arrow in the direction that was last moved**\
**So if you press `D` the player will be a `>`**\
**The goal is to collect all the `*` around the map**\
**You will be notified with a beep when collecting one, and a little tune when all have been collected**

## How to create a map
* **To create a map you need to first create a file with the extension `.KEYMAP`**
* **Open the file in whatever text editor you want**
* **The first number is going to be the map size**
* **Add a comma and put the view size you want**
* **After that line you can go ahead and create your map using characters**
* **Add it to the `maps` folder with the name `map.KEYMAP`**

**Remember to add `*` for the goals**

**`map.KEYMAP`** = 

**5,5**\
**=====**\
**\\|_|/**\
**||*||**\
**/|_|\\**\
**====\***

## Errors
**Upon an app crashing error a log file will be created, if it doesn't exist, and the error message will be added as a new line**\
**log file is `log.txt`**

**If the `maps` folder does not exist an error will be added with '{current date} Error : No map folder'**