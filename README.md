# Dust Utility v3.0
A utility/management tool, which can help you to obtain certain amounts of dust from your collection.

![Overlay](https://i.imgur.com/t0pmlUB.png)

## Known Issues

- If you update the plugin automatically, HDT might not replace the old version with the new one. Just delete all files in `%LocalAppData%\HearthstoneDeckTracker\app-{HDT_VERSION}\Plugins\Spawn.HDT.DustUtility` or wherever you have installed HDT and restart the tracker.
- HDT does not always provide the correct data, in which case the history can get messed up and for example show invalid entries of disenchanted cards. You can delete those entries manually or in order the recreate the history, delete the history file in `%AppData%\HearthstoneDeckTracker\DustUtility\Accounts` for the respective account and restore an older version of the collection file from `%AppData%\HearthstoneDeckTracker\DustUtility\Accounts\{ACCOUNT}`.

## Features

#### Search
- Enter a specific amount of dust (eg. 1600), which you would like to get and the plugin displays a list of available cards based on your filters. With the filter option `Unused Cards Only` checked, the plugin only considers cards, that are not being used in a deck. (For this option, you have to navigate to the 'Play' menu at least once, in order for the plugin to obtain your in-game decks)
- Another possiblity is to enter a card name or mechanic to look for specfic cards (ex. Raza). (Click the help button next to the "Filters" button for more information)

#### Selection Window
- Drag cards into the selection window to create a list of cards that you would like to disenchant.
- With the setting `Auto Disenchanting` enabled, the plugin can automatically disenchant the current card selection by emulating the mouse cursor. (Works like the old HDT export feature)

#### Offline Mode
- The plugin detects the account, which is currently logged into Hearthstone and saves it collection and decks locally, so you are able to use the plugin while Hearthstone is not running.

#### Support for multiple accounts and regions
- With `Offline Mode` enabled, you are able to switch between multiple accounts and regions, if their collection and decks have been saved.

#### Card Images
- Double-click a row to display the actual card image (Golden cards are animated, requires internet connection).

#### Customizable Sort Order
- Order the result for your needs. Sortable properties: Mana Cost, Name, Dust, Class, Set, etc...

#### Card History
- Whenever the plugin saves collection and decks it compares the locally saved collection with your current one and detects which cards are new and which have been disenchanted. (newest to oldest)

#### Decks Info
- Include/exclude decks from search (Use filter option `Unused Cards Only`). You are also able to check the current deck list for each deck.

#### Collection Info
- Check progress and possible amounts of dust for each expansion (Regular and golden cards).

![Overlay](https://i.imgur.com/nwJvA25.png)


## Used Resources
* Hearthstone artwork: [Blizzard Press Center](https://blizzard.gamespress.com/Hearthstone)
* Class and set icons: [HearthSim/hs-icons](https://github.com/HearthSim/hs-icons)
