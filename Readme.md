# Blueprint Breakdown (BPD) For Space Engineers


Ever wondered what your oversized spaceship you built in creative mode might require to build in survival?

Blueprint Breakdown (**BPD**) is a minimal tool that lists a breakdown of blocks, components and resources that makes up a Space Engineers blueprint. **BPD** currently only supports vanilla blocks/components so your mileage may vary with blueprints using mod content from the Steam Workshop.

[Binary Download](https://github.com/DangerRoss/BlueprintBreakdown/raw/master/bpd.exe)


##### Prerequisites
* Space Engineers installed
* .NET Framework (Likely already inlcuded on Windows 10)



## Commands

```
blueprints                      lists local and downloaded blueprints
breakdown {blueprint}           displays blocks, components and resource breakdown of specified blueprint
exit                            exits blueprint breakdown
clear                           clears console output
help                            lists available commands
```

#### blueprints
Lists blueprints from local computer and those downloaded via the Steam Workshop. Blueprints saved in Steam Cloud aren't available. 

```
blueprints

- Local -

Blue Ship
Red Ship

- Workshop -

Some Epic Space Ride
```

#### breakdown
Displays the composition of a blueprint broken down into blocks, components and resources. Resources are represented in grams, displayed in 3 columns for each potential assembly efficiency (world setting) and fractional amounts are rounded up.
```
breakdown Red Ship

Blueprint
Name:                                   Red Ship
Author:                                 Keen

- Blocks -

Total Block Count                       3614

LargeBlockArmorBlock                    1670
LargeBlockArmorSlope                    682
LargeBlockInteriorWall                  515
LargeBlockArmorCorner                   158
LargeHeavyBlockArmorBlock               72
LargeBlockArmorCornerInv                71
LargeBlockSmallThrust                   64
SmallLight                              62
ConveyorTube                            56
LargeRamp                               40
LargeBlockConveyor                      34
AirtightHangarDoor                      20
LargeBlockLargeThrust                   13
LargeBlockGyro                          10
Window1x2Slope                          10
Window1x1Slope                          10
AirVent                                 9
Window1x1FlatInv                        8
Window1x2Flat                           8
OxygenTank                              7
LargeBlockSmallContainer                7
ButtonPanelLarge                        7
ConveyorTubeCurved                      7
Door                                    7
LargeBlockSlideDoor                     6
LargeCameraBlock                        6
Connector                               5
PassengerSeatLarge                      5
LargeBlockLandingGear                   4
LargeHeavyBlockArmorSlope               4
Collector                               4
Window1x2Face                           4
LargeBlockArmorSlope2Base               4
LargeSteelCatwalk                       3
LargeBlockArmorSlope2Tip                3
LargeBlockLargeGenerator                2
LargeBlockBatteryBlock                  2
OxygenGenerator                         2
LargeBlockLargeContainer                2
GravityGenerator                        1
LargeBlockBeacon                        1
LargeBlockCockpit                       1
Window2x3FlatInv                        1
LargeJumpDrive                          1
LargeGatlingTurret                      1
LargeMissileTurret                      1
CockpitOpen                             1
LargeMedicalRoom                        1
LargeMissileLauncher                    1
LargeBlockRadioAntenna                  1

- Components -

Steel Plate                             86,014
Interior Plate                          18,838
Thruster Components                     17,600
Construction Components                 17,257
Metal Grids                             4,516
Reactor Components                      4,000
Small Tube                              3,222
Bulletproof Glass                       1,890
Large Tube                              1,536
Computers                               1,349
Motors                                  1,322
Superconductor                          1,200
Girders                                 565
Power Cells                             280
Radio-Communication Components          80
Displays                                62
Gravity Generator Components            26
Detector Components                     20
Medical Components                      15

- Resources -                           Realistic                     x3                            x10

Iron Ingot                              2,767,853                     922,618                       276,786
Cobalt Ingot                            195,268                       65,090                        19,527
Gravel                                  80,000                        26,667                        8,000
Nickel Ingot                            31,100                        10,367                        3,110
Silicon Wafer                           29,290                        9,764                         2,929
Silver Ingot                            20,430                        6,810                         2,043
Gold Ingot                              20,260                        6,754                         2,026
Platinum Ingot                          7,041                         2,347                         705
```


