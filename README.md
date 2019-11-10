# SEI Pioneer - AVMR

### Requirements
  - [Microsoft Visual Studio Community 2019 v16.0.4+](https://visualstudio.microsoft.com/vs/)
  - [CMake v3.16.0+](https://cmake.org/download/)
  - [OpenCV v4.1.1+](https://opencv.org/releases/)
  - [vlSDK v19.7.1+](https://visionlib.com/)
 
### Build 
  1) Open CMake
  2) Specify source code (e.g., `~/vlSDK/Windows/Examples`)
  3) Specify where to build (e.g., `~/vlSDK/Windows/Examles/build`)
  4) Click on Configure and select Visual Studio
  5) If `OpenCV_DIR` can't be found, change its value to `~/opencv/build`
  6) Click on Generate and Open Project
  7) In Visual Studio, right-click on the project and Build
  8) Go on Project -> Properties and modify the Command Arguments (use Space to separate them)
  9) First argument is the path to the `.vl` file
  10) Second argument is the path to the `license.xml` file
  11) Start Debug session
