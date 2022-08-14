

# boom-tools: a.r.m. avatar randomizer machine
###### tags: `boom-tools` `Unity Layer Randomizer` `devlog`

![](https://i.imgur.com/52zlfzV.jpg)

![](https://i.imgur.com/kFgGaGs.png)

![](https://i.imgur.com/2lOg28K.jpg)

![](https://i.imgur.com/pYkqt1h.png)

This software is in alpha, and getting ready for public release. There are bugs, and the project is bloated with test assets. This repo is to clean up, optimize and get the project ready for an open-source release.   

## Summary

This is a tool that can randomize GameObject layers in Unity, materials  with the feature to export to VRM using uniVRM runtime export example. The editor tools can be configured to export a batch of NFTs along with the erc-721 metadata saved to file. 

## Dependencies
- This project was made with Unity 2021.3.6f1 
- https://unity3d.com/get-unity/download
- This project uses the `uniVRM` package for runtime VRM export
- https://github.com/vrm-c/UniVRM

## Instructions

## CC0 libraries included in this package
https://thebasemesh.com/model-library

## Contribute / Feedback Issues
Please let me know if there is anyway to improve this tool! 

## How does it work?

You can output a frame of what the export camera sees to a rendertexture, and then save that Renter Texture to disc.

![](https://i.imgur.com/brvbNQ5.jpg)

## Tests
Gif of mocap to test the animations
You can do more than just generate random avatars, you can bring in live mocap to create animations using the Unity Recorder to make awesome aniamtion in your generated avatar. This animation was done by r00t, a friend of m3 and mvc

![](https://i.imgur.com/uLwjfwx.gif)


Here are some screenshtos of the randomizer in alpha, so far it can randomize most of the layers in the output of the camera and export erc721 and json and a vrm file of the avatar.

## Where can you use a VRM?

Here is a shot of the avatar exported to Webaverse, the exports were successful!
Gif inside Webaverse
![](https://i.imgur.com/aDdKPCJ.gif)



## License
MIT CC0 


## Contact
@godfreymeyer http://www.twitter.com/godfreymeyer
