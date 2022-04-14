Hello! Welcome to Florarena!

## Setup

#### 1. Development Environment
- Unity Version 2020.3.26f1
    - Download this via Unity Hub

#### 2. Open Project
1. Clone this repository
2. Go to Unity Hub. From there, click `Projects` and then `Add`.

#### 3. Install Dependencies
1. Mirror Networking Library

    - Download latest Unity Package from [Mirror Releases](https://github.com/vis2k/Mirror/releases)
    - In your Unity project, go to `Assets > Import Package > Custom Package...` and select the downloaded `.unitypackage`
    - Import all assets

2. ParrelSync (Used for symbolic link to Unity project clone for multiplayer testing)

    - Follow the same process as above after downloading the [latest release](https://github.com/VeriorPies/ParrelSync/releases)
    - Setup ParrelSync clone:
        - In Unity project, go to `ParrelSync > Clones Manager` and click `Add new clone`
        - `Open in New Editor` the new clone
		
#### 4. Build Client
1. Android
	 
	- Go to `File > Build Settings`
	- Under `Platform` select `Android` and click `Switch Platform`
	- Go to `Build > Build Client (Android)` to build
	- The APK will be located in the `/Build` folder of this repository

2. Windows (Only for testing)

	- Go to `File > Build Settings`
	- Under `Platform` select `Android` and click `Switch Platform`
	- Go to `Build > Build Client (Android)` to build
	- The APK will be located in the `/Build` folder of this repository

#### 6. Build & Deploy Server

For the gold submission, the server has already been deployed on AWS. If you would like detailed instructions on how this is done, please feel free to ask Raghav (@ephemeralrag on Telegram) for more info!
