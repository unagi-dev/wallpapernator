# Wallpapernator

### Description
You know those slick wallpapers on the Windows 10 lock screen (Spotlight images) and also those on www.bing.com, 
this app monitors them for updates and copies the images to a desired location. You then just set your
wallpaper setting to _Slideshow_ and point it to the chosen location. Now you have awesome
wallpapers that are automatically updated.

![alt text](https://raw.githubusercontent.com/unagi-dev/wallpapernator/master/screenshot.jpg "Wallpapernator")

_Note:_ The app has only been tested on Windows 10.

### Installation
- Download the latest msi installer from the [releases](https://github.com/unagi-dev/wallpapernator/releases) page.
- Run Wallpapernator.msi

### Usage
- Configure settings:
- _Wallpaper_ : The folder where images will be saved to. Tip: Point to a cloud drive to easily access wallpapers across multiple machines.
- _Spotlight_ : This is the path where Windows stores the lock screen images. It should be: `C:\Users\YourUsername\AppData\Local\Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets` replacing `YourUsername` with your username. _Note:_ This should be auto detected.
- _Image_ _size_ : Currently only `1920x1080` has been tested. This is the size of the image it looks for in the Spotlight folder.
- _Bing_ _interval_ : The interval to check for new Bing images.
- _Run_ _at_ _startup_ : Run the app on startup. (The app has to run to check for new wallpapers)

### Dev stuff
- Visual Studio 2017 Community Edition
- WPF application
- Open solution
- Run
- For the installer project you will need the _Visual Studio Installer Projects Extension_ (Search for it in Tools > Extension and Updates)
