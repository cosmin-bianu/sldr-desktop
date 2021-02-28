<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<p align="center">
  <a href="https://github.com/cosmin-bianu/sldr-desktop">
    <img src="img/logo.png" alt="Logo" width="80" height="80">
  </a>

  <h3 align="center">SLDR Desktop</h3>

  <p align="center">
    Windows application used to turn Android phones into presentation remote controls. It support multiple presenters and is able to lock individual presenter's devices. It's also capable of running single presenter mode with the ability to switch the currently presenting device. This project earned my computer science certificate in my final year of high school (2019).
    <br />
    <br />
    <a href="https://github.com/cosmin-bianu/sldr-desktop/issues">Report Bug</a>
    ·
    <a href="https://github.com/cosmin-bianu/sldr-desktop/issues">Request Feature</a>
  </p>
</p>



<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary><h2 style="display: inline-block">Table of Contents</h2></summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

### Built With

* [Windows Presentation Foundation](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/?view=netdesktop-5.0)

<!-- GETTING STARTED -->
## Getting Started

To connect the Android Device to the desktop application, the Android and Windows devices **must** be on the same network.

### Prerequisites

* An Android Device
* Windows 10
* [Visual Studio 2019](https://visualstudio.microsoft.com/vs/)

### Installation

* Dowload the application from the [releases page](https://github.com/cosmin-bianu/sldr-desktop/releases)

* Alternatively, build the application on your machine

1. Clone the repo
   ```sh
   git clone https://github.com/cosmin-bianu/sldr-desktop.git sldr-desktop
   ```
2. Open the project in Visual Studio 2019 (or other version)
3. [Build the project](https://docs.microsoft.com/en-us/visualstudio/ide/building-and-cleaning-projects-and-solutions-in-visual-studio?view=vs-2019)
4. Run the application from the IDE or execute it from Slider/bin/Debug Slider/bin/Release

<!-- USAGE EXAMPLES -->
## Usage

1. Make sure the Android and Windows devices are on the same network, otherwise it will not work
2. Open the Slider desktop application and grant it access through the firewall if prompted
3. Open the Android Application and scan the QR code displayed on the screen
4. Open the slides and go into presentation mode. The Slider Desktop app emulates Left Arrow and Right Arrow keypresses, so make sure to have the presentation window selected at all times.

### Multi-presenter

<p align="center">
    <img src="https://github.com/cosmin-bianu/sldr-desktop/raw/master/img/demo.png" height="500">
</p>

If you need multiple Android devices connected at the same time, repeat step 3 for all mobile devices.

The application allows you to lock the controls for devices so that only a select number of Android devices will have slides control
The devices can be locked from the Android Application too, but only for that particular device

If you need to have devices connected in advance, but want to lock the controls for all except one during the whole presentation, you can use single presenter mode where the user can select and change which device will have controls enabled while the rest will have them disabled. 

<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/cosmin-bianu/sldr-desktop/issues) for a list of proposed features (and known issues).

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.


<!-- CONTACT -->
## Contact

Cosmin Bianu - kitsune.cosmin@gmail.com

Project Link: [https://github.com/cosmin-bianu/sldr-desktop](https://github.com/cosmin-bianu/sldr-desktop)

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/cosmin-bianu/sldr-desktop.svg?style=for-the-badge
[contributors-url]: https://github.com/cosmin-bianu/sldr-desktop/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/cosmin-bianu/sldr-desktop.svg?style=for-the-badge
[forks-url]: https://github.com/cosmin-bianu/sldr-desktop/network/members
[stars-shield]: https://img.shields.io/github/stars/cosmin-bianu/sldr-desktop.svg?style=for-the-badge
[stars-url]: https://github.com/cosmin-bianu/sldr-desktop/stargazers
[issues-shield]: https://img.shields.io/github/issues/cosmin-bianu/sldr-desktop.svg?style=for-the-badge
[issues-url]: https://github.com/cosmin-bianu/sldr-desktop/issues
[license-shield]: https://img.shields.io/github/license/cosmin-bianu/sldr-desktop.svg?style=for-the-badge
[license-url]: https://github.com/cosmin-bianu/sldr-desktop/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/cosmin-bianu