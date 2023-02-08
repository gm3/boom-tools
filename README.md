# boom-tools: a.r.m. avatar randomizer machine
###### tags: `boom-tools` `Unity Layer Randomizer` `devlog` ```v.0.0.1-alpha```

![](https://hackmd.io/_uploads/Hy1garWpj.png)

Wiki / API Docs https://github.com/gm3/boom-tools/wiki

## Summary
This is a tool that can randomize layers in Unity using weighted randomization. It can export VRMs using uniVRM. The tool can be configured to export a batch of VRMs with erc-1155 metadata, along with an image. You can use this tool to randomize pretty much anything if customized. 

## Dependencies

- This project was made with Unity 2021.3.6f1 
- https://unity3d.com/get-unity/download
- This project uses the `uniVRM` package for runtime VRM export
- https://github.com/vrm-c/UniVRM

## Instructions
- Push ```spacebar``` to randomize a layer, or click Random All
### Exporting Single Screen Shot
- Push F2 to Export iamge from The "Export Camera" to file.
### Batch Export
- In Batch mode you can export a VRM, the JSON, and an image, total of X times you can set in the batch export configuration script.
### Single Export
- You can also export a single VRM as well as just the JSON if you wish.



