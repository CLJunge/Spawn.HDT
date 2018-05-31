# Dust Utility v3.0 [![Build status](https://ci.appveyor.com/api/projects/status/github/cljunge/spawn.hdt.dustutility?branch=master&svg=true)](https://ci.appveyor.com/project/spawndev/spawn-hdt-dustutility)
A utility/management tool, which can help you to obtain certain amounts of dust out of your collection.
![Overlay](https://i.imgur.com/rvwXuao.png)

## Features
#### Search
- Enter a specific amount of dust (ex. 1600), which you would like to get and the plugin displays a list of available cards based on your set filters. With the filter option 'Unused Cards Only' checked, the plugin only considers cards, that are not being used in a deck. (For this option, you have to navigate to the 'Play' menu at least once, in order for the plugin to retrieve your in-game decks)
- Another possiblity is to enter a card name or mechanic to look for specfic cards (ex. Raza). (Click the help button next to the "Filters" button for more information)
![Overlay](https://i.imgur.com/5BigbrO.png)

#### Selection Window
- Drag cards into the selection window to create a list of cards that you would like to disenchant.
- With the setting `Auto Disenchanting` enabled, the plugin can automatically disenchant the current selection by emulating the mouse cursor.
![Overlay](https://i.imgur.com/X0vApAZ.png)

#### Offline Mode
- The plugin detects the account, which is currently logged into Hearthstone and saves it collection and decks locally, so you are able to use the plugin while Hearthstone is not running.
![Overlay](https://i.imgur.com/0DYdS9x.png)

#### Support for multiple accounts and regions
- With `Offline Mode` enabled, you are able to switch between multiple accounts and regions, if their collection and decks have been saved.
![Overlay](https://i.imgur.com/159yCyu.png)

#### Card Images
- Double-click a row to display the actual card image (Golden cards are animated, requires internet connection).
![Overlay](https://i.imgur.com/Nd8b2Rm.png)

#### Customizable Sort Order
- Order the result for your needs. Sortable properties: Mana Cost, Name, Dust, Class, Set, etc...
![Overlay](https://i.imgur.com/0343Un9.png)

#### Card History
- Whenever the plugin stores collection and decks it compares the locally saved collection with your current one and detects which cards are new and which have been disenchanted. (newest to oldest)
![Overlay](https://i.imgur.com/UiDOJS4.png)

#### Decks Info
- Include/exclude decks from search (Use filter option 'Unused Cards Only'). You are also able to check the current deck list for each deck.
![Overlay](https://i.imgur.com/v2zfoX7.png)

#### Collection Info
- Check progress and possible amounts of dust for each expansion (Adding stats for golden cards later).
![Overlay](https://i.imgur.com/ftOpkoX.png)

## Settings
* Offline Mode: While Hearthstone is running, the plugin stores collection and decks (Decks can only be saved when you are in the "Play" menu) locally.
* Save Interval: The interval for storing collection and decks.
* Check For Updates: Checks if there is a new release available on GitHub after opening the main window.
![Overlay](https://i.imgur.com/i1zl588.png)



## Credits
* Class and set icons [HearthSim/hs-icons](https://github.com/HearthSim/hs-icons)
