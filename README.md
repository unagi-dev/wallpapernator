# Wallpapernator

### Description
You know those slick wallpapers on the Windows 10 lock screen and also those on www.bing.com, 
this app monitors them and copies them to a desired location. You then just set your
wallpaper setting to _Slideshow_ and point it to the chosen location. Now you have sweet
wallpapers that are automatically updated.

![alt text](https://raw.githubusercontent.com/unagi-dev/wallpapernator/master/screenshot.jpg "Wallpapernator")

_Note:_ The app has only been tested on Windows 10.

### Installation
- Download the latest zip: [Wallpapernator 1.0.4](https://github.com/unagi-dev/wallpapernator/raw/master/installers/Wallpapernator-0.1.4.zip)
- Unzip the installer (Wallpapernator.msi).
- Run Wallpapernator.msi

### Usage
- Configure settings
- _Wallpaper_ : The folder where images will be copied to. Tip: Point to a cloud drive to easily access wallpaper across machines.
- _Spotlight_ : This is the path where Windows stores the lock screen images. It should be: `C:\Users\YourUsername\AppData\Local\Packages\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\LocalState\Assets` replacing `YourUsername` with your username. This should be auto detected.
- _Image_ _size_ : Currently only `1920x1080` has been tested. This is the size of the image it looks for in Spotlight.
- _Bing_ _images_ : The interval to check for new Bing images.
- _Run_ _at_ _startup_ : Run the app on startup. (The app has to run to check for wallpapers)

### Dev stuff
- Visual Studio 2017 Community Edition
- WPF application
- Open solution
- Run
- For the installer project you will need the _Visual Studio Installer Projects Extension_ (Search for it in Tools > Extension and Updates)

### Versions


**0.1.4** [Wallpapernator-0.1.4.zip](https://github.com/unagi-dev/wallpapernator/raw/master/installers/Wallpapernator-0.1.4.zip)  
- Redesigned UI
- Added tray context menu (right click)
- Added update check on About page
- Auto detect spotlight images path
- Many UI fixes and tweaks


**0.1.3** [Wallpapernator-0.1.3.zip](https://github.com/unagi-dev/wallpapernator/raw/master/installers/Wallpapernator-0.1.3.zip)  
- Fixed memory usage issue
- Added tray icon
- Run at startup
- About tab
- Fixes and improvements


**0.1.2**  
- Just a test. Initial upload